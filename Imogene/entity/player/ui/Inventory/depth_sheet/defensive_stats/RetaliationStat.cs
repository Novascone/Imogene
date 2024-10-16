using Godot;
using System;

public partial class RetaliationStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = SeparateByCapitals(Name);
		info.tool_tip.Text =  " Retaliation {0} \n * Increases the amount of time you have to retaliate after being hit ";
	}

}
