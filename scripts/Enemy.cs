using Godot;

using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Enemy : Node2D
{
	private Vector2 tileSize = new Vector2(16, 16);
	public const Boolean DEBUG = true;
	public RichTextLabel distanceToPlayerLabel;
	public Vector2 playerPosition;
	public Player player;
	public PackedScene markerPackedScene;
	public List<Vector2> pathToPlayer = [];
	public List<Vector2> obstaclesPositionList = [];



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		markerPackedScene = GD.Load<PackedScene>("res://scenes/enemy/Marker.tscn");


		distanceToPlayerLabel = GetNode<RichTextLabel>("DebugControl/DistanceToPlayerLabel");
		if (GetNode<Player>("../Player") != null)
		{
			player = GetNode<Player>("../Player");
			playerPosition = player.Position;
			// GD.Print("Connected");
			// GD.Print(player.Position);
		}


		getObstacles();
		// mapOutDistanceToPlayer();
		// spawnPathMarker();
	}

	private void spawnPathMarker()
	{

		//will map out coordinate to the player :D
		//with no issues :D
		int num = 0;
		pathToPlayer.ForEach(coordinate =>
		{

			Marker marker = markerPackedScene.Instantiate<Marker>();
			marker.Name = $"marker{num}";
			// GetTree().CurrentScene.AddChild(marker);
			GetTree().CurrentScene.CallDeferred("add_child", marker);
			// CallDeferred("add_child", marker);
			marker.GlobalPosition = coordinate;
			num += 1;
		});
		num = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player != null && playerPosition != player.Position)
		{
			playerPosition = player.Position;
			//clear marker
			//map distance
			//spawn marker
			
			clearOutMarkers();
			mapOutDistanceToPlayer();
			spawnPathMarker();
			debugUpdate();
		}


	}

	public void debugUpdate()
	{
		if (DEBUG)
		{
			getDistanceToPlayer();
			//this won't fail :D

		}
	}

	public void getDistanceToPlayer()
	{
		Vector2 positionDifference = playerPosition - Position;
		// GD.Print(positionDifference);
		// distanceToPlayerLabel.Text = $"Distance: \n({Math.Abs(positionDifference.X / 16)}, {Math.Abs(positionDifference.Y / 16)})";
		distanceToPlayerLabel.Text = $"Distance: \n({Math.Abs(positionDifference.X / 16) + Math.Abs(positionDifference.Y / 16)})";


	}

	public void mapOutDistanceToPlayer()
	{
		Vector2 positionDifference = playerPosition - Position;
		float distaceToPlayer = Math.Abs(positionDifference.X / 16) + Math.Abs(positionDifference.Y / 16);
		float calculatedDistance = distaceToPlayer;
		Vector2 newPos = this.Position;

		int maxIterations = 100;
		int currentIterations = 0;
		// bool movementUsed = false;
		float newX = 0;
		float newY = 0;

		while (calculatedDistance > 0 && currentIterations < maxIterations)
		{
			//both need to do the calculation
			//lowest value goes


			if (positionDifference.X != 0)
			{
				// float resultAdd = ((positionDifference.X + 16) - player.Position.X);
				// float resultSubtract = ((positionDifference.X - 16) - player.Position.X);

				//checks the difference between going left and going right
				float resultAdd = ((Position.X + 16) - player.Position.X);
				float resultSubtract = ((Position.X - 16) - player.Position.X);


				// resultAdd.CompareTo(resultSubtract);
				//lowest difference


				newX = newPos.X + (Math.Abs(resultAdd).CompareTo(Math.Abs(resultSubtract)) < 0 ? 16 : -16);

				// newPos.X += Math.Abs(resultAdd).CompareTo(Math.Abs(resultSubtract)) < 0 ? 16 : -16;
				// GD.Print($"newX: {newX}");
				// GD.Print($"newPos.X: {newPos.X}");
				// movementUsed = true;
			}

			if (positionDifference.Y != 0
			// && !movementUsed
			)
			{
				// float resultAdd = ((positionDifference.Y + 16) - player.Position.Y);
				// float resultSubtract = ((positionDifference.Y - 16) - player.Position.Y);
				float resultAdd = ((Position.Y + 16) - player.Position.Y);
				float resultSubtract = ((Position.Y - 16) - player.Position.Y);

				// resultAdd.CompareTo(resultSubtract);
				// newPos.Y += Math.Abs(resultAdd).CompareTo(Math.Abs(resultSubtract)) < 0 ? 16 : -16;

				newY = newPos.Y +( Math.Abs(resultAdd).CompareTo(Math.Abs(resultSubtract)) < 0 ? 16 : -16);
			}

			//checks the value of the positions, if it is equal or X is less than
			//it will go with that,
			if ( differenceFromPlayerOneDimensional(new Vector2(newX, Position.Y)).CompareTo(differenceFromPlayerOneDimensional(new Vector2(Position.X, newY))) < 1
				)
			{
				// GD.Print("New Pos X chosen");
				newPos.X = newX;
			}
			else
			{
				newPos.Y = newY;
			}
			GD.Print("New Pos: " + newPos);

			Vector2 newPosDifference = playerPosition - newPos;
			float newDistanceToPlayer = Math.Abs(newPosDifference.X / 16) + Math.Abs(newPosDifference.Y / 16);
			calculatedDistance = newDistanceToPlayer < calculatedDistance ? newDistanceToPlayer : calculatedDistance;
			positionDifference = playerPosition - newPos;
			pathToPlayer.Add(newPos);
			currentIterations += 1;
			// movementUsed = false;
			//this reset is messing it up during its pathfinding.

			newX = -999;
			newY = -999;
		}

		GD.Print(string.Join(",", pathToPlayer));
		GD.Print($"Current Iterations: {currentIterations}");
	}

	public void clearOutMarkers()
	{
		for (int i = 0; i < pathToPlayer.Count; i++)
		{
			Node2D marker = GetTree().CurrentScene.GetNode<Node2D>($"marker{i}");
			if (marker != null)
			{
				GD.Print("called");
				GD.Print(marker);
				marker.Free();
			}

		}
		pathToPlayer.Clear();
	}


	public float differenceFromPlayerOneDimensional(Vector2 newPosition)
	{
		Vector2 difference = playerPosition - newPosition;
		GD.Print($"difference result: {Math.Abs(difference.X / 16) + Math.Abs(difference.Y / 16)}");

        if (obstaclesPositionList.Contains(newPosition))
        {
			GD.Print($"Obstacle found at {newPosition}");
			return 9999;
        }
		return Math.Abs(difference.X / 16) + Math.Abs(difference.Y / 16);
	}

	public bool isCoordinateValid()
	{
		return true;
	}
	
	public void getObstacles()
    {
		Node2D obstacleContainer = GetTree().CurrentScene.GetNode<Node2D>("Obstacles");
		foreach (Node item in obstacleContainer.GetChildren())
		{
			if (item.GetType().ToString() == "Obstacle")
			{
				Obstacle ob = (Obstacle)item;
				// GD.Print("found");
				obstaclesPositionList.Add(ob.Position);
			}
		}

		GD.Print(string.Join(",", obstaclesPositionList));
    }
	/*
	A* Algorithm
	Needs to find the best path to the player
	Step 1, find the path to the player
		Step 1.1 - find the shortest distance to player
			Step 1.1.1 - Find the coordiante of the player.
			Map out every path to get to the player.
				if the calculated path is greater than the distance in 1D, stop calculating.
					May have some issues.
						What to do if a coordinate is invalid.
						If a blocked path is found, it should recalulate the route taking into account the blocked coordinate
					

	*/
}
