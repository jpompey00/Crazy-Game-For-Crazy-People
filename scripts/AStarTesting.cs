using Godot;
using System;
using System.Collections.Generic;

public partial class AStarTesting : Node2D
{
	Vector2 testing = new Vector2(64, 64);
	public PackedScene markerPackedScene;
	List<Vector2I> coordinatesForRect = [];
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Vector2I test2 = (Vector2I)testing;
		// GD.Print($"Testing cast: {test2}");

		markerPackedScene = GD.Load<PackedScene>("res://scenes/enemy/Marker.tscn");

		Rect2I testRect = new Rect2I(0, 0, 768, 384);


		Vector2I top_left = testRect.Position;
		Vector2I top_right = testRect.Position + new Vector2I(testRect.Size.X, 0);
		Vector2I bottom_left = testRect.Position + new Vector2I(0, testRect.Size.Y);
		Vector2I bottom_right = testRect.Position + testRect.Size;

		coordinatesForRect.Add(top_left);
		coordinatesForRect.Add(top_right);
		coordinatesForRect.Add(bottom_left);
		coordinatesForRect.Add(bottom_right);

		GD.Print($"Top Left: {top_left}\nTop Right: {top_right}\nBottom Left: {bottom_left}\nBottom Right: {bottom_right}");
		spawnPathMarker();
		start();

	}


	public void start()
	{
		AStarGrid2D astarGrid = new AStarGrid2D();
		astarGrid.Region = new Rect2I(0, 0, 768, 384);
		astarGrid.CellSize = new Vector2I(16, 16);

		//this is the setup I want.
		astarGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		astarGrid.DefaultComputeHeuristic = AStarGrid2D.Heuristic.Manhattan;
		astarGrid.Update();

		//
		astarGrid.SetPointSolid(new Vector2I(208, 80) / 16);
		astarGrid.SetPointWeightScale(new Vector2I(192, 80) / 16, 4);

		GD.Print(astarGrid.GetIdPath(Vector2I.Zero, new Vector2I(3, 4))); // Prints [(0, 0), (1, 1), (2, 2), (3, 3), (3, 4)]
		GD.Print(string.Join(",", astarGrid.GetPointPath(Vector2I.Zero, new Vector2I(3, 4))));

		Vector2 vec = new Vector2(256, 128);
		GD.Print($"Vector2: {vec} | ID Conversion: {vec / 16}");
		GD.Print($"Proof: {astarGrid.GetPointPosition((Vector2I)vec / 16)}");



		

		foreach(Vector2I vector in astarGrid.GetIdPath(Vector2I.Zero, new Vector2I(256, 128)/16))
        {
			spawnPathMarker(vector);
        }

		
		// List<Vector2I> list = [];
		// // Print ALL grid IDs
		// for (int x = astarGrid.Region.Position.X; x < astarGrid.Region.End.X; x++)
		// {
		// 	for (int y = astarGrid.Region.Position.Y; y < astarGrid.Region.End.Y; y++)
		// 	{
		// 		list.Add(new Vector2I(x, y));
		// 		// Vector2I id = 
		// 		// GD.Print(id);
		// 	}
		// }

		// GD.Print(string.Join(",", list));
	}
	private void spawnPathMarker()
	{

		//will map out coordinate to the player :D
		//with no issues :D
		int num = 0;
		coordinatesForRect.ForEach(coordinate =>
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

	private void spawnPathMarker(Vector2I vecId)
    {
        Marker marker = markerPackedScene.Instantiate<Marker>();
			marker.Name = $"marker{vecId.ToString()}";
			// GetTree().CurrentScene.AddChild(marker);
			GetTree().CurrentScene.CallDeferred("add_child", marker);
			// CallDeferred("add_child", marker);
			marker.GlobalPosition = vecId * 16;
			// num += 1;
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
