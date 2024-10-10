using Godot;
using System;

public partial class Journal : Control
{
	[Export] public Control quest_tab;
	[Export] public Control quest_page;
	[Export] public Control bestiary_tab;
	[Export] public Control bestiary_page;
	[Export] public Control lore_tab;
	[Export] public Control lore_page;
	[Export] public Control tutorial_tab;
	[Export] public Control tutorial_page;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
