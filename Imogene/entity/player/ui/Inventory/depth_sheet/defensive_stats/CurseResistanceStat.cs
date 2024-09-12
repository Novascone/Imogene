using Godot;
using System;

public partial class CurseResistanceStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Curse Resistance {0} \n * Reduces curse effectiveness \n * Increased by skills and gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
