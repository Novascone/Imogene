using Godot;
using System;

public partial class VitalityStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Vitality {0} \n * Primary stat for health \n * Increases health points by {1}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void GetStatInfo(string stat_value)
	{
		stat_number = stat_value;
		value.Text = stat_value;
		GD.Print("stat number " + stat_number);
		info_text.Text = string.Format(set_info_text, stat_number, 2 * stat_value.ToInt());
		// vitality_info.Text = string.Format(vitality_info_text, vitality_UI, 2 * vitality_UI.ToInt()); // 2 variable(s)
	}
	
}
