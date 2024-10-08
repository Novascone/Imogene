using Godot;
using System;

public partial class Hitbox : Node3D


{
	public enum DamageType {None, Physical, Spell, Other}
	public DamageType type { get; set; } = DamageType.None;

	public enum PhysicalDamageType {None, Straight, Pierce, Slash, blunt}
	public PhysicalDamageType physical_damage_type { get; set; } = PhysicalDamageType.None;

	public enum SpellDamageType {None, Straight, Fire, Cold, Lightning, Holy}
	public SpellDamageType spell_damage_type { get; set; } = SpellDamageType.None;
	
	public enum OtherDamageType {None, Bleed, Poison, Curse}
	public OtherDamageType other_damage_type { get; set; } = OtherDamageType.None;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
