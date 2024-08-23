using Godot;
using System;

public partial class TargetDummy : Enemy
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		speed = walk_speed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}
}
