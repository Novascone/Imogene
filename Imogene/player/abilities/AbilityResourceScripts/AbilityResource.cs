using Godot;
using System;

public partial class AbilityResource : Resource
{

    // Base class for loading abilities
	// [Export]
    // public int id { get; set; }
    // [Export]
    // public string class_type { get; set; }
    // [Export]
    // public string ability_type { get; set; }
    // [Export]
    // public string name { get; set; }
    // [Export]
    // public string description { get; set; }
    // [Export]
    // public string ability_path { get; set; }
    // [Export]
    // public Texture2D icon { get; set; }
    // [Export]
    // public string type;
    // [Export] 
    // public PackedScene modifier_1;
    // [Export] 
    // public PackedScene modifier_2;
    // [Export] 
    // public PackedScene modifier_3;
    // [Export] 
    // public PackedScene modifier_4;
    // [Export] 
    // public PackedScene modifier_5;

    public enum ClassType {General, Brigian, Mage, Monk, Rogue, Shaman}
    [Export] public ClassType class_type;
    public enum GeneralAbilityType {Melee, Ranged, Defensive, Movement, Unique, Toy}
    [Export] public GeneralAbilityType general_ability_type;
    public enum ClassAbilityType {None, Basic, Kernel, Defensive, Mastery, Movement, Specialized, Unique, Toy}
    [Export] public ClassAbilityType class_ability_type;
    public enum DamageType {None, Slash, Peirce, Blunt, Bleed, Poison, Fire, Cold, Lightning, Holy}
    [Export] public DamageType damage_type;
    [Export] public string description { get; set; }
    [Export] public Texture2D icon { get; set; }
    
}
