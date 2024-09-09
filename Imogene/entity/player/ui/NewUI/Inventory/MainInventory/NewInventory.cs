using Godot;
using System;

public partial class NewInventory : Control
{

	[Export] public MainInventory main;
	[Export] public Control depth_sheet;
	[Export] public Control mats;
	[Export] public Control temp_buttons;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
