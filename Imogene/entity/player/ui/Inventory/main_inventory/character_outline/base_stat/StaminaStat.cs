using Godot;
using System;

public partial class StaminaStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  "  Stamina {0} \n * Primary stat for resource and regeneration \n * Increases health and resource regeneration by {1} \n * Increases health by {2}";
	}

	public override void GetStatInfo(float stat_value_ui)
	{
		stat_value = stat_value_ui.ToString();
		value.Text = stat_value;
		info.tool_tip.Text = string.Format(info.tool_tip.Text, stat_value, stat_value, stat_value);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
