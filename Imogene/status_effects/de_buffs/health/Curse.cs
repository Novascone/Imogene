using Godot;
using System;

public partial class Curse : StatusEffect
{

	public float max_health { get; set; } = 0.0f;
	public Curse()
	{
		name = "curse";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		max_stacks = 1;
	}

	[Signal] public delegate void CursedEventHandler(Entity entity_, bool cursed_);

	public override void Apply(Entity entity_)
	{
		base.Apply(entity_);
		max_health = entity_.health.max_value;
		entity_.health.current_value = entity_.health.max_value * 0.5f;
		entity_.health.handicap_value = entity_.health.max_value * 0.5f;
		EmitSignal(nameof(Cursed), entity_, applied);
		
	}

    public override void Remove(Entity entity_)
    {
        base.Remove(entity_);
		EmitSignal(nameof(Cursed), entity_, applied);
		entity_.health.max_value = max_health;
    }
}
