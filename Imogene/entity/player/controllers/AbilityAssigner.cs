using Godot;
using System;
// Ability controller
// Load abilities for the player, and sends the abilities the player has to the UI 
public partial class AbilityAssigner : Node
{

	public bool can_use_abilities;
	UI ui;
	// Called when the node enters the scene tree for the first time.
	
	private CustomSignals _customSignals; // Custom signal instance
	
	public override void _Ready()
	{
		
	}

    public override void _Process(double delta)
    {
	
    }

	

    public void GetAbilities(Player player) // Loads abilities
	{
		// foreach(Ability ability in player.abilities)
		// {
		// 	LoadAbility(ability);
		// }
   	}

	public void AssignAbility(Player player, Ability ability, string bind, string cross, string level)
	{
		ability.assigned_button = bind;
		ability.cross = cross;
		ability.level = level;
		player.ui.AssignAbility(ability.cross, ability.level, ability.assigned_button, ability.Name, ability.icon);
	}

    private void LoadAbilitiesHelper(Player player, Ability ability) // Adds ability to abilities list
    {
       	// Ability new_ability = (Ability)LoadAbility(ability.name, ability.class_type, ability.ability_type);
		// player.abilities.Add(new_ability);
		// player.abilities_storage.AddChild(new_ability);
		// new_ability.GetPlayerInfo(player);
		// foreach(AbilityCategory ability_category in player.ui.abilities.categories.GetChildren())
		// {
		// 	if (ability_category.IsInGroup(ability_resource.type))
		// 	{
		// 		ability_category.AddAbility(ability_resource);
		// 	}
		// }
		
		// _customSignals.EmitSignal(nameof(CustomSignals.AvailableAbilities), ability_resource);
    }

	public  Node LoadAbility(Player player, string name, string class_type, string ability_type) // Loads an ability from a string
    {
        var scene = GD.Load<PackedScene>("res://entity/player/abilities/" + class_type + "/" + ability_type + "/" + name + "/" + name + ".tscn");
        Ability ability = (Ability)scene.Instantiate();
		
		player.abilities.AddChild(ability);
		ability.AbilityPressed += player.OnAbilityPressed;
		ability.AbilityQueue += player.OnAbilityQueue;
		ability.AbilityCheck += player.OnAbilityCheck;
		ability.AbilityReleased += player.OnAbilityReleased;
		player.controllers.input_controller.AbilitySubscribe(ability);
		player.controllers.movement_controller.AbilitySubscribe(ability);
		
		ability.AbilityFinished += player.OnAbilityFinished;
		GD.Print("finished subscribing signals to ability controller " + player.controllers.ability_controller.Name);
        return ability;
    }


	public void AssignAbilities(Player player)
	{	
		
		// if(!player.test_abilities_assigned)
		// {
			// AssignAbilityHelper("RCrossPrimaryRightAssign", player.small_fireball);
			// AssignAbilityHelper("LCrossPrimaryUpAssign", player.slash);
			// AssignAbilityHelper("RCrossPrimaryDownAssign", player.jump);
			// AssignAbilityHelper("LCrossPrimaryRightAssign", player.effects_test);
			// AssignAbilityHelper("LCrossPrimaryLeftAssign", player.projectile);
			// AssignAbilityHelper("RCrossPrimaryLeftAssign", player.dash);
			// AssignAbilityHelper("LCrossPrimaryDownAssign", player.whirlwind);
			// AssignAbilityHelper("RCrossPrimaryUpAssign", player.kick);
		// }
		// player.test_abilities_assigned = true;
	}
	
	 private void AssignAbility(string button_name, Ability ability)
	{
			// _customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), abilityResource.name, button_name, abilityResource.icon);
			// player.ui.abilities.AbilityAssigned(ability_resource, button_name);
			// player.ui.hud.AbilityAssigned(ability_resource, button_name);
			// GD.Print("hud ability assigned");
			ability.assigned_button = button_name;
			// GD.Print("Ability Assigned");
	}

	// public bool StatusEffectPreventingAbilities()
	// {
	// 	if(player.status_effect_controller.dazed || player.status_effect_controller.frozen || player.status_effect_controller.feared || player.status_effect_controller.hexed || player.status_effect_controller.staggered)
	// 	{
	// 		return true;
	// 	}
	// 	return false;
	// }

	public void SubscribeToUI(UI ui)
	{
		// ui.abilities.melee_abilities.AbilityChanged += OnAbilityChanged;
	}

    // private void OnAbilityChanged(string new_ability, string new_button_assignment)
    // {
    //     GD.Print("Got ability changed signal!");
	// 	GD.Print("new button assignment " + new_button_assignment+ " new ability " + new_ability);
	// 	// foreach(Ability ability in player.abilities)
	// 	// {
	// 	// 	if (ability.Name == new_ability)
	// 	// 	{
	// 	// 		ability.useable = true;
	// 	// 		ability.CheckAssignment(new_button_assignment);
	// 	// 	}
	// 	// }
	// 	// player.ui.abilities.ability_changed = false;
	// 	// player.ui.abilities.ability_to_change = null;
	// 	// player.ui.abilities.button_to_bind = null;
		
    // }

    
	public void ChangeAbilityAssignment(Player player, string cross, string level, string bind, string ability_name)
	{
		GD.Print("Changing ability assignment in ability assigner for " + ability_name);
		foreach(Ability ability in player.abilities.GetChildren())
		{
			if(ability.Name == ability_name)
			{
				ability.cross = cross;
				ability.level = level;
				ability.assigned_button = bind;
				GD.Print("to " + ability.assigned_button + " on " + ability.cross + " " + ability.level);
			}
		}
	}

	public void ClearAbility(Player player, string ability_name)
	{
		foreach(Ability ability in player.abilities.GetChildren())
		{
			if(ability.Name == ability_name)
			{
				ability.cross = "";
				ability.level = "";
				ability.assigned_button = "";
				GD.Print(ability.Name + " Was cleared");
			}
		}
	}
}
