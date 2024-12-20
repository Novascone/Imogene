using Godot;
using System;

public partial class Slow : StatusEffect
{
	public StatModifier SlowModifier = new(StatModifier.ModificationType.MultiplyCurrent);

	public Slow()
	{
		EffectName = "slow";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		AltersSpeed = true;
		SlowModifier.Mod = -0.6f;
		Duration = 5;
		MaxStacks = 5;
	}
	
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		// GD.Print(entity.Name + " Speed before slow " + entity.speed);
		// GD.Print("Applying slow");
		
		CreateTimerIncrementStack(entity);
		
		if(entity.MovementSpeed.CurrentValue >= entity.MovementSpeed.BaseValue || entity.MovementSpeed.CurrentValue == entity.MovementSpeed.BaseValue/2.0f)
		{
			entity.MovementSpeed.AddModifier(SlowModifier);
		}
		else
		{
		}
		
	}

	public override void TimerTimeout(Entity entity)
    {
		if(!Removed)
		{
			if(CurrentStacks == 1)
			{
				Remove(entity);
			}
			else
			{
				GetTree().CreateTimer(Duration).Timeout += () => TimerTimeout(entity);
			}
			if(CurrentStacks > 0)
			{
				CurrentStacks -= 1;
			}
			
		}
		
    }

    public override void Remove(Entity entity)
    {
		if(!Removed)
		{
			base.Remove(entity);
			entity.MovementSpeed.RemoveModifier(SlowModifier);
		}
       
    }
}
