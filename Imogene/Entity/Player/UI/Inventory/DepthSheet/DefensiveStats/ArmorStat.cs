using Godot;
using System;

public partial class ArmorStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Total Armor {0} \n * Reduces incoming damage by {1} \n * Increased by gear ";
	}

	public override void GetStatInfo(float stat_value_ui_)
	{
		stat_value = stat_value_ui_.ToString();
		value.Text = stat_value;
		info.tool_tip.Text = string.Format(info.tool_tip.Text, stat_value, stat_value);
		//physical_melee_power_info.Text = string.Format(physical_melee_power_info_text, physical_melee_power_UI);
	}
}
