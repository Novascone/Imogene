using Godot;
using System;

public partial class HUDButton : Button
{
	[Export] public string button_bind;
	[Export] public string side;
	[Export] public string level;
	[Export] public Label label;
 	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = button_bind;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
