using Godot;
using System;

public partial class CharismaStat : Stat
{
	public override void _Ready()
	{
		label.Text = Name + ":";
		set_info_text =  "  Charisma {0} \n * Primary stat for character interaction \n * Increases special interactions";
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
		// charisma_info.Text = string.Format(charisma_info_text, charisma_UI); // 1 variable(s)
	}
}
