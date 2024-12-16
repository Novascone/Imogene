using Godot;
using System;
using System.Linq;
// Ability controller
// Load abilities for the player, and sends the abilities the player has to the UI 
public partial class AbilityAssigner : Node
{
	public static void AssignAbility(Player player_, Ability ability_, string bind_, Ability.Cross cross_, Ability.Tier tier_)
	{
		ability_.AssignedButton = bind_;
		ability_.AbilityCross = cross_;
		ability_.AbilityTier = tier_;
		player_.PlayerUI.AssignAbility(ability_.AbilityCross, ability_.AbilityTier, ability_.AssignedButton, ability_.Name, ability_.Icon);
	}

	public static Node LoadAbility(Player player_, string name_, string class_type_, string ability_type_) // Loads an ability from a string
    {
        var scene = GD.Load<PackedScene>("res://Entity/Player/Abilities/" + class_type_ + "/" + ability_type_ + "/" + name_ + "/" + name_ + ".tscn");
        Ability ability = (Ability)scene.Instantiate();
		
		player_.Abilities.AddChild(ability);
		ability.AbilityPressed += player_.OnAbilityPressed;
		ability.AbilityQueue += player_.OnAbilityQueue;
		ability.AbilityCheck += player_.OnAbilityCheck;
		ability.AbilityReleased += player_.OnAbilityReleased;
		player_.PlayerControllers.input_controller.AbilitySubscribe(ability);
		player_.PlayerControllers.movement_controller.AbilitySubscribe(ability);
		
		ability.AbilityFinished += player_.OnAbilityFinished;
        return ability;
    }
	
	public static void ChangeAbilityAssignment(Player player_, Ability.Cross cross_, Ability.Tier tier_, string bind_, string ability_name_)
	{
		foreach(Ability ability in player_.Abilities.GetChildren().Cast<Ability>())
		{
			if(ability.Name == ability_name_)
			{
				ability.AbilityCross = cross_;
				ability.AbilityTier = tier_;
				ability.AssignedButton = bind_;
			}
		}
	}

	public static void ClearAbility(Player player_, string ability_name_)
	{
		foreach(Ability ability in player_.Abilities.GetChildren().Cast<Ability>())
		{
			if(ability.Name == ability_name_)
			{
				ability.AbilityCross = Ability.Cross.None;
				ability.AbilityTier = Ability.Tier.None;
				ability.AssignedButton = "";
			}
		}
	}
}
