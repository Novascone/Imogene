using Godot;
using System;

public partial class DexterityStat : UIStat
{
	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  " Dexterity {0}  \n * Primary stat for ranged damage\n * Increases damage by {1} \n * Increases critical chance by {2}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
