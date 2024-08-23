using Godot;
using System;
using System.Collections.Generic;
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

    public int resource_cost;
    public int charges;
    public int charges_used;
    public float cast_time;
    public Timer cooldown_timer;
    public bool rotate_on_soft;
    public bool rotate_on_soft_far;
    public bool rotate_on_soft_close;
    public bool rotate_on_held;

    private CustomSignals _customSignals; // Custom signal instance

    public States state;

    public enum States
    {
        not_queued,
        queued
    }
    public void QueueAbility()
    {
        if(player.can_use_abilities)
        {
            if(UIButton())
            {
                // GD.Print("this is a UI button");
                if(button_pressed)
                {
                    if(this.state == States.not_queued)
                    {   
                        if(CanAfford() && CheckCross())
                        {
                            GD.Print("queueing ability");
                            this.state = States.queued;
                        }
                    }
                }
            }
            else
            {
                if(this.state == States.not_queued)
                {   
                    if(CanAfford() && CheckCross())
                    {
                        GD.Print("queueing ability");
                        this.state = States.queued;
                    }
                }
            }
        }
    }

    public void CheckCanUseAbility()
    {
        if(state == States.queued)
        {
            if(!button_held)
            {
                if(rotate_on_soft)
                {
                    if(player.targeting_system.soft_targeting && player.targeting_system.enemy_to_soft_target)
                    {
                        // GD.Print("Rotate soft ability");
                        SoftRotateAbility();
                    }
                    else
                    {
                        Execute();
                    }
                }
                else
                {
                    // GD.Print("Execute non rotation");
                    Execute();
                }
            }
            else if (button_held)
            {
                if(rotate_on_held)
                {
                    // GD.Print("Button is held, waiting for player to complete rotation");
                    if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
                    {
                        Execute();
                        // player.movementController.movement_input_allowed = true;
                    }
                }
                else
                {
                    Execute();
                }
            }

        }
    }

    public bool CanAfford()
    {
        if(charges - charges_used >= 0 && charges > 0)
        {
           
            if(charges - charges_used != 0)
            {
                 GD.Print("Knock back* Can afford because of charges");
                return true;
            }
            else
            {
                GD.Print("Knock back* Can not afford because of charges");
                return false;
            }
            
        }
        else if(player.resource - resource_cost >= 0 || resource_cost == 0)
        {
            if(cooldown_timer != null)
            {
                if(cooldown_timer.TimeLeft == 0)
                {
                    GD.Print("Can afford because of cooldown");
                    return true;
                }
                else
                {
                    GD.Print("Can't afford because of cooldown");
                    return false;
                }
            }
            else
            {
                GD.Print("Can afford because of resource");
                return true;
            }
        }
        else
        {
            GD.Print("Can't afford because resource cost");
            return false;
        }
    
    }


    public void SoftRotateAbility()
    {
        if(!rotate_on_soft_far && player.targeting_system.enemy_close)
        {
            player.movement_controller.movement_input_allowed = false;
            GD.Print("Setting player movement to false");
            player.targeting_system.SoftTargetRotation();
            if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
            {
                // GD.Print("Execute rotation");
                Execute();
                // player.movementController.movement_input_allowed = true;
            }
        }
        else if (rotate_on_soft_far && player.targeting_system.enemy_far)
        {
            player.movement_controller.movement_input_allowed = false;
            // GD.Print("Setting player movement to false");
            player.targeting_system.SoftTargetRotation();
            if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
            {
                // GD.Print("Execute rotation");
                Execute();
                // player.movementController.movement_input_allowed = true;
            }
        }
        else
        {
            Execute();
        }
        
        
    }

    public override void _UnhandledInput(InputEvent @event) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        if(assigned_button != null)
        {
            if(@event.IsActionPressed(assigned_button) && CheckCross())
            {
                // GD.Print("Assigned button " + assigned_button);
                // GD.Print(this.Name + "Action strength " + );
                button_pressed = true;
                frames_held = 1;
                button_released = false;
                // GD.Print(this.Name + " has been pressed");
            }
            if(@event.IsActionReleased(assigned_button) && CheckCross())
            {
                button_pressed = false;
                button_released = true;
                // GD.Print(this.Name + " has been released");              
                // GD.Print("button released");
            }
        }
		
	}

    public bool UIButton()
    {
        if(assigned_button == "B")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckHeld()
    {
        // GD.Print(Name + " calling checkheld");
        
            if(frames_held < frames_held_threshold)
            {
                button_held = false;
            }
            else
            {
                button_held = true;
            }
            if(frames_held > 0 && !button_released)
            {
                frames_held += 1;
            }
            else
            {
                frames_held = 0;
            }
                
            return button_held;
    }

    public override void _PhysicsProcess(double delta)
    {
        // GD.Print(Name + " held " + CheckHeld());
       
    }

    public virtual void Execute() // Default execute
    {
        // GD.Print("access ability child");
    }

    public void AddToAbilityList(Ability ability) // Adds the passed ability to the list of abilities if it is not already there
    {
        // GD.Print("adding ability to list");
        if(!player.abilities_in_use.Contains(ability))
        {
            player.abilities_in_use.AddFirst(ability);
            in_use = true;
        }

        player.ability_in_use = player.abilities_in_use.First.Value;
    }
    public void RemoveFromAbilityList(Ability ability) // Removes ability from abilities used
    {
        // GD.Print("removing ability from list");
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

    public virtual void AnimationHandler(Player s, string animation)
    {
        
    }
    public void GetPlayerInfo(Player player_info) // Handles the player info
    {
        player = player_info;
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