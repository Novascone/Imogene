using Godot;
using System;

public partial class NewJournalPage : Node
{
	[Export] public Label label;
	[Export] public string page_text;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
