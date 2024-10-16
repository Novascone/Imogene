using Godot;
using System;

public partial class FireResistanceStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Fire Resistance {0} \n * Reduces incoming fire damage \n * Increased by skills and gear ";
	}
	
}
