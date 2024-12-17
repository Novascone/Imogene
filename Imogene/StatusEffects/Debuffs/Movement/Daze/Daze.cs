using Godot;
using System;

public partial class Daze : StatusEffect
{
	
	public Daze()
	{
		name = "daze";
		prevents_abilities = true;
		type = EffectType.Debuff;
		category = EffectCategory.Movement;
		duration = 5;
		max_stacks = 1;
	}

	public override void Apply(Entity entity_)
	{
		base.Apply(entity_);
		CreateTimerIncrementStack(entity_);
	}

	public override void timer_timeout(Entity entity_)
    {
		Remove(entity_);
    }

	
}
