using Godot;
using System;

public partial class SpellRangedPowerStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Spell ranged power {0} \n Increases magic ranged DPS by 1 every 15 points \n * +3 for every point of intellect +2 for every point of dexterity \n * Bonuses obtainable on gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void GetStatInfo(string stat_value_ui)
	{
		stat_value = stat_value_ui;
		value.Text = stat_value;
		info_text.Text = string.Format(set_info_text, stat_value);
		//spell_ranged_power_info.Text = string.Format(spell_ranged_power_info_text, spell_ranged_power_UI);
	}
}
