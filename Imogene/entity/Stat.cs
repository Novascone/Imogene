using Godot;
using System;
using System.Collections.Generic;

public partial class Stat : Resource
{
	public Stat (StatType type, float stat_value)
	{
		stat_type = type;
		base_value = stat_value;
		current_value = base_value;
	}

	public enum StatType
	{
		//Base
		strength, dexterity, intellect, vitality, stamina,
		wisdom, charisma,
		
		//Gear
		main_hand_damage, off_hand_damage, damage_bonus,
		main_hand_attacks_per_second, off_hand_attacks_per_second,
		health_bonus,
		
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

	public StatType stat_type { get; set; }
	public float base_value { get; set; } 
	public float current_value { get; set; }
	public float max_value { get; set; }
	
	public List<StatModifier> modifiers = new List<StatModifier>();

	public void AddModifier(StatModifier stat_modifier)
	{
	
		if(!modifiers.Contains(stat_modifier))
		{
			modifiers.Add(stat_modifier);
			stat_modifier.Apply(this);
			GD.Print("added modifier");
		}
	}


    public void RemoveModifier(StatModifier stat_modifier)
	{
		if(modifiers.Contains(stat_modifier))
		{
			modifiers.Remove(stat_modifier);
			stat_modifier.Release(this);
			GD.Print("removed modifier");
		}
		
	}

	public void PrintModifiers()
	{
		foreach(StatModifier modifier in modifiers)
		{
			GD.Print(modifier);
		}
	}

	
}
