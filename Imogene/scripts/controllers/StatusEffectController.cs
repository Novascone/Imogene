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

	public void AddStatusEffect(StatusEffect effect)
	{
		if(!entity.status_effects.Contains(effect))
		{
			entity.status_effects.Add(effect);
		}
		else if (effect.current_stacks < effect.max_stacks)
		{
			effect.current_stacks += 1;
		}
	}
}
