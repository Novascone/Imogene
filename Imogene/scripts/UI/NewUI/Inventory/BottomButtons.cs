using Godot;
using System;

public partial class BottomButtons : Control
{

	[Export] public BottomButton abilities;
	[Export] public BottomButton journal;
	[Export] public BottomButton achievements;
	[Export] public BottomButton social;
	[Export] public BottomButton options;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
