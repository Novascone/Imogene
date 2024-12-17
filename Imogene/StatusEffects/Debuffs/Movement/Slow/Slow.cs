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
		slow.Mod = -0.6f;
		duration = 5;
		max_stacks = 5;
	}
	
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		// GD.Print(entity.Name + " Speed before slow " + entity.speed);
		// GD.Print("Applying slow");
		
		CreateTimerIncrementStack(entity);
		
		if(entity.MovementSpeed.CurrentValue >= entity.MovementSpeed.BaseValue || entity.MovementSpeed.CurrentValue == entity.MovementSpeed.BaseValue/2.0f)
		{
			entity.MovementSpeed.AddModifier(slow);
		}
		else
		{
		}
		
	}

	public override void timer_timeout(Entity entity)
    {
		if(!removed)
		{
			if(current_stacks == 1)
			{
				Remove(entity);
			}
			else
			{
				GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
			}
			if(current_stacks > 0)
			{
				current_stacks -= 1;
			}
			
		}
		
    }

    public override void Remove(Entity entity)
    {
		if(!removed)
		{
			base.Remove(entity);
			entity.MovementSpeed.RemoveModifier(slow);
		}
       
    }
}
