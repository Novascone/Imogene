using Godot;
using System;

public partial class DamageStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		// info.tool_tip.Text =  "   Damage {0} \n * Total damage per second done by character by the average of power modifiers \n * Physical melee dps {1} \n * spell melee dps {2} \n * Physical ranged dps {3} \n * spell ranged dps {4} \n These 4 catagories are your dps when using a skill of the respective type. \n * Combination of power modifiers melee and ranged, in both their physical and spell forms, \n     damage, attack speed, critical hit chance, and critical hit damage";
		info.tool_tip.Text =  "  Damage {0}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void GetStatInfo(float stat_value_ui, float p_m_dps, float s_m_dps, float p_r_dps, float s_r_dps)
	{
		stat_value = stat_value_ui.ToString();
		value.Text = stat_value;
		info.tool_tip.Text = string.Format(info.tool_tip.Text, stat_value, p_m_dps, s_m_dps, p_r_dps, s_r_dps);
		// damage_info.Text = string.Format(damage_info_text, damage_UI, physical_melee_dps_UI, spell_melee_dps_UI, physical_ranged_dps_UI, spell_ranged_dps_UI); // 1 variable(s)
	}
}
