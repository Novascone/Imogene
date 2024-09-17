using Godot;
using System;

public partial class Slow : StatusEffect
{
	public StatModifier slow = new(StatModifier.ModificationType.multiply_current);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		slow.mod = -0.6f;
		duration = 5;
		effect_type = "movement";
		max_stacks = 5;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public override void Apply(Entity entity)
	{

		// GD.Print(entity.Name + " Speed before slow " + entity.speed);
		// GD.Print("Applying slow");
		
		if(current_stacks == 0)
		{
			GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
			entity.entity_controllers.status_effect_controller.SetEffectBooleans(this);
			GD.Print("Adding slow to " + entity.Name);
		}
		current_stacks += 1;
		
		if(entity.movement_speed.current_value >= entity.movement_speed.base_value)
		{
			GD.Print("Adding slow modifier");
			entity.movement_speed.AddModifier(slow);
		}
		else
		{
			GD.Print("speed has already been decreased");
		}
		
		GD.Print(entity.Name + " Speed after slow " + entity.movement_speed.current_value);
	}

	private void timer_timeout(Entity entity)
    {
		GD.Print("timer timeout");
        if(current_stacks == 1)
		{
			entity.movement_speed.RemoveModifier(slow);
			GD.Print("removing slow from " + entity.Name);
			entity.entity_controllers.status_effect_controller.RemoveStatusEffect(entity, this);
			// this_entity.previous_movement_effects_count = this_entity.movement_effects.Count;
			GD.Print("entity speed reset to " + entity.movement_speed.current_value);
			// RemoveStatusEffect(this);
		}
		else
		{
			GD.Print("creating another timer");
			GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
		}
		if(current_stacks > 0)
		{
			current_stacks -= 1;
			GD.Print("decrementing stacks");
		}
		
		GD.Print("current stacks of slow " + current_stacks);
    }
}
