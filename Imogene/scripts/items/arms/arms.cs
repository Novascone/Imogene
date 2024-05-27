using Godot;
using System;

public partial class arms : Node
{
	[Export]
	public int strength;
	[Export]
	public int dexterity;
	[Export]
	public int intellect;
	[Export]
	public int vitality;
	[Export]
	public int stamina;
	[Export]
	public int wisdom;
	[Export]
	public int charisma;
	[Export]
	public float critical_hit_chance;
	[Export]
	public float critical_hit_damage;
	[Export]
	public int armor;
	[Export]
	public float poise;
	[Export]
	public int block;
	[Export]
	public int retaliation;
	[Export]
	public int physical_resistance;
	[Export]
	public int thrust_resistance;
	[Export]
	public int slash_resistance;
	[Export]
	public int blunt_resistance;
	[Export]
	public int bleed_resistance;
	[Export]
	public int poison_resistance;
	[Export]
	public int curse_resistance;
	[Export]
	public int spell_resistance;
	[Export]
	public int fire_resistance;
	[Export]
	public int cold_resistance;
	[Export]
	public int lightning_resistance;
	[Export]
	public int holy_resistance;
	[Export]
	public float health_bonus;
	[Export]
	public float health_regen;
	[Export]
	public float health_retaliate;
	[Export]
	public float resource_regen;
	[Export]
	public float resource_cost_reduction;
	[Export]
	public string type;
	[Export]
	public string name;
	[Export]
	public string description;
	public Ability special_ability;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
