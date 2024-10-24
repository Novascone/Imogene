using Godot;
using System;

public partial class Stagger : StatusEffect
{
	public Stagger()
	{
		name = "stagger";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		max_stacks = 1;
	}
}
