using Godot;
using System;

public partial class CooldownReductionStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Cooldown reduction {0} \n * Cooldown reduction of all skills ";
	}

}
