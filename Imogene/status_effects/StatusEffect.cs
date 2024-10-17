using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class StatusEffect : Node3D
{
	public string name;
	[Export] public string description { get; set; }
    [Export] public StatusEffectResource resource { get; set; }
	[Export] public bool alters_speed { get; set; }
	[Export] public bool prevents_movement { get; set; }
	public bool prevents_input{ get; set; }
	[Export] public bool prevents_abilities { get; set; }
	[Export] public bool adds_additional_effects { get; set; }
	public bool removed { get; set; } = false;
	public EffectType type { get; set; } = EffectType.None;
	public EffectCategory category { get; set; } = EffectCategory.None;
	public bool applied { get; set; } = false;
 	public SceneTreeTimer duration_timer { get; set; } = null;
	public float duration { get; set; } = 0.0f;
	public int max_stacks { get; set; } = 0;
	public int current_stacks { get; set; } = 0;
	public Entity caster { get; set; } = null;
	public States state  { get; set; } = States.NotQueued;

	public enum States{ NotQueued, Queued }
	public enum EffectType { None, Buff, Debuff, Tradeoff }

	public enum EffectCategory { None, Movement, Health, Damage, General }

	[Signal] public delegate void StatusEffectFinishedEventHandler();
	[Signal] public delegate void AddAdditionalStatusEffectEventHandler(StatusEffect effect_);
	

	public virtual void Apply(Entity entity_)
	{
		if(!applied)
		{
			applied = true;
		}
	}

	public virtual void CreateTimerIncrementStack(Entity entity_) // Creates timer and increments stacks
	{
		if(current_stacks == 0)
		{
			duration_timer = GetTree().CreateTimer(duration);
			duration_timer.Timeout += () => timer_timeout(entity_);
			current_stacks += 1;
		}
		else if(max_stacks > 1)
		{
			current_stacks += 1;
		}
	}

	public virtual void timer_timeout(Entity entity_)
	{

	}

	public virtual void Remove(Entity entity_)
	{
		if(!removed)
		{
			removed = true;
			current_stacks = 0;
			EmitSignal(nameof(StatusEffectFinished));
		}
	}
}
