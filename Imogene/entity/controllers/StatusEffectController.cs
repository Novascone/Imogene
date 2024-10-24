using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

public partial class StatusEffectController : Node
{
	

	
	public bool movement_prevented { get; set; } = false;
	public bool input_prevented { get; set; } = false;
	public bool speed_altered { get; set; } = false;
	public bool abilities_prevented  { get; set; } = false;
	public bool tethered { get; set; } = false;
	public int fear_duration { get; set; } = 5;
	// Movement

	// Buffs
	
	// De-buffs
	public Slow slow { get; set; } = new();
	public Daze daze { get; set; } = new();
	public Chill chill { get; set; } = new();
	public Freeze freeze { get; set; } = new();
	public Fear fear { get; set; } = new();
	public Hamstring hamstring { get; set; } = new();
	public Tether tether { get; set; } = new();
	public Stun stun { get; set; } = new();
	public Hex hex { get; set; } = new();
	public Knockback knockback { get; set; } = new();
	public Bleed bleed { get; set; } = new();
	public Burn burn { get; set; } = new();
	public Poison poison { get; set; } = new();
	public Weak weak { get; set; } = new();
	public Curse curse { get; set; } = new();
	public Virulent virulent { get; set; } = new();
	
	public Dictionary<StatusEffect, bool> status_effects  { get; set; } = new Dictionary<StatusEffect, bool>();


