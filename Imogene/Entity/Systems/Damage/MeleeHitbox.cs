using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeHitbox : Area3D
{
	// Called when the node enters the scene tree for the first time.

	[Export] public float Damage { get; set; }
	
	[Export] public float PostureDamage { get; set; }
	public bool IsCritical;
	public List<StatusEffect> Effects = new List<StatusEffect>();
	[Export] public string Effect1;
	[Export] public string Effect2;
	[Export] public string Effect3;

	public float DamageAmp { get; set;} = 1.0f;

	public enum DamageType {None, Physical, Spell, Other}
	public DamageType Type { get; set; } = DamageType.None;

	public enum PhysicalDamageType {None, Straight, Pierce, Slash, Blunt}
	public PhysicalDamageType PhysicalDamage { get; set; } = PhysicalDamageType.None;
	
	public enum SpellDamageType {None, Straight, Fire, Cold, Lightning, Holy}
	public SpellDamageType SpellDamage { get; set; } = SpellDamageType.None;
	
	public enum OtherDamageType {None, Bleed, Poison, Curse}
	public OtherDamageType OtherDamage { get; set; } = OtherDamageType.None;

	public float SetDamage(Entity entity)
	{
		
		Damage *= DamageAmp;
		return Damage;
	}


}
