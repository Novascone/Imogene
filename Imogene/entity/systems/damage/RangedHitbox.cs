using Godot;
using System;
using System.Collections.Generic;

public partial class RangedHitbox : RigidBody3D
{
	// [Export] public string damage_type { get; set; }
	[Export] public float damage { get; set; }
	[Export] public float posture_damage { get; set; }
	public List<StatusEffect> effects = new List<StatusEffect>();
	public bool is_critical;

	public bool hit;

	public enum DamageType {None, Physical, Spell, Other}
	public DamageType type { get; set; } = DamageType.None;

	public enum PhysicalDamageType {None, Straight, Pierce, Slash, blunt}
	public PhysicalDamageType physical_damage_type { get; set; } = PhysicalDamageType.None;
	
	public enum SpellDamageType {None, Straight, Fire, Cold, Lightning, Holy}
	public SpellDamageType spell_damage_type { get; set; } = SpellDamageType.None;
	
	public enum OtherDamageType {None, Bleed, Poison, Curse}
	public OtherDamageType other_damage_type { get; set; } = OtherDamageType.None;

    public override void _PhysicsProcess(double delta)
    {
        if(hit)
		{
			QueueFree();
		}
    }

	public void _on_body_entered(Node3D body)
	{
		// GD.Print("body entered " + body.Name);
		QueueFree();
	}

	public void _on_despawn_timeout()
	{
		QueueFree();
	}
}
