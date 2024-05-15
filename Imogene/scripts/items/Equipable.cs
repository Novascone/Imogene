using Godot;
using System;

public partial class Equipable : Item
{
    [Export]
    public string slot;
    [Export]
    public string item_path;
    [Export]
    public int strength;

    public override void UseItem()
    {
        // GD.Print(mesh.GetType());
    }

    public static explicit operator Equipable(PackedScene v)
    {
        throw new NotImplementedException();
    }
}
