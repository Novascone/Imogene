using Godot;
using System;

public partial class HUDHealth : Control
{
	[Export] public TextureProgressBar hit_points { get; set; }
	[Export] public Control health_movement_debuffs { get; set; }
	[Export] public Control health__movement_buffs { get; set; }

}
