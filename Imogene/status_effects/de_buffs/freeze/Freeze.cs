using Godot;
using System;

public partial class Freeze : StatusEffect
{

	public StatModifier stop = new(StatModifier.ModificationType.nullify);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		prevents_movement = true;
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
		
		if(current_stacks == 0)
		{
			current_stacks += 1;
			GD.Print("Frozen");
			entity.movement_speed.AddModifier(stop);
			GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
			entity.entity_controllers.status_effect_controller.SetEffectBooleans(this);
		}
		
	}

	private void timer_timeout(Entity entity)
    {
		GD.Print("timer timeout");
		current_stacks -= 1;
		entity.entity_controllers.status_effect_controller.RemoveStatusEffect(entity, this);
		entity.movement_speed.RemoveModifier(stop);
		// this_entity.previous_movement_effects_count = this_entity.movement_effects.Count;
		GD.Print("entity is no longer frozen");
		// RemoveStatusEffect(this);
		GD.Print("current stacks of " + this.Name + " " + current_stacks);
    }

}
