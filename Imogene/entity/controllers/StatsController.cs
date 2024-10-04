using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

// Stat controller
// Updates and sends stats for the player, more information on how the stats are calculated in the Imogene_Journal
public partial class StatsController : Node
{


	// Damage
	public float damage_per_second { get; set; } = 0.0f;
	public float critical_hit_modifier { get; set; } = 0.0f;
	public float damage_estimate { get; set; } = 0.0f;

	// Power Modifiers
	public float physical_melee_power_modifier { get; set; } = 0.0f;
	public float spell_melee_power_modifier { get; set; } = 0.0f;
	public float physical_ranged_power_modifier { get; set; } = 0.0f;
	public float spell_ranged_power_modifier { get; set; } = 0.0f;
	public float power_modifier_average { get; set; } = 0.0f;

	// Damage Modifiers
	public float average_damage_modifier { get; set; } = 0.0f;
	public float damage_resistance_level_scale { get; set; } = 0.0f;
	public float recovery_level_scale { get; set; } = 0.0f;
	public float average_damage_resistance { get; set; } = 0.0f;
	public float resistance { get; set; } = 0.0f;
 	public float recovery { get; set; } = 0.0f;

	public Stat[] base_stats { get; set; } = new Stat[8];
	public float[] summary_stats { get; set; } = new float[3];
	public Stat[] damage_modifiers { get; set; } = new Stat[12];
	public Stat[] resistance_stats { get; set; } = new Stat[12];
	public Stat[] depth_stats { get; set; } = new Stat[47];
	
	
	[Signal] public delegate void UpdateStatsEventHandler(Player player);

	public void SetDamage(Entity entity)
	{
		entity.combined_damage = entity.main_hand_damage.base_value  + entity.off_hand_damage.base_value  + entity.damage_bonus.base_value ;
		damage_per_second = entity.main_hand_attacks_per_second.base_value  * entity.combined_damage;
	}

	public void SetDamageEstimate(Entity entity)
	{
		damage_estimate = (float)Math.Round(damage_per_second * power_modifier_average * critical_hit_modifier, 2);
	}

	public void SetPowerModifiers(Entity entity)
	{
		physical_melee_power_modifier = 1 + (entity.physical_melee_power.base_value /100);
		spell_melee_power_modifier = 1 + (entity.spell_melee_power.base_value /100);
		physical_ranged_power_modifier = 1 + (entity.physical_ranged_power.base_value /100);
		spell_ranged_power_modifier = 1 + (entity.spell_ranged_power.base_value /100);
		power_modifier_average = (physical_melee_power_modifier + spell_melee_power_modifier + physical_ranged_power_modifier + spell_ranged_power_modifier) / 4;
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
		resistance = entity.health.max_value * average_damage_resistance * entity.armor.base_value;
	}

	

	public void SetRecoveryEstimate(Entity entity)
	{
		recovery = (float) Math.Round((entity.health_regeneration.base_value  + entity.resource_regeneration.base_value  + entity.posture_regeneration.base_value ) / 3, 2);
	}

	public void SetBaseStats(Entity entity)
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

	public void SetDepthStats(Entity entity)
	{
		depth_stats[0] = entity.physical_melee_power;
		depth_stats[1] = entity.spell_melee_power;
		depth_stats[2] = entity.physical_ranged_power;
		depth_stats[3] = entity.spell_ranged_power;
		depth_stats[4] = entity.wisdom_scaler;
		depth_stats[5] = entity.critical_hit_chance;
		depth_stats[6] = entity.critical_hit_damage;
		depth_stats[7] = entity.power;
		depth_stats[8] = entity.physical_damage;
		depth_stats[9] = entity.pierce_damage;
		depth_stats[10] = entity.slash_damage;
		depth_stats[11] = entity.blunt_damage;
		depth_stats[12] = entity.bleed_damage;
		depth_stats[13] = entity.poison_damage;
		depth_stats[14] = entity.curse_damage;
		depth_stats[15] = entity.spell_damage;
		depth_stats[16] = entity.fire_damage;
		depth_stats[17] = entity.cold_damage;
		depth_stats[18] = entity.lightning_damage;
		depth_stats[19] = entity.holy_damage;
		depth_stats[20] = entity.attack_speed_increase;
		depth_stats[21] = entity.cooldown_reduction;
		depth_stats[22] = entity.armor;
		depth_stats[23] = entity.poise;
		depth_stats[24] = entity.block_amount;
		depth_stats[25] = entity.retaliation;
		depth_stats[26] = entity.physical_resistance;
		depth_stats[27] = entity.pierce_resistance;
		depth_stats[28] = entity.slash_resistance;
		depth_stats[29] = entity.blunt_resistance;
		depth_stats[30] = entity.bleed_resistance;
		depth_stats[31] = entity.poison_resistance;
		depth_stats[32] = entity.curse_resistance;
		depth_stats[33] = entity.spell_resistance;
		depth_stats[34] = entity.fire_resistance;
		depth_stats[35] = entity.cold_resistance;
		depth_stats[36] = entity.lightning_resistance;
		depth_stats[37] = entity.holy_resistance;
		depth_stats[38] = entity.health;
		depth_stats[39] = entity.health_bonus;
		depth_stats[40] = entity.health_regeneration;
		depth_stats[41] = entity.health_on_retaliation;
		depth_stats[42] = entity.resource;
		depth_stats[43] = entity.resource_regeneration;
		depth_stats[44] = entity.resource_cost_reduction;
		depth_stats[45] = entity.posture_regeneration;
		depth_stats[46] = entity.movement_speed;
	}

	public void SetStats(Entity entity)
	{
		SetBaseStats(entity);
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
		SetDepthStats(entity);
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

	public void SetAttackSpeed(Entity entity)
	{
		// entity.main_hand_attacks_per_second = entity.main_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
		// entity.off_hand_attacks_per_second = entity.off_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
	}

	public void SetCriticalHitModifier(Entity entity)
	{
		critical_hit_modifier = 1 + (entity.critical_hit_chance.base_value  + entity.critical_hit_damage.base_value );
	}

	public void SetRegenerations(Entity entity)
	{
		entity.health_regeneration.base_value = (float)Math.Round(1 + entity.stamina.base_value / recovery_level_scale * entity.health_regeneration_bonus.base_value , 2);
		entity.resource_regeneration.base_value = (float)Math.Round(1 + entity.stamina.base_value  / recovery_level_scale * entity.resource_regeneration_bonus.base_value , 2);
		entity.posture_regeneration.base_value = (float)Math.Round(1 + entity.stamina.base_value  / recovery_level_scale * (1 + (entity.poise.base_value  / 100)), 2);
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
