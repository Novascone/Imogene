using Godot;
using System;

public partial class Reeling : StatusEffect
{
	public Reeling()
	{
		name = "reeling";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		max_stacks = 1;
	}
}
