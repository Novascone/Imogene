using Godot;
using System;

public partial class EnemyMovementController : Controller
{
	public bool speed_altered_check;
	public bool movement_stopped_check;
	public bool movement_check_completed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GD.Print("Enemy movement controller loaded");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(StatusEffectsPreventingMovement())
		{
			// GD.Print(entity.Name + " can't move");
		}
		
		
	}

	public bool StatusEffectsPreventingMovement()
	{
		if(entity.status_effect_controller.frozen || entity.status_effect_controller.stunned || entity.status_effect_controller.hamstrung || entity.status_effect_controller.hexed)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool StatusEffectsAffectingSpeed()
	{
		if (entity.status_effect_controller.on_fire || entity.status_effect_controller.stealth || entity.status_effect_controller.transpose || entity.status_effect_controller.bull || entity.status_effect_controller.slowed || entity.status_effect_controller.chilled)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	
}
