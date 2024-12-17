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
		hitbox.damage = 10;
		hitbox.type = MeleeHitbox.DamageType.Other;
		hitbox.other_damage_type = MeleeHitbox.OtherDamageType.Bleed;
	}
}
