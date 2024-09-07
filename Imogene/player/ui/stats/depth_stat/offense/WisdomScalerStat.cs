using Godot;
using System;

public partial class WisdomScalerStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Wisdom Scaler {0} \n Increases by one for every 20 wisdom \n * Scales how powerful attacks that scale with wisdom are ";
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
		//wisdom_scaler_info.Text = string.Format(wisdom_scaler_info_text, wisdom_scaler_UI);
	}
}
