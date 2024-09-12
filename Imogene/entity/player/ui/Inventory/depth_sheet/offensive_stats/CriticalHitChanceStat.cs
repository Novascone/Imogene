using Godot;
using System;

public partial class CriticalHitChanceStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text = " Critical hit chance {0} \n * Percentage chance for a hit to be a critical hit \n * Increased by skills and gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
