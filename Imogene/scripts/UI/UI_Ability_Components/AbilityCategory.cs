using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AbilityCategory : PanelContainer
{
	private GridContainer ability_container;
	private HBoxContainer ability_modifier_container;
	private AbilityButton button_clicked;
	private Button button_to_be_assigned;
	private List<AbilityButton> abilities = new List<AbilityButton>();
	private List<Button> modifiers = new List<Button>();
	private CustomSignals _customSignals;
	AbilityResource roll = ResourceLoader.Load<AbilityResource>("res://resources/roll.tres");
	AbilityResource basic_attack = ResourceLoader.Load<AbilityResource>("res://resources/basic_attack.tres");
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ability_container = GetNode<GridContainer>("VBoxContainer/PanelContainer/GridContainer");
		ability_modifier_container = GetNode<HBoxContainer>("VBoxContainer/PanelContainer/ModifierTreeContainer/Modifier/VBoxContainer/AbilityModifiers");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");

		_customSignals.AddToAbilitySelection += HandleAddToAbilitySelection;
		_customSignals.SendButtonClicked += HandleButtonClicked;

		foreach(AbilityButton ability in ability_container.GetChildren().Cast<AbilityButton>())
		{
			abilities.Add(ability);
		}
		// GD.Print(abilities.Count);

		foreach(AbilityButton modifier in ability_modifier_container.GetChildren().Cast<AbilityButton>())
		{
			modifiers.Add(modifier);
		}
		AddAbility(roll.name, roll.type_of_ability, roll.icon);
		AddAbility(basic_attack.name,basic_attack.type_of_ability, basic_attack.icon);
		foreach(AbilityButton button in abilities)
		{
			GD.Print(button.ability_name);
		}
		SendAbilitiesToPlayer(abilities);
		
	}

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

	public void SendAbilitiesToPlayer(List<AbilityButton> abilities)
	{
		foreach(AbilityButton ability in abilities)
		{
			if(ability.assigned)
			{
				_customSignals.EmitSignal(nameof(CustomSignals.AvailableAbilities), ability.ability_name);
			}
			
		}
		
	}
	public void _on_accept_button_down()
	{
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), button_clicked.ability_name, button_to_be_assigned.Name, button_clicked.Icon);
	}

}
