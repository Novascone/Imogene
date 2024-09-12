using Godot;
using System;

public partial class VitalityStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  " Vitality {0} \n * Primary stat for health \n * Increases health points by {1}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
