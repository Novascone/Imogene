using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;

public partial class AbilityCategory : PanelContainer
{
	private AbilitiesInterface this_interface; // Reference to the AbilitiesInterface
	private GridContainer ability_container_page_1; // First page of abilities
	private GridContainer ability_container_page_2; // Second page of abilities
	private HBoxContainer ability_modifier_container; // Modifiers for abilities
	
	private VBoxContainer button_to_be_assigned_container; // The ability that is going to be/is assigned, and the accept/ cancel buttons
	private AbilityButton button_clicked; // The button from the list of abilities that was clicked
	private Button current_button; // Last button clicked on
	private Button button_to_be_assigned_label; // Cross button that the ability is going to be assigned to
	private Button button_to_be_assigned; // The button that the ability will be assigned to displayed at the bottom of the UI node
	private Button action_button_to_be_assigned; // The action button (HUD) that is going to be assigned
	private List<AbilityButton> abilities = new List<AbilityButton>(); // List of ability buttons
	private List<Button> modifiers = new List<Button>(); // List of modifiers
	private CustomSignals _customSignals;

	[Signal] public delegate void AbilityChangedEventHandler(string previous_ability, string new_ability );
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ability_container_page_1 = GetNode<GridContainer>("VBoxContainer/PanelContainer/HBoxContainer/GridContainerPage1");
		ability_container_page_2 = GetNode<GridContainer>("VBoxContainer/PanelContainer/HBoxContainer/GridContainerPage2");
		ability_modifier_container = GetNode<HBoxContainer>("VBoxContainer/PanelContainer/ModifierTreeContainer/Modifier/VBoxContainer/AbilityModifiers");
		button_to_be_assigned_container = GetNode<VBoxContainer>("VBoxContainer/PanelContainer/AssignedAndAccepted");
		button_to_be_assigned = GetNode<Button>("VBoxContainer/PanelContainer/AssignedAndAccepted/ButtonToBeAssigned/VBoxContainer/ButtonToBeAssigned");
		button_to_be_assigned_label = GetNode<Button>("VBoxContainer/PanelContainer/AssignedAndAccepted/ButtonToBeAssigned/VBoxContainer/ButtonToBeAssignedLabel");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		
		// _customSignals.AddToAbilitySelection += HandleAddToAbilitySelection;
		// _customSignals.AvailableAbilities += HandleAvailableAbilities;
		// _customSignals.AbilitySelected += HandleAbilitySelected;
		// _customSignals.ButtonName += HandleButtonName;
		// _customSignals.AbilityAccept += HandleAbilityAccept;
		// _customSignals.AbilityCancel += HandleAbilityCancel;
		// _customSignals.CurrentAbilityBoundOnCrossButton += HandleCurrentAbilityBoundOnCrossButton;
		// _customSignals.AbilityAssigned += HandleAbilityAssigned;
		

		foreach(AbilityButton ability in ability_container_page_1.GetChildren().Cast<AbilityButton>()) // check abilities available and set them in the category page
		{
			if(ability.GetIndex() < 11)
			{
				abilities.Add(ability);
			}
		}
		if(abilities.Count > 12)
		{
			foreach(AbilityButton ability in ability_container_page_2.GetChildren().Cast<AbilityButton>())
			{
				if(ability.GetIndex() > 11)
				{
					abilities.Add(ability);
				}
			}
		}
		

