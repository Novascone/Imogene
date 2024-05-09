using Godot;
using System;
using System.Collections.Generic;

public partial class SkillResource : Resource
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
