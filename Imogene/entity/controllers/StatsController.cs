using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

// Stat controller
// Updates and sends stats for the player, more information on how the stats are calculated in the Imogene_Journal
public partial class StatsController : Node
{

	
	[Signal] public delegate void UpdateStatsEventHandler(Player player);

	
	

	// Damage
	
	public float damage_per_second;
	public float damage_estimate;

	// Power Modifiers
	public float physical_melee_power_modifier;
	public float spell_melee_power_modifier;
	public float physical_ranged_power_modifier;
	public float spell_ranged_power_modifier;
	public float power_modifier_average;

	// Damage Modifiers
	public float average_damage_modifier;

	public float average_damage_resistance;
	public float resistance;
	
	public float damage_resistance_level_scale;
	public float recovery_level_scale;
	public float recovery;

	public List<Stat> depth_stats = new List<Stat>();

	public Stat[] base_stats = new Stat[8];

	public float[] summary_stats = new float[3];

	public Stat[] resistance_stats = new Stat[12];
	public Stat[] damage_modifiers = new Stat[12];

	public void SetResistanceStats(Entity entity)
	{	
		resistance_stats[0] = entity.physical_resistance;
		resistance_stats[1] = entity.pierce_resistance;
		resistance_stats[2] = entity.slash_resistance;
		resistance_stats[3] = entity.blunt_resistance;
		resistance_stats[4] = entity.bleed_resistance;
		resistance_stats[5] = entity.poison_resistance;
		resistance_stats[6] = entity.curse_resistance;
		resistance_stats[7] = entity.spell_resistance;
		resistance_stats[8] = entity.fire_resistance;
		resistance_stats[9] = entity.cold_resistance;
		resistance_stats[10] = entity.lightning_resistance;
		resistance_stats[11] = entity.holy_resistance;
	}
	public void SetDamageModifierStats(Entity entity)
	{
		damage_modifiers[0] = entity.physical_damage;
		damage_modifiers[1] = entity.pierce_damage;
		damage_modifiers[2] = entity.slash_damage;
		damage_modifiers[3] = entity.blunt_damage;
		damage_modifiers[4] = entity.bleed_damage;
		damage_modifiers[5] = entity.poison_damage;
		damage_modifiers[6] = entity.curse_damage;
		damage_modifiers[7] = entity.spell_damage;
		damage_modifiers[8] = entity.fire_damage;
		damage_modifiers[9] = entity.cold_damage;
		damage_modifiers[10] = entity.lightning_damage;
		damage_modifiers[11] = entity.holy_damage;
	}
	public void SetUIBaseStats(Entity entity)
	{
		base_stats[0] = entity.level;
		base_stats[1] = entity.strength;
		base_stats[2] = entity.dexterity;
		base_stats[3] = entity.intellect;
		base_stats[4] = entity.vitality;
		base_stats[5] = entity.stamina;
		base_stats[6] = entity.wisdom;
		base_stats[7] = entity.charisma;
	}

	public void SetSummaryStats()
	{
		summary_stats[0] = damage_estimate;
		summary_stats[1] = resistance;
		summary_stats[2] = recovery;
	}

	public void SetUIDepthStats(Entity entity)
	{
		depth_stats.Add(entity.physical_melee_power);
		depth_stats.Add(entity.spell_melee_power);
		depth_stats.Add(entity.physical_ranged_power);
		depth_stats.Add(entity.spell_ranged_power);
		depth_stats.Add(entity.wisdom_scaler);
		depth_stats.Add(entity.critical_hit_chance);
		depth_stats.Add(entity.critical_hit_damage);
		depth_stats.Add(entity.power);
		depth_stats.Add(entity.physical_damage);
		depth_stats.Add(entity.pierce_damage);
		depth_stats.Add(entity.slash_damage);
		depth_stats.Add(entity.blunt_damage);
		depth_stats.Add(entity.bleed_damage);
		depth_stats.Add(entity.poison_damage);
		depth_stats.Add(entity.curse_damage);
		depth_stats.Add(entity.spell_damage);
		depth_stats.Add(entity.fire_damage);
		depth_stats.Add(entity.cold_damage);
		depth_stats.Add(entity.lightning_damage);
		depth_stats.Add(entity.holy_damage);
		depth_stats.Add(entity.attack_speed_increase);
		depth_stats.Add(entity.cooldown_reduction);
		depth_stats.Add(entity.armor);
		depth_stats.Add(entity.poise);
		depth_stats.Add(entity.block_amount);
		depth_stats.Add(entity.retaliation);
		depth_stats.Add(entity.physical_resistance);
		depth_stats.Add(entity.pierce_resistance);
		depth_stats.Add(entity.slash_resistance);
		depth_stats.Add(entity.blunt_resistance);
		depth_stats.Add(entity.bleed_resistance);
		depth_stats.Add(entity.poison_resistance);
		depth_stats.Add(entity.curse_resistance);
		depth_stats.Add(entity.spell_resistance);
		depth_stats.Add(entity.fire_resistance);
		depth_stats.Add(entity.cold_resistance);
		depth_stats.Add(entity.lightning_resistance);
		depth_stats.Add(entity.holy_resistance);
		depth_stats.Add(entity.health);
		depth_stats.Add(entity.health_bonus);
		depth_stats.Add(entity.health_regeneration);
		depth_stats.Add(entity.health_on_retaliation);
		depth_stats.Add(entity.resource);
		depth_stats.Add(entity.resource_regeneration);
		depth_stats.Add(entity.resource_cost_reduction);
		depth_stats.Add(entity.posture_regeneration);
		depth_stats.Add(entity.movement_speed);
	}

