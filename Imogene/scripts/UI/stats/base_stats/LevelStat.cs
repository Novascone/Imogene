using Godot;
using System;

public partial class LevelStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		set_info_text =  " Level {0} \n * Level of character";
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
		//level_info.Text = string.Format(level_info_text, level_UI); // 1 variable(s)
	}
}
