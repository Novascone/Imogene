using Godot;
using System;

public partial class Bleed : DOT
{
	
	public Bleed()
	{
		name = "bleed";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		duration = 10;
		max_stacks = 1;
		hitbox.Damage = 10;
		hitbox.Type = MeleeHitbox.DamageType.Other;
		hitbox.OtherDamage = MeleeHitbox.OtherDamageType.Bleed;
	}
}
