using Godot;
using System;

public partial class AbilityController : Node
{
	public bool AbilityUsePrevented { get; set; } = false;
    public bool DoneRotating { get; set; } = true;

    [Signal] public delegate void ResourceEffectEventHandler(Player player, float resourceChange);
    [Signal] public delegate void RotatePlayerEventHandler();
    [Signal] public delegate void ReleaseInputControlEventHandler();
    
	public void QueueAbility(Player player, Ability ability)
    {
        if(!player.PlayerUI.preventing_movement && !player.PlayerUI.capturing_input && !AbilityUsePrevented)
        {
        
            if(ability.State == Ability.States.NotQueued)
            {   
                if(CanAfford(player, ability) && CheckCross(player, ability))
                {
                    ability.State = Ability.States.Queued;
                }
            }
            
        }
    }

    public static void AbilityFrameCheck(Player player)
    {
        player.AbilityInUse?.FrameCheck(player);
    }

	public void CheckCanUseAbility(Player player, Ability ability)
    {
        if(ability.State == Ability.States.Queued)
        {
            if(!ability.ButtonHeld)
            {
                if(ability.RotateOnSoft)
                {
                    if(player.PlayerSystems.TargetingSystem.EnemyToSoftTarget)
                    {
                        SoftRotateAbility(player, ability);
                    }
                    else
                    {
                        ability.Execute(player);
                        if(ability.ResourceChange != 0){EmitSignal(nameof(ResourceEffect), player, ability.ResourceChange);}
                        AddToAbilityList(player, ability);
                    }
                }
                else
                {
                    ability.Execute(player);
                    if(ability.ResourceChange != 0){EmitSignal(nameof(ResourceEffect), player, ability.ResourceChange);}
                    AddToAbilityList(player, ability);
                }
            }
            else if (ability.ButtonHeld)
            {
                if(ability.RotateOnHeld)
                {
					if(player.PlayerSystems.TargetingSystem.EnemyPointedToward != null)
                    {
                        SoftRotateAbility(player, ability);
                    }
                    else if(MathF.Round(player.CurrentYRotation - player.PreviousYRotation, 1) == 0 && player.PlayerSystems.TargetingSystem.EnemyPointedToward == null)
                    {
                        EmitSignal(nameof(ReleaseInputControl));
                        ability.Execute(player);
						if(ability.ResourceChange != 0){EmitSignal(nameof(ResourceEffect), player, ability.ResourceChange);}
                        AddToAbilityList(player, ability);
                    }
                }
                else
                {
                    ability.Execute(player);
                    if(ability.ResourceChange != 0){EmitSignal(nameof(ResourceEffect), player, ability.ResourceChange);}
                    AddToAbilityList(player, ability);
                }
            }

        }
    }

    public static bool CheckCross(Player player, Ability ability) // Checks what cross the ability is assigned to
    {
        if(ability.AbilityCross == Ability.Cross.Left)
		{
			if(player.LCrossPrimarySelected)
			{
				if(ability.AbilityTier == Ability.Tier.Primary)
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
				if(ability.AbilityTier == Ability.Tier.Secondary)
                {
                    return true;
                }
				else
                {
                    return false;
                }
			}
		}
		else if(ability.AbilityCross == Ability.Cross.Right)
		{
			if(player.RCossPrimarySelected)
			{
				if(ability.AbilityTier == Ability.Tier.Primary)
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
                if(ability.AbilityTier == Ability.Tier.Secondary)
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
        if(ability.Charges - ability.ChargesUsed >= 0 && ability.Charges > 0)
        {
           
            if(ability.Charges - ability.ChargesUsed != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else if(player.Resource.CurrentValue - ability.ResourceChange >= 0 || ability.ResourceChange == 0)
        {
            if(ability.CooldownTimer != null)
            {
                if(ability.CooldownTimer.TimeLeft == 0)
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

            DoneRotating = false;
        }

        if(DoneRotating && player.PlayerSystems.TargetingSystem.EnemyPointedToward != null)
        {
            if(ability.ButtonHeld)
            {
                
                EmitSignal(nameof(RotatePlayer));
            }
            
            ability.Execute(player);
            if(ability.ResourceChange != 0){EmitSignal(nameof(ResourceEffect), player, ability.ResourceChange);}
            
        }
        else if(!DoneRotating)
        {
            EmitSignal(nameof(RotatePlayer));
        }
        else
        {
            ability.Execute(player);
            if(ability.ResourceChange != 0){EmitSignal(nameof(ResourceEffect), player, ability.ResourceChange);}
        }
                
    }

	public static void AddToAbilityList(Player player, Ability ability) // Adds the passed ability to the list of abilities if it is not already there
    {
    
        if(!player.AbilitiesInUseList.Contains(ability))
        {
            player.AbilitiesInUseList.AddFirst(ability);
            ability.InUse = true;
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
        ability.InUse = false;
    }

    internal void OnNearInteractable(bool nearInteractable)
    {
        AbilityUsePrevented = nearInteractable;
    }

    internal void HandleAbilitiesPrevented(bool abilitiesPrevented)
    {
        AbilityUsePrevented = abilitiesPrevented;
    }


    internal void HandleRotationFinished(bool finished)
    {
        DoneRotating = finished;
    }

    public void Subscribe(Player player)
    {
        player.EntityControllers.EntityStatusEffectsController.AbilitiesPrevented += HandleAbilitiesPrevented;

        player.PlayerSystems.InteractSystem.NearInteractable += OnNearInteractable;
        player.PlayerSystems.TargetingSystem.RotationForAbilityFinished += HandleRotationFinished;
    }

     public void Unsubscribe(Player player)
    {
        player.EntityControllers.EntityStatusEffectsController.AbilitiesPrevented -= HandleAbilitiesPrevented;

        player.PlayerSystems.InteractSystem.NearInteractable -= OnNearInteractable;
        player.PlayerSystems.TargetingSystem.RotationForAbilityFinished -= HandleRotationFinished;
    }

   
}
