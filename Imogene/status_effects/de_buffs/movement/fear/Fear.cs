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

	public Fear(Entity entity_)
	{
		prevents_input = true;
		caster = entity_;
		name = "fear";
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		duration = entity_.entity_controllers.status_effect_controller.fear_duration;
		max_stacks = 1;
	}
	

    public override void Apply(Entity entity_)
    {
		base.Apply(entity_);
        CreateTimerIncrementStack(entity_);
		entity_._direction = caster.Transform.Basis.Z * 2;
	
    }

    public override void timer_timeout(Entity entity_)
    {
		if(!removed)
		{
			 Remove(entity_);
		}
      
    }

    public override void Remove(Entity entity_)
    {
        base.Remove(entity_);
		entity_._direction = Vector3.Zero;
    }
}
