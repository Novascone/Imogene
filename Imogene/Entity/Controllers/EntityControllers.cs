using Godot;
using System;

public partial class EntityControllers : Node
{
	[Export] public StatsController EntityStatsController { get; set; }
	[Export] public StatusEffectController EntityStatusEffectsController { get; set; }
	
}
