using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

// Stat controller
// Updates and sends stats for the player, more information on how the stats are calculated in the Imogene_Journal
public partial class StatsController : Node
{

	
	[Signal] public delegate void StatsUpdateEventHandler(Player player);

	
	

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



    public void SetPowers(Entity entity)
	{
		entity.physical_melee_power.base_value = (2 * entity.strength.current_value) + entity.dexterity.current_value;
		entity.spell_melee_power.base_value = entity.strength.current_value + entity.dexterity.current_value + (3 * entity.intellect.current_value);
		entity.physical_ranged_power.base_value = entity.strength.current_value + (3 * entity.dexterity.current_value);
		entity.spell_ranged_power.base_value = (2 * entity.dexterity.current_value) + (3 * entity.intellect.current_value);		
	}

	public void SetWisdomScaler(Entity entity)
	{
		entity.wisdom_scaler.base_value = entity.wisdom.current_value/20;
	}

	public void SetDamageModifiers(Entity entity)
	{
		for(int i = 0; i < entity.damage_stats.Count; i ++)
		{
			entity.damage_modifiers[i] = 1 + (entity.damage_stats[i].current_value / 100);
		}

		float damage_modifiers_total = 0;

		foreach(float modifier in entity.damage_modifiers)
		{
			damage_modifiers_total += modifier;
		}
		average_damage_modifier = damage_modifiers_total / entity.damage_modifiers.Count;
	}

	public void SetDamage(Entity entity)
	{
		GD.Print("setting combined damage");
		entity.combined_damage = entity.main_hand_damage.current_value + entity.off_hand_damage.current_value + entity.damage_bonus.current_value;
		damage_per_second = entity.attacks_per_second.current_value * entity.combined_damage;
	}

	public void SetCriticalHitModifier(Entity entity)
	{
		entity.critical_hit_modifier = 1 + (entity.critical_hit_chance.current_value + entity.critical_hit_damage.current_value);
	}

	public void SetPowerModifiers(Entity entity)
	{
		physical_melee_power_modifier = 1 + (entity.physical_melee_power.current_value/100);
		spell_melee_power_modifier = 1 + (entity.spell_melee_power.current_value/100);
		physical_ranged_power_modifier = 1 + (entity.physical_ranged_power.current_value/100);
		spell_ranged_power_modifier = 1 + (entity.spell_ranged_power.current_value/100);
		power_modifier_average = (physical_melee_power_modifier + spell_melee_power_modifier + physical_ranged_power_modifier + spell_ranged_power_modifier) / 4;
	}

	public void SetDamageEstimate(Entity entity)
	{
		damage_estimate = (float)Math.Round(damage_per_second * power_modifier_average * entity.critical_hit_modifier, 2);
	}

	public void SetScales(Entity entity)
	{
		damage_resistance_level_scale = entity.level.current_value * 50;
		recovery_level_scale = entity.level.current_value * 100;
	}

	

	public void SetDamageResistances(Entity entity)
	{

		for(int i = 0; i < entity.resistance_stats.Count; i ++)
		{
			entity.resistances[i] = (float)Math.Round(entity.resistance_stats[i].current_value / damage_resistance_level_scale, 2);
		}

		float total_resistance = 0;

		foreach(float resistance in entity.resistances)
		{
			total_resistance += resistance;
		}
		average_damage_resistance = total_resistance / entity.resistances.Count;
		
	}

	public void SetResistanceEstimate(Entity entity)
	{
		resistance = entity.health.max_value * average_damage_resistance * entity.damage_resistance_armor;
	}

	public void SetRegenerations(Entity entity)
	{
		entity.health_regeneration.base_value = (float)Math.Round(1 + entity.stamina.current_value / recovery_level_scale * entity.health_regeneration_bonus.current_value, 2);
		entity.resource_regeneration.base_value = (float)Math.Round(1 + entity.stamina.current_value / recovery_level_scale * entity.resource_regeneration_bonus.current_value, 2);
		entity.posture_regeneration.base_value = (float)Math.Round(1 + entity.stamina.current_value / recovery_level_scale * (1 + (entity.poise.current_value / 100)), 2);
	}

	public void SetRecoveryEstimate(Entity entity)
	{
		recovery = (float) Math.Round((entity.health_regeneration.current_value + entity.resource_regeneration.current_value + entity.posture_regeneration.current_value) / 3, 2);
	}



	
	public void UpdateStats(Entity entity) // Updates stats 															*** NEEDS ADDITIONS AND TO CHANGE DAMAGE CALCULATIONS ***
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


		

