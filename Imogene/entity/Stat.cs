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
		None,
		//Base
		Strength, Dexterity, Intellect, Vitality, Stamina,
		Wisdom, Charisma,
		
		//Gear
		MainHandDamage, OffHandDamage, DamageBonus,
		MainHandAttacksPerSecond, OffHandAttacksPerSecond,
		AttackSpeedIncrease, HealthBonus,
		
		//Movement
		MovementSpeed, FallSpeed, JumpSpeed,
		
		//Offense
		CriticalHitChance, CriticalHitDamage, PostureDamage,
		Power, PhysicalDamage, PierceDamage, SlashDamage,
		BluntDamage, BleedDamage, PoisonDamage, CurseDamage,
		SpellDamage, FireDamage, ColdDamage, LightningDamage,
		HolyDamage,
		
		//Defense
		BlockAmount, Retaliation,
		Armor, Poise, PhysicalResistance, PierceResistance,
		SlashResistance, BluntResistance, BleedResistance,
		PoisonResistance, CurseResistance, SpellResistance,
		FireResistance, ColdResistance, LightningResistance,
		HolyResistance,

		//Health
		Health, HealthOnRetaliation,
		HealthRegenerationBonus,

		//Resource
		Resource, Posture, ResourceCostReduction, ResourceRegenerationBonus,
		CooldownReduction,

		//Level
		Level,

		//Powers
		PhysicalMeleePower,
		SpellMeleePower,
		PhysicalRangedPower,
		SpellRangedPower,

		//Wisdom
		WisdomScaler,

		//Regeneration
		HealthRegeneration,
		ResourceRegeneration,
		PostureRegeneration

	}

	public StatType type { get; set; } = StatType.None;
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
		foreach(StatModifier _modifier in modifiers)
		{
			GD.Print("Modifier " + _modifier);
		}
	}

	
}
