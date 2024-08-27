using Godot;
using System;

public partial class Slow : StatusEffect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		duration = 5;
		duration_timer.WaitTime = duration;
		effect_type = "movement";
		max_stacks = 5;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}

	public override void Apply(Entity entity)
	{
		
		if(this_entity == null)
		{
			 this_entity = entity;
		}
		GD.Print(entity.Name + " Speed before slow " + entity.speed);
		GD.Print("Applying slow");
		if(GetParent() == null)
		{
			entity.AddChild(this);
		}
		if(current_stacks == 0)
		{
			GetTree().CreateTimer(duration).Timeout += () => timer_timeout();
			entity.status_effects.Add(this);
			entity.status_effect_controller.SetEffectBooleans(this);
		}
		current_stacks += 1;
		
		if(entity.speed >= entity.walk_speed)
		{
			entity.speed *= 0.7f;
		}
		else
		{
			GD.Print("speed has already been decreased");
		}
		
		GD.Print(entity.Name + " Speed after slow " + entity.speed);
	}

	private void timer_timeout()
    {
		GD.Print("timer timeout");
        if(current_stacks == 1)
		{
			this_entity.speed = this_entity.walk_speed;
			this_entity.status_effect_controller.RemoveStatusEffect(this);
			// this_entity.previous_movement_effects_count = this_entity.movement_effects.Count;
			GD.Print("entity speed reset to " + this_entity.speed);
			// RemoveStatusEffect(this);
		}
		else
		{
			GD.Print("creating another timer");
			GetTree().CreateTimer(duration).Timeout += () => timer_timeout();
		}
		if(current_stacks > 0)
		{
			current_stacks -= 1;
		}
		
		GD.Print("current stacks " + current_stacks);
    }
}
