using Godot;
using System;
using System.Collections.Generic;

public partial class Stat : Resource
{
	public Stat (StatType type_, float value_)
	{
		type = type_;
		base_value = value_;
		current_value = base_value;
	}

	public enum StatType
	{
		none,
		//Base
		strength, dexterity, intellect, vitality, stamina,
		wisdom, charisma,
		
		//Gear
		main_hand_damage, off_hand_damage, damage_bonus,
		main_hand_attacks_per_second, off_hand_attacks_per_second,
		attack_speed_increase, health_bonus,
		
		//Movement
		movement_speed, fall_speed, jump_speed,
		
		//Offense
		critical_hit_chance, critical_hit_damage, posture_damage,
		power, physical_damage, pierce_damage, slash_damage,
		blunt_damage, bleed_damage, poison_damage, curse_damage,
		spell_damage, fire_damage, cold_damage, lightning_damage,
		holy_damage,
		
		//Defense
		block_amount, retaliation,
		armor, poise, physical_resistance, pierce_resistance,
		slash_resistance, blunt_resistance, bleed_resistance,
		poison_resistance, curse_resistance, spell_resistance,
		fire_resistance, cold_resistance, lightning_resistance,
		holy_resistance,

		//Health
		health, health_on_retaliation,
		health_regeneration_bonus,

		//Resource
		resource, posture, resource_cost_reduction, resource_regeneration_bonus,
		cooldown_reduction,

		//Level
		level,

		//Powers
		physical_melee_power,
		spell_melee_power,
		physical_ranged_power,
		spell_ranged_power,

		//Wisdom
		wisdom_scaler,

		//Regeneration
		health_regeneration,
		resource_regeneration,
		posture_regeneration

	}

	public StatType type { get; set; } = StatType.none;
	public float base_value { get; set; } = 0.0f;
	public float current_value { get; set; } = 0.0f;
	public float max_value { get; set; } = 0.0f;
	
	public List<StatModifier> modifiers = new List<StatModifier>();

	public void AddModifier(StatModifier stat_modifier_)
	{
	
		if(!modifiers.Contains(stat_modifier_))
		{
			
			modifiers.Add(stat_modifier_);
			stat_modifier_.Apply(this);
			PrintModifiers();
		}
	}


    public void RemoveModifier(StatModifier stat_modifier_)
	{
		if(modifiers.Contains(stat_modifier_))
		{
			modifiers.Remove(stat_modifier_);
			stat_modifier_.Release(this);
			
		}
		
	}

	public void ResetToBase()
	{
		current_value = base_value;
	}

	public void PrintModifiers()
	{
		foreach(StatModifier modifier in modifiers)
		{
			GD.Print("Modifier " + modifier);
		}
	}

	
}
