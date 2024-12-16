using Godot;
using System;

public partial class ResistanceStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		info.tool_tip.Text =  " Resistance {0} \n * Total damage the character can resist \n * Calculated by health, armor, resistances";
	}
	
}
