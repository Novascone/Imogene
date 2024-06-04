using Godot;
using System;

public partial class AbilityResource : Resource
{

    // Base class for loading abilities
	[Export]
    public int id { get; set; }
    [Export]
    public string name { get; set; }
    [Export]
    public string description { get; set; }
    [Export]
    public string ability_path { get; set; }
    [Export]
    public Texture2D icon { get; set; }
    [Export]
    public string type;
    [Export] 
    public PackedScene modifier_1;
    [Export] 
    public PackedScene modifier_2;
    [Export] 
    public PackedScene modifier_3;
    [Export] 
    public PackedScene modifier_4;
    [Export] 
    public PackedScene modifier_5;
    
}
