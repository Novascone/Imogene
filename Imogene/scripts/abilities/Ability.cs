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
    public string cross_type { get; set; }
    public string assigned_button { get; set; }



    public bool in_use = true;
    public int pressed = 0;
    public bool animation_finished = false;

    private CustomSignals _customSignals; // Custom signal instance

    public override void _Ready()
    {
      
    }

   

    public virtual void Execute(player s)
    {
        // GD.Print("Execute");
    }

    public virtual void AnimationHandler(player s, string animation)
    {
        
    }
}