using Godot;
using System;

public partial class BlockAmountStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Block amount {0} \n * Amount of damage that can be blocked by weapon/shield ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
