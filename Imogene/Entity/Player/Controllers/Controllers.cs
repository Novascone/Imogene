using Godot;
using System;

public partial class Controllers : Node
{
	[Export] public InputController input_controller { get; set; }
	[Export] public MovementController movement_controller { get; set; }
	[Export] public AbilityAssigner ability_assigner { get; set; }
	[Export] public AbilityController ability_controller { get; set; }
	[Export] public EquipmentController equipment_controller { get; set; }
	[Export] public RayCast3D near_wall { get; set; }
	[Export] public RayCast3D on_wall { get; set; }
	
}
