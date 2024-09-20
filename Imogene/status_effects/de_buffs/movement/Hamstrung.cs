using Godot;
using System;

public partial class Hamstrung : StatusEffect
{
	public StatModifier stop = new(StatModifier.ModificationType.nullify);
	public Hamstrung()
	{
		name = "hamstrung";
		type = EffectType.debuff;
		category = EffectCategory.movement;
		prevents_movement = true;
		duration = 5;
		max_stacks = 1;
	}
	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		entity.movement_speed.AddModifier(stop);
		CreateTimerIncrementStack(entity);
		
		
	}

	public override void timer_timeout(Entity entity)
    {
		Remove(entity);
    }

    public override void Remove(Entity entity)
    {
		if(!removed)
		{
			base.Remove(entity);
			entity.movement_speed.RemoveModifier(stop);
		}
        
    }
}
