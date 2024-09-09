using Godot;
using System;

public partial class Areas : Node
{
	[Export] public Area3D vision;
	[Export] public Area3D interact;
	[Export] public Area3D pick_up_items;
	[Export] public Area3D near;
	[Export] public Area3D far;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
