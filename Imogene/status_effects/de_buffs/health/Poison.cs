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
		hitbox.damage = 15;
		hitbox.type = MeleeHitbox.DamageType.Other;
		hitbox.other_damage_type = MeleeHitbox.OtherDamageType.Poison;
	}
}
