using Godot;
using System;
using System.Collections;

// Stat controller
// Updates and sends stats for the player, more information on how the stats are calculated in the Imogene_Journal
public partial class StatsController : Node
{

	
	[Signal] public delegate void StatsUpdateEventHandler(Player player);
	
	

	public void UpdateStats(Entity entity) // Updates stats 															*** NEEDS ADDITIONS AND TO CHANGE DAMAGE CALCULATIONS ***
	{


		// Calculates stats
		

		entity.calculation_stats["physical_melee_power"] = (2 * entity.base_stats["strength"]) + entity.base_stats["dexterity"];
		entity.calculation_stats["physical_ranged_power"] = entity.base_stats["strength"] + (3 * entity.base_stats["dexterity"]);
		entity.calculation_stats["spell_melee_power"] = entity.base_stats["strength"] + entity.base_stats["dexterity"] + (3 * entity.base_stats["intellect"]);
		entity.calculation_stats["spell_ranged_power"] = (2 * entity.base_stats["dexterity"]) + (3 * entity.base_stats["intellect"]);

		// physical_melee_damage = physical_melee_power/15;
		// physical_ranged_damage = physical_ranged_power/15;
		// spell_melee_damage = spell_melee_power/15;
		// spell_ranged_damage = spell_ranged_power/15;
		entity.depth_stats["wisdom_scaler"] = entity.base_stats["wisdom"]/20;

		// damage
		entity.calculation_stats["combined_damage"] = entity.calculation_stats["main_hand_damage"] + entity.calculation_stats["off_hand_damage"] + entity.calculation_stats["damage_bonus"];
		entity.calculation_stats["aps"] = entity.calculation_stats["base_aps"] * (1 + entity.calculation_stats["aps_modifiers"]);
		entity.calculation_stats["base_dps"] = entity.calculation_stats["aps"] * entity.calculation_stats["combined_damage"];
		entity.calculation_stats["combined_damage"] = entity.calculation_stats["main_hand_damage"] + entity.calculation_stats["off_hand_damage"] + 2 + 1 + 1;

		entity.calculation_stats["aps"] = 1.71f;
		entity.calculation_stats["base_dps"] = entity.calculation_stats["aps"] * entity.calculation_stats["combined_damage"];
		entity.calculation_stats["skill_mod"] = 1.15f;
		entity.calculation_stats["crit_mod"] = 1 + (entity.depth_stats["critical_hit_chance"] * entity.depth_stats["critical_hit_damage"]);

		entity.calculation_stats["physical_melee_power_mod"] = 1 + (entity.calculation_stats["physical_melee_power"]/100);
		entity.calculation_stats["spell_melee_power_mod"] = 1 + (entity.calculation_stats["spell_melee_power"]/100);
		entity.calculation_stats["physical_ranged_power_mod"] = 1 + (entity.calculation_stats["physical_ranged_power"]/100);
		entity.calculation_stats["spell_ranged_power_mod"] = 1 + (entity.calculation_stats["spell_ranged_power"]/100);

		entity.calculation_stats["power_mod_avg"] = (entity.calculation_stats["physical_melee_power_mod"] + entity.calculation_stats["spell_melee_power_mod"] + entity.calculation_stats["physical_ranged_power_mod"] + entity.calculation_stats["spell_ranged_power_mod"]) / 4;

		entity.calculation_stats["physical_melee_dps"] = entity.calculation_stats["base_dps"] * entity.calculation_stats["physical_melee_power_mod"] * entity.calculation_stats["skill_mod"] * entity.calculation_stats["crit_mod"];
		entity.calculation_stats["spell_melee_dps"] = entity.calculation_stats["base_dps"] * entity.calculation_stats["spell_melee_power_mod"] * entity.calculation_stats["skill_mod"] * entity.calculation_stats["crit_mod"];
		entity.calculation_stats["physical_ranged_dps"] = entity.calculation_stats["base_dps"] * entity.calculation_stats["physical_ranged_power_mod"] * entity.calculation_stats["skill_mod"] * entity.calculation_stats["crit_mod"];
		entity.calculation_stats["spell_ranged_dps"] = entity.calculation_stats["base_dps"] * entity.calculation_stats["spell_ranged_power_mod"] * entity.calculation_stats["skill_mod"] * entity.calculation_stats["crit_mod"];

		entity.calculation_stats["physical_melee_dps"] = (float)Math.Round(entity.calculation_stats["physical_melee_dps"], 2);
		entity.calculation_stats["spell_melee_dps"] = (float)Math.Round(entity.calculation_stats["spell_melee_dps"], 2);
		entity.calculation_stats["physical_ranged_dps"] = (float)Math.Round(entity.calculation_stats["physical_ranged_dps"], 2);
		entity.calculation_stats["spell_ranged_dps"] = (float)Math.Round(entity.calculation_stats["spell_ranged_dps"], 2);

		entity.calculation_stats["total_dps"] = (float)Math.Round(entity.calculation_stats["base_dps"] * entity.calculation_stats["power_mod_avg"] * entity.calculation_stats["skill_mod"] * entity.calculation_stats["crit_mod"], 2);
		// total_dps = (float)Math.Round(total_dps,2);
		entity.summary_stats["damage"] = entity.calculation_stats["total_dps"];

		// mitigation
		entity.damage_resistance_stats["damage_resistance_armor"] = (float)Math.Round(entity.depth_stats["armor"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		// entity.dr_phys = (float)Math.Round(entity.physical_resistance/entity.dr_lvl_scale, 2);
		entity.damage_resistance_stats["damage_resistance_physical"] = (float)Math.Round(entity.depth_stats["physical_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_slash"] = (float)Math.Round(entity.depth_stats["slash_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_pierce"] = (float)Math.Round(entity.depth_stats["pierce_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_blunt"] = (float)Math.Round(entity.depth_stats["blunt_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_bleed"] = (float)Math.Round(entity.depth_stats["bleed_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_poison"] = (float)Math.Round(entity.depth_stats["poison_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_spell"] = (float)Math.Round(entity.depth_stats["spell_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_fire"] = (float)Math.Round(entity.depth_stats["fire_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_cold"] = (float)Math.Round(entity.depth_stats["cold_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_lightning"] = (float)Math.Round(entity.depth_stats["lightning_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		entity.damage_resistance_stats["damage_resistance_holy"] = (float)Math.Round(entity.depth_stats["holy_resistance"]/entity.calculation_stats["damage_resistance_level_scale"], 2);
		
		entity.damage_resistance_stats["average_damage_resistance"] = (entity.damage_resistance_stats["damage_resistance_physical"] 
																	+ entity.damage_resistance_stats["damage_resistance_slash"]
																	+ entity.damage_resistance_stats["damage_resistance_pierce"]
																	+ entity.damage_resistance_stats["damage_resistance_blunt"]
																	+ entity.damage_resistance_stats["damage_resistance_bleed"]
																	+ entity.damage_resistance_stats["damage_resistance_poison"]
																	+ entity.damage_resistance_stats["damage_resistance_spell"] 
																	+ entity.damage_resistance_stats["damage_resistance_fire"]
																	+ entity.damage_resistance_stats["damage_resistance_cold"]
																	+ entity.damage_resistance_stats["damage_resistance_lightning"]
																	+ entity.damage_resistance_stats["damage_resistance_holy"]) / 11;

		
		entity.summary_stats["resistance"]= (float)Math.Round(entity.depth_stats["maximum_health"] * (entity.damage_resistance_stats["damage_resistance_armor"] * entity.damage_resistance_stats["average_damage_resistance"]),2);
		// GD.Print("ave resistance: " + entity.avg_res_dr);
		// GD.Print("resistance: " + entity.resistance);
		// GD.Print("max health: " + entity.maximum_health + " of " + entity.Name);

		// recovery

		entity.depth_stats["health_regeneration"] = (float)Math.Round(1 + entity.base_stats["stamina"]/entity.calculation_stats["recovery_level_scale"] * entity.calculation_stats["health_regeneration_bonus"], 2);
		entity.depth_stats["resource_regeneration"] = (float)Math.Round(1 + entity.base_stats["stamina"]/entity.calculation_stats["recovery_level_scale"] * entity.calculation_stats["resource_regeneration_bonus"], 2);
		entity.depth_stats["posture_regeneration"] = (float)Math.Round(1 + entity.base_stats["stamina"]/entity.calculation_stats["recovery_level_scale"] * (1 + entity.depth_stats["poise"]/ 100), 2);
		entity.summary_stats["recovery"] = (float)Math.Round((entity.depth_stats["health_regeneration"] + entity.depth_stats["resource_regeneration"] + entity.depth_stats["posture_regeneration"]) / 3, 2);

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
