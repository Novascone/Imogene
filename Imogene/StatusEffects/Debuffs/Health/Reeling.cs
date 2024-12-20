using Godot;
using System;

public partial class Reeling : StatusEffect
{
	public Reeling()
	{
		EffectName = "reeling";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		MaxStacks = 1;
	}
}
