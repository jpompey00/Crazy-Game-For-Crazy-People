using Godot;

using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;

//V2
public partial class Enemy : Node2D
{
	public const Boolean DEBUG = false;
	
	private Vector2 tileSize = new Vector2(16, 16);
	public RichTextLabel distanceToPlayerLabel;
	public Vector2 playerPosition;
	public Player player;
	public PackedScene markerPackedScene;
	public List<Vector2> pathToPlayer = [];
	public World worldNode;
	public GameManager gameManager;

	public Stats stats { get; set; }

	[Signal]
	public delegate void enemyActionCompletedEventHandler();




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		stats = new Stats(9, 9, 9, 9);

		markerPackedScene = GD.Load<PackedScene>("res://scenes/enemy/Marker.tscn");
		distanceToPlayerLabel = GetNode<RichTextLabel>("DebugControl/DistanceToPlayerLabel");
		if (GetNode<Player>("../Player") != null)
		{
			player = GetNode<Player>("../Player");
			playerPosition = player.Position;
			// GD.Print("Connected");
			// GD.Print(player.Position);
		}
		worldNode = (World)GetTree().CurrentScene;
		gameManager = worldNode.GetNode<GameManager>("Game Manager");

		GD.Print(worldNode);
		GD.Print(gameManager);
		gameManager.advanceTurnSignal += enemyAi;

	}

	//think about how to do this and influence it for later
	public void enemyAi()
    {
		GD.Print("enemy Ai activated");

		if(pathToPlayer.Count > 0)
        {
			Position = pathToPlayer[1];
			GD.Print($"Position: {Position}");
			GD.Print(string.Join(",", pathToPlayer));
            
        }
		
		Timer placeholderTimer = new Timer();
		placeholderTimer.Timeout += () =>
		{
			EmitSignal(SignalName.enemyActionCompleted);
			GD.Print("enemy Ai completed");
			placeholderTimer.QueueFree();
		};

		AddChild(placeholderTimer);
		placeholderTimer.Start(player.actionCooldownTime);

    }

	private void spawnPathMarker()
	{//will map out coordinate to the player :D
	 //with no issues :D
		if (pathToPlayer.Count > 0)
		{
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
			// clearOutMarkers();
			debugUpdate(0);
			clearPathToPlayer();
			getPathToPlayer();
			debugUpdate(1);
			


		}



	}


	public void getPathToPlayer()
	{
		pathToPlayer.Clear();
		pathToPlayer.AddRange(worldNode.astarGrid.GetPointPath((Vector2I)Position / 16, (Vector2I)playerPosition / 16));



	}


	public void debugUpdate(int flag)
	{
		if (DEBUG)
		{
            switch (flag)
            {
				case 0:
				clearOutMarkers();
					break;
				case 1:
				spawnPathMarker();
					break;
				default:
					break;
            }
			
			
		}
	}


	public void clearOutMarkers()
	{
		for (int i = 0; i < pathToPlayer.Count; i++)
		{
			Node2D marker = GetTree().CurrentScene.GetNode<Node2D>($"marker{i}");
			if (marker != null)
			{
				// GD.Print("called");
				// GD.Print(marker);
				marker.Free();
			}

		}
		
	}

	public void clearPathToPlayer()
    {
        pathToPlayer.Clear();
    }



}
