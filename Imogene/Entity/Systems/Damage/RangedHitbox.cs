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
		damage *= damage_amp;
		return damage;
		
	}
}
