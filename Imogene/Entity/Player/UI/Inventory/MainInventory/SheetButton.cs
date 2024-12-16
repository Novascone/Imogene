using Godot;
using System;

public partial class SheetButton : GenericInventoryButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text = "Sheet \n * Click to view depth stats ";
	}

}
