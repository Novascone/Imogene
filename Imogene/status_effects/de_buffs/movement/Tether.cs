using Godot;
using System;

public partial class Tether : StatusEffect
{	public Area3D tether;
	
	public Tether(Area3D confined_Area)
	{
		tether = confined_Area;
		name = "tether";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		duration = 5;
		max_stacks = 1;
		


	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
