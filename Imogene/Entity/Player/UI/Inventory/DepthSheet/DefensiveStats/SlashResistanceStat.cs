using Godot;
using System;

public partial class SlashResistanceStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Slash Resistance {0} \n * Reduces Slash effectiveness on character \n * Increased by skills and gear ";
	}

}
