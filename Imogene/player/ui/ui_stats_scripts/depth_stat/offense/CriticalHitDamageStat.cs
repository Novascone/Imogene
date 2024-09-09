using Godot;
using System;

public partial class CriticalHitDamageStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Critical hit damage {0} \n * Multiplier applied to base damage if a hit is critical \n * Increased by skills and gear  ";
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
		//critical_hit_damage_info.Text = string.Format(critical_hit_damage_text, critical_hit_damage_UI);
	}
}
