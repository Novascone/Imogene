using Godot;
using System;

public partial class MaximumResourceStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text =  " Maximum resource {0} \n * Total amount of resource \n * Increased by skills and gear ";
	}

}
