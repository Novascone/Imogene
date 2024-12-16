using Godot;
using System;

public partial class SpellResistanceStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Spell Resistance {0} \n * Reduces incoming magic damage \n * Increased by skills and gear ";
	}
	
}
