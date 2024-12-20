using Godot;
using System;

public partial class Burn : DOT
{
	public Burn()
	{
		EffectName = "burn";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		Duration = 5;
		MaxStacks = 1;
		Hitbox.Damage = 20;
		Hitbox.Type = MeleeHitbox.DamageType.Spell;
		Hitbox.SpellDamage = MeleeHitbox.SpellDamageType.Fire;
	}
}
