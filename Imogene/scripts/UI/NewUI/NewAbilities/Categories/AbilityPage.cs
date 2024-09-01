using Godot;
using System;

public partial class AbilityPage : Control
{
	[Export] public Label title;
	[Export] public string title_text;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		title.Text = title_text;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
