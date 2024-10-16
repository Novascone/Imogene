using Godot;
using System;

public partial class PoisonDamageStat : UIStat
{
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Poison Damage {0} \n * Increases poison damage by multiplier \n * Bonuses obtainable on gear ";
	}

}
