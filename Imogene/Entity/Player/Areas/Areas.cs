using Godot;
using System;

public partial class Areas : Node
{
	// [Export] public Area3D vision;
	[Export] public Area3D Interact { get; set; }
	[Export] public Area3D PickUpItems { get; set; }
	[Export] public Area3D Near { get; set; }
	[Export] public Area3D Far { get; set; }

}
