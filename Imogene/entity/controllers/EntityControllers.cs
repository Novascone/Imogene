using Godot;
using System;

public partial class EntityControllers : Node
{
	[Export] public StatsController stats_controller { get; set; }
	[Export] public StatusEffectController status_effect_controller { get; set; }
	
}
