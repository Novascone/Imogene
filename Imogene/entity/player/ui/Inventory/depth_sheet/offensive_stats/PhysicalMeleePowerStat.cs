using Godot;
using System;
using System.Text;


public partial class PhysicalMeleePowerStat : Stat
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Physical melee power {0} \n * Increases physical melee DPS by 1 every 15 points \n * +2 for every point of strength +1 for every point of dexterity \n * Bonuses obtainable on gear ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
