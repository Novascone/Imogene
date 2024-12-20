using Godot;
using System;

public partial class Poison : DOT
{
	public Poison()
	{
		EffectName = "poison";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		Duration = 15;
		MaxStacks = 1;
		Hitbox.Damage = 8;
		Hitbox.Type = MeleeHitbox.DamageType.Other;
		Hitbox.OtherDamage = MeleeHitbox.OtherDamageType.Poison;
	}
}
