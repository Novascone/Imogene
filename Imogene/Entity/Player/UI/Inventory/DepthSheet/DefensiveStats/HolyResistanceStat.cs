using Godot;
using System;

public partial class HolyResistanceStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
		// info.info.Text =  " Holy Resistance {0} \n * Reduces incoming holy damage \n * Increased by skills and gear ";
	}
}
