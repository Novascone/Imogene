using Godot;
using System;

public partial class EnemyDebug : Node
{
	[Export] public MeshInstance3D CollisionLines;
	[Export] public MeshInstance3D RayLines;
	[Export] public MeshInstance3D DirectionLines;
	[Export] public MeshInstance3D MovingLine;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
