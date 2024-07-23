using Godot;
using System;

// Stat controller
// Updates and sends stats for the player, more information on how the stats are calculated in the Imogene_Journal
public partial class StatController : Controller
{

	private CustomSignals _customSignals; // Custom signal instance
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }

	public void UpdateStats() // Updates stats 															*** NEEDS ADDITIONS AND TO CHANGE DAMAGE CALCULATIONS ***
	{


		// Calculates stats
		

		entity.physical_melee_power = (2 * entity.strength) + entity.dexterity;
		entity.physical_ranged_power = entity.strength + (3 * entity.dexterity);
		entity.spell_melee_power = entity.strength + entity.dexterity + (3 * entity.intellect);
		entity.spell_ranged_power = (2 * entity.dexterity) + (3 * entity.intellect);

		// physical_melee_damage = physical_melee_power/15;
		// physical_ranged_damage = physical_ranged_power/15;
		// spell_melee_damage = spell_melee_power/15;
		// spell_ranged_damage = spell_ranged_power/15;
		entity.wisdom_scaler = entity.wisdom/20;

		// damage
		entity.combined_damage = entity.main_hand_damage + entity.off_hand_damage + entity.damage_bonus;
		entity.aps = entity.base_aps * (1 + entity.aps_modifiers);
		entity.base_dps = entity.aps * entity.combined_damage;
		entity.combined_damage = entity.main_hand_damage + entity.off_hand_damage + 2 + 1 + 1;

		entity.aps = 1.71f;
		entity.base_dps = entity.aps * entity.combined_damage;
		entity.skill_mod = 1.15f;
		entity.crit_mod = 1 + (entity.critical_hit_chance * entity.critical_hit_damage);

		entity.physical_melee_power_mod = 1 + (entity.physical_melee_power/100);
		entity.spell_melee_power_mod = 1 + (entity.spell_melee_power/100);
		entity.physical_ranged_power_mod = 1 + (entity.physical_ranged_power/100);
		entity.spell_ranged_power_mod = 1 + (entity.spell_ranged_power/100);

		entity.power_mod_avg = (entity.physical_melee_power_mod + entity.spell_melee_power_mod + entity.physical_ranged_power_mod + entity.spell_ranged_power_mod) / 4;

		entity.physical_melee_dps = entity.base_dps * entity.physical_melee_power_mod * entity.skill_mod * entity.crit_mod;
		entity.spell_melee_dps = entity.base_dps * entity.spell_melee_power_mod * entity.skill_mod * entity.crit_mod;
		entity.physical_ranged_dps = entity.base_dps * entity.physical_ranged_power_mod * entity.skill_mod * entity.crit_mod;
		entity.spell_ranged_dps = entity.base_dps * entity.spell_ranged_power_mod * entity.skill_mod * entity.crit_mod;

		entity.physical_melee_dps = (float)Math.Round(entity.physical_melee_dps,2);
		entity.spell_melee_dps = (float)Math.Round(entity.spell_melee_dps,2);
		entity.physical_ranged_dps = (float)Math.Round(entity.physical_ranged_dps,2);
		entity.spell_ranged_dps = (float)Math.Round(entity.spell_ranged_dps,2);

		entity.total_dps = (float)Math.Round(entity.base_dps * entity.power_mod_avg * entity.skill_mod * entity.crit_mod,2);
		// total_dps = (float)Math.Round(total_dps,2);
		entity.damage = entity.total_dps;

		// mitigation
		entity.dr_armor = (float)Math.Round(entity.armor/entity.dr_lvl_scale, 2);
		GD.Print("dr_armor of " + entity.dr_armor + " for " + entity.identifier);
		entity.dr_phys = (float)Math.Round(entity.physical_resistance/entity.dr_lvl_scale, 2);
		entity.dr_slash = (float)Math.Round(entity.slash_resistance/entity.dr_lvl_scale, 2);
		entity.dr_thrust = (float)Math.Round(entity.thrust_resistance/entity.dr_lvl_scale, 2);
		entity.dr_blunt = (float)Math.Round(entity.blunt_resistance/entity.dr_lvl_scale, 2);
		entity.dr_bleed = (float)Math.Round(entity.bleed_resistance/entity.dr_lvl_scale, 2);
		entity.dr_poison = (float)Math.Round(entity.poison_resistance/entity.dr_lvl_scale, 2);
		entity.dr_spell = (float)Math.Round(entity.spell_resistance/entity.dr_lvl_scale, 2);
		entity.dr_fire = (float)Math.Round(entity.fire_resistance/entity.dr_lvl_scale, 2);
		entity.dr_cold = (float)Math.Round(entity.cold_resistance/entity.dr_lvl_scale, 2);
		entity.dr_lightning = (float)Math.Round(entity.lightning_resistance/entity.dr_lvl_scale, 2);
		entity.dr_holy = (float)Math.Round(entity.holy_resistance/entity.dr_lvl_scale, 2);
		
		entity.avg_res_dr = (entity.dr_phys + entity.dr_slash + entity.dr_thrust + entity.dr_blunt + entity.dr_bleed + entity.dr_poison + entity.dr_spell + entity.dr_fire + entity.dr_cold + entity.dr_poison + entity.dr_holy) / 11;

		
		entity.resistance = (float)Math.Round(entity.maximum_health * (entity.dr_armor * entity.avg_res_dr),2);
		GD.Print("ave resistance: " + entity.avg_res_dr);
		GD.Print("resistance: " + entity.resistance);
		GD.Print("max health: " + entity.maximum_health + " of " + entity.Name);

		// recovery

		entity.health_regen = (float)Math.Round(1 + entity.stamina/entity.rec_lvl_scale * entity.health_regen_bonus, 2);
		entity.resource_regen = (float)Math.Round(1 + entity.stamina/entity.rec_lvl_scale * entity.resource_regen_bonus, 2);
		entity.posture_regen = (float)Math.Round(1 + entity.stamina/entity.rec_lvl_scale * (1 + entity.poise/100), 2);
		entity.recovery = (float)Math.Round((entity.health_regen + entity.resource_regen + entity.posture_regen) / 3, 2);
		if(player != null)
		{
			player.ui.hud.health.MaxValue = entity.maximum_health;

		}
		

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
