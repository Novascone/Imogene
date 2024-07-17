using Godot;
using System;

public partial class DamageSystem : EntitySystem
{
	
	public Timer dot_timer;
	public Timer slow_timer;
	public Timer stun_timer;
	
	private CustomSignals _customSignals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dot_timer = GetNode<Timer>("DoTTimer");
		dot_timer.Timeout += OnDoTTickTimeout;
		slow_timer = GetNode<Timer>("SlowTimer");

		
		stun_timer = GetNode<Timer>("StunTimer");

		slow_timer.Timeout += OnSlowTickTimeout;
		stun_timer.Timeout += OnStunTickTimeout;

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	 public bool Crit()
	{
		float random_float = GD.Randf();
		if(random_float < entity.critical_hit_chance)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void TakeDamage(string damage_type, float amount, bool is_critical) // Applies damage to an entity
	{
		
		amount = DamageMitigation(damage_type, amount);
		GD.Print("Amount of damage " + amount);
		if(entity.health - amount > 0)
		{
			entity.health -= amount;
			entity.health = MathF.Round(entity.health,2);
			if(entity is Enemy enemy)
			{
				enemy.health_bar.Value = entity.health;
				_customSignals.EmitSignal(nameof(CustomSignals.EnemyHealthChangedUI), enemy, entity.health);
			}
		}
		else
		{
			entity.dead = true;
			GD.Print("dead");
		}

		GD.Print(entity.identifier + " took " + amount + " of " + damage_type + " damage") ;
		GD.Print(entity.identifier + " " + entity.health);

		if(is_critical)
		{
			if(damage_type == "Slash" || damage_type == "Fire")
			{
				if(entity.taking_dot)
				{
					entity.dot_duration += 5;
					GD.Print("Already taking DoT added more to duration");
				}
				else
				{
					entity.dot_duration = 5;
				}
				
				if(damage_type == "Slash")
				{
					entity.dot_damage_type = "Bleed";
				}
				else
				{
					entity.dot_damage_type = damage_type;
				}
				DoT(entity.dot_damage_type, DamageMitigation(entity.dot_damage_type,(float)(amount * 5)), entity.dot_duration);
				GD.Print("The hit is critical");
			}
			if(damage_type == "Cold")
			{
				if(entity.slowed)
				{
					entity.slow_duration += 5;
				}
				else
				{
					entity.slow_duration = 5;
				}
				Slow();
			}
			if(damage_type == "Lightning")
			{
				if(entity.stunned)
				{
					entity.stun_duration += 5;
				}
				else
				{
					entity.stun_duration = 5;
				}
				Stun();
			}
			
		}
	}

	public float DamageMitigation(string damage_type, float amount)
	{
		float mitigated_damage = amount;
		GD.Print(mitigated_damage + " of damage going into mitigation ");
		mitigated_damage *= 1 - entity.dr_armor;
		GD.Print("Damage reduced by armor to " + mitigated_damage);
		if(damage_type == "Slash" || damage_type == "Thrust" || damage_type == "Blunt")
		{
			mitigated_damage *= 1 - entity.dr_phys;
			GD.Print("Damage reduced by physical resistance to " + mitigated_damage);
			if(damage_type == "Slash")
			{
				mitigated_damage *= 1 - entity.dr_slash;
				GD.Print("Damage reduced by slash resistance to " + mitigated_damage);
				return MathF.Round(mitigated_damage,2);
				
			}
			if(damage_type == "Thrust")
			{
				mitigated_damage *= 1 - entity.dr_thrust;
				return MathF.Round(mitigated_damage,2);
			}
			if(damage_type == "Blunt")
			{
				mitigated_damage *= 1 - entity.dr_blunt;
				return MathF.Round(mitigated_damage,2);
			}
		}
		if(damage_type == "Bleed")
		{
			mitigated_damage *= 1 - entity.dr_bleed;
			GD.Print("Damage reduced by bleed resistance to " + mitigated_damage);
			return MathF.Round(mitigated_damage,2);
		}
		if(damage_type == "Poison")
		{
			mitigated_damage *= 1 - entity.dr_poison;
			return MathF.Round(mitigated_damage,2);
		}
		if(damage_type == "Fire" || damage_type == "Cold" ||  damage_type == "Lightning" || damage_type == "Holy")
		{
			mitigated_damage *= 1 - entity.dr_spell;
			if(damage_type == "Fire")
			{
				mitigated_damage *= 1 - entity.dr_fire;
				return MathF.Round(mitigated_damage,2);
			}
			if(damage_type == "Cold")
			{
				mitigated_damage *= 1 - entity.dr_cold;
				return MathF.Round(mitigated_damage,2);
			}
			if(damage_type == "Lightning")
			{
				mitigated_damage *= 1 - entity.dr_lightning;
				return MathF.Round(mitigated_damage,2);
			}
			if(damage_type == "Holy")
			{
				mitigated_damage *= 1 - entity.dr_holy;
				return MathF.Round(mitigated_damage,2);
			}
		}
		return MathF.Round(mitigated_damage,2);
	}

	public void DoT(string damage_type, float amount, int duration)
	{
		dot_timer.Start();
		entity.dot_in_seconds = amount / duration;
		entity.dot_in_seconds = MathF.Round(entity.dot_in_seconds, 2);
		entity.taking_dot = true;
		// GD.Print("Taking " + amount + " of " + damage_type + " over " + dot_timer.TimeLeft + " seconds ");
	}

	private void OnDoTTickTimeout()
	{
		GD.Print("One tick of " + entity.dot_in_seconds + " " + entity.dot_damage_type);
		GD.Print("DoT duration " + entity.dot_duration);
		if(entity.health > 0)
		{
			entity.health -= entity.dot_in_seconds;
			entity.health = MathF.Round(entity.health,2);
			if(entity is Enemy enemy)
			{
				enemy.health_bar.Value = entity.health;
				_customSignals.EmitSignal(nameof(CustomSignals.EnemyHealthChangedUI), entity.health);
			}
		}
		else
		{
			entity.dead = true;
			GD.Print("Dead");
		}
		
		
		GD.Print(entity.identifier + " health " + entity.health);
		entity.dot_duration -= 1;
		if(entity.dot_duration == 0)
		{
			dot_timer.Stop();
			entity.dot_damage_type = null;
			entity.taking_dot = false;
		}
	}

	public void Slow()
	{
		slow_timer.Start();
		if(!entity.slowed)
		{
			entity.speed /= 2;
		}
		
		entity.slowed = true;
	}

	 private void OnSlowTickTimeout()
    {
        GD.Print(entity.identifier + " is slowed for " + entity.slow_duration);
		
		entity.slow_duration -= 1;
		if(entity.slow_duration == 0)
		{
			slow_timer.Stop();
			entity.slowed = false;
			entity.speed = entity.speed *= 2;
		}
    }

	public void Stun()
	{
		stun_timer.Start();
		entity.can_move = false;
		entity.stunned = true;
		if(entity.posture_broken)
		{
			GD.Print(entity.identifier + " posture broken");
		}
	}

	private void OnStunTickTimeout()
    {

       GD.Print(entity.identifier + " is stunned for " + entity.stun_duration);

		if(entity.stun_duration > 0)
		{
			entity.stun_duration -= 1;
		}
		else
		{
			entity.stun_duration = 5;
		}
	   
	   if(entity.stun_duration == 0)
	   {
			stun_timer.Stop();
			entity.stunned = false;
			entity.can_move = true;
	   }
    }

}
