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
		
		damage *= damage_amp;
		return damage;
	}


}
