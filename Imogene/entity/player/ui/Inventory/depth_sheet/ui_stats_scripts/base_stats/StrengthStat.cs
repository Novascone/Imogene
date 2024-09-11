using Godot;
using System;

public partial class StrengthStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		// info.info.Text =  " Strength {0} \n * Primary stat for melee damage \n * Increases damage by {1} \n * Increases health by {2} \n * Increases critical hit damage by {3}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
	
}
