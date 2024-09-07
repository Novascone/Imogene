using Godot;
using System;

public partial class WhirlwindHitbox : MeleeHitbox
{

	// Keep track of each enemy that enters the hitbox and only apply damage to them once
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
