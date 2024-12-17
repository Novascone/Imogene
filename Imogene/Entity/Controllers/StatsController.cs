using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

// Stat controller
// Updates and sends stats for the player, more information on how the stats are calculated in the Imogene_Journal
public partial class StatsController : Node
{


	// Damage
	public float DamagePerSecond { get; set; } = 0.0f;
	public float CriticalHitModifier { get; set; } = 0.0f;
	public float DamageEstimate { get; set; } = 0.0f;

	

	public float DamageResistanceLevelScale { get; set; } = 0.0f;
	public float RecoveryLevelScale { get; set; } = 0.0f;
	public float AverageDamageResistance { get; set; } = 0.0f;
	public float Resistance { get; set; } = 0.0f;
 	public float Recovery { get; set; } = 0.0f;

	public Stat[] BaseStats { get; set; } = new Stat[8];
	public float[] SummaryStats { get; set; } = new float[3];
	public Stat[] DamageModifiers { get; set; } = new Stat[12];
	public Stat[] ResistanceStats { get; set; } = new Stat[12];
	public Stat[] DepthStats { get; set; } = new Stat[47];
	
	
	[Signal] public delegate void UpdateStatsEventHandler(Player player);

	public void SetDamage(Entity entity)
	{
		entity.CombinedDamage = entity.MainHandDamage.BaseValue  + entity.OffHandDamage.BaseValue  + entity.DamageBonus.BaseValue ;
		DamagePerSecond = entity.CombinedDamage;
	}

	public void SetDamageEstimate()
	{
		DamageEstimate = (float)Math.Round(DamagePerSecond * CriticalHitModifier, 2);
	}

	


	public void SetScales(Entity entity)
	{
		DamageResistanceLevelScale = entity.Level.BaseValue * 50;
		RecoveryLevelScale = entity.Level.BaseValue * 100;
	}

	public void SetDamageResistances(Entity entity)
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

	public void SetResistanceEstimate(Entity entity)
	{
		Resistance = entity.Health.MaxValue * AverageDamageResistance * entity.Armor.BaseValue;
	}

	

	public void SetRecoveryEstimate(Entity entity)
	{
		Recovery = (float) Math.Round((entity.HealthRegeneration.BaseValue  + entity.ResourceRegeneration.BaseValue ) / 3, 2);
	}

	public void SetBaseStats(Entity entity)
	{
		BaseStats[0] = entity.Level;
		BaseStats[1] = entity.Strength;
		BaseStats[2] = entity.Dexterity;
		BaseStats[3] = entity.Intellect;
		BaseStats[4] = entity.Vitality;
		BaseStats[5] = entity.Stamina;

	}

	public void SetSummaryStats()
	{
		SummaryStats[0] = DamageEstimate;
		SummaryStats[1] = Resistance;
		SummaryStats[2] = Recovery;
	}



	public void SetResistanceStats(Entity entity)
	{	
		
	}

	public void SetDepthStats(Entity entity)
	{
		
		DepthStats[21] = entity.CooldownReduction;
		DepthStats[22] = entity.Armor;
		DepthStats[24] = entity.BlockAmount;
		DepthStats[25] = entity.Retaliation;
		DepthStats[38] = entity.Health;
		DepthStats[39] = entity.HealthBonus;
		DepthStats[40] = entity.HealthRegeneration;
		DepthStats[41] = entity.HealthOnRetaliation;
		DepthStats[42] = entity.ResourceRegeneration;
		DepthStats[44] = entity.ResourceCostReduction;
		DepthStats[46] = entity.MovementSpeed;
	}

	public void SetStats(Entity entity)
	{
		SetBaseStats(entity);
		SetDamage(entity);
		SetDamageEstimate();
		SetScales(entity);
		SetDamageResistances(entity);
		SetResistanceEstimate(entity);
		SetSummaryStats();
		SetDepthStats(entity);
	}

   

	

	public void SetAttackSpeed(Entity entity)
	{
		// entity.main_hand_attacks_per_second = entity.main_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
		// entity.off_hand_attacks_per_second = entity.off_hand.attacks_per_second * (1 + entity.attack_speed_increase.base_value);
	}

	

	public void SetRegenerations(Entity entity)
	{
		entity.HealthRegeneration.BaseValue = (float)Math.Round(1 + entity.Stamina.BaseValue / RecoveryLevelScale * entity.HealthRegeneration.BaseValue	 , 2);
		entity.ResourceRegeneration.BaseValue = (float)Math.Round(1 + entity.Stamina.BaseValue  / RecoveryLevelScale * entity.ResourceRegeneration.BaseValue , 2);
	}

	public void Update(Entity entity) // Updates stats 															*** NEEDS ADDITIONS AND TO CHANGE DAMAGE CALCULATIONS ***
	{


		// Calculates stats
	
	
		SetDamage(entity);

		SetDamageEstimate();

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
