using Godot;
using System;

public partial class MaximumHealthStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text =  " Maximum Health {0} \n * Your total amount of health, if health is reduced to zero you die ";
	}

}
