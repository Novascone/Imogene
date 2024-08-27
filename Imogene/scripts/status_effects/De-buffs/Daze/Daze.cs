using Godot;
using System;

public partial class Daze : StatusEffect
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
		
		if(this_entity == null)
		{
			 this_entity = entity;
		}
		if(GetParent() == null)
		{
			entity.AddChild(this);
		}
		if(current_stacks == 0)
		{
			GD.Print("Dazed");
			current_stacks += 1;
			entity.status_effects.Add(this);
			entity.status_effect_controller.SetEffectBooleans(this);
			GetTree().CreateTimer(duration).Timeout += () => timer_timeout();
		}
		
	}

	private void timer_timeout()
    {
		GD.Print("timer timeout");
     
		this_entity.status_effect_controller.RemoveStatusEffect(this);
		// this_entity.previous_movement_effects_count = this_entity.movement_effects.Count;
		GD.Print("entity can attack");
		// RemoveStatusEffect(this);
		if(current_stacks > 0)
		{
			current_stacks -= 1;
		}
		
		GD.Print("current stacks " + current_stacks);
    }

	
}
