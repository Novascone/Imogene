using Godot;
using System;

public partial class Controllers : Node
{
	[Export] public InputController InputController { get; set; }
	[Export] public MovementController MovementController { get; set; }
	[Export] public AbilityAssigner AbilityAssigner { get; set; }
	[Export] public AbilityController AbilityController { get; set; }
	[Export] public EquipmentController EquipmentController { get; set; }
	[Export] public RayCast3D NearWall { get; set; }
	[Export] public RayCast3D OnWall { get; set; }
	
}
