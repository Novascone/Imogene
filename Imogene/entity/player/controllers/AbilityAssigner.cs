using Godot;
using System;
using System.Linq;
// Ability controller
// Load abilities for the player, and sends the abilities the player has to the UI 
public partial class AbilityAssigner : Node
{
	public static void AssignAbility(Player player_, Ability ability_, string bind_, Ability.Cross cross_, Ability.Tier tier_)
	{
		ability_.assigned_button = bind_;
		ability_.cross = cross_;
		ability_.tier = tier_;
		player_.PlayerUI.AssignAbility(ability_.cross, ability_.tier, ability_.assigned_button, ability_.Name, ability_.icon);
	}

	public static Node LoadAbility(Player player_, string name_, string class_type_, string ability_type_) // Loads an ability from a string
    {
        var scene = GD.Load<PackedScene>("res://entity/player/abilities/" + class_type_ + "/" + ability_type_ + "/" + name_ + "/" + name_ + ".tscn");
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
				ability.cross = cross_;
				ability.tier = tier_;
				ability.assigned_button = bind_;
			}
		}
	}

	public static void ClearAbility(Player player_, string ability_name_)
	{
		foreach(Ability ability in player_.Abilities.GetChildren().Cast<Ability>())
		{
			if(ability.Name == ability_name_)
			{
				ability.cross = Ability.Cross.None;
				ability.tier = Ability.Tier.None;
				ability.assigned_button = "";
			}
		}
	}
}
