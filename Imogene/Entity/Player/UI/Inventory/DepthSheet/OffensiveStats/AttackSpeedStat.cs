using Godot;
using System;

public partial class AttackSpeedStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Attack speed {0} \n * Based off of weapon speed \n * Increased by skills and gear ";
	}

}
