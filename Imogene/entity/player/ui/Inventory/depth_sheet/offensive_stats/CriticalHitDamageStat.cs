using Godot;
using System;

public partial class CriticalHitDamageStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Critical hit damage {0} \n * Multiplier applied to base damage if a hit is critical \n * Increased by skills and gear  ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
