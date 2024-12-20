using Godot;
using System;

public partial class Bleed : DOT
{
	
	public Bleed()
	{
		EffectName = "bleed";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		Duration = 10;
		MaxStacks = 1;
		Hitbox.Damage = 10;
		Hitbox.Type = MeleeHitbox.DamageType.Other;
		Hitbox.OtherDamage = MeleeHitbox.OtherDamageType.Bleed;
	}
}
