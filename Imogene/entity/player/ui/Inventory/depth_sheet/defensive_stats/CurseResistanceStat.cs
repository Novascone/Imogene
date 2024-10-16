using Godot;
using System;

public partial class CurseResistanceStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Curse Resistance {0} \n * Reduces curse effectiveness \n * Increased by skills and gear ";
	}

}
