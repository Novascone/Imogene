using Godot;
using System;

public partial class Febrile : StatusEffect
{
	public Febrile()
	{
		EffectName = "febrile";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		MaxStacks = 1;
	}
}
