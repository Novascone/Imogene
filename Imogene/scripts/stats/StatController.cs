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
		

		player.physical_melee_power = (2 * player.strength) + player.dexterity;
		player.physical_ranged_power = player.strength + (3 * player.dexterity);
		player.spell_melee_power = player.strength + player.dexterity + (3 * player.intellect);
		player.spell_ranged_power = (2 * player.dexterity) + (3 * player.intellect);

		// physical_melee_damage = physical_melee_power/15;
		// physical_ranged_damage = physical_ranged_power/15;
		// spell_melee_damage = spell_melee_power/15;
		// spell_ranged_damage = spell_ranged_power/15;
		player.wisdom_scaler = player.wisdom/20;

		// damage
		player.combined_damage = player.main_hand_damage + player.off_hand_damage + player.damage_bonus;
		player.aps = player.base_aps * (1 + player.aps_modifiers);
		player.base_dps = player.aps * player.combined_damage;
		player.combined_damage = player.main_hand_damage + player.off_hand_damage + 2 + 1 + 1;

		player.aps = 1.71f;
		player.base_dps = player.aps * player.combined_damage;
		player.skill_mod = 1.15f;
		player.crit_mod = 1 + (player.critical_hit_chance * player.critical_hit_damage);

		player.physical_melee_power_mod = 1 + (player.physical_melee_power/100);
		player.spell_melee_power_mod = 1 + (player.spell_melee_power/100);
		player.physical_ranged_power_mod = 1 + (player.physical_ranged_power/100);
		player.spell_ranged_power_mod = 1 + (player.spell_ranged_power/100);

		player.power_mod_avg = (player.physical_melee_power_mod + player.spell_melee_power_mod + player.physical_ranged_power_mod + player.spell_ranged_power_mod) / 4;

		player.physical_melee_dps = player.base_dps * player.physical_melee_power_mod * player.skill_mod * player.crit_mod;
		player.spell_melee_dps = player.base_dps * player.spell_melee_power_mod * player.skill_mod * player.crit_mod;
		player.physical_ranged_dps = player.base_dps * player.physical_ranged_power_mod * player.skill_mod * player.crit_mod;
		player.spell_ranged_dps = player.base_dps * player.spell_ranged_power_mod * player.skill_mod * player.crit_mod;

		player.physical_melee_dps = (float)Math.Round(player.physical_melee_dps,2);
		player.spell_melee_dps = (float)Math.Round(player.spell_melee_dps,2);
		player.physical_ranged_dps = (float)Math.Round(player.physical_ranged_dps,2);
		player.spell_ranged_dps = (float)Math.Round(player.spell_ranged_dps,2);

		player.total_dps = (float)Math.Round(player.base_dps * player.power_mod_avg * player.skill_mod * player.crit_mod,2);
		// total_dps = (float)Math.Round(total_dps,2);
		player.damage = player.total_dps;

		// mitigation
		player.dr_armor = (float)Math.Round(player.armor/player.dr_lvl_scale, 2);
		GD.Print("dr_armor " + player.dr_armor);
		player.dr_phys = (float)Math.Round(player.physical_resistance/player.dr_lvl_scale, 2);
		player.dr_slash = (float)Math.Round(player.slash_resistance/player.dr_lvl_scale, 2);
		player.dr_thrust = (float)Math.Round(player.thrust_resistance/player.dr_lvl_scale, 2);
		player.dr_blunt = (float)Math.Round(player.blunt_resistance/player.dr_lvl_scale, 2);
		player.dr_bleed = (float)Math.Round(player.bleed_resistance/player.dr_lvl_scale, 2);
		player.dr_poison = (float)Math.Round(player.poison_resistance/player.dr_lvl_scale, 2);
		player.dr_spell = (float)Math.Round(player.spell_resistance/player.dr_lvl_scale, 2);
		player.dr_fire = (float)Math.Round(player.fire_resistance/player.dr_lvl_scale, 2);
		player.dr_cold = (float)Math.Round(player.cold_resistance/player.dr_lvl_scale, 2);
		player.dr_lightning = (float)Math.Round(player.lightning_resistance/player.dr_lvl_scale, 2);
		player.dr_holy = (float)Math.Round(player.holy_resistance/player.dr_lvl_scale, 2);
		
		player.avg_res_dr = (player.dr_phys + player.dr_slash + player.dr_thrust + player.dr_blunt + player.dr_bleed + player.dr_poison + player.dr_spell + player.dr_fire + player.dr_cold + player.dr_poison + player.dr_holy) / 11;

		player.resistance = (float)Math.Round(player.maximum_health * (player.dr_armor * player.avg_res_dr),2);

		// recovery

		player.health_regen = (float)Math.Round(1 + player.stamina/player.rec_lvl_scale * player.health_regen_bonus, 2);
		player.resource_regen = (float)Math.Round(1 + player.stamina/player.rec_lvl_scale * player.resource_regen_bonus, 2);
		player.posture_regen = (float)Math.Round(1 + player.stamina/player.rec_lvl_scale * (1 + player.poise/100), 2);
		player.recovery = (float)Math.Round((player.health_regen + player.resource_regen + player.posture_regen) / 3, 2);

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
		SendStats();
		
	}

	public void SendStats()
	{
		GD.Print("sending stats from stat controller");
		_customSignals.EmitSignal(nameof(CustomSignals.SendStats),
		player.level, player.strength, player.dexterity, player.intellect, player.vitality, player.stamina, player.wisdom, player.charisma, player.total_dps,

		player.physical_melee_dps, player.spell_melee_dps, player.physical_ranged_dps, player.spell_ranged_dps, player.physical_melee_power, 

		player.spell_melee_power, player.physical_ranged_power, player.spell_ranged_power, player.wisdom_scaler, player.physical_melee_power_mod,

		player.physical_ranged_power_mod, player.spell_ranged_power_mod, player.power_mod_avg, player.damage_bonus, player.combined_damage, player.base_aps,

		player.aps_modifiers, player.aps, player.base_dps, player.skill_mod, player.crit_mod, player.slash_damage, player.thrust_damage, player.blunt_damage, player.bleed_damage,

		player.poison_damage, player.fire_damage, player.cold_damage, player.lightning_damage, player.holy_damage, player.critical_hit_chance, player.critical_hit_damage, player.attack_speed_increase,

		player.cool_down_reduction, player.posture_damage, player.armor, player.poise, player.block_amount, player.retaliation, player.physical_resistance, player.thrust_resistance, player.slash_resistance,

		player.blunt_resistance, player.bleed_resistance, player.poison_resistance, player.curse_resistance, player.spell_resistance, player.fire_resistance, player.cold_resistance, player.lightning_resistance,
		
		player.holy_resistance, player.maximum_health, player.health_bonus, player.health_regen, player.health_regen_bonus, player.posture_regen, player.posture_regen_bonus, player.health_on_retaliate, player.resistance,

		player.maximum_resource, player.resource_regen, player.resource_cost_reduction, player.recovery, player.movement_speed, player.maximum_gold
																);
	}
}
