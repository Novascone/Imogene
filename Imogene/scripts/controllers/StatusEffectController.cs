using Godot;
using System;

public partial class StatusEffectController : Controller
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(entity is Enemy enemy)
		{
			if(enemy.movement_controller.movement_stopped_check && enemy.movement_controller.speed_altered_check)
			{
				
				UpdateMovementEffectsCount();
			}
		}
	}

	public void AddMovementEffect(StatusEffect effect)
	{
		if(!entity.movement_effects.Contains(effect))
		{
			entity.movement_effects.Add(effect);
			effect.current_stacks += 1;
			GD.Print("current stacks " + effect.current_stacks);
			
		}
		else if (effect.current_stacks < effect.max_stacks)
		{
			effect.current_stacks += 1;
			GD.Print("current stacks " + effect.current_stacks);
		}
		else
		{
			GD.Print("Can not add more stacks");
		}
	}

	public void RemoveMovementEffect(StatusEffect effect)
	{
		entity.movement_effects.Remove(effect);
	}

	public void UpdateMovementEffectsCount()
	{
		if(entity.movement_effects.Count > entity.previous_movement_effects_count)
		{
			GD.Print("Incrementing");
			entity.previous_movement_effects_count = entity.movement_effects.Count;
		}
		
	}
}
