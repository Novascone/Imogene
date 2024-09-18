using Godot;
using System;

public partial class EnemyMovementController : Node
{
	public bool speed_altered_check;
	public bool movement_stopped_check;
	public bool movement_check_completed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GD.Print("Enemy movement controller loaded");
	}

	
	public bool StatusEffectsPreventingMovement(Enemy enemy)
	{
		if(enemy.entity_controllers.status_effect_controller.movement_prevented)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool StatusEffectsAffectingSpeed(Enemy enemy)
	{
		if (enemy.entity_controllers.status_effect_controller.abilities_prevented)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	
}
