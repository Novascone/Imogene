using Godot;
using System;

public partial class MainHUD : Control
{
	[Export] public Control useable_1;
	[Export] public Control useable_2;
	[Export] public Control useable_3;
	[Export] public Control useable_4;
	[Export] public Control posture;
	[Export] public Control xp;
	[Export] public Control health;
	[Export] public Control l_cross_secondary;
	[Export] public Control l_cross_primary;
	[Export] public Control r_cross_secondary;
	[Export] public Control r_cross_primary;
	[Export] public Control resource;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
