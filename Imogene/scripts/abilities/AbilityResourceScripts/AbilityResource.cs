using Godot;
using System;

public partial class AbilityResource : Resource
{
	[Export]
    public int id { get; set; }
    [Export]
    public string name { get; set; }
    [Export]
    public string resource_path { get; set; }
    [Export]
    public Texture2D icon { get; set; }
    [Export]
    public string type_of_ability;
}
