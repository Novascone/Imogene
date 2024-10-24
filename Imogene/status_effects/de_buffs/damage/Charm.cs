using Godot;
using System;

public partial class Charm: StatusEffect
{
	public Charm()
	{
		name = "charm";
		type = EffectType.Debuff;
		category = EffectCategory.Damage;
		max_stacks = 1;
	}
}
