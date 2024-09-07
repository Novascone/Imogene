using Godot;
using System;

public partial class EnemyDebug : Node
{
	[Export] public MeshInstance3D collision_lines;
	[Export] public MeshInstance3D ray_lines;
	[Export] public MeshInstance3D direction_lines;
	[Export] public MeshInstance3D direction_moving_line;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
