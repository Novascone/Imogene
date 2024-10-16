using Godot;
using Godot.NativeInterop;
using System;

public partial class LevelStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  " Level {0} \n * Level of character";
	}

}
