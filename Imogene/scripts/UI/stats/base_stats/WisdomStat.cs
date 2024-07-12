using Godot;
using System;

public partial class WisdomStat : Stat
{
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Wisdom {0} \n * Primary stat for interaction ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void GetStatInfo(string stat_value_ui)
	{
		stat_value = stat_value_ui;
		value.Text = stat_value;
		info_text.Text = string.Format(set_info_text, stat_value, 0);
		// wisdom_info.Text = string.Format(wisdom_info_text, wisdom_UI, 0); // 2 variable(s)
	}
}
