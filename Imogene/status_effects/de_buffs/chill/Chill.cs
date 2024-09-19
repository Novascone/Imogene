using Godot;
using System;

public partial class Chill : StatusEffect
{
	private PackedScene freeze_scene;
	private Freeze freeze;
	
	public bool removed_by_freeze;
	public StatModifier slow = new(StatModifier.ModificationType.multiply_current);

	public Chill()
	{
		name = "chill";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		alters_speed = true;
		adds_additional_effects = true;
		slow.mod = -0.6f;
		duration = 5;
		max_stacks = 5;
		freeze_scene = GD.Load<PackedScene>("res://status_effects/de_buffs/freeze/freeze.tscn");
		freeze = (Freeze)freeze_scene.Instantiate();
	}

	public override void Apply(Entity entity)
	{
		
		base.Apply(entity);
		
		if(current_stacks < max_stacks - 1)
		{
			
			GD.Print("Applying Chill");
			
			CreateTimerIncrementStack(entity);
			
			
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
				Remove(entity);
			}
		
			entity.movement_speed.RemoveModifier(slow);
			// entity.entity_controllers.status_effect_controller.AddStatusEffect(entity, freeze);
			EmitSignal(nameof(AddAdditionalStatusEffect), freeze);
			current_stacks = 0;
				
		}
		
	}

	public override void timer_timeout(Entity entity)
    {
		GD.Print("timer timeout chill");
		if(!removed)
		{
			if(current_stacks == 1 && entity.status_effects.Contains(this) && !removed_by_freeze)
			{
				Remove(entity);
			}
			else if(current_stacks > 0 && entity.status_effects.Contains(this))
			{
				if(current_stacks > 0)
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

    public override void Remove(Entity entity)
    {
        base.Remove(entity);
		entity.movement_speed.RemoveModifier(slow);
		GD.Print("removing booleans from chill via timer");
		GD.Print("entity speed reset to " + entity.movement_speed.current_value);
		
    }
}
