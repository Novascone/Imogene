using Godot;
using System;

public partial class HUDCross : Node
{
	[Export] Button up;
	[Export] Label up_label;
	[Export] Button left;
	[Export] Label left_label;
	[Export] Button right;
	[Export] Label right_label;
	[Export] Button down;
	[Export] Label down_label;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
