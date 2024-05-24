using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

public partial class AbilityCategory : PanelContainer
{
	private GridContainer ability_container_page_1; // First page of abilities
	private GridContainer ability_container_page_2; // Second page of abilities
	private HBoxContainer ability_modifier_container; // Modifiers for abilities
	private AbilityButton button_clicked; // The button from the list of abilities that was clicked
	private VBoxContainer button_to_be_assigned_container; // The ability that is going to be/is assigned, and the accept/ cancel buttons
	private Button current_button;
	private Button button_to_be_assigned_label; 
	private Button button_to_be_assigned;
	private Button action_button_to_be_assigned;
	private List<AbilityButton> abilities = new List<AbilityButton>(); // List of ability buttons
	private List<Button> modifiers = new List<Button>(); // List of modifiers
	private CustomSignals _customSignals;
	
	
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
		_customSignals.AvailableAbilities += HandleAvailableAbilities;
		_customSignals.AbilitySelected += HandleAbilitySelected;
		_customSignals.ButtonName += HandleButtonName;
		_customSignals.AbilityAccept += HandleAbilityAccept;
		_customSignals.AbilityCancel += HandleAbilityCancel;
		_customSignals.CurrentAbilityBoundOnCrossButton += HandleCurrentAbilityBoundOnCrossButton;
		_customSignals.AbilityAssigned += HandleAbilityAssigned;
		

		foreach(AbilityButton ability in ability_container_page_1.GetChildren().Cast<AbilityButton>())
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
		
		// GD.Print(abilities.Count);

		foreach(AbilityButton modifier in ability_modifier_container.GetChildren().Cast<AbilityButton>())
		{
			modifiers.Add(modifier);
		}
		
	}

    private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    {
       foreach(AbilityButton abilityButton in abilities)
			{
				if(abilityButton.assigned && abilityButton.ability_name == ability)
				{
					// ability.Text = name;
					abilityButton.button_assigned = button_name;
					GD.Print(abilityButton.Name);
					GD.Print(abilityButton.button_assigned);
					break;
				}
			}
    }

    private void HandleAbilityCancel()
	{
		button_to_be_assigned.Icon = null;
	}
	private void HandleAbilityAccept()
	{
		button_to_be_assigned.Icon = null;
		current_button = button_clicked;
	}

	private void HandleCurrentAbilityBoundOnCrossButton(Texture2D icon)
    {
        button_to_be_assigned.Icon = icon;
    }

    private void HandleButtonName(Button cross_button, string cross_button_name)
    {
        button_to_be_assigned_label.Text = cross_button_name;
		_customSignals.EmitSignal(nameof(CustomSignals.ButtonToBeAssigned), cross_button, button_to_be_assigned);
    }

    private void HandleAvailableAbilities(AbilityResource ability)
    {
        AddAbility(ability.name, ability.type, ability.description, ability.icon);
    }


  

    private void HandleAddToAbilitySelection(string name, string type, string description, Texture2D icon)
    {
		if(this.IsInGroup(type))
		{
			GD.Print("HandleAddToAbilitySelection");
			AddAbility(name, type, description, icon);
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void AddAbility(string name, string type, string description, Texture2D icon)
	{
		if(this.IsInGroup(type))
		{
			foreach(AbilityButton ability in abilities)
			{
				if(!ability.assigned)
				{
					// ability.Text = name;
					ability.ability_name = name;
					ability.ability_type = type;
					ability.info_text.Text = description;
					ability.Icon = icon;
					ability.assigned = true;
					break;
				}
			}
		}
		
	}

	  private void HandleAbilitySelected(AbilityButton button, Button assign_button)
    {
        button_clicked = button;
		action_button_to_be_assigned = assign_button;
		current_button = assign_button;
		if(button_clicked.button_assigned == null)
		{
			button_clicked.button_assigned = assign_button.Name;
		}
		
		// GD.Print(button_clicked.button_assigned);
    }
	public void _on_accept_button_down()
	{	
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityRemoved),button_clicked.ability_name, button_clicked.button_assigned);
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityAccept));
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), button_clicked.ability_name, action_button_to_be_assigned.Name, button_clicked.Icon);
		
	}

	public void _on_cancel_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityCancel));
	}

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

}

