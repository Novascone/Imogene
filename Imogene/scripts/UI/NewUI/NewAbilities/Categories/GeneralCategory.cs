using Godot;
using System;

public partial class GeneralCategory : Control
{
	[Export] public Control melee;
	[Export] public Control ranged;
	[Export] public Control defensive;
	[Export] public Control movement;
	[Export] public Control unique;
	[Export] public Control toy;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
