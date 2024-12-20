using Godot;
using System;

public partial class Curse : StatusEffect
{

	public float max_health { get; set; } = 0.0f;
	public Curse()
	{
		EffectName = "curse";
		Type = EffectType.Debuff;
		Category = EffectCategory.Health;
		MaxStacks = 1;
	}

	[Signal] public delegate void CursedEventHandler(Entity entity, bool cursed);

	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		max_health = entity.Health.MaxValue;
		entity.Health.CurrentValue = entity.Health.MaxValue * 0.5f;
		entity.Health.HandicapValue = entity.Health.MaxValue * 0.5f;
		EmitSignal(nameof(Cursed), entity, Applied);
		
	}

    public override void Remove(Entity entity)
    {
        base.Remove(entity);
		EmitSignal(nameof(Cursed), entity, Applied);
		entity.Health.MaxValue = max_health;
    }
}
