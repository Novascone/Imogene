using Godot;
using System;

public partial class HealthBonusStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		// info.info.Text =  " Health bonus {0} \n * Bonus applied to total health \n * Increased by skills and gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
