using Godot;
using System;

public partial class RecoveryStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Recovery {0} \n * The average of health regen, resource regen, and posture regen \n * Calculated by stamina ";
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
		//recovery_info.Text = string.Format(recovery_info_text, recovery_UI); // 1 variable(s)
	}
}
