using Godot;
using System;

public partial class HUDCross : Control
{
	[Export] public HUDButton up;
	[Export] public Label up_label;
	[Export] public HUDButton left;
	[Export] public Label left_label;
	[Export] public HUDButton right;
	[Export]public Label right_label;
	[Export] public HUDButton down;
	[Export] public Label down_label;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
