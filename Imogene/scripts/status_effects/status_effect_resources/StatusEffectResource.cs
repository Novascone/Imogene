using Godot;
using System;

public partial class StatusEffectResource : Resource
{
	[Export] public int id { get; set; }
    [Export] public string name { get; set; }
    [Export] public string description { get; set; }
    [Export] public string ability_path { get; set; }
    [Export] public Texture2D icon { get; set; }
    [Export] public string type;
    [Export] public bool prevents_movement;
    [Export] public bool alters_speed;
    [Export] public PackedScene modifier_1;
    [Export] public PackedScene modifier_2;
    [Export] public PackedScene modifier_3;
    [Export] public PackedScene modifier_4;
    [Export] public PackedScene modifier_5;
}
