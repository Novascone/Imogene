using Godot;
using System;

public partial class Poison : DOT
{
	public Poison()
	{
		name = "poison";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		duration = 15;
		max_stacks = 1;
		hitbox.Damage = 8;
		hitbox.Type = MeleeHitbox.DamageType.Other;
		hitbox.OtherDamage = MeleeHitbox.OtherDamageType.Poison;
	}
}
