using Godot;
using System;

public partial class Burn : DOT
{
	public Burn()
	{
		name = "burn";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		duration = 5;
		max_stacks = 1;
		hitbox.Damage = 20;
		hitbox.Type = MeleeHitbox.DamageType.Spell;
		hitbox.SpellDamage = MeleeHitbox.SpellDamageType.Fire;
	}
}
