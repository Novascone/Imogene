using Godot;
using System;

public partial class Daze : StatusEffect
{
	
	public Daze()
	{
		EffectName = "daze";
		PreventsAbilities = true;
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		Duration = 5;
		MaxStacks = 1;
	}

	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		CreateTimerIncrementStack(entity);
	}

	public override void TimerTimeout(Entity entity)
    {
		Remove(entity);
    }

	
}
