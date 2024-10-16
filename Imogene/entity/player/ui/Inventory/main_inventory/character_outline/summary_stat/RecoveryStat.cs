using Godot;
using System;

public partial class RecoveryStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		info.tool_tip.Text =  " Recovery {0} \n * The average of health regeneration, resource regeneration, and posture regeneration \n * Calculated by stamina ";
	}

}
