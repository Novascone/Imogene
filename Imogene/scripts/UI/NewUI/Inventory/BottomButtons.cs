using Godot;
using System;

public partial class BottomButtons : Control
{

	[Export] public Control abilities;
	[Export] public Control journal;
	[Export] public Control achievements;
	[Export] public Control social;
	[Export] public Control options;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
