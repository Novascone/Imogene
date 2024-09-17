using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;

public partial class StatusEffectController : Node
{
	

	
	public bool stop_movement_input;
	public bool effect_already_applied;
	// Movement

	// Buffs
	public bool unstoppable;
	public bool transpose;
	public bool bull;
	public bool stealth;
	
	// De-buffs
	public Slow slow = new();
	public bool slowed;
	public Daze daze = new();
	public bool dazed;
	public Chill chill = new();
	public bool chilled;
	public Freeze freeze = new();
	public bool frozen;
	public bool feared;
	public bool hamstrung;
	public bool tethered;
	public Stun stun = new();
	public bool stunned;
	public bool hexed;
	public bool knockedback;

	// Health
	
	// Buffs
	public bool invincible;
	public bool bulwark;
	public bool outstanding_form;

	// De-buffs
	public bool bleeding;
	public bool poisoned;
	public bool burning;
	public bool cursed;
	public bool virulent;
	public bool febrile;
	public bool reeling;
	public bool staggered;

	// Damage

	// Buffs
	public bool overpower;
	public bool walloping_blows;
	public bool formidable;
	public bool orb_of_power;

	// De-buffs
	public bool enfeebled;
	public bool suspended;
	public bool charmed;

	// General

	public bool on_fire;

	// Trade-off
	public bool convert_life;
	public bool convert_power;
	public bool taunting;

	public Dictionary<StatusEffect, bool> status_effects = new Dictionary<StatusEffect, bool>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		status_effects[slow] = false;
		status_effects[daze] = false;
		status_effects[chill] = false;
		status_effects[freeze] = false;
		status_effects[stun] = false;
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

	public void AddStatusEffect(Entity entity, StatusEffect effect)
	{
		var effect_to_add = new StatusEffect();
		effect_to_add = GetEffect(entity, effect, effect_to_add);
		QueueStatusEffect(entity, effect_to_add);
		if(effect_to_add.state == StatusEffect.States.queued)
		{
			GD.Print("effect queued");
			if(!effect_to_add.applied)
			{
				GD.Print("adding new effect to " + entity.Name + " " + effect_to_add.Name);
				entity.status_effects.Add(effect_to_add);
				AddChild(effect_to_add);
				if(effect_to_add.prevents_movement == true)
				{
					stop_movement_input = true;
				}
			}
			ApplyStatusEffect(entity, effect_to_add);
		}
	}

	public bool SlowingEffectApplied(Entity entity, StatusEffect effect)
	{
		if(slowed || chilled || tethered)
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
		if(frozen || feared || hamstrung || stunned || hexed)
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
		GD.Print("removing " + effect.Name);
		entity.status_effects.Remove(effect);
		RemoveChild(effect);
		GD.Print("resetting stacks of " + effect.Name);
		entity.entity_controllers.status_effect_controller.SetEffectBooleans(effect);
		if(effect.prevents_movement == true)
		{
			stop_movement_input = false;
		}
	}

	public void QueueStatusEffect(Entity entity, StatusEffect effect)
	{
		GD.Print("trying to queue status effect");
		if(effect.state == StatusEffect.States.not_queued)
		{   
			GD.Print("status effect not queued");
			if(effect.prevents_movement && !StoppingEffectApplied(entity, effect))
			{
				effect.state = StatusEffect.States.queued;
				GD.Print("queueing status effect");
			}
			else if(effect.alters_speed && !SlowingEffectApplied(entity, effect))
			{
				effect.state = StatusEffect.States.queued;
				GD.Print("queueing status effect");
			}
			else
			{
				effect.state = StatusEffect.States.queued;
			}
		}
	}

	public void ApplyStatusEffect(Entity entity, StatusEffect effect)
	{
		GD.Print("effects count " + entity.status_effects.Count);
		GD.Print("Applying an effect of type " + effect.GetType());
		if(!entity.status_effects.Contains(effect))
		{
			effect.Apply(entity);
			GD.Print("current stacks of " + effect.Name + effect.current_stacks);
		}
		else if (effect.current_stacks < effect.max_stacks && entity.status_effects.Contains(effect))
		{
			effect.Apply(entity);
			
			GD.Print("current stacks " + effect.current_stacks);
		}
		else
		{
			GD.Print("Can not add more stacks");
		}
	
	}

	public void SetEffectBooleans(StatusEffect effect)
	{
		foreach(StatusEffect status_effect in status_effects.Keys)
		{
			if (effect.GetType() == status_effect.GetType())
			{
				status_effects[status_effect] = !status_effects[status_effect];
			}
			GD.Print("Status effect " + status_effect.GetType() + " " + status_effects[status_effect]);
		}

	}

	
	public StatusEffect GetEffect(Entity entity, StatusEffect effect, StatusEffect effect_to_get)
	{

		foreach(StatusEffect status_effect in status_effects.Keys)
		{
			if (effect.GetType() == status_effect.GetType())
			{
				
				if(!status_effects[status_effect])
				{
					GD.Print("status effect being created");
					effect_to_get = (StatusEffect)Activator.CreateInstance(status_effect.GetType());
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

	

	

	// public void UpdateMovementEffectsCount()
	// {
	// 	if(entity.movement_effects.Count > entity.previous_movement_effects_count)
	// 	{
	// 		GD.Print("Incrementing");
	// 		entity.previous_movement_effects_count = entity.movement_effects.Count;
	// 	}
		
	// }
}
