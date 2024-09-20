using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

public partial class StatusEffectController : Node
{
	

	
	public bool movement_prevented;
	public bool input_prevented;
	public bool speed_altered;
	public bool abilities_prevented;
	public bool tethered;
	public bool effect_already_applied;
	// Movement

	// Buffs
	
	// De-buffs
	public Slow slow = new();
	public Daze daze = new();
	public Chill chill = new();
	public Freeze freeze = new();
	public Fear fear = new();
	public Hamstrung hamstrung = new();
	public Tether tether = new();
	public Stun stun = new();
	public Hex hex = new();
	public Knockback knockback = new();
	


	
	public Dictionary<StatusEffect, bool> status_effects = new Dictionary<StatusEffect, bool>();
	[Signal] public delegate void AbilitiesPreventedEventHandler(bool abilities_prevented);
	[Signal] public delegate void MovementPreventedEventHandler(bool movement_prevented);
	[Signal] public delegate void InputPreventedEventHandler(bool input_prevented);
	[Signal] public delegate void SpeedAlteredEventHandler(bool speed_altered);
	[Signal] public delegate void TetheredEventHandler(Entity entity, MeshInstance3D tether, bool tethered);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("status effect controller added");
		status_effects[slow] = false;
		status_effects[daze] = false;
		status_effects[chill] = false;
		status_effects[freeze] = false;
		status_effects[fear] = false;
		status_effects[hamstrung] = false;
		status_effects[tether] = false;
		status_effects[stun] = false;
		status_effects[hex] = false;
		status_effects[knockback] = false;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// if(slowed) { GD.Print(entity.Name + " is slowed"); }

		// if(dazed) { GD.Print(entity.Name + " is dazed"); }

		// if(stunned) { GD.Print(entity.Name + " is stunned"); }

		// if(chilled) { GD.Print(entity.Name + " is chilled"); }

