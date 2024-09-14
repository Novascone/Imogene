using Godot;
using System;

public partial class RecoveryStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = Name + ":";
		// info.info.Text =  " Recovery {0} \n * The average of health regen, resource regen, and posture regen \n * Calculated by stamina ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
