using Godot;
using System;

public partial class HealthRegenerationStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text =  " Health regen {0} \n * Amount of health regenerated per second \n * Increased by skills and gear ";
	}

}
