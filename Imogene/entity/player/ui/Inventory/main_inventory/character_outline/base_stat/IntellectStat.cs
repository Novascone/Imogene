using Godot;
using System;

public partial class IntellectStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  "  Intellect {0} \n * main stat for spell damage \n * Increases spell damage by {1} \n * Increases spell hit chance by {2}";
	}

	public override void GetStatInfo(float stat_value_ui)
	{
		GD.Print("dexterity info");
		stat_value = stat_value_ui.ToString();
		value.Text = stat_value;
		info.tool_tip.Text = string.Format(info.tool_tip.Text, stat_value, stat_value, stat_value);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
