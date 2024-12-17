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
		hitbox.damage = 20;
		hitbox.type = MeleeHitbox.DamageType.Spell;
		hitbox.spell_damage_type = MeleeHitbox.SpellDamageType.Fire;
	}
}
