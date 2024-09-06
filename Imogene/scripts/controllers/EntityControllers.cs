using Godot;
using System;

public partial class EntityControllers : Node
{
	[Export] public StatsController stats_controller;
	[Export] public StatusEffectController status_effect_controller;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
