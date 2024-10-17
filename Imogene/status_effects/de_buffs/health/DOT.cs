using Godot;
using System;

public partial class DOT : StatusEffect
{
	public MeleeHitbox hitbox { get; set; } = new MeleeHitbox();
	public int ticks_used { get; set; } = 0;
	public SceneTreeTimer tick_timer { get; set; } = null;

	public void CreateTickTimer(Entity entity_)
	{
		tick_timer = GetTree().CreateTimer(1);
		tick_timer.Timeout += () => timer_timeout(entity_);
	}
}
