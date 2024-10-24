using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeHitbox : Area3D
{
	// Called when the node enters the scene tree for the first time.

	[Export] public float damage { get; set; }
	
	[Export] public float posture_damage { get; set; }
	public bool is_critical;
	public List<StatusEffect> effects = new List<StatusEffect>();
	[Export] public string effect_1;
	[Export] public string effect_2;
	[Export] public string effect_3;

	public float damage_amp { get; set;} = 1.0f;

	public enum DamageType {None, Physical, Spell, Other}
	public DamageType type { get; set; } = DamageType.None;

	public enum PhysicalDamageType {None, Straight, Pierce, Slash, Blunt}
	public PhysicalDamageType physical_damage_type { get; set; } = PhysicalDamageType.None;
	
	public enum SpellDamageType {None, Straight, Fire, Cold, Lightning, Holy}
	public SpellDamageType spell_damage_type { get; set; } = SpellDamageType.None;
	
	public enum OtherDamageType {None, Bleed, Poison, Curse}
	public OtherDamageType other_damage_type { get; set; } = OtherDamageType.None;

	public float SetDamage(Entity entity_)
	{
		damage *= 1 + (entity_.power.current_value / 100);
		if(type == DamageType.Physical)
		{
			damage *= entity_.physical_damage.current_value;
			if(physical_damage_type == PhysicalDamageType.Pierce)
			{
				damage *= entity_.blunt_damage.current_value;
			}
			if(physical_damage_type == PhysicalDamageType.Slash)
			{
				damage *= entity_.slash_damage.current_value;
			}
			if(physical_damage_type == PhysicalDamageType.Blunt)
			{
				damage *= entity_.blunt_damage.current_value;
			}
		}
		else if(type == DamageType.Spell)
		{
			damage *= entity_.spell_damage.current_value;
			if(spell_damage_type == SpellDamageType.Fire)
			{
				damage *= entity_.fire_damage.current_value;
			}
			if(spell_damage_type == SpellDamageType.Cold)
			{
				damage *= entity_.cold_damage.current_value;
			}
			if(spell_damage_type == SpellDamageType.Lightning)
			{
				damage *= entity_.lightning_resistance.current_value;
			}
			if(spell_damage_type == SpellDamageType.Holy)
			{
				damage *= entity_.holy_damage.current_value;
			}
		}
		else if(type == DamageType.Other)
		{
			if(other_damage_type == OtherDamageType.Bleed)
			{
				damage *= entity_.poison_damage.current_value;
			}
			if(other_damage_type == OtherDamageType.Poison)
			{
				damage *= entity_.poison_damage.current_value;
			}
			if(other_damage_type == OtherDamageType.Curse)
			{
				damage *= entity_.curse_damage.current_value;
			}
		}
		damage *= damage_amp;
		return damage;
	}


}
