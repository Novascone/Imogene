using Godot;
using System;

public partial class AttackSpeedIncreaseStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Attack speed increase {0} \n * Percentage of attack speed \n * Increased by skills and gear ";
	}

}
