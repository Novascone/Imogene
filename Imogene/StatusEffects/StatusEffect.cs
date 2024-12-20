using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class StatusEffect : Node3D
{
	public string EffectName;
	[Export] public string Description { get; set; }
    [Export] public StatusEffectResource Resource { get; set; }
	[Export] public bool AltersSpeed { get; set; }
	[Export] public bool PreventsMovement { get; set; }
	public bool PreventsInput{ get; set; }
	[Export] public bool PreventsAbilities { get; set; }
	[Export] public bool AddsAdditionalEffects { get; set; }
	[Export] public bool AddsEffectToAdditionalEntity { get; set; }
	
	public bool Removed { get; set; } = false;
	public EffectType Type { get; set; } = EffectType.None;
	public EffectCategory Category { get; set; } = EffectCategory.None;
	public bool Applied { get; set; } = false;
 	public SceneTreeTimer DurationTimer { get; set; } = null;
	public float Duration { get; set; } = 0.0f;
	public int MaxStacks { get; set; } = 0;
	public int CurrentStacks { get; set; } = 0;
	public Entity Caster { get; set; } = null;
	public States State  { get; set; } = States.NotQueued;

	public enum States{ NotQueued, Queued }
	public enum EffectType { None, Buff, Debuff, Tradeoff }

	public enum EffectCategory { None, Movement, Health, Damage, General }

	[Signal] public delegate void StatusEffectFinishedEventHandler();
	[Signal] public delegate void AddAdditionalStatusEffectEventHandler(StatusEffect effect);
	[Signal] public delegate void AddStatusEffectToAdditionalEntityEventHandler(Entity entity, StatusEffect effect);
	

	public virtual void Apply(Entity entity)
	{
		if(!Applied)
		{
			Applied = true;
		}
	}

	public virtual void CreateTimerIncrementStack(Entity entity) // Creates timer and increments stacks
	{
		
		if(CurrentStacks == 0)
		{
			DurationTimer = GetTree().CreateTimer(Duration);
			DurationTimer.Timeout += () => TimerTimeout(entity);
			GD.Print("incrementing stacks");
			CurrentStacks += 1;
		}
		else if(MaxStacks > 1)
		{
			
			CurrentStacks += 1;
		}
	}

	public virtual void TimerTimeout(Entity entity)
	{

	}

	public virtual void Remove(Entity entity)
	{
		GD.Print("removed");
		if(!Removed)
		{
			Removed = true;
			CurrentStacks = 0;
			EmitSignal(nameof(StatusEffectFinished));
		}
	}
}
