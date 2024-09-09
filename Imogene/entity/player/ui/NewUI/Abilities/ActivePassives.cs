using Godot;
using System;

public partial class ActivePassives : Control
{
	[Export] public Control active_passive_1;
	[Export] public Control active_passive_2;
	[Export] public Control active_passive_3;
	[Export] public Control active_passive_4;
	[Export] public Control active_passive_5;
	[Export] public Control active_passive_6;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
