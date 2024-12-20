using Godot;
using System;

public partial class Enfeeble : StatusEffect
{
	public Enfeeble()
	{
		EffectName = "enfeeble";
		Type = EffectType.Debuff;
		Category = EffectCategory.Damage;
		MaxStacks = 1;
	}
}
