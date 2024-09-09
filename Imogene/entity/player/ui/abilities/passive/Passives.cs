using Godot;
using System;

public partial class Passives : Control
{
	[Export] public Control general_passives;
	[Export] public Control class_passives;
	[Export] public Control class_title;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ResetPage()
	{
		
	}
}
