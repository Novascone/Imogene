using Godot;
using System;

public partial class Suspend : StatusEffect
{
	public Suspend()
	{
		name = "suspend";
		type = EffectType.Debuff;
		category = EffectCategory.Damage;
		max_stacks = 1;
	}

    public override void Apply(Entity entity_)
    {
        base.Apply(entity_);

		// entity_.
    }
}
