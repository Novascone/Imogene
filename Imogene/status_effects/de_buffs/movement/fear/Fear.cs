using Godot;
using System;

public partial class Fear : StatusEffect
{
	

	public Fear()
	{
		prevents_input = true;
		name = "fear";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		duration = 5;
		max_stacks = 1;
	}

	public Fear(Entity entity)
	{
		prevents_input = true;
		caster = entity;
		name = "fear";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		duration = entity.EntityControllers.status_effect_controller.fear_duration;
		max_stacks = 1;
	}
	

    public override void Apply(Entity entity)
    {
		base.Apply(entity);
        CreateTimerIncrementStack(entity);
		entity.DirectionVector = caster.Transform.Basis.Z * 2;
	
    }

    public override void timer_timeout(Entity entity_)
    {
		if(!removed)
		{
			 Remove(entity_);
		}
      
    }

    public override void Remove(Entity entity)
    {
        base.Remove(entity);
		entity.DirectionVector = Vector3.Zero;
    }
}