		// if(frozen) { GD.Print(entity.Name + " is frozen"); }
	}

	public void AddStatusEffect(Entity entity, StatusEffect effect) // Adds status effect to entity
	{
		var effect_to_add = new StatusEffect();
		effect_to_add = GetEffect(entity, effect, effect_to_add); // Gets status effect to apply
		QueueStatusEffect(entity, effect_to_add); // Checks if ability has been applied
		if(effect_to_add.state == StatusEffect.States.queued)
		{
			
			// if(!effect_to_add.applied)
			// {
				GD.Print("adding new effect to " + entity.Name + " " + effect_to_add.name);
				entity.status_effects.Add(effect_to_add); // adds status effect to entities list of effects
				AddChild(effect_to_add); // adds the effect as a child setting all of its _Ready() values
				effect_to_add.StatusEffectFinished += () => HandleStatusEffectFinished(entity, effect_to_add); // Subscribes to effect finished signal
				if(effect_to_add.adds_additional_effects)
				{
					effect_to_add.AddAdditionalStatusEffect += (effect) => HandleAdditionalStatusEffect(effect, entity); // subscribes to add additional effect signal
				}
				ApplyStatusEffect(entity, effect_to_add);
				// Sets the variable for what the status effects are preventing or altering
				if(effect_to_add.prevents_movement){ movement_prevented = true; EmitSignal(nameof(MovementPrevented), movement_prevented);}
				if(effect_to_add.prevents_input){ input_prevented = true; EmitSignal(nameof(InputPrevented), input_prevented);}
				if(effect_to_add.alters_speed){ speed_altered = true; }
				if(effect_to_add.prevents_abilities){ abilities_prevented = true; EmitSignal(nameof(AbilitiesPrevented), abilities_prevented);}
				if(effect_to_add.name == "tether")
				{
					tethered = true;
					Tether tether_effect = (Tether)effect_to_add;
					EmitSignal(nameof(Tethered),entity, tether_effect.tether, tethered);
				
				}
			// }
			
		}
		else
		{
			ApplyStatusEffect(entity, effect_to_add);
		}
		
	}

   

    public bool SlowingEffectApplied(Entity entity, StatusEffect effect)
	{
		GD.Print("status effect slow in queue " + status_effects[slow]);
		GD.Print("status effect chill in queue " + status_effects[chill]);
		if(status_effects[slow] || status_effects[chill]) // status_effects[tether]
		{
			if(entity.status_effects.Contains(effect))
			{
				return false;
			}
			GD.Print("slowing effect applied");
			return true;
		}
		
		return false;
	}

	public bool StoppingEffectApplied(Entity entity, StatusEffect effect)
	{
		if(status_effects[freeze]) // || status_effects[fear] || status_effects[hamstrung] || status_effects[stunned] || status_effects[hexed]
		{ 
			if(entity.status_effects.Contains(effect))
			{
				return false;
			}
			return true;
		}
		
		return false;
	}

	public void RemoveStatusEffect(Entity entity, StatusEffect effect)
	{
		GD.Print("\n");
		GD.Print("removing " + effect.name);
		effect.QueueFree();
		entity.entity_controllers.status_effect_controller.SetEffectBooleans(effect);
		entity.status_effects.Remove(effect);
		if(!effect.removed)
		{
			GD.Print("removing from controller");
			effect.Remove(entity);
		}
		GD.Print("resetting stacks of " + effect.name);
		
		if(effect.prevents_movement == true){ movement_prevented = false; EmitSignal(nameof(MovementPrevented), movement_prevented);}
		if(effect.prevents_input){ input_prevented = false; EmitSignal(nameof(InputPrevented), input_prevented);}
		if(effect.alters_speed){ speed_altered = false; }
		if(effect.prevents_abilities){ abilities_prevented = false; EmitSignal(nameof(AbilitiesPrevented), abilities_prevented);}
		if(effect.name == "tether"){ tethered = false; EmitSignal(nameof(Tethered),entity, tether.tether, tethered);;}
		
	}

	// Queues effect if it hasn't been instantiated, because .prevents_movement, and .alters speed wont be set,
	// if the effect has been instantiated it will check those properties and wont queue if a similar effect is active
	public void QueueStatusEffect(Entity entity, StatusEffect effect) 
	{
		
		if(!effect.applied)
		{
			effect.state = StatusEffect.States.queued;
		}
		else
		{
			effect.state = StatusEffect.States.not_queued;
		}
	}

	public void ApplyStatusEffect(Entity entity, StatusEffect effect) // Sets booleans for each status effect, and applies the effect to the entity
	{
		GD.Print("effects count " + entity.status_effects.Count);
		GD.Print("Applying an effect of type " + effect.GetType());
		GD.Print("current stacks " + effect.current_stacks);
		if (effect.current_stacks < effect.max_stacks && entity.status_effects.Contains(effect))
		{
			if(effect.current_stacks == 0)
			{
				SetEffectBooleans(effect);
			}
			effect.Apply(entity);
			GD.Print("current stacks " + effect.current_stacks);
		}
		else
		{
			GD.Print("Can not add more stacks");
		}
	
	}

	public void RemoveMovementDebuffs(Entity entity)
	{
		GD.Print("entity status effects count before removing " + entity.status_effects.Count);
		for(int i = entity.status_effects.Count - 1; i >= 0 ; i--) // Iterates list in reverse so that the position of i is not disrupted
		{
			GD.Print("i " + i);
			if(entity.status_effects[i].type == StatusEffect.EffectType.debuff  && entity.status_effects[i].category == StatusEffect.EffectCategory.movement)
			{
				GD.Print("removing movement debuff " + entity.status_effects[i].name);
				RemoveStatusEffect(entity, entity.status_effects[i]);
			}
			
		}
		GD.Print("entity status effects count after removing " + entity.status_effects.Count);
		// foreach(StatusEffect effect in entity.status_effects)
		// {
		// 	if(effect.type == StatusEffect.EffectType.debuff && effect.category == StatusEffect.EffectCategory.movement)
		// 	{
				
		// 	}
		// }
	}

	public void SetEffectBooleans(StatusEffect effect) // Switches the effect from on to off or off to on in the dictionary
	{
		GD.Print("setting effect booleans");
		foreach(StatusEffect status_effect in status_effects.Keys)
		{
			if (effect.GetType() == status_effect.GetType())
			{
				status_effects[status_effect] = !status_effects[status_effect];
			}
			GD.Print("Status effect " + status_effect.GetType() + " " + status_effects[status_effect]);
		}

	}

	// Checks the incoming effect type, if the effect is not on, then a new instance of that effect will be created and returned,
	// if the effect is on the entities status effects will be searched and the matching effect will be returned
	public StatusEffect GetEffect(Entity entity, StatusEffect effect, StatusEffect effect_to_get) 
	{
		foreach(StatusEffect status_effect in status_effects.Keys)
		{
			if (effect.GetType() == status_effect.GetType())
			{
				
				if(!status_effects[status_effect])
				{
					effect_to_get = effect;
					// GD.Print("status effect being created");
					// effect_to_get = (StatusEffect)Activator.CreateInstance(status_effect.GetType());
				}
				else
				{
					foreach(StatusEffect applied_effect in entity.status_effects)
					{
						if(applied_effect.GetType() == status_effect.GetType())
						{
							GD.Print("status effect already exists, returning existing effect");
							effect_to_get = applied_effect;
						}
					}
				}
			}
		}
		
		return effect_to_get;
	}

	// Handlers for signals from the status effects
	 private void HandleAdditionalStatusEffect(StatusEffect effect, Entity entity)
    {
		GD.Print("received signal to add status effect");
        AddStatusEffect(entity, effect);
    }

    private void HandleStatusEffectFinished(Entity entity, StatusEffect effect_to_remove)
    {
		GD.Print("received signal to remove status effect " + effect_to_remove.Name);
		foreach(StatusEffect effect in status_effects.Keys)
		{
			if(effect_to_remove.GetType() == effect.GetType())
			{
				if(status_effects[effect])
				{
					
					RemoveStatusEffect(entity, effect_to_remove);
					effect_to_remove.StatusEffectFinished -= () => HandleStatusEffectFinished(entity, effect_to_remove);
					if(effect_to_remove.adds_additional_effects){ effect_to_remove.AddAdditionalStatusEffect -= (effect) => HandleAdditionalStatusEffect(effect, entity);}
				
					
				}
			}
		}
    }

}
