using Godot;
using System;

public partial class WisdomStat : UIStat
{
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  " Wisdom {0} \n * Primary stat for interaction ";
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
