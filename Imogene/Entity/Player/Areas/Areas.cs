using Godot;
using System;

public partial class Areas : Node
{
	// [Export] public Area3D vision;
	[Export] public Area3D interact { get; set; }
	[Export] public Area3D pickup_items { get; set; }
	[Export] public Area3D near { get; set; }
	[Export] public Area3D far { get; set; }

}
