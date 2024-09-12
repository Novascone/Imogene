using Godot;
using System;

public partial class AbilityController : Node
{
	public bool can_use_abilities = true;
    [Signal] public delegate void ResourceEffectEventHandler(Player player, float resource_change);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	 public void QueueAbility(Player player, Ability ability)
    {
        if(!player.ui.preventing_movement && !player.ui.capturing_input && can_use_abilities)
        {
        
            if(ability.state == Ability.States.not_queued)
            {   
                if(CanAfford(player, ability) && CheckCross(player, ability)) // Check cross was here
                {
                    GD.Print("queueing ability");
                    ability.state = Ability.States.queued;
                }
            }
            
        }

        // if(ability.UIButton())
        // {
        //     GD.Print("this is a UI button");
        //     GD.Print(ability.button_pressed);
        //     if(ability.button_pressed)
        //     {
        //         if(ability.state == Ability.States.not_queued)
        //         {   
        //             if(CanAfford(player, ability) && CheckCross(player, ability)) // Check cross was here
        //             {
        //                 GD.Print("queueing ability");
        //                 ability.state = Ability.States.queued;
        //             }
        //         }
        //     }
        // }
    }

    public void AbilityFrameCheck(Player player)
    {
        if(player.ability_in_use != null)
		{
			GD.Print("ability in use" + player.ability_in_use);
			player.ability_in_use.FrameCheck(player);
		}
    }

	public void CheckCanUseAbility(Player player, Ability ability)
    {
        if(ability.state == Ability.States.queued)
        {
            if(!ability.button_held)
            {
                if(ability.rotate_on_soft)
                {
                    if(player.systems.targeting_system.soft_targeting && player.systems.targeting_system.enemy_to_soft_target)
                    {
                        GD.Print("Rotate soft ability");
                        SoftRotateAbility(player, ability);
						
                    }
                    else
                    {
						GD.Print("Executing");
                        ability.Execute(player);
                        if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
						AddToAbilityList(player, ability);
                    }
                }
                else
                {
                    // GD.Print("Execute non rotation");
					GD.Print("Executing");
                    ability.Execute(player);
                    if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
					AddToAbilityList(player, ability);
                }
            }
            else if (ability.button_held)
            {
                if(ability.rotate_on_held)
                {
					
					AddToAbilityList(player, ability);
                    GD.Print("Button is held, waiting for player to complete rotation");
                    if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
                    {
                        GD.Print("Rotating on held");
                        ability.Execute(player);
						if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
                        AddToAbilityList(player, ability);
                        // player.movementController.movement_input_allowed = true;
                    }
                }
                else
                {
					GD.Print("Executing");
                    ability.Execute(player);
                    if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
					AddToAbilityList(player, ability);
                }
            }

        }
    }

    public bool CheckCross(Player player, Ability ability) // Checks what cross the ability is assigned to
    {
        if(ability.cross == "Left")
		{
			if(player.l_cross_primary_selected)
			{
				if(ability.level == "Primary")
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
				if(ability.level == "Secondary")
                {
                    return true;
                }
				else
                {
                    return false;
                }
			}
		}
		else if(ability.cross == "Right")
		{
			if(player.r_cross_primary_selected)
			{
				if(ability.level == "Primary")
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
                if(ability.level == "Secondary")
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

	public bool CanAfford(Player player, Ability ability)
    {
        if(ability.charges - ability.charges_used >= 0 && ability.charges > 0)
        {
           
            if(ability.charges - ability.charges_used != 0)
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
        else if(player.general_stats["resource"] - ability.resource_change >= 0 || ability.resource_change == 0)
        {
            if(ability.cooldown_timer != null)
            {
                if(ability.cooldown_timer.TimeLeft == 0)
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

	public void SoftRotateAbility(Player player, Ability ability)
    {
        if(!ability.rotate_on_soft_far && player.systems.targeting_system.enemy_close)
        {
			AddToAbilityList(player, ability);
            player.systems.targeting_system.SoftTargetRotation(player);
			ability.stop_movement_input = true;
			GD.Print("Rotating player");
            if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
            {
                GD.Print("Execute rotation");
				// GD.Print("Executing");
                ability.Execute(player);
                if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
				ability.stop_movement_input = false;
                // player.movementController.movement_input_allowed = true;
            }
        }
        else if (ability.rotate_on_soft_far && player.systems.targeting_system.enemy_far)
        {
            // GD.Print("Setting player movement to false");
			GD.Print("Rotating player");
			AddToAbilityList(player, ability);
			ability.stop_movement_input = true; // Not allowing player to move while they are being rotated
            player.systems.targeting_system.SoftTargetRotation(player);
            if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
            {
                GD.Print("Execute rotation");
				// GD.Print("Executing");
                ability.Execute(player);
                if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
				ability.stop_movement_input = false; // Allowing player to move after being rotated
				
                // player.movementController.movement_input_allowed = true;
            }
        }
        else
        {
			GD.Print("Executing");
            ability.Execute(player);
            if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
			AddToAbilityList(player, ability);
        }
    }

	public void AddToAbilityList(Player player, Ability ability) // Adds the passed ability to the list of abilities if it is not already there
    {
        // GD.Print("adding ability to list");
        if(!player.abilities_in_use.Contains(ability))
        {
            player.abilities_in_use.AddFirst(ability);
            ability.in_use = true;
        }

        player.ability_in_use = player.abilities_in_use.First.Value;
    }

    public void RemoveFromAbilityList(Player player, Ability ability) // Removes ability from abilities used
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
        ability.in_use = false;
    }

    internal void OnNearInteractable(bool near_interactable)
    {
        GD.Print("got signal from interact system");
        if(near_interactable == true)
        {
            can_use_abilities = false;
        }
        else
        {
            can_use_abilities = true;
        }
    }
}