	[Signal] public delegate void AbilitiesPreventedEventHandler(bool abilities_prevented_);
	[Signal] public delegate void MovementPreventedEventHandler(bool movement_prevented_);
	[Signal] public delegate void InputPreventedEventHandler(bool input_prevented_);
	[Signal] public delegate void SpeedAlteredEventHandler(bool speed_altered_);
	[Signal] public delegate void TetheredEventHandler(Entity entity_, MeshInstance3D tether_, bool tethered_, float tether_length_);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		status_effects[slow] = false;
		status_effects[daze] = false;
		status_effects[chill] = false;
		status_effects[freeze] = false;
		status_effects[fear] = false;
		status_effects[hamstring] = false;
		status_effects[tether] = false;
		status_effects[stun] = false;
		status_effects[hex] = false;
		status_effects[knockback] = false;
		status_effects[bleed] = false;
		status_effects[burn] = false;
		status_effects[poison] = false;
		status_effects[weak] = false;
		status_effects[curse] = false;
		status_effects[virulent] = false;
	}

	public void AddStatusEffect(Entity entity_, StatusEffect effect_) // Adds status effect to entity
	{
		var _effect_to_add = new StatusEffect();
		_effect_to_add = GetEffect(entity_, effect_, _effect_to_add); // Gets status effect to apply
		GD.Print("trying to add effect " + _effect_to_add.name);
		QueueStatusEffect(_effect_to_add); // Checks if ability has been applied
		if(_effect_to_add.state == StatusEffect.States.Queued)
		{
			
				entity_.status_effects.Add(_effect_to_add); // adds status effect to entities list of effects
				GD.Print("count of status effects " + entity_.status_effects.Count);
				AddChild(_effect_to_add); // adds the effect as a child setting all of its _Ready() values
				
				_effect_to_add.StatusEffectFinished += () => HandleStatusEffectFinished(entity_, _effect_to_add); // Subscribes to effect finished signal
				if(_effect_to_add.adds_additional_effects)
				{
					_effect_to_add.AddAdditionalStatusEffect += (effect) => HandleAdditionalStatusEffect(effect, entity_); // subscribes to add additional effect signal
				}
				if(_effect_to_add.adds_effect_to_additional_entity)
				{
					_effect_to_add.AddStatusEffectToAdditionalEntity += HandleAdditionalEntity;
				}
				ApplyStatusEffect(entity_, _effect_to_add);
				// Sets the variable for what the status effects are preventing or altering
				if(_effect_to_add.prevents_movement){ movement_prevented = true; EmitSignal(nameof(MovementPrevented), movement_prevented);}
				if(_effect_to_add.prevents_input){ input_prevented = true; EmitSignal(nameof(InputPrevented), input_prevented);}
				if(_effect_to_add.alters_speed){ speed_altered = true; }
				if(_effect_to_add.prevents_abilities){ abilities_prevented = true; EmitSignal(nameof(AbilitiesPrevented), abilities_prevented);}
				if(_effect_to_add.name == "tether")
				{
					tethered = true;
					Tether tether_effect = (Tether)_effect_to_add;
					EmitSignal(nameof(Tethered),entity_, tether_effect.tether, tethered, tether_effect.tether_length);
				
				}
			// }
			
		}
		else
		{
			ApplyStatusEffect(entity_, _effect_to_add);
		}
		
	}

    private void HandleAdditionalEntity(Entity entity_, StatusEffect effect_)
    {
		GD.Print("Receiving signal to add an effect to " + entity_.Name);
        AddStatusEffect(entity_, effect_);
    }

    public void RemoveStatusEffect(Entity entity_, StatusEffect effect_)
	{

		effect_.StatusEffectFinished -= () => HandleStatusEffectFinished(entity_, effect_); // unSubscribes to effect finished signal
		if(effect_.adds_additional_effects)
		{
			effect_.AddAdditionalStatusEffect -= (effect) => HandleAdditionalStatusEffect(effect, entity_); // Unsubscribes to add additional effect signal
		}
		effect_.QueueFree();
		// entity_.entity_controllers.status_effect_controller.SetEffectBooleans(effect_);
		entity_.status_effects.Remove(effect_);
		if(!effect_.removed)
		{
			effect_.Remove(entity_);
		}
		
		if(effect_.prevents_movement == true){ movement_prevented = false; EmitSignal(nameof(MovementPrevented), movement_prevented);}
		if(effect_.prevents_input){ input_prevented = false; EmitSignal(nameof(InputPrevented), input_prevented);}
		if(effect_.alters_speed){ speed_altered = false; }
		if(effect_.prevents_abilities){ abilities_prevented = false; EmitSignal(nameof(AbilitiesPrevented), abilities_prevented);}
		if(effect_.name == "tether"){ tethered = false; EmitSignal(nameof(Tethered),entity_, tether.tether, tethered);;}
		
	}

	// Queues effect if it hasn't been instantiated, because .prevents_movement, and .alters speed wont be set,
	// if the effect has been instantiated it will check those properties and wont queue if a similar effect is active
	public void QueueStatusEffect(StatusEffect effect_) 
	{
		
		if(!effect_.applied)
		{
			effect_.state = StatusEffect.States.Queued;
		}
		else
		{
			effect_.state = StatusEffect.States.NotQueued;
		}
	}

	public void ApplyStatusEffect(Entity entity_, StatusEffect effect_) // Sets booleans for each status effect, and applies the effect to the entity
	{
		if (effect_.current_stacks < effect_.max_stacks && entity_.status_effects.Contains(effect_))
		{
			// if(effect_.current_stacks == 0)
			// {
			// 	SetEffectBooleans(effect_);
			// }
			effect_.Apply(entity_);
			GD.Print("applying status effect");
		}
		else
		{
			GD.Print("couldn't apply status effect");
		}
	}

	public void RemoveMovementDebuffs(Entity entity_)
	{

		for(int i = entity_.status_effects.Count - 1; i >= 0 ; i--) // Iterates list in reverse so that the position of i is not disrupted
		{
			
			if(entity_.status_effects[i].type == StatusEffect.EffectType.Debuff  && entity_.status_effects[i].category == StatusEffect.EffectCategory.Movement)
			{
				RemoveStatusEffect(entity_, entity_.status_effects[i]);
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
	public StatusEffect GetEffect(Entity entity_, StatusEffect effect_, StatusEffect effect_to_get_) 
	{
		effect_to_get_ = effect_;
		foreach(StatusEffect _status_effect in status_effects.Keys)
		{
			if (effect_.GetType() == _status_effect.GetType())
			{
			
				foreach(StatusEffect _applied_effect in entity_.status_effects)
				{
					if(_applied_effect.GetType() == _status_effect.GetType())
					{
						effect_to_get_ = _applied_effect;
					}
				}
				
			}
		}
		
		return effect_to_get_;
	}

	// Handlers for signals from the status effects
	private void HandleAdditionalStatusEffect(StatusEffect effect_, Entity entity_)
    {
        AddStatusEffect(entity_, effect_);
    }

	private void AddWeak(Entity entity_, bool weak_)
    {
		GD.Print("Adding weak");
        AddStatusEffect(entity_, weak);
    }

    private void HandleStatusEffectFinished(Entity entity_, StatusEffect effect_to_remove_)
    {
		foreach(StatusEffect _effect in status_effects.Keys)
		{
			if(effect_to_remove_.GetType() == _effect.GetType())
			{
				if(status_effects[_effect])
				{
					
					RemoveStatusEffect(entity_, effect_to_remove_);
					effect_to_remove_.StatusEffectFinished -= () => HandleStatusEffectFinished(entity_, effect_to_remove_);
					if(effect_to_remove_.adds_additional_effects){ effect_to_remove_.AddAdditionalStatusEffect -= (effect) => HandleAdditionalStatusEffect(effect, entity_);}
				
				}
			}
		}
    }

	public void Subscribe(Entity entity_)
	{
		entity_.entity_systems.damage_system.AddStatusEffect += AddStatusEffect;
		entity_.entity_systems.damage_system.Weak += AddWeak;
	}


    public void Unsubscribe(Entity entity_)
	{
		entity_.entity_systems.damage_system.AddStatusEffect -= AddStatusEffect;
		entity_.entity_systems.damage_system.Weak -= AddWeak;
	}

}
