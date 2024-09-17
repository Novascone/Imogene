using Godot;
using System;

public partial class Chill : StatusEffect
{
	private PackedScene freeze_scene;
	private Freeze freeze;
	
	public bool removed_by_freeze;
	public StatModifier slow = new(StatModifier.ModificationType.multiply_current);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		slow.mod = -0.6f;
		duration = 5;
		effect_type = "movement";
		max_stacks = 5;
		freeze_scene = GD.Load<PackedScene>("res://status_effects/de_buffs/freeze/freeze.tscn");
		freeze = (Freeze)freeze_scene.Instantiate();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}

	public override void Apply(Entity entity)
	{
		

		
		if(current_stacks < max_stacks - 1 && !entity.status_effects.Contains(freeze))
		{
			
			GD.Print("Applying Chill");
			
			if(current_stacks == 0)
			{
				GD.Print("creating timer");
				GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
				entity.entity_controllers.status_effect_controller.SetEffectBooleans(this);
				GD.Print("setting booleans via apply");
			}
			current_stacks += 1;
			
			if(entity.movement_speed.current_value >= entity.movement_speed.base_value)
			{
				entity.movement_speed.AddModifier(slow);
			}
			else
			{
				GD.Print("speed has already been decreased");
			}
			
			
		}
		else if(current_stacks == max_stacks - 1 || entity.status_effects.Contains(freeze))
		{
			GD.Print("Trying to apply freeze");
			removed_by_freeze = true;
			// freeze.Apply(entity);
			if(entity.status_effects.Contains(this))
			{
				entity.entity_controllers.status_effect_controller.RemoveStatusEffect(entity, this);
				
			}
		
			entity.movement_speed.RemoveModifier(slow);
			entity.entity_controllers.status_effect_controller.AddStatusEffect(entity, freeze);
			current_stacks = 0;
				
		}
		
	}

	private void timer_timeout(Entity entity)
    {
		GD.Print("timer timeout chill");
        if(current_stacks == 1 && entity.status_effects.Contains(this) && !removed_by_freeze)
		{
			entity.movement_speed.RemoveModifier(slow);
			entity.entity_controllers.status_effect_controller.RemoveStatusEffect(entity, this);
			GD.Print("removing booleans from chill via timer");
			// this_entity.previous_movement_effects_count = this_entity.movement_effects.Count;
			GD.Print("entity speed reset to " + entity.movement_speed.current_value);
			// RemoveStatusEffect(this);
		}
		else if(current_stacks > 0 && entity.status_effects.Contains(this))
		{
			if(current_stacks > 0 )
			{
				GD.Print("creating another timer");
				GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
			}
		}
		else if(removed_by_freeze)
		{
			removed_by_freeze = false;
		}
		if(current_stacks > 0)
		{
			current_stacks -= 1;
		}
		
		
		GD.Print("current stacks of "  + this.Name + " " + current_stacks);
    }
}
