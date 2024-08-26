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

		if(stunned) { GD.Print(entity.Name + " is stunned"); }
	}

	public void AddStatusEffect(StatusEffect effect)
	{
		if(!entity.status_effects.Contains(effect))
		{
			entity.status_effects.Add(effect);
			SetEffectBooleans(effect);
			effect.current_stacks += 1;
			GD.Print("current stacks " + effect.current_stacks);
			
		}
		else if (effect.current_stacks < effect.max_stacks)
		{
			effect.current_stacks += 1;
			GD.Print("current stacks " + effect.current_stacks);
		}
		else
		{
			GD.Print("Can not add more stacks");
		}
		
	}

	public void RemoveStatusEffect(StatusEffect effect)
	{
		entity.status_effects.Remove(effect);
		SetEffectBooleans(effect);
	}

	public void SetEffectBooleans(StatusEffect effect)
	{
		// Set movement effect booleans

		// if(effect is unstoppable) { unstoppable = !unstoppable; }

		// if(effect is Transpose) { transpose = !transpose; }

		// if(effect is Bull) { bull = !bull; }

		// if(effect is stealth){ stealth = !stealth; }

		if(effect is Slow) { slowed = !slowed; }

		// if(effect is Daze) { Dazed = !Dazed; }

		// if(effect is Chill) { chilled = !chilled; }

		// if(effect is Freeze) { frozen = !frozen; }

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
