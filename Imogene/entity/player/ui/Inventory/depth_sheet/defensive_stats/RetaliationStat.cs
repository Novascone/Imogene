using Godot;
using System;

public partial class RetaliationStat : Stat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Retaliation {0} \n * Increases the amount of time you have to retaliate after being hit ";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
