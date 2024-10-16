using Godot;
using System;

public partial class DepthScroller : Control
{
	[Export] public Control vbox { get; set; }

	// Offense
	[Export] UIStat physical_melee_power { get; set; }
	[Export] UIStat spell_melee_power { get; set; }
	[Export] UIStat physical_ranged_power { get; set; }
	[Export] UIStat spell_ranged_power { get; set; }
	[Export] UIStat wisdom_scaler { get; set; }
	[Export] UIStat critical_hit_chance { get; set; }
	[Export] UIStat critical_hit_damage { get; set; }
	[Export] UIStat attack_speed { get; set; }
	[Export] UIStat attack_speed_increase { get; set; }
	[Export] UIStat cool_down_reduction { get; set; }

	// Defense
	[Export] UIStat armor { get; set; }
	[Export] UIStat poise { get; set; }
	[Export] UIStat block_amount { get; set; }
	[Export] UIStat retaliation { get; set; }
	[Export] UIStat physical_resistance { get; set; }
	[Export] UIStat pierce_resistance { get; set; }
	[Export] UIStat slash_resistance { get; set; }
	[Export] UIStat blunt_resistance { get; set; }
	[Export] UIStat bleed_resistance { get; set; }
	[Export] UIStat poison_resistance { get; set; }
	[Export] UIStat curse_resistance { get; set; }
	[Export] UIStat spell_resistance { get; set; }
	[Export] UIStat fire_resistance { get; set; }
	[Export] UIStat cold_resistance { get; set; }
	[Export] UIStat lightning_resistance { get; set; }
	[Export] UIStat holy_resistance { get; set; }

	// Health
	[Export] UIStat maximum_health { get; set; }
	[Export] UIStat health_bonus { get; set; }
	[Export] UIStat health_regeneration { get; set; }
	[Export] UIStat health_retaliation { get; set; }

	// Resource
	[Export] UIStat maximum_resource { get; set; }
	[Export] UIStat resource_regeneration { get; set; }
	[Export] UIStat resource_cost_reduction { get; set; }
	[Export] UIStat posture_regeneration { get; set; }

	// Misc

	[Export] UIStat movement_speed { get; set; }

}
