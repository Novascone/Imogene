using Godot;
using System;

public partial class Fear : StatusEffect
{
	

	public Fear()
	{
		PreventsInput = true;
		EffectName = "fear";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		Duration = 5;
		MaxStacks = 1;
	}

	public Fear(Entity entity)
	{
		PreventsInput = true;
		Caster = entity;
		EffectName = "fear";
		Type = EffectType.Debuff;
		Category = EffectCategory.Movement;
		Duration = entity.EntityControllers.EntityStatusEffectsController.EntityFearDuration;
		MaxStacks = 1;
	}
	

    public override void Apply(Entity entity)
    {
		base.Apply(entity);
        CreateTimerIncrementStack(entity);
		entity.DirectionVector = Caster.Transform.Basis.Z * 2;
	
    }

    public override void TimerTimeout(Entity entity)
    {
		if(!Removed)
		{
			 Remove(entity);
		}
      
    }

    public override void Remove(Entity entity)
    {
        base.Remove(entity);
		entity.DirectionVector = Vector3.Zero;
    }
}