	public void SetUIStats(Entity entity)
	{
		SetUIBaseStats(entity);
		SetPowers(entity);
		SetWisdomScaler(entity);
		SetDamageModifiers(entity);
		SetDamage(entity);
		SetCriticalHitModifier(entity);
		SetPowerModifiers(entity);
		SetDamageEstimate(entity);
		SetScales(entity);
		SetDamageResistances(entity);
		SetResistanceEstimate(entity);
		SetSummaryStats();
		SetUIDepthStats(entity);
	}



    public void SetPowers(Entity entity)
	{
		entity.physical_melee_power.base_value = (2 * entity.strength.base_value ) + entity.dexterity.base_value ;
		entity.spell_melee_power.base_value = entity.strength.base_value  + entity.dexterity.base_value  + (3 * entity.intellect.base_value );
		entity.physical_ranged_power.base_value = entity.strength.base_value  + (3 * entity.dexterity.base_value );
		entity.spell_ranged_power.base_value = (2 * entity.dexterity.base_value ) + (3 * entity.intellect.base_value );		
	}

	public void SetWisdomScaler(Entity entity)
	{
		entity.wisdom_scaler.base_value = entity.wisdom.base_value /20;
	}

	public void SetDamageModifiers(Entity entity)
	{
		SetDamageModifierStats(entity);
		float damage_modifiers_total = 0;
		for(int i = 0; i < damage_modifiers.Length; i ++)
		{
			damage_modifiers_total += 1 + (damage_modifiers[i].base_value / 100);
		}

		
		average_damage_modifier = damage_modifiers_total / damage_modifiers.Length;
	}

	public void SetAttackSpeed(Entity entity)
	{
		// entity.main_hand_attacks_per_second = entity.main_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
		// entity.off_hand_attacks_per_second = entity.off_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
	}

	public void SetDamage(Entity entity)
	{
		entity.combined_damage = entity.main_hand_damage.base_value  + entity.off_hand_damage.base_value  + entity.damage_bonus.base_value ;
		damage_per_second = entity.main_hand_attacks_per_second.base_value  * entity.combined_damage;
	}

	public void SetCriticalHitModifier(Entity entity)
	{
		entity.critical_hit_modifier = 1 + (entity.critical_hit_chance.base_value  + entity.critical_hit_damage.base_value );
	}

	public void SetPowerModifiers(Entity entity)
	{
		physical_melee_power_modifier = 1 + (entity.physical_melee_power.base_value /100);
		spell_melee_power_modifier = 1 + (entity.spell_melee_power.base_value /100);
		physical_ranged_power_modifier = 1 + (entity.physical_ranged_power.base_value /100);
		spell_ranged_power_modifier = 1 + (entity.spell_ranged_power.base_value /100);
		power_modifier_average = (physical_melee_power_modifier + spell_melee_power_modifier + physical_ranged_power_modifier + spell_ranged_power_modifier) / 4;
	}

	public void SetDamageEstimate(Entity entity)
	{
		damage_estimate = (float)Math.Round(damage_per_second * power_modifier_average * entity.critical_hit_modifier, 2);
	}

	public void SetScales(Entity entity)
	{
		damage_resistance_level_scale = entity.level.base_value * 50;
		recovery_level_scale = entity.level.base_value * 100;
	}

	

	public void SetDamageResistances(Entity entity)
	{
		SetScales(entity);
		SetResistanceStats(entity);
		float total_resistances = 0;
		for(int i = 0; i < resistance_stats.Length; i ++)
		{
			total_resistances += (float)Math.Round(resistance_stats[i].base_value / damage_resistance_level_scale, 2);
		}

		
		average_damage_resistance = total_resistances / resistance_stats.Length;
		
	}

	public void SetResistanceEstimate(Entity entity)
	{
		resistance = entity.health.max_value * average_damage_resistance * entity.damage_resistance_armor;
	}

	public void SetRegenerations(Entity entity)
	{
		entity.health_regeneration.base_value = (float)Math.Round(1 + entity.stamina.base_value / recovery_level_scale * entity.health_regeneration_bonus.base_value , 2);
		entity.resource_regeneration.base_value = (float)Math.Round(1 + entity.stamina.base_value  / recovery_level_scale * entity.resource_regeneration_bonus.base_value , 2);
		entity.posture_regeneration.base_value = (float)Math.Round(1 + entity.stamina.base_value  / recovery_level_scale * (1 + (entity.poise.base_value  / 100)), 2);
	}

	public void SetRecoveryEstimate(Entity entity)
	{
		recovery = (float) Math.Round((entity.health_regeneration.base_value  + entity.resource_regeneration.base_value  + entity.posture_regeneration.base_value ) / 3, 2);
	}



	
	public void Update(Entity entity) // Updates stats 															*** NEEDS ADDITIONS AND TO CHANGE DAMAGE CALCULATIONS ***
	{


		// Calculates stats
	
		SetPowers(entity);
		SetWisdomScaler(entity);
		SetDamageModifiers(entity);
		SetDamage(entity);
		SetCriticalHitModifier(entity);
		SetPowerModifiers(entity);
		SetDamageEstimate(entity);

		// mitigation
		SetScales(entity);
		SetDamageResistances(entity);
		SetResistanceEstimate(entity);


	
		if(entity is Player player)
		{
			EmitSignal(nameof(UpdateStats), player);
		}

	}

}
