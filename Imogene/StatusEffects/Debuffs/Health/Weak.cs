using Godot;
using System;

public partial class Weak : StatusEffect
{
	public Weak()
	{
		EffectName = "weak";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		MaxStacks = 1;
	}
}
