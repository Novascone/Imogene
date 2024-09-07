using Godot;
using System;
using System.Collections.Generic;

public partial class MeleeHitbox : Area3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public string damage_type { get; set; }
	[Export] public float damage { get; set; }
	[Export] public float posture_damage { get; set; }
	public bool is_critical;
	public List<StatusEffect> effects = new List<StatusEffect>();
	[Export] public string effect_1;
	[Export] public string effect_2;
	[Export] public string effect_3;
	
}
