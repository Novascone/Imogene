using Godot;
using System;

public partial class ArmorStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Total Armor {0} \n * Reduces incoming damage by {1} \n * Increased by gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void GetStatInfo(string stat_value_ui)
	{
		stat_value = stat_value_ui;
		value.Text = stat_value;
		info_text.Text = string.Format(set_info_text, stat_value,0);
		//armor_info.Text = string.Format(armor_info_text, 0, 0);
	}
}
