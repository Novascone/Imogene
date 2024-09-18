using Godot;
using System;

public partial class Stun : StatusEffect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// base._Ready();
		duration = 5;
		effect_type = "movement";
		max_stacks = 1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}

	public override void Apply(Entity entity)
	{
		
		
		CreateTimerIncrementStack(entity);
		
	}

	public override void timer_timeout(Entity entity)
    {
		GD.Print("timer timeout");
     
		EmitSignal(nameof(StatusEffectFinished));
		// this_entity.previous_movement_effects_count = this_entity.movement_effects.Count;
		GD.Print("entity can now move ");
		// RemoveStatusEffect(this);
		if(current_stacks > 0)
		{
			current_stacks -= 1;
		}
		
		GD.Print("current stacks " + current_stacks);
    }
}
