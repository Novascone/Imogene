using Godot;
using System;

public partial class VitalityStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  " Vitality {0} \n * Primary stat for health \n * Increases health points by {1}";
	}

	public override void GetStatInfo(float stat_value_ui_)
	{
		stat_value = stat_value_ui_.ToString();
		value.Text = stat_value;
		info.tool_tip.Text = string.Format(info.tool_tip.Text, stat_value, stat_value);
	}

}
