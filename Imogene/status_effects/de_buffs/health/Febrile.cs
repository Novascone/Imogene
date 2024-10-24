using Godot;
using System;

public partial class Febrile : StatusEffect
{
	public Febrile()
	{
		name = "febrile";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		max_stacks = 1;
	}
}
