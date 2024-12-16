using Godot;
using System;

public partial class EntitySystems : Node
{
	[Export] public DamageSystem damage_system { get; set; }
	[Export] public ResourceSystem resource_system { get; set; }

}
