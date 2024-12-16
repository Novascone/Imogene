using Godot;
using System;

public partial class WisdomScalerStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Wisdom Scaler {0} \n Increases by one for every 20 wisdom \n * Scales how powerful attacks that scale with wisdom are ";
	}

}
