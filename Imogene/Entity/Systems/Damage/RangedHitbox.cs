using Godot;
using System;
using System.Collections.Generic;

public partial class RangedHitbox : RigidBody3D
{
	// [Export] public string damage_type { get; set; }
	[Export] public float Damage { get; set; } = 0.0f;
	[Export] public float PostureDamage { get; set; } = 0.0f;
	public float DamageAmp { get; set;} = 1.0f;
	public List<StatusEffect> Effects { get; set; } = new List<StatusEffect>();
	public bool IsCritical { get; set; } = false;
	public DamageType Type { get; set; } = DamageType.None;
	public PhysicalDamageType PhysicalDamage { get; set; } = PhysicalDamageType.None;
	public SpellDamageType SpellDamage { get; set; } = SpellDamageType.None;
	public OtherDamageType OtherDamage { get; set; } = OtherDamageType.None;
	

	public enum DamageType {None, Physical, Spell, Other}
	public enum PhysicalDamageType {None, Straight, Pierce, Slash, Blunt}
	public enum SpellDamageType {None, Straight, Fire, Cold, Lightning, Holy}
	public enum OtherDamageType {None, Bleed, Poison, Curse}

   

	public void OnBodyEntered(Node3D body)
	{
		QueueFree();
	}

	public void OnDespawnTimeout()
	{
		QueueFree();
	}

	public float SetDamage(Entity entity)
	{
		Damage *= DamageAmp;
		return Damage;
		
	}
}
