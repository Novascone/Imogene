using Godot;
using System;
// Ability controller
// Load abilities for the player, and sends the abilities the player has to the UI 
public partial class AbilityController : Controller
{
	// Called when the node enters the scene tree for the first time.
	
	private CustomSignals _customSignals; // Custom signal instance
	
	public override void _Ready()
	{
		
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	public void LoadAbilities() // Loads abilities
	{
		if(!player.abilities_loaded)
		{
			_customSignals.EmitSignal(nameof(CustomSignals.LCrossPrimaryOrSecondary), player.l_cross_primary_selected);
			_customSignals.EmitSignal(nameof(CustomSignals.RCrossPrimaryOrSecondary), player.r_cross_primary_selected);
			foreach(AbilityResource ability_resource in player.ability_resources)
			{
				LoadAbilitiesHelper(ability_resource);
			}

		}
		player.abilities_loaded = true;
   }

    private void LoadAbilitiesHelper(AbilityResource ability_resource) // Adds ability to abilities list
    {
       	Ability new_ability = (Ability)player.LoadAbility(ability_resource.name);
		player.abilities.Add(new_ability);
		AddChild(new_ability);
		new_ability.GetPlayerInfo(player);
		_customSignals.EmitSignal(nameof(CustomSignals.AvailableAbilities), ability_resource);
    }

	public void AssignAbilities()
	{	
		if(!player.test_abilities_assigned)
		{
			AssignAbilityHelper("RCrossPrimaryRightAssign", player.roll);
			AssignAbilityHelper("LCrossPrimaryUpAssign", player.slash);
			AssignAbilityHelper("RCrossPrimaryDownAssign", player.jump);
		}
		player.test_abilities_assigned = true;
	}
	
	 private void AssignAbilityHelper(string button_name, AbilityResource abilityResource)
	{
			_customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), abilityResource.name, button_name, abilityResource.icon);
			GD.Print("Ability Assigned");
	}
}
