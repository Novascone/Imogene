using Godot;
using System;

public partial class ReputationButton : GenericInventoryButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		info.tool_tip.Text = "Reputation \n * Click to view reputation";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
