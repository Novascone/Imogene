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
    public string cross{ get; set; }
    public string cross_type { get; set; }
    public string assigned_button { get; set; }
    public string action_button { get; set; }
    public bool cross_selected;
    public player player;

    public bool useable = true;
    public bool in_use = false;
    public int pressed = 0;
    public bool animation_finished = false;

    private CustomSignals _customSignals; // Custom signal instance

    public override void _Ready()
    {
      
    }

   

    public virtual void Execute()
    {
        // GD.Print("Execute");
    }
    public void AddToAbilityList(Ability ability)
    {
        if(!player.abilities_in_use.Contains(ability))
        {
            player.abilities_in_use.AddFirst(ability);
            in_use = true;
        }
        player.ability_in_use = player.abilities_in_use.First.Value;
    }
    public void RemoveFromAbilityList(Ability ability)
    {
        
        player.abilities_in_use.Remove(ability);
        if(player.abilities_in_use.Count > 0)
        {
            player.ability_in_use = player.abilities_in_use.First.Value;
        }
        else
        {
            player.ability_in_use = null;
        }
        in_use = false;
    }

    public virtual void AnimationHandler(player s, string animation)
    {
        
    }
    public bool CheckCross()
    {
        if(cross == "left")
		{
			if(player.l_cross_primary_selected)
			{
				if(cross_type == "primary")
				{
					return true;
				}
                else
                {
                    return false;
                }
			}
			else
			{
				if(cross_type == "secondary")
                {
                    return true;
                }
				else
                {
                    return false;
                }
			}
		}
		else if(cross == "right")
		{
			if(player.r_cross_primary_selected)
			{
				if(cross_type == "primary")
				{
					return true;
				}
                else
                {
                    return false;
                }
			}
			else
			{
                if(cross_type == "secondary")
                {
                    return true;
                }
				else
                {
                    return false;
                }
			}
		}

        return false;
		
    }

    public void CheckAssignment(string incoming_button_assignment)
    {
        if(incoming_button_assignment == "LCrossPrimaryUpAssign")
        {
            cross = "left";
            cross_type = "primary";
            assigned_button = "RB";
            action_button = "LCrossPrimaryUpAssign";
        }
        if(incoming_button_assignment == "LCrossPrimaryRightAssign")
        {
            cross = "left";
            cross_type = "primary";
            assigned_button = "RT";
            action_button = "LCrossPrimaryRightAssign";
        }
        if(incoming_button_assignment == "LCrossPrimaryLeftAssign")
        {
            cross = "left";
            cross_type = "primary";
            assigned_button = "LB";
        }
        if(incoming_button_assignment == "LCrossPrimaryDownAssign")
        {
            cross = "left";
            cross_type = "primary";
            assigned_button = "LT";
        }



        if(incoming_button_assignment == "LCrossSecondaryUpAssign")
        {
            cross = "left";
            cross_type = "secondary";
            assigned_button = "RB";
        }
        if(incoming_button_assignment == "LCrossSecondaryRightAssign")
        {
            cross = "left";
            cross_type = "secondary";
            assigned_button = "RT";
        }
        if(incoming_button_assignment == "LCrossSecondaryLeftAssign")
        {
            cross = "left";
            cross_type = "secondary";
            assigned_button = "LB";
        }
        if(incoming_button_assignment == "LCrossSecondaryDownAssign")
        {
            cross = "left";
            cross_type = "secondary";
            assigned_button = "LT";
        }



        if(incoming_button_assignment == "RCrossPrimaryUpAssign")
        {
            cross = "right";
            cross_type = "primary";
            assigned_button = "Y";
        }
        if(incoming_button_assignment == "RCrossPrimaryRightAssign")
        {
            cross = "right";
            cross_type = "primary";
            assigned_button = "B";
        }
        if(incoming_button_assignment == "RCrossPrimaryLeftAssign")
        {
            cross = "right";
            cross_type = "primary";
            assigned_button = "X";
        }
        if(incoming_button_assignment == "RCrossPrimaryDownAssign")
        {
            cross = "right";
            cross_type = "primary";
            assigned_button = "A";
        }



        if(incoming_button_assignment == "RCrossSecondaryUpAssign")
        {
            cross = "right";
            cross_type = "secondary";
            assigned_button = "Y";
        }
        if(incoming_button_assignment == "RCrossSecondaryRightAssign")
        {
            cross = "right";
            cross_type = "secondary";
            assigned_button = "B";
        }
        if(incoming_button_assignment == "RCrossSecondaryLeftAssign")
        {
            cross = "right";
            cross_type = "secondary";
            assigned_button = "X";
        }
        if(incoming_button_assignment == "RCrossSecondaryDownAssign")
        {
            cross = "right";
            cross_type = "secondary";
            assigned_button = "A";
        }
    }
}