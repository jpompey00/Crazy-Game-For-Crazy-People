using Godot;
using System;
using System.Collections.Generic;

public partial class World : Node2D
{
	//make dimensions constants maybe
	public AStarGrid2D astarGrid = new AStarGrid2D();
	public List<Obstacle> obstacles = [];
	public int lastObsCount = -1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		astarGrid.Region = new Rect2I(0, 0, 768, 384);
		astarGrid.CellSize = new Vector2I(16, 16);

		astarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		astarGrid.DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan;

		astarGrid.Update();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		checkForObstacles();
	}


	public AStarGrid2D getAStarGrid2D()
	{
		return astarGrid;
	}


	public void checkForObstacles()
	{
		if (lastObsCount != obstacles.Count)
		{
			Node2D obstacleContainer = GetNode<Node2D>("Obstacles");

			foreach (Node item in obstacleContainer.GetChildren())
			{
				if (item.GetType().ToString() == "Obstacle")
				{
					Obstacle ob = (Obstacle)item;
					// GD.Print("found");
					obstacles.Add(ob);
					// GD.Print($"ob.Position : {ob.GlobalPosition}");
					astarGrid.SetPointSolid((Vector2I)ob.GlobalPosition / 16);
				}
			}
			lastObsCount = obstacles.Count;

			// GD.Print(string.Join(",", obstaclesPositionList));
		}

	}


}
