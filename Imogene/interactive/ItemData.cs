using Godot;
using System;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string item_name;
    [Export] public Texture2D icon {get; set;}
    [Export] public PackedScene item_model_prefab {get; set;}
    
}
