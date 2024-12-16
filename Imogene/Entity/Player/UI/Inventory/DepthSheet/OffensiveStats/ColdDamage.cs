using Godot;
using System;

public partial class ColdDamage : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Cold Damage {0} \n * Increases cold damage by multiplier \n * Bonuses obtainable on gear ";
	}

}
