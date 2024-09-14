using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class StatusEffect : Node3D
{
	[Export] public string description { get; set; }
    [Export] public StatusEffectResource resource { get; set; }
    [Export] public string effect_type { get; set; }
	[Export] public bool alters_speed;
	[Export] public bool prevents_movement;
	public bool applied;
 	public Timer duration_timer;
	public int duration;
	public int max_stacks;
	public int current_stacks;
	public StatModifier slow = new(StatModifier.ModificationType.Multiply);
	public States state;

	public enum States
    {
        not_queued,
        queued
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		slow.mod = -0.3f;
		duration_timer = GetNode<Timer>("DurationTimer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(current_stacks > 0)
		{
			applied = true;
		}
		else
		{
			applied = false;
		}
	}

	

	public virtual void Apply(Entity entity)
	{
		GD.Print("Base status effect apply");
	}

	


	// public void AddStatusEffect(StatusEffect effect)
	// {
	// 	if(!entity.status_effects.Contains(effect))
	// 	{
	// 		entity.status_effects.Add(effect);
	// 	}
	// 	else if (effect.current_stacks < effect.max_stacks)
	// 	{
	// 		effect.current_stacks += 1;
	// 	}
	// }

	// public void RemoveStatusEffect(StatusEffect effect)
	// {
	// 	entity.status_effects.Remove(effect);
	// }
}
