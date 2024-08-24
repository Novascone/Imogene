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
		GD.Print("Enemy movement controller loaded");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(entity.movement_effects.Count > entity.previous_movement_effects_count)
		{
			movement_check_completed = false;
		}
		else
		{
			movement_check_completed = true;
		}
		CanMove();
		SpeedAltered();
		
	}

	public bool CanMove()
	{
		if(entity.movement_effects.Count > entity.previous_movement_effects_count)
		{
			movement_stopped_check = true;
			foreach(StatusEffect effect in entity.movement_effects)
			{
				if (effect.resource.prevents_movement)
				{
					GD.Print("Movement disabled");
					return false;
				}
				else
				{
					return true;
				}
			}
		}
		return true;
		
	}

	public bool SpeedAltered()
	{
		if(entity.movement_effects.Count > entity.previous_movement_effects_count)
		{
			speed_altered_check = true;
			foreach(StatusEffect effect in entity.movement_effects)
			{
				if (effect.resource.alters_speed)
				{
					GD.Print("Speed is being altered by movement effect");
					return true;
				}
				else 
				{
					return false;
				}
			}
		}

		return false;
	}

	
}
