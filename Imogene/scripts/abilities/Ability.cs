using Godot;
using System;
using System.ComponentModel;


public partial class Ability : Node3D
{
    [Export]
    public string description { get; set; }

    [Export]
    public Resource resource { get; set; }

    public virtual void Execute(player s)
    {
        GD.Print("Execute");
    }
}