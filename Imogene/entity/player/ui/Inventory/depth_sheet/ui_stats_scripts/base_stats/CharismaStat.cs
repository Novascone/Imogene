using Godot;
using System;

public partial class CharismaStat : Stat
{
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  "  Charisma {0} \n * Primary stat for character interaction \n * Increases special interactions";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
