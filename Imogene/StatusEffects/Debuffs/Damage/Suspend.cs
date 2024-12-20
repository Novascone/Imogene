using Godot;
using System;

public partial class Suspend : StatusEffect
{
	public Suspend()
	{
		EffectName = "suspend";
		Type = EffectType.Debuff;
		Category = EffectCategory.Damage;
		MaxStacks = 1;
	}

    public override void Apply(Entity entity)
    {
        base.Apply(entity);

		// entity_.
    }
}
