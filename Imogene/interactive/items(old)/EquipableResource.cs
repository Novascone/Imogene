using Godot;
using System;

public partial class EquipableResource : ItemResource
{
    [Export]
    public string slot;
    [Export]
    public string equipable_type;
    [Export]
    public string item_path;

    public override void UseItem()
    {
        GD.Print("equipable");
    }

    public static explicit operator EquipableResource(PackedScene v)
    {
        throw new NotImplementedException();
    }

    public override void PrintItemStats()
    {
    }


}
