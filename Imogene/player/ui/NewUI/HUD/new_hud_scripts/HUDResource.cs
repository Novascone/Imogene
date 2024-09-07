using Godot;
using System;

public partial class HUDResource : Control
{
	[Export] public TextureProgressBar resource_points;
	[Export] public Control resource_damage_buffs;
	[Export] public Control resource_damage_debuffs;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
