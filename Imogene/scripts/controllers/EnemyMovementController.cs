using Godot;
using System;

public partial class EnemyMovementController : Controller
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Enemy movement controller loaded");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		CanMove();
		SpeedAltered();
	}

	public bool CanMove()
	{
		
		if(entity.movement_effects.Count > entity.previous_movement_effects_count)
		{
			entity.previous_damage_effects_count = entity.movement_effects.Count;
			foreach(StatusEffect effect in entity.movement_effects)
			{
				if (effect.resource.prevents_movement)
				{
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
			entity.previous_movement_effects_count = entity.movement_effects.Count;
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
