using Godot;
using System;

public partial class HUDResource : Control
{
	[Export] public TextureProgressBar resource_points { get; set; }
	[Export] public Control resource_damage_buffs { get; set; }
	[Export] public Control resource_damage_debuffs { get; set; }
}
