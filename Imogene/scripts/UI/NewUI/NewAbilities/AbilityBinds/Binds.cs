using Godot;
using System;

public partial class Binds : Control
{
	[Export] public Control l_cross_primary_assignment;
	[Export] public Control r_cross_primary_assignment;
	[Export] public Control l_cross_secondary_assignment;
	[Export] public Control r_cross_secondary_assignment;
	[Export] public Control passives;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_close_button_down()
	{
		
	}
}
