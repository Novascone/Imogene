using Godot;
using System;
using System.Collections.Generic;

public partial class RangedHitbox : RigidBody3D
{
	// [Export] public string damage_type { get; set; }
	[Export] public float damage { get; set; } = 0.0f;
	[Export] public float posture_damage { get; set; } = 0.0f;
	public float damage_amp { get; set;} = 1.0f;
	public List<StatusEffect> effects { get; set; } = new List<StatusEffect>();
	public bool is_critical { get; set; } = false;
	public DamageType type { get; set; } = DamageType.None;
	public PhysicalDamageType physical_damage_type { get; set; } = PhysicalDamageType.None;
	public SpellDamageType spell_damage_type { get; set; } = SpellDamageType.None;
	public OtherDamageType other_damage_type { get; set; } = OtherDamageType.None;
	

	public enum DamageType {None, Physical, Spell, Other}
	public enum PhysicalDamageType {None, Straight, Pierce, Slash, Blunt}
	public enum SpellDamageType {None, Straight, Fire, Cold, Lightning, Holy}
	public enum OtherDamageType {None, Bleed, Poison, Curse}

   

	public void _on_body_entered(Node3D body_)
	{
		// GD.Print("body entered " + body.Name);
		QueueFree();
	}

	public void _on_despawn_timeout()
	{
		QueueFree();
	}

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
