using Godot;
using System;

public partial class DamageStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		// info.info.Text =  "   Damage {0} \n * Total damage per second done by character by the average of power modifiers \n * Physical melee dps {1} \n * spell melee dps {2} \n * Physical ranged dps {3} \n * spell ranged dps {4} \n These 4 catagories are your dps when using a skill of the respective type. \n * Combination of power modifiers melee and ranged, in both their physical and spell forms, \n     damage, attack speed, critical hit chance, and critical hit damage";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GetStatInfo(string stat_value_ui, string p_m_dps, string s_m_dps, string p_r_dps, string s_r_dps)
	{
		stat_value = stat_value_ui;
		value.Text = stat_value;
		// info.info.Text = string.Format(info.info.Text, stat_value, p_m_dps, s_m_dps, p_r_dps, s_r_dps);
		// damage_info.Text = string.Format(damage_info_text, damage_UI, physical_melee_dps_UI, spell_melee_dps_UI, physical_ranged_dps_UI, spell_ranged_dps_UI); // 1 variable(s)
	}
}
