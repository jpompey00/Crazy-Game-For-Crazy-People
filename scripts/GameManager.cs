using Godot;
using System;

public partial class GameManager : Node
{
	/*
		turn based system
		how is a turn based system achieved.
		1. Check when its the players turn
		2. If its the player turn wait for player action, send a signal from the player node that a player action has occured
		3. If player action occured, game manager will pick up the signal
		4. Game manager will then activate the enemy AI
			in this scenario the Enemy Ai's purpose is to apporach the player coordinate by coordinate. 
	*/
	// Called when the node enters the scene tree for the first time.
	//ideally will be an enemy list, but will just be enemy node for now.
	[Signal]
	public delegate void advanceTurnSignalEventHandler();

	[Signal]
	public delegate void playerTurnStartEventHandler();
	public Enemy enemy;
	public Player player;

	public bool playerTurn;

	public override void _Ready()
	{
		enemy = GetNode<Enemy>("../Enemy");
		player = GetNode<Player>("../Player");
		// GD.Print(enemy);
		GD.Print("Emit Signal");

		//need a function to track all the enemy actions then start the turn after that

		enemy.enemyActionCompleted += playerTurnStarted;
		player.playerTurnEnded += playerTurnEnded;
		
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		// advanceTurn();
    }

	public void playerTurnEnded()
	{
		//whatever other operations
		advanceTurn();
    }

	public void advanceTurn()
	{
		EmitSignal(SignalName.advanceTurnSignal);
	}

	public void playerTurnStarted()
    {
		EmitSignal(SignalName.playerTurnStart);
    }
}
