using Godot;
using System;

public partial class MovementSpeedStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text =  " Movement speed {0} \n * Movement speed of character \n * Increased by skills and gear ";
	}

}
