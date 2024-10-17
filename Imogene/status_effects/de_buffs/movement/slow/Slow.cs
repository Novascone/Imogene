using Godot;
using System;

public partial class Slow : StatusEffect
{
	public StatModifier slow = new(StatModifier.ModificationType.MultiplyCurrent);

	public Slow()
	{
		name = "slow";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		alters_speed = true;
		slow.mod = -0.6f;
		duration = 5;
		max_stacks = 5;
	}
	
	public override void Apply(Entity entity_)
	{
		base.Apply(entity_);
		// GD.Print(entity.Name + " Speed before slow " + entity.speed);
		// GD.Print("Applying slow");
		
		CreateTimerIncrementStack(entity_);
		
		if(entity_.movement_speed.current_value >= entity_.movement_speed.base_value || entity_.movement_speed.current_value == entity_.movement_speed.base_value/2.0f)
		{
			entity_.movement_speed.AddModifier(slow);
		}
		else
		{
		}
		
	}

	public override void timer_timeout(Entity entity_)
    {
		if(!removed)
		{
			if(current_stacks == 1)
			{
				Remove(entity_);
			}
			else
			{
				GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity_);
			}
			if(current_stacks > 0)
			{
				current_stacks -= 1;
			}
			
		}
		
    }

    public override void Remove(Entity entity_)
    {
		if(!removed)
		{
			base.Remove(entity_);
			entity_.movement_speed.RemoveModifier(slow);
		}
       
    }
}
