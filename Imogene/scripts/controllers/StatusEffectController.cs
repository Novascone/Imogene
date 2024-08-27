using Godot;
using System;
using System.Dynamic;

public partial class StatusEffectController : Controller
{
	// Movement

	// Buffs
	public bool unstoppable;
	public bool transpose;
	public bool bull;
	public bool stealth;
	
	// De-buffs
	public bool slowed;
	public bool dazed;
	public bool chilled;
	public bool frozen;
	public bool feared;
	public bool hamstrung;
	public bool tethered;
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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(slowed) { GD.Print(entity.Name + " is slowed"); }

		if(dazed) { GD.Print(entity.Name + " is dazed"); }

		if(stunned) { GD.Print(entity.Name + " is stunned"); }

		if(chilled) { GD.Print(entity.Name + " is chilled"); }

		if(frozen) { GD.Print(entity.Name + " is frozen"); }
	}

	public void AddStatusEffect(StatusEffect effect)
	{
		GD.Print("adding status effect");
		QueueStatusEffect(effect);
		if(effect.state == StatusEffect.States.queued)
		{
			GD.Print("effect queued");
			ApplyStatusEffect(effect);
		}
		
		
	}

	public bool SlowingEffectApplied( StatusEffect effect)
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

	public bool StoppingEffectApplied(StatusEffect effect)
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

	public void RemoveStatusEffect(StatusEffect effect)
	{
		GD.Print("removing " + effect.Name);
		entity.status_effects.Remove(effect);
		GD.Print("resetting stacks of " + effect.Name);
		SetEffectBooleans(effect);
	}

	public void QueueStatusEffect(StatusEffect effect)
	{
		GD.Print("trying to queue status effect");
		if(effect.state == StatusEffect.States.not_queued)
		{   
			GD.Print("status effect not queued");
			if(effect.resource.prevents_movement && !StoppingEffectApplied(effect))
			{
				effect.state = StatusEffect.States.queued;
				GD.Print("queueing status effect");
			}
			else if(effect.resource.alters_speed && !SlowingEffectApplied(effect))
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

	public void ApplyStatusEffect(StatusEffect effect)
	{
		GD.Print("effects count " + entity.status_effects.Count);
		if(!entity.status_effects.Contains(effect))
		{
			effect.Apply(entity);
			GD.Print("current stacks " + effect.current_stacks);
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
	
		// Set movement effect booleans

		// if(effect is unstoppable) { unstoppable = !unstoppable; }

		// if(effect is Transpose) { transpose = !transpose; }

		// if(effect is Bull) { bull = !bull; }

		// if(effect is stealth){ stealth = !stealth; }

		if(effect is Slow) { slowed = !slowed; }

		if(effect is Daze) { dazed = !dazed; }

		if(effect is Chill) { chilled = !chilled; }

		if(effect is Freeze) { frozen = !frozen; }

		// if(effect is Fear) { feared = !feared; }

		// if(effect is Hamstring) { hamstrung = !hamstrung; }

		// if(effect is Tether) { tethered = !tethered; }

		if(effect is Stun) { stunned = !stunned; }

		// if(effect is Hex) { hexed = !hexed; }

		// if(effect is Knockback) { knockedback = !knockedback; }

		// Set health effect booleans

		// if(effect is Invincible) { invincible = !invincible; }

		// if(effect is Knockback) { bulwark = !bulwark; }

		// if(effect is OutstandingForm) { outstanding_form = !outstanding_form; }

		// if(effect is Bleed) { bleeding = !bleeding; }

		// if(effect is Poison) { poisoned = !poisoned; }

		// if(effect is Burn) { burning = !burning; }

		// if(effect is Curse) { cursed = !cursed; }

		// if(effect is Virulent) { virulent = !virulent; }

		// if(effect is Febrile) { febrile = !febrile; }

		// if(effect is Reel) { reeling = !reeling; }

		// if(effect is Stagger) { staggered = !staggered; }

		// Set damage booleans

		// if(effect is Overpower) { overpower = !overpower; }

		// if(effect is Wallop) { walloping_blows = !walloping_blows; }

		// if(effect is Formidable) { formidable = !formidable; }

		// if(effect is OrbOfPower) { orb_of_power = !orb_of_power; }

		// if(effect is Enfeeble) { enfeebled = !enfeebled; }

		// if(effect is Suspend) { suspended = !suspended; }

		// if(effect is Charm) { charmed = !charmed; }

		// Set general booleans

		// if(effect is OnFire) { on_fire = !on_fire; }

		// Set trade-off booleans

		// if(effect is ConvertLife) { convert_life = !convert_life; }

		// if(effect is ConvertPower) { convert_power = !convert_power; }

		// if(effect is Taunt) { taunting = !taunting; }

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
