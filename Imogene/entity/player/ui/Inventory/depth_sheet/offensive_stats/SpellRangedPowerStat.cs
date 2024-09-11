using Godot;
using System;

public partial class SpellRangedPowerStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		// info.info.Text =  " Spell ranged power {0} \n Increases magic ranged DPS by 1 every 15 points \n * +3 for every point of intellect +2 for every point of dexterity \n * Bonuses obtainable on gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
