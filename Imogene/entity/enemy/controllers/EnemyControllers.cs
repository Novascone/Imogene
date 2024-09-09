using Godot;
using System;

public partial class EnemyControllers : Node
{
	[Export] public EnemyMovementController movement_controller;
	[Export] public EnemyAbilityController ability_controller;
	[Export] public Node3D ray_position;
	[Export] public StateMachine state_machine;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
