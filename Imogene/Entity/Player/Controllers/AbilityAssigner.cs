using Godot;
using System;
using System.Linq;
// Ability controller
// Load abilities for the player, and sends the abilities the player has to the UI 
public partial class AbilityAssigner : Node
{
	public static void AssignAbility(Player player, Ability ability, string bind, Ability.Cross cross, Ability.Tier tier)
	{
		ability.AssignedButton = bind;
		ability.AbilityCross = cross;
		ability.AbilityTier = tier;
		player.PlayerUI.AssignAbility(ability.AbilityCross, ability.AbilityTier, ability.AssignedButton, ability.Name, ability.Icon);
	}

	public static Node LoadAbility(Player player, string name, string classType, string abilityType) // Loads an ability from a string
    {
        var scene = GD.Load<PackedScene>("res://Entity/Player/Abilities/" + classType + "/" + abilityType + "/" + name + "/" + name + ".tscn");
        Ability ability = (Ability)scene.Instantiate();
		
		player.Abilities.AddChild(ability);
		ability.AbilityPressed += player.OnAbilityPressed;
		ability.AbilityQueue += player.OnAbilityQueue;
		ability.AbilityCheck += player.OnAbilityCheck;
		ability.AbilityReleased += player.OnAbilityReleased;
		player.PlayerControllers.InputController.AbilitySubscribe(ability);
		player.PlayerControllers.MovementController.AbilitySubscribe(ability);
		
		ability.AbilityFinished += player.OnAbilityFinished;
        return ability;
    }
	
	public static void ChangeAbilityAssignment(Player player, Ability.Cross cross, Ability.Tier tier, string bind, string abilityName)
	{
		foreach(Ability ability in player.Abilities.GetChildren().Cast<Ability>())
		{
			if(ability.Name == abilityName)
			{
				ability.AbilityCross = cross;
				ability.AbilityTier = tier;
				ability.AssignedButton = bind;
			}
		}
	}

	public static void ClearAbility(Player player, string abilityName)
	{
		foreach(Ability ability in player.Abilities.GetChildren().Cast<Ability>())
		{
			if(ability.Name == abilityName)
			{
				ability.AbilityCross = Ability.Cross.None;
				ability.AbilityTier = Ability.Tier.None;
				ability.AssignedButton = "";
			}
		}
	}
}
