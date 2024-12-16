using Godot;
using System;

public partial class DamageStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		info.tool_tip.Text =  "  Damage {0}";
	}

	public override void GetStatInfo(float stat_value_ui_, float physical_melee_dps_, float spell_melee_dps, float physical_ranged_dps_, float spell_ranged_dps_)
	{
		stat_value = stat_value_ui_.ToString();
		value.Text = stat_value;
		info.tool_tip.Text = string.Format(info.tool_tip.Text, stat_value, physical_melee_dps_, spell_melee_dps, physical_ranged_dps_, spell_ranged_dps_);
	}
}
