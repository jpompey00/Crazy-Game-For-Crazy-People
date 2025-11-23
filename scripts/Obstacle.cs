using Godot;
using System;

public partial class Obstacle : Node2D
{
	public bool isObstacle { get; set;} 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		isObstacle = true;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
