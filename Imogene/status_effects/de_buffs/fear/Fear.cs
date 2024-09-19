using Godot;
using System;

public partial class Fear : StatusEffect
{
	Vector3 caster_direction;

	public Fear()
	{
		name = "fear";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		duration = 5;
		max_stacks = 1;
	}
	

    public override void Apply(Entity entity)
    {
        CreateTimerIncrementStack(entity);
		base.Apply(entity);
		entity.direction = caster_direction;
		
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
		entity.direction = Vector3.Zero;
    }
}
