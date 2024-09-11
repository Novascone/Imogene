using Godot;
using System;

public partial class ColdResistanceStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		// info.info.Text =  " Cold Resistance {0} \n * Reduces incoming cold damage \n * Increased by skills and gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
