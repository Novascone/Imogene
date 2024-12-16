using Godot;
using System;
using System.Collections.Generic;

public partial class Stat : Resource
{
	public Stat (StatType constructedType, float constructedValue)
	{
		Type = constructedType;
		BaseValue = constructedValue;
		CurrentValue = BaseValue;
	}

	public enum StatType
	{
		None,
		//Base
		Strength, Dexterity, Intellect, Vitality, Stamina,
		
		
		//Gear
		MainHandDamage, OffHandDamage, DamageBonus, HealthBonus,
		
		//Movement
		MovementSpeed, FallSpeed, JumpSpeed,
		
		//Offense
		
		
		//Defense
		BlockAmount, Retaliation,
		Armor,

		//Health
		Health, HealthOnRetaliation,
		HealthRegenerationBonus,

		//Resource
		Resource, ResourceCostReduction, ResourceRegenerationBonus,
		CooldownReduction,

		//Level
		Level,
		
		//Regeneration
		HealthRegeneration,
		ResourceRegeneration,
		PostureRegeneration

	}

	public StatType Type { get; set; } = StatType.None;
	public float BaseValue { get; set; } = 0.0f;
	public float CurrentValue { get; set; } = 0.0f;
	public float HandicapValue { get; set; } = 0.0f;
	public float MaxValue { get; set; } = 0.0f;
	
	public List<StatModifier> modifiers = new();

	public void AddModifier(StatModifier statModifier)
	{
	
		if(!modifiers.Contains(statModifier))
		{
			
			modifiers.Add(statModifier);
			statModifier.Apply(this);
			PrintModifiers();
		}
	}


    public void RemoveModifier(StatModifier statModifier)
	{
		if(modifiers.Contains(statModifier))
		{
			modifiers.Remove(statModifier);
			statModifier.Release(this);
			
		}
		
	}

	public void ResetToBase()
	{
		CurrentValue = BaseValue;
	}

	public void PrintModifiers()
	{
		foreach(StatModifier modifier in modifiers)
		{
			GD.Print("Modifier " + modifier);
		}
	}

	
}
