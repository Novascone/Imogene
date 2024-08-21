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

    public override void _Process(double delta)
    {
        if(player.ui.abilities.ability_changed)
		{
			foreach(Ability ability in player.abilities)
			{
				if (ability.Name == player.ui.abilities.ability_to_change)
				{
					ability.useable = true;
					ability.CheckAssignment(player.ui.abilities.button_to_bind);
				}
			}
			player.ui.abilities.ability_changed = false;
			player.ui.abilities.ability_to_change = null;
			player.ui.abilities.button_to_bind = null;
		}
		// if(Input.IsActionJustPressed("five"))
		// {
		// 	AddAbilityResource("Hitscan");
		// }
    }

	public void AddAbilityResource(string ability_name)
	{
		AbilityResource ability_resource = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/" + ability_name + "/" + ability_name + ".tres");
		player.ability_resources.Add(ability_resource);
		LoadAbilitiesHelper(ability_resource);
		GD.Print("Added " + ability_name);
		foreach(Ability ability in player.abilities)
		{
			GD.Print(ability.Name);
		}
	}

    public void LoadAbilities() // Loads abilities
	{
		if(!player.abilities_loaded)
		{
			// _customSignals.EmitSignal(nameof(CustomSignals.LCrossPrimaryOrSecondary), player.l_cross_primary_selected);
			// _customSignals.EmitSignal(nameof(CustomSignals.RCrossPrimaryOrSecondary), player.r_cross_primary_selected);
			player.ui.hud.LCrossPrimaryOrSecondary(player.l_cross_primary_selected);
			player.ui.hud.RCrossPrimaryOrSecondary(player.r_cross_primary_selected);
			foreach(AbilityResource ability_resource in player.ability_resources)
			{
				LoadAbilitiesHelper(ability_resource);
			}

		}
		player.abilities_loaded = true;
   }

    private void LoadAbilitiesHelper(AbilityResource ability_resource) // Adds ability to abilities list
    {
       	Ability new_ability = (Ability)LoadAbility(ability_resource.name, ability_resource.class_type, ability_resource.ability_type);
		player.abilities.Add(new_ability);
		AddChild(new_ability);
		new_ability.GetPlayerInfo(player);
		foreach(AbilityCategory ability_category in player.ui.abilities.categories.GetChildren())
		{
			if (ability_category.IsInGroup(ability_resource.type))
			{
				ability_category.AddAbility(ability_resource);
			}
		}
		
		// _customSignals.EmitSignal(nameof(CustomSignals.AvailableAbilities), ability_resource);
    }

	public  Node LoadAbility(string name, string class_type, string ability_type) // Loads an ability from a string
    {
        var scene = GD.Load<PackedScene>("res://scripts/abilities/" + class_type + "/" + ability_type + "/" + name + "/" + name + ".tscn");
        var sceneNode = scene.Instantiate();
        return sceneNode;
    }


	public void AssignAbilities()
	{	
		if(!player.test_abilities_assigned)
		{
			AssignAbilityHelper("RCrossPrimaryRightAssign", player.roll);
			AssignAbilityHelper("LCrossPrimaryUpAssign", player.slash);
			AssignAbilityHelper("RCrossPrimaryDownAssign", player.jump);
			AssignAbilityHelper("LCrossPrimaryRightAssign", player.hitscan);
			AssignAbilityHelper("LCrossPrimaryLeftAssign", player.projectile);
			AssignAbilityHelper("RCrossPrimaryLeftAssign", player.dash);
			AssignAbilityHelper("LCrossPrimaryDownAssign", player.whirlwind);
		}
		player.test_abilities_assigned = true;
	}
	
	 private void AssignAbilityHelper(string button_name, AbilityResource ability_resource)
	{
			// _customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), abilityResource.name, button_name, abilityResource.icon);
			player.ui.abilities.AbilityAssigned(ability_resource, button_name);
			player.ui.hud.AbilityAssigned(ability_resource, button_name);
			GD.Print("hud ability assigned");
			foreach(Ability ability in player.abilities)
			{
				if(ability.Name == ability_resource.name)
				{
					ability.CheckAssignment(button_name);
				}
			}
			GD.Print("Ability Assigned");
	}

	

}
