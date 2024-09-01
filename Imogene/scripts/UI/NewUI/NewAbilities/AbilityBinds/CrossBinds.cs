using Godot;
using System;

public partial class CrossBinds : Control
{

	[Export] public Control up;
	[Export] public Control left;
	[Export] public Control right;
	[Export] public Control down;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
