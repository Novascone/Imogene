using Godot;
using System;

public partial class Incorrigible : StatusEffect
{
	public Incorrigible()
	{
		EffectName = "incorrigible";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		MaxStacks = 1;
	}
}
