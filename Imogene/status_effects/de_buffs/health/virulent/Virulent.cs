using Godot;
using System;

public partial class Virulent : StatusEffect
{
	VirulentHitbox hitbox { get; set; } 
	Poison poison = new();

	public Virulent()
	{
		name = "virulent";
		type = EffectType.Debuff;
		category = EffectCategory.Health;
		max_stacks = 1;
		duration = 1;
		hitbox = (VirulentHitbox)ResourceLoader.Load<PackedScene>("res://status_effects/de_buffs/health/virulent/virulent_hitbox.tscn").Instantiate();
	}

    public override void Apply(Entity entity_)
    {
        base.Apply(entity_);
		EmitSignal(nameof(AddAdditionalStatusEffect), poison);
		hitbox.root_infected = entity_;
		entity_.AddChild(hitbox);
		CreateTimerIncrementStack(entity_);
    }

    public override void timer_timeout(Entity entity_)
    {
        base.timer_timeout(entity_);
		Remove(entity_);
    }

    public override void Remove(Entity entity_)
    {
        base.Remove(entity_);
		entity_.RemoveChild(hitbox);
    }
}
