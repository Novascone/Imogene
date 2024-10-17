using Godot;
using System;

public partial class Bleed : DOT
{
	
	public Bleed()
	{
		name = "bleed";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		duration = 5;
		max_stacks = 1;
		hitbox.damage = 10;
		hitbox.type = MeleeHitbox.DamageType.Other;
		hitbox.other_damage_type = MeleeHitbox.OtherDamageType.Bleed;
	}

    public override void Apply(Entity entity_)
    {
        base.Apply(entity_);
		CreateTickTimer(entity_);
    }

    
	public override void timer_timeout(Entity entity_)
    {
		entity_.entity_systems.damage_system.TakeDamage(entity_, hitbox, hitbox.damage, false);
		ticks_used += 1;
		if (ticks_used < duration)
		{
			GetTree().CreateTimer(1).Timeout += () => timer_timeout(entity_);
		}
		else
		{
			ticks_used = 0;
		}
    }

}
