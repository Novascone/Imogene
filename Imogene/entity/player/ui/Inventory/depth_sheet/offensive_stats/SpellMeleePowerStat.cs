using Godot;
using System;


public partial class SpellMeleePowerStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Spell melee power {0} \n Increases magic melee DPS by 1 every 15 points \n * +3 for every point of intellect +1 for every point of dexterity + 1 for every point of strength \n * Bonuses  obtainable on gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
