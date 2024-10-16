using Godot;
using System;

public partial class ResourceCostReductionStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text =  " Resource cost reduction {0} \n * Amount resource cost of skills are reduced by \n * Increased by skills and gear ";
	}

}
