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
	
	
	[Signal] public delegate void UpdateStatsEventHandler(Player player_);

	public void SetDamage(Entity entity_)
	{
		entity_.combined_damage = entity_.main_hand_damage.base_value  + entity_.off_hand_damage.base_value  + entity_.damage_bonus.base_value ;
		damage_per_second = entity_.main_hand_attacks_per_second.base_value  * entity_.combined_damage;
	}

	public void SetDamageEstimate()
	{
		damage_estimate = (float)Math.Round(damage_per_second * power_modifier_average * critical_hit_modifier, 2);
	}

	public void SetPowerModifiers(Entity entity_)
	{
		physical_melee_power_modifier = 1 + (entity_.physical_melee_power.base_value /100);
		spell_melee_power_modifier = 1 + (entity_.spell_melee_power.base_value /100);
		physical_ranged_power_modifier = 1 + (entity_.physical_ranged_power.base_value /100);
		spell_ranged_power_modifier = 1 + (entity_.spell_ranged_power.base_value /100);
		power_modifier_average = (physical_melee_power_modifier + spell_melee_power_modifier + physical_ranged_power_modifier + spell_ranged_power_modifier) / 4;
	}

	public void SetDamageModifiers(Entity entity_)
	{
		SetDamageModifierStats(entity_);
		float damage_modifiers_total = 0;
		for(int i = 0; i < damage_modifiers.Length; i ++)
		{
			damage_modifiers_total += 1 + (damage_modifiers[i].base_value / 100);
		}

		
		average_damage_modifier = damage_modifiers_total / damage_modifiers.Length;
	}

	public void SetScales(Entity entity_)
	{
		damage_resistance_level_scale = entity_.level.base_value * 50;
		recovery_level_scale = entity_.level.base_value * 100;
	}

	public void SetDamageResistances(Entity entity_)
	{
		SetScales(entity_);
		SetResistanceStats(entity_);
		float total_resistances = 0;
		for(int i = 0; i < resistance_stats.Length; i ++)
		{
			total_resistances += (float)Math.Round(resistance_stats[i].base_value / damage_resistance_level_scale, 2);
		}

		
		average_damage_resistance = total_resistances / resistance_stats.Length;
		
	}

	public void SetResistanceEstimate(Entity entity_)
	{
		resistance = entity_.health.max_value * average_damage_resistance * entity_.armor.base_value;
	}

	

	public void SetRecoveryEstimate(Entity entity_)
	{
		recovery = (float) Math.Round((entity_.health_regeneration.base_value  + entity_.resource_regeneration.base_value  + entity_.posture_regeneration.base_value ) / 3, 2);
	}

	public void SetBaseStats(Entity entity_)
	{
		base_stats[0] = entity_.level;
		base_stats[1] = entity_.strength;
		base_stats[2] = entity_.dexterity;
		base_stats[3] = entity_.intellect;
		base_stats[4] = entity_.vitality;
		base_stats[5] = entity_.stamina;
		base_stats[6] = entity_.wisdom;
		base_stats[7] = entity_.charisma;
	}

	public void SetSummaryStats()
	{
		summary_stats[0] = damage_estimate;
		summary_stats[1] = resistance;
		summary_stats[2] = recovery;
	}

	public void SetDamageModifierStats(Entity entity_)
	{
		damage_modifiers[0] = entity_.physical_damage;
		damage_modifiers[1] = entity_.pierce_damage;
		damage_modifiers[2] = entity_.slash_damage;
		damage_modifiers[3] = entity_.blunt_damage;
		damage_modifiers[4] = entity_.bleed_damage;
		damage_modifiers[5] = entity_.poison_damage;
		damage_modifiers[6] = entity_.curse_damage;
		damage_modifiers[7] = entity_.spell_damage;
		damage_modifiers[8] = entity_.fire_damage;
		damage_modifiers[9] = entity_.cold_damage;
		damage_modifiers[10] = entity_.lightning_damage;
		damage_modifiers[11] = entity_.holy_damage;
	}

	public void SetResistanceStats(Entity entity_)
	{	
		resistance_stats[0] = entity_.physical_resistance;
		resistance_stats[1] = entity_.pierce_resistance;
		resistance_stats[2] = entity_.slash_resistance;
		resistance_stats[3] = entity_.blunt_resistance;
		resistance_stats[4] = entity_.bleed_resistance;
		resistance_stats[5] = entity_.poison_resistance;
		resistance_stats[6] = entity_.curse_resistance;
		resistance_stats[7] = entity_.spell_resistance;
		resistance_stats[8] = entity_.fire_resistance;
		resistance_stats[9] = entity_.cold_resistance;
		resistance_stats[10] = entity_.lightning_resistance;
		resistance_stats[11] = entity_.holy_resistance;
	}

	public void SetDepthStats(Entity entity_)
	{
		depth_stats[0] = entity_.physical_melee_power;
		depth_stats[1] = entity_.spell_melee_power;
		depth_stats[2] = entity_.physical_ranged_power;
		depth_stats[3] = entity_.spell_ranged_power;
		depth_stats[4] = entity_.wisdom_scaler;
		depth_stats[5] = entity_.critical_hit_chance;
		depth_stats[6] = entity_.critical_hit_damage;
		depth_stats[7] = entity_.power;
		depth_stats[8] = entity_.physical_damage;
		depth_stats[9] = entity_.pierce_damage;
		depth_stats[10] = entity_.slash_damage;
		depth_stats[11] = entity_.blunt_damage;
		depth_stats[12] = entity_.bleed_damage;
		depth_stats[13] = entity_.poison_damage;
		depth_stats[14] = entity_.curse_damage;
		depth_stats[15] = entity_.spell_damage;
		depth_stats[16] = entity_.fire_damage;
		depth_stats[17] = entity_.cold_damage;
		depth_stats[18] = entity_.lightning_damage;
		depth_stats[19] = entity_.holy_damage;
		depth_stats[20] = entity_.attack_speed_increase;
		depth_stats[21] = entity_.cooldown_reduction;
		depth_stats[22] = entity_.armor;
		depth_stats[23] = entity_.poise;
		depth_stats[24] = entity_.block_amount;
		depth_stats[25] = entity_.retaliation;
		depth_stats[26] = entity_.physical_resistance;
		depth_stats[27] = entity_.pierce_resistance;
		depth_stats[28] = entity_.slash_resistance;
		depth_stats[29] = entity_.blunt_resistance;
		depth_stats[30] = entity_.bleed_resistance;
		depth_stats[31] = entity_.poison_resistance;
		depth_stats[32] = entity_.curse_resistance;
		depth_stats[33] = entity_.spell_resistance;
		depth_stats[34] = entity_.fire_resistance;
		depth_stats[35] = entity_.cold_resistance;
		depth_stats[36] = entity_.lightning_resistance;
		depth_stats[37] = entity_.holy_resistance;
		depth_stats[38] = entity_.health;
		depth_stats[39] = entity_.health_bonus;
		depth_stats[40] = entity_.health_regeneration;
		depth_stats[41] = entity_.health_on_retaliation;
		depth_stats[42] = entity_.resource;
		depth_stats[43] = entity_.resource_regeneration;
		depth_stats[44] = entity_.resource_cost_reduction;
		depth_stats[45] = entity_.posture_regeneration;
		depth_stats[46] = entity_.movement_speed;
	}

	public void SetStats(Entity entity_)
	{
		SetBaseStats(entity_);
		SetPowers(entity_);
		SetWisdomScaler(entity_);
		SetDamageModifiers(entity_);
		SetDamage(entity_);
		SetCriticalHitModifier(entity_);
		SetPowerModifiers(entity_);
		SetDamageEstimate();
		SetScales(entity_);
		SetDamageResistances(entity_);
		SetResistanceEstimate(entity_);
		SetSummaryStats();
		SetDepthStats(entity_);
	}

    public void SetPowers(Entity entity_)
	{
		entity_.physical_melee_power.base_value = (2 * entity_.strength.base_value ) + entity_.dexterity.base_value ;
		entity_.spell_melee_power.base_value = entity_.strength.base_value  + entity_.dexterity.base_value  + (3 * entity_.intellect.base_value );
		entity_.physical_ranged_power.base_value = entity_.strength.base_value  + (3 * entity_.dexterity.base_value );
		entity_.spell_ranged_power.base_value = (2 * entity_.dexterity.base_value ) + (3 * entity_.intellect.base_value );		
	}

	public void SetWisdomScaler(Entity entity_)
	{
		entity_.wisdom_scaler.base_value = entity_.wisdom.base_value /20;
	}

	public void SetAttackSpeed(Entity entity_)
	{
		// entity.main_hand_attacks_per_second = entity.main_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
		// entity.off_hand_attacks_per_second = entity.off_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
	}

	public void SetCriticalHitModifier(Entity entity_)
	{
		critical_hit_modifier = 1 + (entity_.critical_hit_chance.base_value  + entity_.critical_hit_damage.base_value );
	}

	public void SetRegenerations(Entity entity_)
	{
		entity_.health_regeneration.base_value = (float)Math.Round(1 + entity_.stamina.base_value / recovery_level_scale * entity_.health_regeneration_bonus.base_value , 2);
		entity_.resource_regeneration.base_value = (float)Math.Round(1 + entity_.stamina.base_value  / recovery_level_scale * entity_.resource_regeneration_bonus.base_value , 2);
		entity_.posture_regeneration.base_value = (float)Math.Round(1 + entity_.stamina.base_value  / recovery_level_scale * (1 + (entity_.poise.base_value  / 100)), 2);
	}

	public void Update(Entity entity_) // Updates stats 															*** NEEDS ADDITIONS AND TO CHANGE DAMAGE CALCULATIONS ***
	{


		// Calculates stats
	
		SetPowers(entity_);
		SetWisdomScaler(entity_);
		SetDamageModifiers(entity_);
		SetDamage(entity_);
		SetCriticalHitModifier(entity_);
		SetPowerModifiers(entity_);
		SetDamageEstimate();

		// mitigation
		SetScales(entity_);
		SetDamageResistances(entity_);
		SetResistanceEstimate(entity_);


	
		if(entity_ is Player player)
		{
			EmitSignal(nameof(UpdateStats), player);
		}

	}

}
