using Godot;
using System;
using System.ComponentModel;


public partial class Ability : Node3D
{
    [Export]
    public string description { get; set; }

    [Export]
    public Resource resource { get; set; }
    [Export]
    public string ability_type { get; set; }

    public bool in_use = true;
    public int pressed = 0;
    public bool animation_finished = false;

    public virtual void Execute(player s)
    {
        GD.Print("Execute");
    }

    public virtual void AnimationHandler(player s, string animation)
    {
        
    }
}