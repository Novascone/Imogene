using Godot;
using System;
using System.Collections.Generic;


public partial class DamageSystem : Node
{
	// Timers
	[Export] public Timer health_regen_timer;
	public Timer dot_timer;
	public Timer slow_timer;
	public Timer stun_timer;

	public bool dead;

	// Damage numbers parameters
	private Node3D spawn_point;
	private float spread = 90;
	private float height = 60;
	[Export] PackedScene damage_number_3d_template;
	private Queue<DamageNumber3D> damage_number_3d_pool = new Queue<DamageNumber3D>();
	private StatModifier remove_health = new(StatModifier.ModificationType.AddCurrent);
	private StatModifier add_health  = new(StatModifier.ModificationType.AddCurrent);
	
	private CustomSignals _customSignals;
	// Called when the node enters the scene tree for the first time.
	
	public void SubscribeToHurtboxSignals(Entity entity)
	{
		entity.hurtbox.AreaEntered += (area) => OnHurtboxAreaEntered(area, entity);
		entity.hurtbox.BodyEntered += (body) => OnHurtboxBodyEntered(body, entity);
	}

	public void SubscribeEntityToHealthRegen(Entity entity)
	{
		health_regen_timer.Timeout += () => OnHealthRegenTickTimeout(entity);
	}

    private void OnHurtboxBodyEntered(Node3D body, Entity entity)
    {
        	if(body is RangedHitbox ranged_box)
			{
				foreach(StatusEffect status_effect in ranged_box.effects)
				{
					GD.Print("Applying " + status_effect.Name + " to " + Name);
					entity.entity_controllers.status_effect_controller.AddStatusEffect(entity, status_effect);
				}
				if(body is RangedHitbox)
				{
					GD.Print(entity.Name + " Is taking " + ranged_box.damage  + " damage");
					TakeDamage(entity, ranged_box.damage_type, ranged_box.damage, ranged_box.is_critical);
					entity.entity_systems.resource_system.Posture(entity, ranged_box.posture_damage);
				}
			}
    }

    private void OnHurtboxAreaEntered(Area3D area, Entity entity)
    {
         if(area is MeleeHitbox melee_box)
		{
			// GD.Print(Name + " hurtbox entered by " + melee_box.Name);
			if(area is MeleeHitbox)
			{
				foreach(StatusEffect status_effect in melee_box.effects)
				{
					GD.Print("Applying " + status_effect.Name + " to " + Name);
					entity.entity_controllers.status_effect_controller.AddStatusEffect(entity, status_effect);
				}
				
				TakeDamage(entity, melee_box.damage_type, melee_box.damage, melee_box.is_critical);
				entity.entity_systems.resource_system.Posture(entity, melee_box.posture_damage);
			}
		}
    }

    public void SpawnDamageNumber(float value, bool is_critical)
	{
		DamageNumber3D damage_number = GetDamageNumber();
		Vector3 position = spawn_point.GlobalTransform.Origin;
		AddChild(damage_number, true);
		damage_number.SetValuesAndAnimate(value, is_critical, position, height, spread);
	}

	public DamageNumber3D GetDamageNumber()
	{
		if(damage_number_3d_pool.Count > 0)
		{
			return damage_number_3d_pool.Dequeue();
		}

		else
		{
			DamageNumber3D new_damage_number = (DamageNumber3D)damage_number_3d_template.Instantiate();
			new_damage_number.TreeExiting += () => damage_number_3d_pool.Enqueue(new_damage_number);
			return new_damage_number;
		}
	}


