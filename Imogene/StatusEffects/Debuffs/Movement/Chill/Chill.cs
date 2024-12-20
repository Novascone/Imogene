using Godot;
using System;

public partial class Chill : StatusEffect
{
	private Freeze Freeze { get; set; } = null;
	public bool RemovedByFreeze { get; set; } = false;
	public StatModifier Slow { get; set; } = new(StatModifier.ModificationType.MultiplyCurrent);

	public Chill()
	{
		EffectName = "chill";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		AltersSpeed = true;
		AddsAdditionalEffects = true;
		Slow.Mod = -0.6f;
		Duration = 5;
		MaxStacks = 5;
	}

	public override void Apply(Entity entity)
	{
		
		base.Apply(entity);
		if(CurrentStacks < MaxStacks - 1)
		{
						
			CreateTimerIncrementStack(entity);
			if(entity.MovementSpeed.CurrentValue >= entity.MovementSpeed.BaseValue)
			{
				entity.MovementSpeed.AddModifier(Slow);
			}
			else
			{
			}
		}
		else if(CurrentStacks == MaxStacks - 1 || entity.StatusEffects.Contains(Freeze))
		{
			RemovedByFreeze = true;
			if(entity.StatusEffects.Contains(this))
			{
				Remove(entity);
			}
		
			entity.MovementSpeed.RemoveModifier(Slow);
			Freeze = new();
			EmitSignal(nameof(AddAdditionalStatusEffect), Freeze);
			CurrentStacks = 0;
				
		}
		
	}

	public override void TimerTimeout(Entity entity)
    {
		if(!Removed)
		{
			if(CurrentStacks == 1 && entity.StatusEffects.Contains(this) && !RemovedByFreeze)
			{
				Remove(entity);
			}
			else if(CurrentStacks > 0 && entity.StatusEffects.Contains(this))
			{
				if(CurrentStacks > 0)
				{
					GetTree().CreateTimer(Duration).Timeout += () => TimerTimeout(entity);
				}
			}
			else if(RemovedByFreeze)
			{
				RemovedByFreeze = false;
			}
			if(CurrentStacks > 0)
			{
				CurrentStacks -= 1;
			}
		}
        
    }

    public override void Remove(Entity entity)
    {
        base.Remove(entity);
		entity.MovementSpeed.RemoveModifier(Slow);		
    }
}
