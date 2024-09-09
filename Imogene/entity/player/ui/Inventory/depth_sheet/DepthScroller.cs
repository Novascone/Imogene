using Godot;
using System;

public partial class DepthScroller : Control
{
	[Export] Control vbox;

	// Offense
	[Export] Stat physical_melee_power;
	[Export] Stat spell_melee_power;
	[Export] Stat physical_ranged_power;
	[Export] Stat spell_ranged_power;
	[Export] Stat wisdom_scaler;
	[Export] Stat critical_hit_chance;
	[Export] Stat critical_hit_damage;
	[Export] Stat attack_speed;
	[Export] Stat attack_speed_increase;
	[Export] Stat cool_down_reduction;

	// Defense
	[Export] Stat armor;
	[Export] Stat poise;
	[Export] Stat block_amount;
	[Export] Stat retaliation;
	[Export] Stat physical_resistance;
	[Export] Stat pierce_resistance;
	[Export] Stat slash_resistance;
	[Export] Stat blunt_resistance;
	[Export] Stat bleed_resistance;
	[Export] Stat poison_resistance;
	[Export] Stat curse_resistance;
	[Export] Stat spell_resistance;
	[Export] Stat fire_resistance;
	[Export] Stat cold_resistance;
	[Export] Stat lightning_resistance;
	[Export] Stat holy_resistance;

	// Health
	[Export] Stat maximum_health;
	[Export] Stat health_bonus;
	[Export] Stat health_regeneration;
	[Export] Stat health_retaliation;

	// Resource
	[Export] Stat maximum_resource;
	[Export] Stat resource_regeneration;
	[Export] Stat resource_cost_reduction;
	[Export] Stat posture_regeneration;

	// Misc

	[Export] Stat movement_speed;









	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
