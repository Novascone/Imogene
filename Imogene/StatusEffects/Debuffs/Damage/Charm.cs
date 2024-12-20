using Godot;
using System;

public partial class Charm: StatusEffect
{
	public Charm()
	{
		EffectName = "charm";
		Type = EffectType.Debuff;
		Category = EffectCategory.Damage;
		MaxStacks = 1;
	}
}
