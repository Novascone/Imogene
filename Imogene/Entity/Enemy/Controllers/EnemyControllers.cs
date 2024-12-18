using Godot;
using System;

public partial class EnemyControllers : Node
{
	[Export] public EnemyMovementController MovementController { get; set; }
	[Export] public EnemyAbilityController AbilityController { get; set; }
	[Export] public Node3D RayPosition { get; set; }
	[Export] public StateMachine StateMachine { get; set; }
}
