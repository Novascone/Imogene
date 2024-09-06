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
		if(enemy.entity_controllers.status_effect_controller.frozen || enemy.entity_controllers.status_effect_controller.stunned || enemy.entity_controllers.status_effect_controller.hamstrung || enemy.entity_controllers.status_effect_controller.hexed)
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
		if (enemy.entity_controllers.status_effect_controller.on_fire || enemy.entity_controllers.status_effect_controller.stealth || enemy.entity_controllers.status_effect_controller.transpose || enemy.entity_controllers.status_effect_controller.bull || enemy.entity_controllers.status_effect_controller.slowed || enemy.entity_controllers.status_effect_controller.chilled)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	
}
