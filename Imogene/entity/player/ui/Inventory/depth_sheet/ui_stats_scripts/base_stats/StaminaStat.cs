using Godot;
using System;

public partial class StaminaStat : UIStat
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  "  Stamina {0} \n * Primary stat for resource and regeneration \n * Increases health and resource regeneration by {1} \n * Increases health by {2}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
}
