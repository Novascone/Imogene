using Godot;
using System;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string ItemName {get; set;}
    [Export] public Texture2D Icon {get; set;}
    [Export] public PackedScene ItemModelPrefab {get; set;}
    
}
