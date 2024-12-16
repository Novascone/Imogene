using Godot;
using System;

public partial class EnemyControllers : Node
{
	[Export] public EnemyMovementController movement_controller { get; set; }
	[Export] public EnemyAbilityController ability_controller { get; set; }
	[Export] public Node3D ray_position { get; set; }
	[Export] public StateMachine state_machine { get; set; }
}
