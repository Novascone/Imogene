using Godot;
using System;

public partial class SmallFireball : Projectile
{

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		projectile_to_load = GD.Load<PackedScene>("res://scripts/abilities/General/Active/SmallFireball/fireball_projectile.tscn");
		projectile_velocity = 15;
		damage_type = "fire";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}
}


