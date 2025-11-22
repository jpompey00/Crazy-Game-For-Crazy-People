using Godot;
using System;
using System.Diagnostics;

public partial class Player : Node2D
{
	private Vector2 tileSize = new Vector2(16,16);

	//debug flag
	public const Boolean DEBUG = true;

	public Stats stats;
	public const int HEALTH = 10;
	public const int STAMINA = 10;
	public const int STRENGTH = 10;
	public const int DEFENSE = 10;

	String raycastPath = "Raycasts/";
	public RayCast2D upRaycast;
	public RayCast2D downRaycast;
	public RayCast2D leftRaycast;
	public RayCast2D rightRaycast;

	public RichTextLabel statDebug;
	public RichTextLabel positionDebug;
	public override void _Ready()
	{
		statDebug = GetNode<RichTextLabel>("DebugControl/Stat Label");
		positionDebug = GetNode<RichTextLabel>("DebugControl/Position Label");
		//sets the stats
		stats = new Stats(HEALTH, STAMINA, STRENGTH, DEFENSE);


		upRaycast = GetNode<RayCast2D>(raycastPath + "Up");
		downRaycast = GetNode<RayCast2D>(raycastPath + "Down");
		leftRaycast = GetNode<RayCast2D>(raycastPath + "Left");
		rightRaycast = GetNode<RayCast2D>(raycastPath + "Right");
		

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		debugUpdate();

	}

	public override void _PhysicsProcess(double delta)
	{
		// base._PhysicsProcess(delta);

		if (Input.IsActionJustPressed("up") && !upRaycast.IsColliding())
		{
			move(0, -1);
		}
		else if (Input.IsActionJustPressed("down") && !downRaycast.IsColliding())
		{
			move(0, 1);
		}
		else if (Input.IsActionJustPressed("left") && !leftRaycast.IsColliding())
		{
			move(-1, 0);
		}
		else if (Input.IsActionJustPressed("right") && !rightRaycast.IsColliding())
        {
			move(1, 0);
        }
	}
	
	public void move(int x, int y)
	{

		//vector mathematics.
		Position += new Vector2(x, y) * tileSize;
		//make sprite follow it.

		GD.Print($"New Position: {Position.X}, {Position.Y}");
    }



	public void debugUpdate()
    {
		if (DEBUG)
        {
			statDebug.Text = $"Hp:{stats.health}\nStamina:{stats.stamina}\nStrength:{stats.strength}\nDefense:{stats.defense}";
			positionDebug.Text = $"({Position.X},{Position.Y})";
        }
    }
}
