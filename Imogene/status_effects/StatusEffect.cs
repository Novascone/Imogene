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
	[Export] public bool prevents_abilities;
	[Export] public bool adds_additional_effects;
	[Signal] public delegate void StatusEffectFinishedEventHandler();
	[Signal] public delegate void AddAdditionalStatusEffectEventHandler(StatusEffect effect);
	public bool applied;
 	public Timer duration_timer;
	public int duration;
	public int max_stacks;
	public int current_stacks;
	
	
	public States state;

	public enum States
    {
        not_queued,
        queued
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// slow.modification_type = StatModifier.ModificationType.multiply_current;
		
		
		// duration_timer = GetNode<Timer>("DurationTimer");
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

	public virtual void CreateTimerIncrementStack(Entity entity) // Creates timer and increments stacks
	{
		if(current_stacks == 0)
		{
			GD.Print("creating timer");
			GetTree().CreateTimer(duration).Timeout += () => timer_timeout(entity);
			GD.Print("setting booleans via apply");
			current_stacks += 1;
		}
		else if(max_stacks > 1)
		{
			current_stacks += 1;
		}
	}

	public virtual void timer_timeout(Entity entity)
	{

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
