using Godot;
using System;

public partial class Stun : StatusEffect
{
	public Stun()
	{
		name = "stun";
		type = EffectType.buff;
		category = EffectCategory.movement;
		duration = 5;
		max_stacks = 1;
	}

	public override void Apply(Entity entity)
	{
		base.Apply(entity);
		
		CreateTimerIncrementStack(entity);
		
	}

	public override void timer_timeout(Entity entity)
    {
		GD.Print("timer timeout");
     
		Remove(entity);
		
		GD.Print("current stacks " + current_stacks);
    }
}
