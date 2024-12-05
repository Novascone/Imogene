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
        if(!player_.PlayerUI.preventing_movement && !player_.PlayerUI.capturing_input && !ability_use_prevented)
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
        player_.AbilityInUse?.FrameCheck(player_);
    }

	public void CheckCanUseAbility(Player player, Ability ability)
    {
        if(ability.state == Ability.States.Queued)
        {
            if(!ability.button_held)
            {
                if(ability.rotate_on_soft)
                {
                    if(player.PlayerSystems.targeting_system.enemy_to_soft_target)
                    {
                        SoftRotateAbility(player, ability);
                    }
                    else
                    {
                        ability.Execute(player);
                        if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
                        AddToAbilityList(player, ability);
                    }
                }
                else
                {
                    ability.Execute(player);
                    if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
                    AddToAbilityList(player, ability);
                }
            }
            else if (ability.button_held)
            {
                if(ability.rotate_on_held)
                {
					if(player.PlayerSystems.targeting_system.enemy_pointed_toward != null)
                    {
                        SoftRotateAbility(player, ability);
                    }
                    else if(MathF.Round(player.CurrentYRotation - player.PreviousYRotation, 1) == 0 && player.PlayerSystems.targeting_system.enemy_pointed_toward == null)
                    {
                        EmitSignal(nameof(ReleaseInputControl));
                        ability.Execute(player);
						if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
                        AddToAbilityList(player, ability);
                    }
                }
                else
                {
                    ability.Execute(player);
                    if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
                    AddToAbilityList(player, ability);
                }
            }

        }
    }

    public static bool CheckCross(Player player, Ability ability) // Checks what cross the ability is assigned to
    {
        if(ability.cross == Ability.Cross.Left)
		{
			if(player.LCrossPrimarySelected)
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
			if(player.RCossPrimarySelected)
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

	public static bool CanAfford(Player player, Ability ability)
    {
        if(ability.charges - ability.charges_used >= 0 && ability.charges > 0)
        {
           
            if(ability.charges - ability.charges_used != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else if(player.Resource.CurrentValue - ability.resource_change >= 0 || ability.resource_change == 0)
        {
            if(ability.cooldown_timer != null)
            {
                if(ability.cooldown_timer.TimeLeft == 0)
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

	public void SoftRotateAbility(Player player, Ability ability)
    {
    
        if(!player.AbilitiesInUseList.Contains(ability))
        {
            AddToAbilityList(player, ability);
            
            EmitSignal(nameof(RotatePlayer));

            done_rotating = false;
        }

        if(done_rotating && player.PlayerSystems.targeting_system.enemy_pointed_toward != null)
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
            EmitSignal(nameof(RotatePlayer));
        }
        else
        {
            ability.Execute(player);
            if(ability.resource_change != 0){EmitSignal(nameof(ResourceEffect), player, ability.resource_change);}
        }
                
    }

	public static void AddToAbilityList(Player player, Ability ability) // Adds the passed ability to the list of abilities if it is not already there
    {
    
        if(!player.AbilitiesInUseList.Contains(ability))
        {
            player.AbilitiesInUseList.AddFirst(ability);
            ability.in_use = true;
        }

        player.AbilityInUse = player.AbilitiesInUseList.First.Value;
    }

    public static void RemoveFromAbilityList(Player player, Ability ability) // Removes ability from abilities used
    {
        player.AbilitiesInUseList.Remove(ability);
        if(player.AbilitiesInUseList.Count > 0)
        {
            player.AbilityInUse = player.AbilitiesInUseList.First.Value;
        }
        else
        {
            player.AbilityInUse = null;
        }
        ability.in_use = false;
    }

    internal void OnNearInteractable(bool nearInteractable)
    {
        ability_use_prevented = nearInteractable;
    }

    internal void HandleAbilitiesPrevented(bool abilities_prevented_)
    {
        ability_use_prevented = abilities_prevented_;
    }


    internal void HandleRotationFinished(bool finished_)
    {
        done_rotating = finished_;
    }

    public void Subscribe(Player player)
    {
        player.EntityControllers.status_effect_controller.AbilitiesPrevented += HandleAbilitiesPrevented;

        player.PlayerSystems.interact_system.NearInteractable += OnNearInteractable;
        player.PlayerSystems.targeting_system.RotationForAbilityFinished += HandleRotationFinished;
    }

     public void Unsubscribe(Player player)
    {
        player.EntityControllers.status_effect_controller.AbilitiesPrevented -= HandleAbilitiesPrevented;

        player.PlayerSystems.interact_system.NearInteractable -= OnNearInteractable;
        player.PlayerSystems.targeting_system.RotationForAbilityFinished -= HandleRotationFinished;
    }

   
}
