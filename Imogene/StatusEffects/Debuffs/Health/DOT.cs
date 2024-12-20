using Godot;
using System;

public partial class DOT : StatusEffect
{
	public MeleeHitbox Hitbox { get; set; } = new MeleeHitbox();
	public int TicksUsed { get; set; } = 0;
	public SceneTreeTimer TickTimer { get; set; } = null;

	public override void Apply(Entity entity)
    {
        base.Apply(entity);
		CreateTickTimer(entity);
    }

	public void CreateTickTimer(Entity entity)
	{
		TickTimer = GetTree().CreateTimer(1);
		TickTimer.Timeout += () => TimerTimeout(entity);
	}

	public override void TimerTimeout(Entity entity)
    {
		entity.EntitySystems.damage_system.TakeDamage(entity, Hitbox, Hitbox.Damage, false);
		TicksUsed += 1;
		if (TicksUsed < Duration)
		{
			GetTree().CreateTimer(1).Timeout += () => TimerTimeout(entity);
		}
		else
		{
			TicksUsed = 0;
			Remove(entity);
		}
    }

}
