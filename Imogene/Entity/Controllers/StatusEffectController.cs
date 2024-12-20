using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

public partial class StatusEffectController : Node
{
	

	
	public bool EntityMovementPrevented { get; set; } = false;
	public bool EntityInputPrevented { get; set; } = false;
	public bool EntitySpeedAltered { get; set; } = false;
	public bool EntityAbilitiesPrevented  { get; set; } = false;
	public bool EntityTethered { get; set; } = false;
	public int EntityFearDuration  { get; set; } = 5;
	// Movement

	// Buffs
	
	// De-buffs
	public Slow SlowEffect { get; set; } = new();
	public Daze DazeEffect { get; set; } = new();
	public Chill ChillEffect { get; set; } = new();
	public Freeze FreezeEffect { get; set; } = new();
	public Fear FearEffect { get; set; } = new();
	public Hamstring HamstringEffect { get; set; } = new();
	public Tether TetherEffect { get; set; } = new();
	public Stun StunEffect { get; set; } = new();
	public Hex HexEffect { get; set; } = new();
	public Knockback KnockbackEffect { get; set; } = new();
	public Bleed BleedEffect { get; set; } = new();
	public Burn BurnEffect { get; set; } = new();
	public Poison PoisonEffect { get; set; } = new();
	public Weak WeakEffect { get; set; } = new();
	public Curse CurseEffect { get; set; } = new();
	public Virulent VirulentEffect { get; set; } = new();
	public Incorrigible IncorrigibleEffect { get; set; } = new();
	public Febrile FebrileEffect { get; set; } = new();
	public Reeling ReelingEffect { get; set; } = new();
	public Stagger StaggerEffect { get; set; } = new();
	public Enfeeble EnfeebleEffect { get; set; } = new();
	public Suspend SuspendEffect  { get; set; } = new();
	public Charm CharmEffect { get; set; } = new();
	
	public List<StatusEffect> StatusEffects  { get; set; } = new List<StatusEffect>();


