using Godot;
using System;

public partial class HUDHealth : Control
{
	[Export] public TextureProgressBar hit_points;
	[Export] public Control health_movement_debuffs;
	[Export] public Control health__movement_buffs;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
