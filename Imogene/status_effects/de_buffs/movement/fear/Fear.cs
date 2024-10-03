using Godot;
using System;

public partial class Fear : StatusEffect
{
	

	public Fear()
	{
		prevents_input = true;
		name = "fear";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		duration = 5;
		max_stacks = 1;
	}

	public Fear(Entity entity)
	{
		prevents_input = true;
		caster = entity;
		name = "fear";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		duration = entity.entity_controllers.status_effect_controller.fear_duration;
		max_stacks = 1;
	}
	

    public override void Apply(Entity entity)
    {
		base.Apply(entity);
        CreateTimerIncrementStack(entity);
		entity._direction = caster.Transform.Basis.Z * 2;
	
    }

    public override void timer_timeout(Entity entity)
    {
		if(!removed)
		{
			 Remove(entity);
		}
      
    }

    public override void Remove(Entity entity)
    {
        base.Remove(entity);
		entity._direction = Vector3.Zero;
    }
}