		foreach(AbilityButton modifier in ability_modifier_container.GetChildren().Cast<AbilityButton>()) // Add modifiers for abilities to category page
		{
			modifiers.Add(modifier);
		}
		
	}

	public void GetAbilityInterfaceInfo(AbilitiesInterface i) // Gets information about the AbilityInterface
	{
		this_interface = i;
	}


	public void AbilityAssigned(AbilityResource ability_resource, string button_name) // Assign abilities to Ability buttons
	{
		// foreach(AbilityButton ability_button in abilities)
		// {
		// 	if(ability_button.assigned && ability_button.ability_name == ability_resource.name)
		// 	{
		// 		// ability.Text = name;
		// 		ability_button.button_assigned = button_name;
		// 		// GD.Print(ability_button.Name);
		// 		// GD.Print(ability_button.button_assigned);
		// 		break;
		// 	}
		// }
	}

    
	public void AbilityCancel() // Remove button that would be assigned's icon
	{
		button_to_be_assigned.Icon = null;
	}
	
	public void AbilityAccept() // Remove button that would be assigned's icon, Store the AbilityButton
	{
		button_to_be_assigned.Icon = null;
		current_button = button_clicked;
	}

	// private void HandleAvailableAbilities(AbilityResource ability)
    // {
    //     AddAbility(ability.name, ability.type, ability.description, ability.icon);
    // }

	public void CurrentAbilityBoundOnCrossButton(Texture2D icon) // Set the ability selected icon to the display
    {
        button_to_be_assigned.Icon = icon;
    }


	public void ButtonName(Button cross_button, string cross_button_name) // Get the button names and give the AbilityButton that information
	{
		button_to_be_assigned_label.Text = cross_button_name;
		foreach(AbilityButton ability_button in abilities)
		{
			ability_button.ButtonToBeAssigned(cross_button, button_to_be_assigned);
		}
	}

    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	
	}

	public void AddAbility(AbilityResource ability_resource) // Add ability to category
	{
		// if(this.IsInGroup(ability_resource.type))
		// {
		// 	foreach(AbilityButton ability in abilities)
		// 	{
		// 		if(!ability.assigned)
		// 		{
		// 			// ability.Text = name;
		// 			ability.ability_resource = ability_resource;
		// 			ability.ability_name = ability_resource.name;
		// 			ability.ability_type = ability_resource.type;
		// 			ability.info_text.Text = ability_resource.description;
		// 			ability.Icon = ability_resource.icon;
		// 			ability.category = this;
		// 			ability.assigned = true;
		// 			break;
		// 		}
		// 	}
		// }
		
	}

	
	public void AbilitySelected(AbilityButton button, Button assign_button) // Set the proper information when an ability is clicked
	{
		// GD.Print("button  " + button.Name);
		// GD.Print("assign button " + assign_button.Name);
		button_clicked = button;
		action_button_to_be_assigned = assign_button;
		current_button = assign_button;
		if(button_clicked.button_assigned == null)
		{
			button_clicked.button_assigned = assign_button.Name;
		}
		// GD.Print("button clicked " + button.Name);
		// GD.Print("action_button_to_be_assigned " + action_button_to_be_assigned.Name);
		// GD.Print("current_button " + current_button.Name);
	}



	public void _on_accept_button_down() // When accept is clicked remove old ability, call respective AbilityAccept, and AbilityAssigned methods for the category and the interface, set variables for AbilityController to check
	{	
		GD.Print("accept button down");
		// _customSignals.EmitSignal(nameof(CustomSignals.AbilityRemoved),button_clicked.ability_name, button_clicked.button_assigned);
		EmitSignal(nameof(AbilityChanged),button_clicked.ability_name, action_button_to_be_assigned.Name);
		this_interface.AbilityRemoved(button_clicked.button_assigned);
		AbilityAccept();
		this_interface.AbilityAccept();
		this_interface.AbilityAssigned(button_clicked.ability_resource, action_button_to_be_assigned.Name);
		// _customSignals.EmitSignal(nameof(CustomSignals.AbilityAccept));
		this_interface.ability_changed = true;
		
		this_interface.ability_to_change = button_clicked.ability_name;
		this_interface.button_to_bind = action_button_to_be_assigned.Name;
		// _customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), button_clicked.ability_name, action_button_to_be_assigned.Name, button_clicked.Icon);

		
	}

	public void _on_cancel_button_down() // Call respective Ability Cancel methods
	{
		// _customSignals.EmitSignal(nameof(CustomSignals.AbilityCancel));
		this_interface.AbilityCancel();
		AbilityCancel();
		button_clicked.AbilityCancel();

	}

	// Show/hide ability pages
	public void _on_last_page_button_down()
	{
		ability_container_page_1.Show();
		ability_container_page_2.Hide();
	}

	public void _on_next_page_button_down()
	{
		ability_container_page_2.Show();
		ability_container_page_1.Hide();
	}

	// private void HandleAbilityAccept()
	// {
	// 	button_to_be_assigned.Icon = null;
	// 	current_button = button_clicked;
	// }

	

	// private void HandleAbilityCancel()
	// {
	// 	button_to_be_assigned.Icon = null;
	// }

	// private void HandleAbilitySelected(AbilityButton button, Button assign_button)
    // {
    //     button_clicked = button;
	// 	action_button_to_be_assigned = assign_button;
	// 	current_button = assign_button;
	// 	if(button_clicked.button_assigned == null)
	// 	{
	// 		button_clicked.button_assigned = assign_button.Name;
	// 	}
		
	// 	// GD.Print(button_clicked.button_assigned);
    // }

	// private void HandleAvailableAbilities(AbilityResource ability)
    // {
    //     AddAbility(ability.name, ability.type, ability.description, ability.icon);
    // }

    // private void HandleAddToAbilitySelection(AbilityResource ability_resource)
    // {
	// 	if(this.IsInGroup(type))
	// 	{
	// 		GD.Print("HandleAddToAbilitySelection");
	// 		AddAbility(name, type, description, icon);
	// 	}
    // }

	 // private void HandleButtonName(Button cross_button, string cross_button_name)
    // {
    //     button_to_be_assigned_label.Text = cross_button_name;
	// 	_customSignals.EmitSignal(nameof(CustomSignals.ButtonToBeAssigned), cross_button, button_to_be_assigned);
    // }

	// private void HandleCurrentAbilityBoundOnCrossButton(Texture2D icon)
    // {
    //     button_to_be_assigned.Icon = icon;
    // }

	 // private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    // {
    //    foreach(AbilityButton abilityButton in abilities)
	// 		{
	// 			if(abilityButton.assigned && abilityButton.ability_name == ability)
	// 			{
	// 				// ability.Text = name;
	// 				abilityButton.button_assigned = button_name;
	// 				GD.Print("here");
	// 				GD.Print(abilityButton.Name);
	// 				GD.Print(abilityButton.button_assigned);
	// 				GD.Print("here");
	// 				break;
	// 			}
	// 		}
    // }

}

