using Godot;
using System;

public partial class WisdomScalerStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		// info =  " Wisdom Scaler {0} \n Increases by one for every 20 wisdom \n * Scales how powerful attacks that scale with wisdom are ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
