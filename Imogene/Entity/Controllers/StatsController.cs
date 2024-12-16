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
		entity_.CombinedDamage = entity_.MainHandDamage.BaseValue  + entity_.OffHandDamage.BaseValue  + entity_.DamageBonus.BaseValue ;
		damage_per_second = entity_.CombinedDamage;
	}

	public void SetDamageEstimate()
	{
		damage_estimate = (float)Math.Round(damage_per_second * power_modifier_average * critical_hit_modifier, 2);
	}

	


	public void SetScales(Entity entity_)
	{
		damage_resistance_level_scale = entity_.Level.BaseValue * 50;
		recovery_level_scale = entity_.Level.BaseValue * 100;
	}

	public void SetDamageResistances(Entity entity_)
	{
		// SetScales(entity_);
		// SetResistanceStats(entity_);
		// float _total_resistances = 0;
		// for(int i = 0; i < resistance_stats.Length; i ++)
		// {
		// 	_total_resistances += (float)Math.Round(resistance_stats[i].base_value / damage_resistance_level_scale, 2);
		// }

		
		// average_damage_resistance = _total_resistances / resistance_stats.Length;
		
	}

	public void SetResistanceEstimate(Entity entity_)
	{
		resistance = entity_.Health.MaxValue * average_damage_resistance * entity_.Armor.BaseValue;
	}

	

	public void SetRecoveryEstimate(Entity entity_)
	{
		recovery = (float) Math.Round((entity_.HealthRegeneration.BaseValue  + entity_.ResourceRegeneration.BaseValue ) / 3, 2);
	}

	public void SetBaseStats(Entity entity_)
	{
		base_stats[0] = entity_.Level;
		base_stats[1] = entity_.Strength;
		base_stats[2] = entity_.Dexterity;
		base_stats[3] = entity_.Intellect;
		base_stats[4] = entity_.Vitality;
		base_stats[5] = entity_.Stamina;

	}

	public void SetSummaryStats()
	{
		summary_stats[0] = damage_estimate;
		summary_stats[1] = resistance;
		summary_stats[2] = recovery;
	}



	public void SetResistanceStats(Entity entity_)
	{	
		
	}

	public void SetDepthStats(Entity entity_)
	{
		
		depth_stats[21] = entity_.CooldownReduction;
		depth_stats[22] = entity_.Armor;
		depth_stats[24] = entity_.BlockAmount;
		depth_stats[25] = entity_.Retaliation;
		depth_stats[38] = entity_.Health;
		depth_stats[39] = entity_.HealthBonus;
		depth_stats[40] = entity_.HealthRegeneration;
		depth_stats[41] = entity_.HealthOnRetaliation;
		depth_stats[42] = entity_.Resource;
		depth_stats[43] = entity_.ResourceRegeneration;
		depth_stats[44] = entity_.ResourceCostReduction;
		depth_stats[46] = entity_.MovementSpeed;
	}

	public void SetStats(Entity entity_)
	{
		SetBaseStats(entity_);
		SetDamage(entity_);
		SetDamageEstimate();
		SetScales(entity_);
		SetDamageResistances(entity_);
		SetResistanceEstimate(entity_);
		SetSummaryStats();
		SetDepthStats(entity_);
	}

   

	

	public void SetAttackSpeed(Entity entity_)
	{
		// entity.main_hand_attacks_per_second = entity.main_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
		// entity.off_hand_attacks_per_second = entity.off_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
	}

	

	public void SetRegenerations(Entity entity_)
	{
		entity_.HealthRegeneration.BaseValue = (float)Math.Round(1 + entity_.Stamina.BaseValue / recovery_level_scale * entity_.HealthRegeneration.BaseValue	 , 2);
		entity_.ResourceRegeneration.BaseValue = (float)Math.Round(1 + entity_.Stamina.BaseValue  / recovery_level_scale * entity_.ResourceRegeneration.BaseValue , 2);
	}

	public void Update(Entity entity_) // Updates stats 															*** NEEDS ADDITIONS AND TO CHANGE DAMAGE CALCULATIONS ***
	{


		// Calculates stats
	
	
		SetDamage(entity_);

		SetDamageEstimate();

		// mitigation
		SetScales(entity_);
		SetDamageResistances(entity_);
		SetResistanceEstimate(entity_);


	
		if(entity_ is Player _player)
		{
			EmitSignal(nameof(UpdateStats), _player);
		}

	}

}
