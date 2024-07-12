using Godot;
using System;

public partial class StrengthStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Strength {0} \n * Primary stat for melee damage \n * Increases damage by {1} \n * Increases health by {2} \n * Increases critical hit damage by {3}";
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
		info_text.Text = string.Format(set_info_text, stat_number, 0, stat_number, 0);
	}
	
}
