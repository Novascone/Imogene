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
		slow.Mod = -0.6f;
		duration = 5;
		max_stacks = 5;
	}

	public override void Apply(Entity entity)
	{
		
		base.Apply(entity);
		if(current_stacks < max_stacks - 1)
		{
						
			CreateTimerIncrementStack(entity);
			if(entity.MovementSpeed.CurrentValue >= entity.MovementSpeed.BaseValue)
			{
				entity.MovementSpeed.AddModifier(slow);
			}
			else
			{
			}
		}
		else if(current_stacks == max_stacks - 1 || entity.StatusEffects.Contains(freeze))
		{
			removed_by_freeze = true;
			if(entity.StatusEffects.Contains(this))
			{
				Remove(entity);
			}
		
			entity.MovementSpeed.RemoveModifier(slow);
			freeze = new();
			EmitSignal(nameof(AddAdditionalStatusEffect), freeze);
			current_stacks = 0;
				
		}
		
	}

	public override void timer_timeout(Entity entity)
    {
		if(!removed)
		{
			if(current_stacks == 1 && entity.StatusEffects.Contains(this) && !removed_by_freeze)
			{
				Remove(entity);
			}
			else if(current_stacks > 0 && entity.StatusEffects.Contains(this))
			{
				if(current_stacks > 0)
				{
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
		}
        
    }

    public override void Remove(Entity entity_)
    {
        base.Remove(entity_);
		entity_.MovementSpeed.RemoveModifier(slow);		
    }
}
