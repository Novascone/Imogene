using Godot;
using System;

public partial class BleedResistanceStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Bleed Resistance {0} \n * Reduces bleeding time \n * Increased by skills and gear ";
	}
	
}
