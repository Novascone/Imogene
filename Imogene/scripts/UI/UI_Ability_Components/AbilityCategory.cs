using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

public partial class AbilityCategory : PanelContainer
{
	private GridContainer ability_container;
	private HBoxContainer ability_modifier_container;
	private AbilityButton button_clicked;
	private VBoxContainer button_to_be_assigned_container;
	private Button button_to_be_assigned_label;
	private Button button_to_be_assigned;
	private List<AbilityButton> abilities = new List<AbilityButton>();
	private List<Button> modifiers = new List<Button>();
	private CustomSignals _customSignals;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ability_container = GetNode<GridContainer>("VBoxContainer/PanelContainer/GridContainer");
		ability_modifier_container = GetNode<HBoxContainer>("VBoxContainer/PanelContainer/ModifierTreeContainer/Modifier/VBoxContainer/AbilityModifiers");
		button_to_be_assigned_container = GetNode<VBoxContainer>("VBoxContainer/PanelContainer/AssignedAndAccepted");
		button_to_be_assigned = GetNode<Button>("VBoxContainer/PanelContainer/AssignedAndAccepted/ButtonToBeAssigned/VBoxContainer/ButtonToBeAssigned");
		button_to_be_assigned_label = GetNode<Button>("VBoxContainer/PanelContainer/AssignedAndAccepted/ButtonToBeAssigned/VBoxContainer/ButtonToBeAssignedLabel");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");

		_customSignals.AddToAbilitySelection += HandleAddToAbilitySelection;
		_customSignals.AvailableAbilities += HandleAvailableAbilities;
		_customSignals.SendButtonClicked += HandleButtonClicked;
		_customSignals.ButtonName += HandleButtonName;

		foreach(AbilityButton ability in ability_container.GetChildren().Cast<AbilityButton>())
		{
			abilities.Add(ability);
		}
		// GD.Print(abilities.Count);

		foreach(AbilityButton modifier in ability_modifier_container.GetChildren().Cast<AbilityButton>())
		{
			modifiers.Add(modifier);
		}
		// AddAbility(roll.name, roll.type_of_ability, roll.icon);
		// AddAbility(basic_attack.name,basic_attack.type_of_ability, basic_attack.icon);
		// foreach(AbilityButton button in abilities)
		// {
		// 	GD.Print(button.ability_name);
		// }
		// SendAbilitiesToPlayer(abilities);
		
	}

    private void HandleButtonName(Button cross_button, string cross_button_name)
    {
        button_to_be_assigned_label.Text = cross_button_name;
		_customSignals.EmitSignal(nameof(CustomSignals.ButtonToBeAssigned), cross_button, button_to_be_assigned);
    }

    private void HandleAvailableAbilities(AbilityResource ability)
    {
        GD.Print("received");
        AddAbility(ability.name, ability.type_of_ability, ability.icon);
    }

    // private void HandleAvailableAbilities(AbilityResource ability)
    // {
    // 	GD.Print("received");
    //     AddAbility(ability.name, ability.type_of_ability, ability.icon);
    // }

    private void HandleButtonClicked(AbilityButton button, Button assign_button)
    {
        button_clicked = button;
		button_to_be_assigned = assign_button;
    }

    private void HandleAddToAbilitySelection(string name, string type, Texture2D icon)
    {
		if(this.IsInGroup(type))
		{
			AddAbility(name,type, icon);
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void AddAbility(string name, string type, Texture2D icon)
	{
		foreach(AbilityButton ability in abilities)
		{
			if(!ability.assigned)
			{
				// ability.Text = name;
				ability.ability_name = name;
				ability.ability_type = type;
				ability.Icon = icon;
				ability.assigned = true;
				break;
			}
		}
	}

	// public void SendAbilitiesToPlayer(List<AbilityButton> abilities)
	// {
	// 	foreach(AbilityButton ability in abilities)
	// 	{
	// 		if(ability.assigned)
	// 		{
	// 			_customSignals.EmitSignal(nameof(CustomSignals.AvailableAbilities), ability.ability_name);
	// 		}
			
	// 	}
		
	// }
	public void _on_accept_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), button_clicked.ability_name, button_to_be_assigned.Name, button_clicked.Icon);
	}

}

