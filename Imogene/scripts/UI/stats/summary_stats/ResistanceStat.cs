using Godot;
using System;

public partial class ResistanceStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Resistance {0} \n * Total damage the character can resist \n * Calculated by health, armor, resistances";
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
		//resistance_info.Text = string.Format(resistance_info_text, resistance_UI); // 1 variable(s)
	}
}
