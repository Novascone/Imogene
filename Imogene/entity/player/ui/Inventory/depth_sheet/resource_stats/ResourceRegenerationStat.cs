using Godot;
using System;

public partial class ResourceRegenerationStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text =  " Resource regeneration {0} \n * Amount of resource regenerated per second \n * Increased by skills and gear ";
	}

}
