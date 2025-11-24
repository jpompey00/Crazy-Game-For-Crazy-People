using Godot;
using System;

public partial class World : Node2D
{
	//make dimensions constants maybe
	public AStarGrid2D astarGrid = new AStarGrid2D();

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
	}


	public AStarGrid2D getAStarGrid2D()
	{
		return astarGrid;
	}
	

}
