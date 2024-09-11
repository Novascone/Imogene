using Godot;
using System;

public partial class IntellectStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		// info.info.Text =  "  Intellect {0} \n * main stat for spell damage \n * Increases spell damage by {1} \n * Increases spell hit chance by {2}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
