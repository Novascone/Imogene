using Godot;
using System;
using System.ComponentModel;


// Ability class, abilities must have a script, a resource, and a scene
// The scene will be a Node3D named the same as the ability, capitalized
// The resource will have an ID, Name, Description, Ability Path, Icon, Type, and up to 5 modifiers
// The Ability Path is not utilized right now. The Type decides which category it will be in
// The script controls how the ability will function

public partial class Ability : Node3D
{
    [Export] public string description { get; set; }
    [Export] public AbilityResource resource { get; set; }
    [Export] public string ability_type { get; set; }
    
    public string cross{ get; set; }
    public string cross_type { get; set; }
    public string assigned_button { get; set; }
    public string action_button { get; set; }
    public bool cross_selected;
    public Player player;
    public bool button_pressed;
    public bool button_released;
    public bool button_held;
    public int frames_held;
    public int frames_held_threshold = 20;

    public bool useable = true;
    public bool in_use = false;
    public int pressed = 0;
    public bool animation_finished = false;
    public bool ready_to_use;

    private CustomSignals _customSignals; // Custom signal instance

    public TargetingSystem targeting_system;

    public override void _Ready()
    {
      
    }
    

    public void CheckCanUse()
    {
       
    }

    public override void _UnhandledInput(InputEvent @event) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        if(assigned_button != null)
        {
            if(@event.IsActionPressed(assigned_button) && CheckCross())
            {
                button_pressed = true;
                button_released = false;
            }
            if(@event.IsActionReleased(assigned_button) && CheckCross())
            {
                button_pressed = false;
                if(frames_held < frames_held_threshold)
                {
                    button_released = true;
                }
                else
                {
                    frames_held = 0;
                    button_released = false;
                }                   
                // GD.Print("button released");
            }
        }
		
	}

    public virtual void Execute() // Default execute
    {
        GD.Print("access ability child");
    }

    public void AddToAbilityList(Ability ability) // Adds the passed ability to the list of abilities if it is not already there
    {
        GD.Print("adding ability to list");
        if(!player.abilities_in_use.Contains(ability))
        {
            player.abilities_in_use.AddFirst(ability);
            in_use = true;
        }

        player.ability_in_use = player.abilities_in_use.First.Value;
    }
    public void RemoveFromAbilityList(Ability ability) // Removes ability from abilities used
    {
        GD.Print("removing ability from list");
        player.abilities_in_use.Remove(ability);
        if(player.abilities_in_use.Count > 0)
        {
            player.ability_in_use = player.abilities_in_use.First.Value;
        }
        else
        {
            player.ability_in_use = null;
        }
        if(resource.type == "melee")
        {
            player.targeting_system.looking_at_soft = false;
        }

        in_use = false;
    }

    public virtual void AnimationHandler(Player s, string animation)
    {
        
    }
    public void GetPlayerInfo(Player playerinfo) // Handles the player info
    {
        player = playerinfo;
        targeting_system = player.targeting_system;
        player.tree.AnimationFinished += OnAnimationFinished;
    }

    public virtual void OnAnimationFinished(StringName animName)
    {
        // throw new NotImplementedException();
    }

    public bool CheckCross() // Checks what cross the ability is assigned to
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

    public void CheckAssignment(string incoming_button_assignment) // Checks the button the ability is assigned to
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