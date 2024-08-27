using Godot;
using System;

public partial class Freeze : StatusEffect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
			current_stacks += 1;
			entity.status_effects.Add(this);
			GD.Print("Frozen");
			GetTree().CreateTimer(duration).Timeout += () => timer_timeout();
			entity.status_effect_controller.SetEffectBooleans(this);
		}
		
	}

	private void timer_timeout()
    {
		GD.Print("timer timeout");
		current_stacks -= 1;
		this_entity.status_effect_controller.RemoveStatusEffect(this);
		// this_entity.previous_movement_effects_count = this_entity.movement_effects.Count;
		GD.Print("entity is no longer frozen");
		// RemoveStatusEffect(this);
		GD.Print("current stacks of " + this.Name + " " + current_stacks);
    }

}
