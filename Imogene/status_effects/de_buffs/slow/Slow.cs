using Godot;
using System;

public partial class Slow : StatusEffect
{
	public StatModifier slow = new(StatModifier.ModificationType.multiply_current);

	public Slow()
	{
		name = "slow";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		alters_speed = true;
		slow.mod = -0.6f;
		duration = 5;
		max_stacks = 5;
	}
	
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		// GD.Print(entity.Name + " Speed before slow " + entity.speed);
		// GD.Print("Applying slow");
		
		CreateTimerIncrementStack(entity);
		
		if(entity.movement_speed.current_value >= entity.movement_speed.base_value || entity.movement_speed.current_value == entity.movement_speed.base_value/2.0f)
		{
			GD.Print("Adding slow modifier");
			entity.movement_speed.AddModifier(slow);
		}
		else
		{
			GD.Print("speed has already been decreased");
		}
		
		GD.Print(entity.Name + " Speed after slow " + entity.movement_speed.current_value);
	}

	public override void timer_timeout(Entity entity)
    {
		if(!removed)
		{
			GD.Print("timer timeout");
			if(current_stacks == 1)
			{
				Remove(entity);
			}
			else
			{
				GD.Print("creating another timer");
				GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
			}
			if(current_stacks > 0)
			{
				current_stacks -= 1;
				GD.Print("decrementing stacks");
			}
			
			GD.Print("current stacks of slow " + current_stacks);
		}
		
    }

    public override void Remove(Entity entity)
    {
		if(!removed)
		{
			base.Remove(entity);
			entity.movement_speed.RemoveModifier(slow);
			GD.Print("removing slow from " + entity.Name);
			GD.Print("entity speed reset to " + entity.movement_speed.current_value);
		}
       
    }
}
