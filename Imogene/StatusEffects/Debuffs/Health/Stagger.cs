using Godot;
using System;

public partial class Stagger : StatusEffect
{
	public Stagger()
	{
		EffectName = "stagger";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		MaxStacks = 1;
	}
}
