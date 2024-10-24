using Godot;
using System;

public partial class Incorrigible : StatusEffect
{
	public Incorrigible()
	{
		name = "incorrigible";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		max_stacks = 1;
	}
}
