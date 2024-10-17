using Godot;
using System;

public partial class Chill : StatusEffect
{
	private Freeze freeze { get; set; } = null;
	public bool removed_by_freeze { get; set; } = false;
	public StatModifier slow { get; set; } = new(StatModifier.ModificationType.MultiplyCurrent);

	public Chill()
	{
		name = "chill";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		alters_speed = true;
		adds_additional_effects = true;
		slow.mod = -0.6f;
		duration = 5;
		max_stacks = 5;
	}

	public override void Apply(Entity entity_)
	{
		
		base.Apply(entity_);
		
		if(current_stacks < max_stacks - 1)
		{
						
			CreateTimerIncrementStack(entity_);
			if(entity_.movement_speed.current_value >= entity_.movement_speed.base_value)
			{
				entity_.movement_speed.AddModifier(slow);
			}
			else
			{
			}
		}
		else if(current_stacks == max_stacks - 1 || entity_.status_effects.Contains(freeze))
		{
			removed_by_freeze = true;
			if(entity_.status_effects.Contains(this))
			{
				Remove(entity_);
			}
		
			entity_.movement_speed.RemoveModifier(slow);
			freeze = new();
			EmitSignal(nameof(AddAdditionalStatusEffect), freeze);
			current_stacks = 0;
				
		}
		
	}

	public override void timer_timeout(Entity entity_)
    {
		if(!removed)
		{
			if(current_stacks == 1 && entity_.status_effects.Contains(this) && !removed_by_freeze)
			{
				Remove(entity_);
			}
			else if(current_stacks > 0 && entity_.status_effects.Contains(this))
			{
				if(current_stacks > 0)
				{
					GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity_);
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
		}
        
    }

    public override void Remove(Entity entity_)
    {
        base.Remove(entity_);
		entity_.movement_speed.RemoveModifier(slow);		
    }
}
