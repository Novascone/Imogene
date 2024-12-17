using Godot;
using System;

public partial class Weak : StatusEffect
{
	public Weak()
	{
		name = "weak";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		max_stacks = 1;
	}
}
