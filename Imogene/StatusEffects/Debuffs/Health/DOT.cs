using Godot;
using System;

public partial class DOT : StatusEffect
{
	public MeleeHitbox hitbox { get; set; } = new MeleeHitbox();
	public int ticks_used { get; set; } = 0;
	public SceneTreeTimer tick_timer { get; set; } = null;

	public override void Apply(Entity entity_)
    {
        base.Apply(entity_);
		CreateTickTimer(entity_);
    }

	public void CreateTickTimer(Entity entity_)
	{
		tick_timer = GetTree().CreateTimer(1);
		tick_timer.Timeout += () => timer_timeout(entity_);
	}

	public override void timer_timeout(Entity entity)
    {
		entity.EntitySystems.damage_system.TakeDamage(entity, hitbox, hitbox.Damage, false);
		ticks_used += 1;
		if (ticks_used < duration)
		{
			GetTree().CreateTimer(1).Timeout += () => timer_timeout(entity);
		}
		else
		{
			ticks_used = 0;
			Remove(entity);
		}
    }

}
