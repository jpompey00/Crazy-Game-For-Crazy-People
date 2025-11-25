using Godot;
using System;
using System.Diagnostics;

public partial class Player : Node2D
{
	private Vector2 tileSize = new Vector2(16, 16);
	public GameManager gameManager;

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

	[Export]
	public float actionCooldownTime = 0.35f;
	public bool actionCooldown = false;

	Timer actionCooldownTimer = new Timer();

	[Signal]
	public delegate void playerTurnEndedEventHandler();
	public override void _Ready()
	{
		AddChild(actionCooldownTimer);
		statDebug = GetNode<RichTextLabel>("DebugControl/Stat Label");
		positionDebug = GetNode<RichTextLabel>("DebugControl/Position Label");
		//sets the stats
		stats = new Stats(HEALTH, STAMINA, STRENGTH, DEFENSE);


		upRaycast = GetNode<RayCast2D>(raycastPath + "Up");
		downRaycast = GetNode<RayCast2D>(raycastPath + "Down");
		leftRaycast = GetNode<RayCast2D>(raycastPath + "Left");
		rightRaycast = GetNode<RayCast2D>(raycastPath + "Right");

		gameManager = GetNode<GameManager>("../Game Manager");
		GD.Print(gameManager);

		gameManager.playerTurnStart += playerTurnStarted;

		actionCooldownTimer.Timeout += () =>
		{
			GD.Print("Action cooldown ended");
			actionCooldown = false;
			actionCooldownTimer.Stop();
		};

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		debugUpdate();

	}

	public override void _PhysicsProcess(double delta)
	{
		// base._PhysicsProcess(delta);
		movement();
	}

	public void move(int x, int y)
	{
		// startActionCooldown();
		//vector mathematics.
		Position += new Vector2(x, y) * tileSize;
		//make sprite follow it.
		actionTaken();
		// GD.Print($"New Position: {Position.X}, {Position.Y}");
	}



	public void debugUpdate()
	{
		if (DEBUG)
		{
			statDebug.Text = $"Hp:{stats.health}\nStamina:{stats.stamina}\nStrength:{stats.strength}\nDefense:{stats.defense}";
			positionDebug.Text = $"({Position.X},{Position.Y})";
		}
	}



	public void movement()
	{

		if (Input.IsActionPressed("up") && !upRaycast.IsColliding() && actionCooldown == false)
		{
			move(0, -1);

		}
		else if (Input.IsActionPressed("down") && !downRaycast.IsColliding() && actionCooldown == false)
		{
			move(0, 1);
		}
		else if (Input.IsActionPressed("left") && !leftRaycast.IsColliding() && actionCooldown == false)
		{
			move(-1, 0);
		}
		else if (Input.IsActionPressed("right") && !rightRaycast.IsColliding() && actionCooldown == false)
		{
			move(1, 0);
		}
	}


	public void startActionCooldown()
	{
		actionCooldown = true;
		GD.Print("Action Cooldown Started");
		actionCooldownTimer.Start(actionCooldownTime);
	}

	public void actionTaken()
	{
		GD.Print("Player Turn Ended");
		actionCooldown = true;
		EmitSignal(SignalName.playerTurnEnded);
	}
	
	public void playerTurnStarted()
    {
		actionCooldown = false;
    }
}
