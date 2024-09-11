using Godot;
using System;

public partial class CooldownReductionStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		// info.info.Text =  " Cooldown reduction {0} \n * Cooldown reduction of all skills ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
