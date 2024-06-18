using Godot;
using System;

public partial class Hitbox : Area3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] public string damage_type { get; set; }
	[Export] public float damage { get; set; }
	public bool is_critical;
	
}
