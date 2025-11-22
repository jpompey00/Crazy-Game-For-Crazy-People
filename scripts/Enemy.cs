using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : Node2D
{
	private Vector2 tileSize = new Vector2(16, 16);
	public const Boolean DEBUG = true;
	public RichTextLabel distanceToPlayerLabel;
	public Vector2 playerPosition;
	public Player player;
	public PackedScene markerPackedScene;
	public List<Vector2> pathToPlayer = [];



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

		int maxIterations = 9999;
		int currentIterations = 0;
		bool movementUsed = false;

		while (calculatedDistance > 0 && currentIterations < maxIterations)
		{
			if (positionDifference.X != 0)
			{
				// float resultAdd = ((positionDifference.X + 16) - player.Position.X);
				// float resultSubtract = ((positionDifference.X - 16) - player.Position.X);
				
				//checks the difference between going left and going right
				float resultAdd = ((Position.X + 16) - player.Position.X);
				float resultSubtract = ((Position.X - 16) - player.Position.X);


				// resultAdd.CompareTo(resultSubtract);
				//lowest difference
				newPos.X += Math.Abs(resultAdd).CompareTo(Math.Abs(resultSubtract)) < 0 ? 16 : -16;
				movementUsed = true;
			}

			if (positionDifference.Y != 0 && !movementUsed)
			{
				// float resultAdd = ((positionDifference.Y + 16) - player.Position.Y);
				// float resultSubtract = ((positionDifference.Y - 16) - player.Position.Y);
				float resultAdd = ((Position.Y + 16) - player.Position.Y);
				float resultSubtract = ((Position.Y - 16) - player.Position.Y);

				// resultAdd.CompareTo(resultSubtract);
				newPos.Y += Math.Abs(resultAdd).CompareTo(Math.Abs(resultSubtract)) < 0 ? 16 : -16;
			}

			Vector2 newPosDifference = playerPosition - newPos;
			float newDistanceToPlayer = Math.Abs(newPosDifference.X / 16) + Math.Abs(newPosDifference.Y / 16);
			calculatedDistance = newDistanceToPlayer < calculatedDistance ? newDistanceToPlayer : calculatedDistance;
			positionDifference = playerPosition - newPos;
			pathToPlayer.Add(newPos);
			currentIterations += 1;
			movementUsed = false;
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
