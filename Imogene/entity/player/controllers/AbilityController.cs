using Godot;
using System;

public partial class AbilityController : Node
{
	public bool ability_use_prevented { get; set; } = false;
    public bool done_rotating { get; set; } = true;

    [Signal] public delegate void ResourceEffectEventHandler(Player player_, float resource_change_);
    [Signal] public delegate void RotatePlayerEventHandler();
    [Signal] public delegate void ReleaseInputControlEventHandler();
    
	public void QueueAbility(Player player_, Ability ability_)
    {
        if(!player_.ui.preventing_movement && !player_.ui.capturing_input && !ability_use_prevented)
        {
        
            if(ability_.state == Ability.States.NotQueued)
            {   
                if(CanAfford(player_, ability_) && CheckCross(player_, ability_))
                {
                    ability_.state = Ability.States.Queued;
                }
            }
            
        }
    }

    public static void AbilityFrameCheck(Player player_)
    {
        player_.ability_in_use?.FrameCheck(player_);
    }

	public void CheckCanUseAbility(Player player_, Ability ability_)
    {
        if(ability_.state == Ability.States.Queued)
        {
            if(!ability_.button_held)
            {
                if(ability_.rotate_on_soft)
                {
                    if(player_.systems.targeting_system.enemy_to_soft_target)
                    {
                        SoftRotateAbility(player_, ability_);
                    }
                    else
                    {
                        ability_.Execute(player_);
                        if(ability_.resource_change != 0){EmitSignal(nameof(ResourceEffect), player_, ability_.resource_change);}
                        AddToAbilityList(player_, ability_);
                    }
                }
                else
                {
                    ability_.Execute(player_);
                    if(ability_.resource_change != 0){EmitSignal(nameof(ResourceEffect), player_, ability_.resource_change);}
                    AddToAbilityList(player_, ability_);
                }
            }
            else if (ability_.button_held)
            {
                if(ability_.rotate_on_held)
                {
					if(player_.systems.targeting_system.enemy_pointed_toward != null)
                    {
                        SoftRotateAbility(player_, ability_);
                    }
                    else if(MathF.Round(player_.current_y_rotation - player_.previous_y_rotation, 1) == 0 && player_.systems.targeting_system.enemy_pointed_toward == null)
                    {
                        EmitSignal(nameof(ReleaseInputControl));
                        ability_.Execute(player_);
						if(ability_.resource_change != 0){EmitSignal(nameof(ResourceEffect), player_, ability_.resource_change);}
                        AddToAbilityList(player_, ability_);
                    }
                }
                else
                {
                    ability_.Execute(player_);
                    if(ability_.resource_change != 0){EmitSignal(nameof(ResourceEffect), player_, ability_.resource_change);}
                    AddToAbilityList(player_, ability_);
                }
            }

        }
    }

    public static bool CheckCross(Player player_, Ability ability_) // Checks what cross the ability is assigned to
    {
        if(ability_.cross == Ability.Cross.Left)
		{
			if(player_.l_cross_primary_selected)
			{
				if(ability_.tier == Ability.Tier.Primary)
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
				if(ability_.tier == Ability.Tier.Secondary)
                {
                    return true;
                }
				else
                {
                    return false;
                }
			}
		}
		else if(ability_.cross == Ability.Cross.Right)
		{
			if(player_.r_cross_primary_selected)
			{
				if(ability_.tier == Ability.Tier.Primary)
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
                if(ability_.tier == Ability.Tier.Secondary)
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

	public static bool CanAfford(Player player_, Ability ability_)
    {
        if(ability_.charges - ability_.charges_used >= 0 && ability_.charges > 0)
        {
           
            if(ability_.charges - ability_.charges_used != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else if(player_.resource.current_value - ability_.resource_change >= 0 || ability_.resource_change == 0)
        {
            if(ability_.cooldown_timer != null)
            {
                if(ability_.cooldown_timer.TimeLeft == 0)
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
                return true;
            }
        }
        else
        {
            return false;
        }
    
    }

	public void SoftRotateAbility(Player player_, Ability ability_)
    {
    
        if(!player_.abilities_in_use_list.Contains(ability_))
        {
            AddToAbilityList(player_, ability_);
            
            EmitSignal(nameof(RotatePlayer));

            done_rotating = false;
        }

        if(done_rotating && player_.systems.targeting_system.enemy_pointed_toward != null)
        {
            if(ability_.button_held)
            {
                
                EmitSignal(nameof(RotatePlayer));
            }
            
            ability_.Execute(player_);
            if(ability_.resource_change != 0){EmitSignal(nameof(ResourceEffect), player_, ability_.resource_change);}
            
        }
        else if(!done_rotating)
        {
            EmitSignal(nameof(RotatePlayer));
        }
        else
        {
            ability_.Execute(player_);
            if(ability_.resource_change != 0){EmitSignal(nameof(ResourceEffect), player_, ability_.resource_change);}
        }
                
    }

	public static void AddToAbilityList(Player player_, Ability ability_) // Adds the passed ability to the list of abilities if it is not already there
    {
    
        if(!player_.abilities_in_use_list.Contains(ability_))
        {
            player_.abilities_in_use_list.AddFirst(ability_);
            ability_.in_use = true;
        }

        player_.ability_in_use = player_.abilities_in_use_list.First.Value;
    }

    public static void RemoveFromAbilityList(Player player_, Ability ability_) // Removes ability from abilities used
    {
        player_.abilities_in_use_list.Remove(ability_);
        if(player_.abilities_in_use_list.Count > 0)
        {
            player_.ability_in_use = player_.abilities_in_use_list.First.Value;
        }
        else
        {
            player_.ability_in_use = null;
        }
        ability_.in_use = false;
    }

    internal void OnNearInteractable(bool near_interactable_)
    {
        ability_use_prevented = near_interactable_;
    }

    internal void HandleAbilitiesPrevented(bool abilities_prevented_)
    {
        ability_use_prevented = abilities_prevented_;
    }


    internal void HandleRotationFinished(bool finished_)
    {
        done_rotating = finished_;
    }

    public void Subscribe(Player player_)
    {
        player_.entity_controllers.status_effect_controller.AbilitiesPrevented += HandleAbilitiesPrevented;

        player_.systems.interact_system.NearInteractable += OnNearInteractable;
        player_.systems.targeting_system.RotationForAbilityFinished += HandleRotationFinished;
    }

     public void Unsubscribe(Player player_)
    {
        player_.entity_controllers.status_effect_controller.AbilitiesPrevented -= HandleAbilitiesPrevented;

        player_.systems.interact_system.NearInteractable -= OnNearInteractable;
        player_.systems.targeting_system.RotationForAbilityFinished -= HandleRotationFinished;
    }

   
}
