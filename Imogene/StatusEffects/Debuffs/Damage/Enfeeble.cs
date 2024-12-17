using Godot;
using System;

public partial class Enfeeble : StatusEffect
{
	public Enfeeble()
	{
		name = "enfeeble";
		type = EffectType.Debuff;
		category = EffectCategory.Damage;
		max_stacks = 1;
	}
}
