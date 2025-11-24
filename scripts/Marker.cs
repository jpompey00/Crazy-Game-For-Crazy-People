using Godot;
using System;

public partial class Marker : Node2D
{
	RichTextLabel coordateLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		coordateLabel = GetNode<RichTextLabel>("CoordinateLabel");
		coordateLabel.Text = this.Position.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