	[Signal] public delegate void AbilitiesPreventedEventHandler(bool abilitiesPrevented);
	[Signal] public delegate void MovementPreventedEventHandler(bool movementPrevented);
	[Signal] public delegate void InputPreventedEventHandler(bool inputPrevented);
	[Signal] public delegate void SpeedAlteredEventHandler(bool speedAltered);
	[Signal] public delegate void TetheredEventHandler(Entity entity, MeshInstance3D tether, bool tethered, float tetherLength);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StatusEffects.Add(SlowEffect);
		StatusEffects.Add(DazeEffect);
		StatusEffects.Add(ChillEffect);
		StatusEffects.Add(FreezeEffect);
		StatusEffects.Add(FearEffect);
		StatusEffects.Add(HamstringEffect);
		StatusEffects.Add(TetherEffect);
		StatusEffects.Add(StunEffect);
		StatusEffects.Add(HexEffect);
		StatusEffects.Add(KnockbackEffect);
		StatusEffects.Add(BleedEffect);
		StatusEffects.Add(BurnEffect);
		StatusEffects.Add(PoisonEffect);
		StatusEffects.Add(WeakEffect);
		StatusEffects.Add(CurseEffect);
		StatusEffects.Add(VirulentEffect);
		StatusEffects.Add(IncorrigibleEffect);
		StatusEffects.Add(FebrileEffect);
		StatusEffects.Add(ReelingEffect);
		StatusEffects.Add(StaggerEffect);
		StatusEffects.Add(EnfeebleEffect);
		StatusEffects.Add(SuspendEffect);
		StatusEffects.Add(CharmEffect);

	}

	public void AddStatusEffect(Entity entity, StatusEffect effect) // Adds status effect to entity
	{
		var effectToAdd = new StatusEffect();
		effectToAdd = GetEffect(entity, effect, effectToAdd); // Gets status effect to apply
		GD.Print("trying to add effect " + effectToAdd.EffectName);
		QueueStatusEffect(effectToAdd); // Checks if ability has been applied
		if(effectToAdd.State == StatusEffect.States.Queued)
		{
			
				entity.StatusEffects.Add(effectToAdd); // adds status effect to entities list of effects
				GD.Print("count of status effects " + entity.StatusEffects.Count);
				AddChild(effectToAdd); // adds the effect as a child setting all of its _Ready() values
				
				effectToAdd.StatusEffectFinished += () => HandleStatusEffectFinished(entity, effectToAdd); // Subscribes to effect finished signal
				if(effectToAdd.AddsAdditionalEffects)
				{
					effectToAdd.AddAdditionalStatusEffect += (effect) => HandleAdditionalStatusEffect(effect, entity); // subscribes to add additional effect signal
				}
				if(effectToAdd.AddsEffectToAdditionalEntity)
				{
					effectToAdd.AddStatusEffectToAdditionalEntity += HandleAdditionalEntity;
				}
				ApplyStatusEffect(entity, effectToAdd);
				// Sets the variable for what the status effects are preventing or altering
				if(effectToAdd.PreventsMovement){ EntityMovementPrevented = true; EmitSignal(nameof(MovementPrevented), EntityMovementPrevented);}
				if(effectToAdd.PreventsInput){ EntityInputPrevented = true; EmitSignal(nameof(InputPrevented), EntityInputPrevented);}
				if(effectToAdd.AltersSpeed){ EntitySpeedAltered = true; }
				if(effectToAdd.PreventsAbilities){ EntityAbilitiesPrevented = true; EmitSignal(nameof(AbilitiesPrevented), EntityAbilitiesPrevented);}
				if(effectToAdd.EffectName == "tether")
				{
					EntityTethered = true;
					Tether tether_effect = (Tether)effectToAdd;
					EmitSignal(nameof(Tethered),entity, tether_effect.TetherMesh, EntityTethered, tether_effect.TetherScene);
				
				}
			// }
			
		}
		else
		{
			ApplyStatusEffect(entity, effectToAdd);
		}
		
	}

    private void HandleAdditionalEntity(Entity entity, StatusEffect effect)
    {
		GD.Print("Receiving signal to add an effect to " + entity.Name);
        AddStatusEffect(entity, effect);
    }

    public void RemoveStatusEffect(Entity entity, StatusEffect effect)
	{

		effect.StatusEffectFinished -= () => HandleStatusEffectFinished(entity, effect); // unSubscribes to effect finished signal
		if(effect.AddsAdditionalEffects)
		{
			effect.AddAdditionalStatusEffect -= (effect) => HandleAdditionalStatusEffect(effect, entity); // Unsubscribes to add additional effect signal
		}
		effect.QueueFree();
		// entity_.entity_controllers.status_effect_controller.SetEffectBooleans(effect_);
		entity.StatusEffects.Remove(effect);
		if(!effect.Removed)
		{
			effect.Remove(entity);
		}
		
		if(effect.PreventsMovement == true){ EntityMovementPrevented = false; EmitSignal(nameof(MovementPrevented), EntityMovementPrevented);}
		if(effect.PreventsInput){ EntityInputPrevented = false; EmitSignal(nameof(InputPrevented), EntityInputPrevented);}
		if(effect.AltersSpeed){ EntitySpeedAltered = false; }
		if(effect.PreventsAbilities){ EntityAbilitiesPrevented = false; EmitSignal(nameof(AbilitiesPrevented), EntityAbilitiesPrevented);}
		if(effect.EffectName == "tether"){ EntityTethered = false; EmitSignal(nameof(Tethered),entity, TetherEffect.TetherMesh, EntityTethered);;}
		
	}

	// Queues effect if it hasn't been instantiated, because .prevents_movement, and .alters speed wont be set,
	// if the effect has been instantiated it will check those properties and wont queue if a similar effect is active
	public void QueueStatusEffect(StatusEffect effect) 
	{
		
		if(!effect.Applied)
		{
			effect.State = StatusEffect.States.Queued;
		}
		else
		{
			effect.State = StatusEffect.States.NotQueued;
		}
	}

	public void ApplyStatusEffect(Entity entity, StatusEffect effect) // Sets booleans for each status effect, and applies the effect to the entity
	{
		if (effect.CurrentStacks < effect.MaxStacks && entity.StatusEffects.Contains(effect))
		{
			// if(effect_.current_stacks == 0)
			// {
			// 	SetEffectBooleans(effect_);
			// }
			effect.Apply(entity);
			GD.Print("applying status effect");
		}
		else
		{
			GD.Print("couldn't apply status effect");
		}
	}

	public void RemoveMovementDebuffs(Entity entity)
	{

		for(int i = entity.StatusEffects.Count - 1; i >= 0 ; i--) // Iterates list in reverse so that the position of i is not disrupted
		{
			
			if(entity.StatusEffects[i].Type == StatusEffect.EffectType.Debuff  && entity.StatusEffects[i].Category == StatusEffect.EffectCategory.Movement)
			{
				RemoveStatusEffect(entity, entity.StatusEffects[i]);
			}
			
		}
	}

	// public void SetEffectBooleans(StatusEffect effect_) // Switches the effect from on to off or off to on in the dictionary
	// {

	// 	foreach(StatusEffect _status_effect in status_effects.Keys)
	// 	{
	// 		if (effect_.GetType() == _status_effect.GetType())
	// 		{
	// 			status_effects[_status_effect] = !status_effects[_status_effect];
	// 		}
	// 	}

	// }

	// Checks the incoming effect type, if the effect is not on, then a new instance of that effect will be created and returned,
	// if the effect is on the entities status effects will be searched and the matching effect will be returned
	public StatusEffect GetEffect(Entity entity, StatusEffect effect, StatusEffect effectToGet) 
	{
		effectToGet = effect;
		foreach(StatusEffect statusEffect in StatusEffects)
		{
			if (effect.GetType() == statusEffect.GetType())
			{
			
				foreach(StatusEffect appliedEffect in entity.StatusEffects)
				{
					if(appliedEffect.GetType() == statusEffect.GetType())
					{
						effectToGet = appliedEffect;
					}
				}
				
			}
		}
		
		return effectToGet;
	}

	// Handlers for signals from the status effects
	private void HandleAdditionalStatusEffect(StatusEffect effect, Entity entity)
    {
        AddStatusEffect(entity, effect);
    }

	private void AddWeak(Entity entity, bool weak)
    {
		GD.Print("Adding weak");
        AddStatusEffect(entity, WeakEffect);
    }

    private void HandleStatusEffectFinished(Entity entity, StatusEffect effectToRemove)
    {

		RemoveStatusEffect(entity, effectToRemove);
					effectToRemove.StatusEffectFinished -= () => HandleStatusEffectFinished(entity, effectToRemove);
					if(effectToRemove.AddsAdditionalEffects){ effectToRemove.AddAdditionalStatusEffect -= (effect) => HandleAdditionalStatusEffect(effect, entity);}
					GD.Print("removed " + effectToRemove.EffectName);
		// foreach(StatusEffect _effect in status_effects)
		// {
		// 	if(effect_to_remove_.GetType() == _effect.GetType())
		// 	{
				
					
		// 			RemoveStatusEffect(entity_, effect_to_remove_);
		// 			effect_to_remove_.StatusEffectFinished -= () => HandleStatusEffectFinished(entity_, effect_to_remove_);
		// 			if(effect_to_remove_.adds_additional_effects){ effect_to_remove_.AddAdditionalStatusEffect -= (effect) => HandleAdditionalStatusEffect(effect, entity_);}
		// 		GD.Print("removed " + effect_to_remove_.name);
				
		// 	}
		// }
    }

	public void Subscribe(Entity entity)
	{
		entity.EntitySystems.damage_system.AddStatusEffect += AddStatusEffect;
		entity.EntitySystems.damage_system.Weak += AddWeak;
	}


    public void Unsubscribe(Entity entity)
	{
		entity.EntitySystems.damage_system.AddStatusEffect -= AddStatusEffect;
		entity.EntitySystems.damage_system.Weak -= AddWeak;
	}

}
