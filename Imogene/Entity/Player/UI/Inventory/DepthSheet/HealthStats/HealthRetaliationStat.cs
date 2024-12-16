using Godot;
using System;

public partial class HealthRetaliationStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text =  " Health on Retaliation {0} \n * Amount of health you can regain during retaliation period \n * Increased by skills and gear ";
	}

}
