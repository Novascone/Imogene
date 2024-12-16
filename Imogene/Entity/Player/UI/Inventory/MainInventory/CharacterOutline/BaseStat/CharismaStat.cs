using Godot;
using System;

public partial class CharismaStat : UIStat
{
	public override void _Ready()
	{
		base._Ready();
		label.Text = Name + ":";
		info.tool_tip.Text =  "  Charisma {0} \n * Primary stat for character interaction \n * Increases special interactions";
	}

}
