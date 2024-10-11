using Godot;
using System;

public partial class AbilityController : Node
{
	public bool ability_use_prevented = false;
    public bool done_rotating = true;

    [Signal] public delegate void ResourceEffectEventHandler(Player player, float resource_change);
    [Signal] public delegate void RotatePlayerEventHandler();
    [Signal] public delegate void ReleaseInputControlEventHandler();
    
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	 public void QueueAbility(Player player, Ability ability)
    {
        if(!player.ui.preventing_movement && !player.ui.capturing_input && !ability_use_prevented)
        {
        
            if(ability.state == Ability.States.NotQueued)
            {   
                if(CanAfford(player, ability) && CheckCross(player, ability)) // Check cross was here
                {
                    // GD.Print("queueing ability");
                    ability.state = Ability.States.Queued;
                }
            }
            
        }
    }

    public void AbilityFrameCheck(Player player)
    {
        if(player.ability_in_use != null)
		{
			// GD.Print("ability in use" + player.ability_in_use);
			player.ability_in_use.FrameCheck(player);
		}
    }

	public void CheckCanUseAbility(Player player, Ability ability)
    {
        //GD.Print("Checking can use ability");
       
        if(ability.state == Ability.States.Queued)
        {
            if(!ability.button_held)
            {
                if(ability.rotate_on_soft)
                {
                    if(player.systems.targeting_system.enemy_to_soft_target)
                    {
                        //GD.Print("Rotate soft ability");
                        SoftRotateAbility(player, ability);
						
                    }
                    else
                    {
						//GD.Print("Executing");
                        
                        ability.Execute(player);
                        if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
						AddToAbilityList(player, ability);
                    }
                }
                else
                {
                    // GD.Print("Execute non rotation");
					//GD.Print("Executing");
                    
                    ability.Execute(player);
                    if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
					AddToAbilityList(player, ability);
                }
            }
            else if (ability.button_held)
            {
                if(ability.rotate_on_held)
                {
                    GD.Print("difference in rotation " + MathF.Round(player.current_y_rotation - player.previous_y_rotation, 1)); 
					if(player.systems.targeting_system.enemy_pointed_toward != null)
                    {
                        //GD.Print("Soft targeting enemy while holding down ability");
                        SoftRotateAbility(player, ability);
                    }
                    else if(MathF.Round(player.current_y_rotation - player.previous_y_rotation, 1) == 0 && player.systems.targeting_system.enemy_pointed_toward == null)
                    {
                        GD.Print("Rotating on held");
                        EmitSignal(nameof(ReleaseInputControl));
                        ability.Execute(player);
						if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
                        AddToAbilityList(player, ability);
                    }
                }
                else
                {
					//GD.Print("Executing");
                    ability.Execute(player);
                    if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
					AddToAbilityList(player, ability);
                }
            }

        }
    }

    public bool CheckCross(Player player, Ability ability) // Checks what cross the ability is assigned to
    {
        GD.Print("Checking cross for " + ability.Name);
        GD.Print("primary left selected " + player.l_cross_primary_selected);
        if(ability.cross == Ability.Cross.Left)
		{
			if(player.l_cross_primary_selected)
			{
				if(ability.tier == Ability.Tier.Primary)
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
				if(ability.tier == Ability.Tier.Secondary)
                {
                    return true;
                }
				else
                {
                    return false;
                }
			}
		}
		else if(ability.cross == Ability.Cross.Right)
		{
			if(player.r_cross_primary_selected)
			{
				if(ability.tier == Ability.Tier.Primary)
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
                if(ability.tier == Ability.Tier.Secondary)
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
                //GD.Print("Knock back* Can afford because of charges");
                return true;
            }
            else
            {
                //GD.Print("Knock back* Can not afford because of charges");
                return false;
            }
            
        }
        else if(player.resource.current_value - ability.resource_change >= 0 || ability.resource_change == 0)
        {
            if(ability.cooldown_timer != null)
            {
                if(ability.cooldown_timer.TimeLeft == 0)
                {
                    //GD.Print("Can afford because of cooldown");
                    return true;
                }
                else
                {
                    //GD.Print("Can't afford because of cooldown");
                    return false;
                }
            }
            else
            {
                //GD.Print("Can afford because of resource");
                return true;
            }
        }
        else
        {
            //GD.Print("Can't afford because resource cost");
            return false;
        }
    
    }

	public void SoftRotateAbility(Player player, Ability ability)
    {
        
        // if(!ability.rotate_on_soft_far && (player.systems.targeting_system.enemy_close || player.systems.targeting_system.enemy_pointed_toward != null))
        // {
        
       
            if(!player.abilities_in_use_list.Contains(ability))
            {
                AddToAbilityList(player, ability);
                
                EmitSignal(nameof(RotatePlayer));
                //GD.Print("Rotating player");
                done_rotating = false;
            }
            // if(ability.button_held)
            // {
                //GD.Print("Ability held, done rotating: " + done_rotating);
            if(done_rotating && player.systems.targeting_system.enemy_pointed_toward != null)
            {
                if(ability.button_held)
                {
                    
                    EmitSignal(nameof(RotatePlayer));
                }
                
                ability.Execute(player);
                if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
                
            }
            else if(!done_rotating)
            {
                // done_rotating = false;
                EmitSignal(nameof(RotatePlayer));
            }
            else
            {
                ability.Execute(player);
                if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
            }
                
    }

	public void AddToAbilityList(Player player, Ability ability) // Adds the passed ability to the list of abilities if it is not already there
    {
        // GD.Print("adding ability to list");
        if(!player.abilities_in_use_list.Contains(ability))
        {
            player.abilities_in_use_list.AddFirst(ability);
            ability.in_use = true;
        }

        player.ability_in_use = player.abilities_in_use_list.First.Value;
    }

    public void RemoveFromAbilityList(Player player, Ability ability) // Removes ability from abilities used
    {
        // GD.Print("removing ability from list");
        player.abilities_in_use_list.Remove(ability);
        if(player.abilities_in_use_list.Count > 0)
        {
            player.ability_in_use = player.abilities_in_use_list.First.Value;
        }
        else
        {
            player.ability_in_use = null;
        }
        ability.in_use = false;
    }

    internal void OnNearInteractable(bool near_interactable)
    {
        //GD.Print("got signal from interact system");
        if(near_interactable == true)
        {
            ability_use_prevented = true;
        }
        else
        {
            ability_use_prevented = false;
        }
    }

    internal void HandleAbilitiesPrevented(bool abilities_prevented)
    {
        ability_use_prevented = abilities_prevented;
    }


    internal void HandleRotationFinished(bool finished)
    {
        //GD.Print("Rotation finished in ability controller");
        done_rotating = finished;
    }

   
}
