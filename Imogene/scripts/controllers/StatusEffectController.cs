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
	}

	public void AddMovementEffect(StatusEffect effect)
	{
		if(!entity.movement_effects.Contains(effect))
		{
			entity.movement_effects.Add(effect);
			effect.current_stacks += 1;
			GD.Print("current stacks " + effect.current_stacks);
			if(entity is Enemy enemy)
			{
				enemy.movement_controller.SpeedAltered();
			}
			
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
}