    public bool Crit(Entity entity)
	{
		float random_float = GD.Randf();
		if(random_float < entity.critical_hit_chance.current_value)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void TakeDamage(Entity entity, string damage_type, float amount, bool is_critical) // Applies damage to an entity
	{
		
		amount = DamageMitigation(entity, damage_type, amount);
		
		if(entity.health.current_value - amount > 0)
		{
			remove_health.modification = StatModifier.ModificationType.AddCurrent;
			remove_health.value_to_add = -amount;
			entity.health.AddModifier(remove_health);
			GD.Print(entity.Name + " current health " + entity.health.current_value);
			if(entity is Enemy enemy)
			{
				enemy.ui.health_bar.Value = enemy.health.current_value;
				GD.Print("enemy ui health value " + enemy.ui.health_bar.Value);
				spawn_point = enemy.head;
				SpawnDamageNumber(amount, is_critical);
			}
			
			HealthRegen();
		}
		else
		{
			dead = true;
			GD.Print("dead");
		}
	}

	public void HealthRegen()
	{
		GD.Print("starting health regen timer");
		health_regen_timer.Start();
	}

	private void OnHealthRegenTickTimeout(Entity entity)
    {
		GD.Print("Heal regen tick timeout");
        if(entity.health.current_value < entity.health.max_value && entity.health_regeneration.current_value > 0)
		{
			add_health.modification = StatModifier.ModificationType.AddCurrent;
			add_health.value_to_add = entity.health_regeneration.current_value;
			entity.health.AddModifier(add_health);
			health_regen_timer.Start();
			if(entity is Enemy enemy)
			{
				enemy.ui.health_bar.Value = enemy.health.current_value;
			}
		}
    }

	public float DamageMitigation(Entity entity, string damage_type, float amount)
	{
		float mitigated_damage = amount;
		GD.Print(mitigated_damage + " of damage going into mitigation ");
		mitigated_damage *= 1 - (entity.armor.current_value / 100);
		// GD.Print("Damage reduced by armor to " + mitigated_damage);
		if(damage_type == "Slash" || damage_type == "Thrust" || damage_type == "Blunt")
		{
			mitigated_damage *= entity.physical_resistance.current_value;
			// GD.Print("Damage reduced by physical resistance to " + mitigated_damage);
			if(damage_type == "Slash")
			{
				mitigated_damage *= entity.slash_resistance.current_value;
				// GD.Print("Damage reduced by slash resistance to " + mitigated_damage);
				return MathF.Round(mitigated_damage);
				
			}
			if(damage_type == "Pierce")
			{
				mitigated_damage *= entity.slash_resistance.current_value;
				return MathF.Round(mitigated_damage);
			}
			if(damage_type == "Blunt")
			{
				mitigated_damage *= entity.blunt_resistance.current_value;
				return MathF.Round(mitigated_damage);
			}
		}
		if(damage_type == "Bleed")
		{
			mitigated_damage *= entity.bleed_resistance.current_value;
			GD.Print("Damage reduced by bleed resistance to " + mitigated_damage);
			return MathF.Round(mitigated_damage);
		}
		if(damage_type == "Poison")
		{
			mitigated_damage *= entity.poison_resistance.current_value;
			return MathF.Round(mitigated_damage);
		}
		if(damage_type == "Fire" || damage_type == "Cold" ||  damage_type == "Lightning" || damage_type == "Holy")
		{
			mitigated_damage *= entity.poison_resistance.current_value;
			if(damage_type == "Fire")
			{
				mitigated_damage *= entity.fire_resistance.current_value;
				return MathF.Round(mitigated_damage);
			}
			if(damage_type == "Cold")
			{
				mitigated_damage *= entity.cold_resistance.current_value;
				return MathF.Round(mitigated_damage);
			}
			if(damage_type == "Lightning")
			{
				mitigated_damage *= entity.lightning_resistance.current_value;
				return MathF.Round(mitigated_damage);
			}
			if(damage_type == "Holy")
			{
				mitigated_damage *= entity.holy_resistance.current_value;
				return MathF.Round(mitigated_damage);
			}
		}
		return MathF.Round(mitigated_damage);
	}

	// public void DoT(string damage_type, float amount, int duration)
	// {
	// 	dot_timer.Start();
	// 	entity.dot_in_seconds = amount / duration;
	// 	entity.dot_in_seconds = MathF.Round(entity.dot_in_seconds);
	// 	entity.taking_dot = true;
	// 	// GD.Print("Taking " + amount + " of " + damage_type + " over " + dot_timer.TimeLeft + " seconds ");
	// }

	// private void OnDoTTickTimeout()
	// {
	// 	GD.Print("One tick of " + entity.dot_in_seconds + " " + entity.dot_damage_type);
	// 	GD.Print("DoT duration " + entity.dot_duration);
	// 	if(entity.health > 0)
	// 	{
	// 		entity.health -= entity.dot_in_seconds;
	// 		entity.health = MathF.Round(entity.health);
	// 		if(entity is Enemy enemy)
	// 		{
	// 			enemy.health_bar.Value = entity.health;
	// 			_customSignals.EmitSignal(nameof(CustomSignals.EnemyHealthChangedUI), entity.health);
	// 		}
	// 	}
	// 	else
	// 	{
	// 		entity.dead = true;
	// 		GD.Print("Dead");
	// 	}
		
		
	// 	GD.Print(entity.identifier + " health " + entity.health);
	// 	entity.dot_duration -= 1;
	// 	if(entity.dot_duration == 0)
	// 	{
	// 		dot_timer.Stop();
	// 		entity.dot_damage_type = null;
	// 		entity.taking_dot = false;
	// 	}
	// }

	// public void Slow()
	// {
	// 	slow_timer.Start();
	// 	if(!entity.slowed)
	// 	{
	// 		entity.speed /= 2;
	// 	}
		
	// 	entity.slowed = true;
	// }

	//  private void OnSlowTickTimeout()
    // {
    //     GD.Print(entity.identifier + " is slowed for " + entity.slow_duration);
		
	// 	entity.slow_duration -= 1;
	// 	if(entity.slow_duration == 0)
	// 	{
	// 		slow_timer.Stop();
	// 		entity.slowed = false;
	// 		entity.speed = entity.speed *= 2;
	// 	}
    // }

	// public void Stun()
	// {
	// 	stun_timer.Start();
	// 	entity.can_move = false;
	// 	entity.stunned = true;
	// 	if(entity.posture_broken)
	// 	{
	// 		GD.Print(entity.identifier + " posture broken");
	// 	}
	// }

	// private void OnStunTickTimeout()
    // {

    //    GD.Print(entity.identifier + " is stunned for " + entity.stun_duration);

	// 	if(entity.stun_duration > 0)
	// 	{
	// 		entity.stun_duration -= 1;
	// 	}
	// 	else
	// 	{
	// 		entity.stun_duration = 5;
	// 	}
	   
	//    if(entity.stun_duration == 0)
	//    {
	// 		stun_timer.Stop();
	// 		entity.stunned = false;
	// 		entity.can_move = true;
	//    }
    // }

}