		// recovery

		
		if(entity is Player player)
		{
			EmitSignal(nameof(StatsUpdate), player);
		}
		// if(player != null)
		// {
		// 	player.ui.hud.health.MaxValue = entity.maximum_health;

		// }
		

		// GD.Print("combined damage " + combined_damage);
		// GD.Print("base dps " + base_dps);
		// GD.Print("aps " + aps);
		// GD.Print("skill mod " + skill_mod);
		// GD.Print("crit mod " + crit_mod);
		// GD.Print("physical melee power mod " + physical_melee_power_mod);
		// GD.Print("spell melee power mod " + spell_melee_power_mod);
		// GD.Print("physical ranged power mod " + physical_ranged_power_mod);
		// GD.Print("spell ranged power mod " + spell_ranged_power_mod);
		// GD.Print("physical melee dps " + physical_melee_dps);
		// GD.Print("spell melee dps " + spell_melee_dps);
		// GD.Print("physical ranged dps " + physical_ranged_dps);
		// GD.Print("spell ranged dps " + spell_ranged_dps);

		// GD.Print("dr_armor " + dr_armor);
		// GD.Print("dr_phys " + dr_phys);
		// GD.Print("dr_slash " + dr_slash);
		// GD.Print("dr_thrust " + dr_thrust);
		// GD.Print("dr_blunt " + dr_blunt);
		// GD.Print("dr_bleed " + dr_bleed);
		// GD.Print("dr_poison " + dr_poison);
		// GD.Print("dr_spell " + dr_spell);
		// GD.Print("dr_fire " + dr_fire);
		// GD.Print("dr_cold " + dr_cold);
		// GD.Print("dr_lightning " + dr_lightning);
		// GD.Print("dr_holy " + dr_holy);

		// GD.Print("avg_res " + avg_res_dr);
		// GD.Print("maximum health " + maximum_health);
		// GD.Print("resistance " + resistance);

		// GD.Print("health_regen " + player.health_regen);
		// GD.Print("resource_regen " + player.resource_regen);
		// GD.Print("posture_regen " + player.posture_regen);
		// GD.Print("recovery " + player.recovery);
		// GD.Print("Stats have been updated from the stat controller");
		// if(entity is Player)
		// {
		// 	SendStats();
		// }
		
		
	}

	// public void SendStats()
	// {
	// 	GD.Print("sending stats from stat controller");
	// 	_customSignals.EmitSignal(nameof(CustomSignals.SendStats),
	// 	entity.level, entity.strength, entity.dexterity, entity.intellect, entity.vitality, entity.stamina, entity.wisdom, entity.charisma, entity.total_dps,

	// 	entity.physical_melee_dps, entity.spell_melee_dps, entity.physical_ranged_dps, entity.spell_ranged_dps, entity.physical_melee_power, 

	// 	entity.spell_melee_power, entity.physical_ranged_power, entity.spell_ranged_power, entity.wisdom_scaler, entity.physical_melee_power_mod,

	// 	entity.physical_ranged_power_mod, entity.spell_ranged_power_mod, entity.power_mod_avg, entity.damage_bonus, entity.combined_damage, entity.base_aps,

	// 	entity.aps_modifiers, entity.aps, entity.base_dps, entity.skill_mod, entity.crit_mod, entity.slash_damage, entity.thrust_damage, entity.blunt_damage, entity.bleed_damage,

	// 	entity.poison_damage, entity.fire_damage, entity.cold_damage, entity.lightning_damage, entity.holy_damage, entity.critical_hit_chance, entity.critical_hit_damage, entity.attack_speed_increase,

	// 	entity.cool_down_reduction, entity.posture_damage, entity.armor, entity.poise, entity.block_amount, entity.retaliation, entity.physical_resistance, entity.thrust_resistance, entity.slash_resistance,

	// 	entity.blunt_resistance, entity.bleed_resistance, entity.poison_resistance, entity.curse_resistance, entity.spell_resistance, entity.fire_resistance, entity.cold_resistance, entity.lightning_resistance,
		
	// 	entity.holy_resistance, entity.maximum_health, entity.health_bonus, entity.health_regen, entity.health_regen_bonus, entity.maximum_posture, entity.posture_regen, entity.posture_regen_bonus, entity.health_on_retaliate, entity.resistance,

	// 	entity.maximum_resource, entity.resource_regen, entity.resource_cost_reduction, entity.recovery, entity.movement_speed, entity.maximum_gold
	// 															);
	// }
}
