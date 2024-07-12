using Godot;
using System;

public partial class DexterityStat : Stat
{
	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		base._Ready();
		set_info_text =  " Dexterity {0}  \n * Primary stat for ranged damage\n * Increases damage by {1} \n * Increases critical chance by {2}";
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
		info_text.Text = string.Format(set_info_text, stat_number, 0, 0);
		
	}

}
