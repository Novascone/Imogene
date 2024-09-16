using Godot;
using System;

public partial class DepthScroller : Control
{
	[Export] public Control vbox;

	// Offense
	[Export] UIStat physical_melee_power;
	[Export] UIStat spell_melee_power;
	[Export] UIStat physical_ranged_power;
	[Export] UIStat spell_ranged_power;
	[Export] UIStat wisdom_scaler;
	[Export] UIStat critical_hit_chance;
	[Export] UIStat critical_hit_damage;
	[Export] UIStat attack_speed;
	[Export] UIStat attack_speed_increase;
	[Export] UIStat cool_down_reduction;

	// Defense
	[Export] UIStat armor;
	[Export] UIStat poise;
	[Export] UIStat block_amount;
	[Export] UIStat retaliation;
	[Export] UIStat physical_resistance;
	[Export] UIStat pierce_resistance;
	[Export] UIStat slash_resistance;
	[Export] UIStat blunt_resistance;
	[Export] UIStat bleed_resistance;
	[Export] UIStat poison_resistance;
	[Export] UIStat curse_resistance;
	[Export] UIStat spell_resistance;
	[Export] UIStat fire_resistance;
	[Export] UIStat cold_resistance;
	[Export] UIStat lightning_resistance;
	[Export] UIStat holy_resistance;

	// Health
	[Export] UIStat maximum_health;
	[Export] UIStat health_bonus;
	[Export] UIStat health_regeneration;
	[Export] UIStat health_retaliation;

	// Resource
	[Export] UIStat maximum_resource;
	[Export] UIStat resource_regeneration;
	[Export] UIStat resource_cost_reduction;
	[Export] UIStat posture_regeneration;

	// Misc

	[Export] UIStat movement_speed;









	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SubScribeToStatSignals(Player player)
	{
		// foreach(Control control in vbox.GetChildren())
		// {
		// 	if(control is Stat stat)
		// 	{
		// 		player.entity_controllers.stats_controller
		// 	}
		// }
	}
}
