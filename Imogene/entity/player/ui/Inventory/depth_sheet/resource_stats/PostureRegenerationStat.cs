using Godot;
using System;

public partial class PostureRegenerationStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text =  " Posture regen {0} \n * Amount of posture regenerated per second \n * Increased by skills and gear ";
	}

}
