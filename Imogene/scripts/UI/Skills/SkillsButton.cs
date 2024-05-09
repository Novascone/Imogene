using Godot;
using System;
using System.Collections.Generic;

public partial class SkillsButton : Button
{
	[Export]
	public bool assigned;

	public Resource[] modifiers = new Resource[5];
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
