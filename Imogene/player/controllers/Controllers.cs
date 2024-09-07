using Godot;
using System;

public partial class Controllers : Node
{
	[Export] public InputController input_controller;
	[Export] public MovementController movement_controller;
	[Export] public AbilityAssigner ability_assigner;
	[Export] public AbilityController ability_controller;
	// [Export] public StatController stat_controller;
	[Export] public EquipmentController equipment_controller;
	[Export] public RayCast3D near_wall;
	[Export] public RayCast3D on_wall;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
