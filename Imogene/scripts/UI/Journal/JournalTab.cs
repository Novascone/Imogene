using Godot;
using System;

public partial class JournalTab : PanelContainer
{
	public Label text;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		text = GetNode<Label>("HBoxContainer/PanelContainer/Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
