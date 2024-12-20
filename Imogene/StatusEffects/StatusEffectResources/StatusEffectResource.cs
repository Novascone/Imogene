using Godot;
using System;

public partial class StatusEffectResource : Resource
{
	[Export] public int ID { get; set; }
    [Export] public string Name { get; set; }
    [Export] public string Description { get; set; }
    [Export] public string AbilityPath { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public string Type;
    [Export] public bool PreventsMovement;
    [Export] public bool AltersSpeed;
    [Export] public PackedScene Modifier1;
    [Export] public PackedScene Modifier2;
    [Export] public PackedScene Modifier3;
    [Export] public PackedScene Modifier4;
    [Export] public PackedScene Modifier5;
}
