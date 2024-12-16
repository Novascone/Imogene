using Godot;
using System;

public partial class PoiseStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Poise {0} \n * Reduces incoming posture damage and increases crowd control resistance ";
	}

}
