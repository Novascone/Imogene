using Godot;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

public partial class AbilitiesInterface : Control
{
	// Left Cross Primary
	private GridContainer l_cross_primary;
	private Button l_cross_primary_up_assign;
	private Button l_cross_primary_down_assign;
	private Button l_cross_primary_left_assign;
	private Button l_cross_primary_right_assign;

	// Left Cross Secondary
	private GridContainer l_cross_secondary;
	private Button l_cross_secondary_up_assign;
	private Button l_cross_secondary_down_assign;
	private Button l_cross_secondary_left_assign;
	private Button l_cross_secondary_right_assign;

	// Right Cross Primary
	private GridContainer r_cross_primary;
	private Button r_cross_primary_up_assign;
	private Button r_cross_primary_down_assign;
	private Button r_cross_primary_left_assign;
	private Button r_cross_primary_right_assign;

	// Right Cross Secondary
	private GridContainer r_cross_secondary;
	private Button r_cross_secondary_up_assign;
	private Button r_cross_secondary_down_assign;
	private Button r_cross_secondary_left_assign;
	private Button r_cross_secondary_right_assign;

	


	public PanelContainer ability_binds;
	public PanelContainer ability_types;
	// public AbilityCategory melee_abilities;
	public PanelContainer ranged_abilities;
	public PanelContainer movement_abilities;
	public PanelContainer defense_abilities;
	public PanelContainer class_abilities;
	public PanelContainer Special_abilities;
	public PanelContainer passives;
	public PanelContainer accept_cancel;
	public VBoxContainer close_ability_binds;
	public PanelContainer button_to_be_assigned_container;

	public Button button_to_be_assigned; // The button an ability will be assigned to in the interface
	public string button_to_be_assigned_text; // The input associated with that button
	public Button button_to_be_assigned_icon; // The icon of the ability that is going to be assigned
	

	public UI this_ui; // Reference to the UI
	public bool ability_changed; // Boolean to let the Ability controller know an ability has been changed
	
	public string ability_to_change; // Name of the ability that is going to change
	public string button_to_bind; // New button the ability is bound to

	private PanelContainer current_ui;
	private PanelContainer previous_ui;
	private PanelContainer temp_ui;

	public AbilityCategory current_ability_category;

	private PanelContainer assign_ability;
	public Control categories;
	public CrossAssignment selected_button;
	[Export] public AbilityCategory melee_abilities;
	// private CustomSignals _customSignals; // Instance of CustomSignals
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		ability_binds = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds");
		ability_types = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/Types/AbilityTypes");
		// melee_abilities = GetNode<AbilityCategory>("PanelContainer/AbilityContainer/PanelContainer/Categories/MeleeAbilities");
		// current_ability_category = melee_abilities;
		// ranged_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/RangedAbilities");
		// movement_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/MovementAbilities");
		// defense_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/DefenseAbilities");
		// class_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/ClassAbilities");
		// Special_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/SpecialAbilities");
		passives = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/Passives");
		// accept_cancel = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/AssignedAndAccept/AcceptCancel");
		
		close_ability_binds = GetNode<VBoxContainer>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/Close");
		categories = GetNode<Control>("PanelContainer/AbilityContainer/PanelContainer/Categories");

		foreach(AbilityCategory ability_category in categories.GetChildren().Cast<AbilityCategory>())
		{
			// GD.Print(ability_category.Name);
			ability_category.GetAbilityInterfaceInfo(this);
		}
		
		// button_to_be_assigned = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AssignedAndAccept/ButtonToBeAssigned/VBoxContainer/ButtonToBeAssigned/");
		// button_to_be_assigned_icon = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AssignedAndAccept/ButtonToBeAssigned/VBoxContainer/Button");
		l_cross_primary = GetNode<GridContainer>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary");
		l_cross_primary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryUpAssign");
		l_cross_primary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryDownAssign");
		l_cross_primary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryLeftAssign");
		l_cross_primary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryRightAssign");
		foreach(Control control in l_cross_primary.GetChildren()){if(control is CrossAssignment cross){cross.GetAbilityInterfaceInfo(this);}}
		

		l_cross_secondary = GetNode<GridContainer>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary");
		l_cross_secondary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryUpAssign");
		l_cross_secondary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryDownAssign");
		l_cross_secondary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryLeftAssign");
		l_cross_secondary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryRightAssign");
		foreach(Control control in l_cross_secondary.GetChildren()){if(control is CrossAssignment cross){cross.GetAbilityInterfaceInfo(this);}}

		r_cross_primary = GetNode<GridContainer>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary");
		r_cross_primary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryUpAssign");
		r_cross_primary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryDownAssign");
		r_cross_primary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryLeftAssign");
		r_cross_primary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryRightAssign");
		foreach(Control control in r_cross_primary.GetChildren()){if(control is CrossAssignment cross){cross.GetAbilityInterfaceInfo(this);}}

		r_cross_secondary = GetNode<GridContainer>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary");
		r_cross_secondary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryUpAssign");
		r_cross_secondary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryDownAssign");
		r_cross_secondary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryLeftAssign");
		r_cross_secondary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/Binds/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryRightAssign");
		foreach(Control control in r_cross_secondary.GetChildren()){if(control is CrossAssignment cross){cross.GetAbilityInterfaceInfo(this);}}

		assign_ability = GetNode<PanelContainer>("AssignAbility");

		// _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.AbilityAccept += HandleAbilityAccept;
		// _customSignals.AbilityCancel += HandleAbilityCancel;
		// _customSignals.AbilityAssigned += HandleAbilityAssigned;
		// _customSignals.AbilityRemoved += HandleAbilityRemoved;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
		}
		
	}

	// Set current and previous UIs
	public void AbilityAccept()
    {
        current_ui = ability_binds;
		previous_ui = melee_abilities;
		NavigateAbilities();
    }

	public void AbilityCancel()
	{
		current_ui = ability_binds;
		previous_ui = melee_abilities;
		NavigateAbilities();
	}

	public void GetUIInfo(UI i) // Get UI info
	{
		this_ui = i;
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		NavigateAbilities();
	}


	public void NavigateAbilities() // Enables navigation through abilities, keeps track of current and previous UI
	{
		
		if(current_ui == null)
		{
			current_ui = ability_binds;
			
		}
		if(current_ui.IsInGroup("assignment"))
		{
			// _customSignals.EmitSignal(nameof(CustomSignals.AbilityUISecondaryOpen),true);
			this_ui.abilities_secondary_ui_open = true;
			// button_to_be_assigned.Show();
			// accept_cancel.Show();
		}
		else if(current_ui.IsInGroup("selection"))
		{
			// _customSignals.EmitSignal(nameof(CustomSignals.AbilityUISecondaryOpen),true);
			this_ui.abilities_secondary_ui_open = true;
			// button_to_be_assigned.Hide();
			
		}
		else
		{
			// _customSignals.EmitSignal(nameof(CustomSignals.AbilityUISecondaryOpen),false);
			this_ui.abilities_secondary_ui_open = false;
		}
		
		if(Input.IsActionJustPressed("B"))
		{
			if(current_ui.IsInGroup("selection"))
			{
				previous_ui = ability_binds;
			}
			if(current_ui == ability_binds)
			{
				Hide();
			}
			temp_ui = current_ui;
			current_ui = previous_ui;
			previous_ui = temp_ui;
			if(previous_ui != null){previous_ui.Hide();}
			if(current_ui != null){current_ui.Show();}
		}
		if(previous_ui != null && current_ui != null)
		{
			previous_ui.Hide();
			current_ui.Show();
		}
		
	}

	public void AssignmentButtonNavigation()
	{
		previous_ui = ability_binds;
		current_ui = ability_types;
		previous_ui.Hide();
		current_ui.Show();
	}

	public void _on_melee_abilities_button_down()
	{
		previous_ui = ability_types;
		current_ui = melee_abilities;
		current_ability_category = melee_abilities;
		current_ability_category.ButtonName(selected_button, button_to_be_assigned_text);

		GD.Print("Assigning button name");
		GD.Print("button text " + button_to_be_assigned_text);
		current_ability_category.CurrentAbilityBoundOnCrossButton(selected_button.Icon);
		
		previous_ui.Hide();
		current_ui.Show();
		// accept_cancel.Show();
	}

	public void _on_close_button_down()
	{
		GD.Print("close");
		Hide();
		// _customSignals.EmitSignal(nameof(CustomSignals.AbilityUISecondaryOpen),false);
		this_ui.abilities_secondary_ui_open = false;
		this_ui.CloseInterface();
		// _customSignals.EmitSignal(nameof(CustomSignals.HideCursor));
		// _customSignals.EmitSignal(nameof(CustomSignals.UIPreventingMovement),false);
	}

	
	public void _on_add_skill_button_down()
	{
		AbilityResource ability = ResourceLoader.Load<AbilityResource>("res://resources/roll.tres");
		// selected_button.Icon = ability.icon;
		// _customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), ability.name, selected_button.Name , ability.icon);
		// _customSignals.EmitSignal(nameof(CustomSignals.AddToAbilitySelection), ability.name, ability.type, ability.icon);
	}

	

	public void AbilityAssigned(AbilityResource ability_resource, string button_name)
	{
		this_ui.hud.AbilityAssigned(ability_resource, button_name);
		if(button_name == "LCrossPrimaryUpAssign"){l_cross_primary_up_assign.Icon  = ability_resource.icon;}
		if(button_name == "LCrossPrimaryRightAssign"){l_cross_primary_right_assign.Icon  = ability_resource.icon;}
		if(button_name == "LCrossPrimaryLeftAssign"){l_cross_primary_left_assign.Icon  = ability_resource.icon;}
		if(button_name == "LCrossPrimaryDownAssign"){l_cross_primary_down_assign.Icon  = ability_resource.icon;}
		
		if(button_name == "LCrossSecondaryUpAssign"){l_cross_secondary_up_assign.Icon  = ability_resource.icon;}
		if(button_name == "LCrossSecondaryRightAssign"){l_cross_secondary_right_assign.Icon  = ability_resource.icon;}
		if(button_name == "LCrossSecondaryLeftAssign"){l_cross_secondary_left_assign.Icon  = ability_resource.icon;}
		if(button_name == "LCrossSecondaryDownAssign"){l_cross_secondary_down_assign.Icon  = ability_resource.icon;}

		if(button_name == "RCrossPrimaryUpAssign"){r_cross_primary_up_assign.Icon  = ability_resource.icon;}
		if(button_name == "RCrossPrimaryRightAssign"){r_cross_primary_right_assign.Icon  = ability_resource.icon;}
		if(button_name == "RCrossPrimaryLeftAssign"){r_cross_primary_left_assign.Icon  = ability_resource.icon;}
		if(button_name == "RCrossPrimaryDownAssign"){r_cross_primary_down_assign.Icon  = ability_resource.icon;}

		if(button_name == "RCrossSecondaryUpAssign"){r_cross_secondary_up_assign.Icon  = ability_resource.icon;}
		if(button_name == "RCrossSecondaryRightAssign"){r_cross_secondary_right_assign.Icon  = ability_resource.icon;}
		if(button_name == "RCrossSecondaryLeftAssign"){r_cross_secondary_left_assign.Icon  = ability_resource.icon;}
		if(button_name == "RCrossSecondaryDownAssign"){r_cross_secondary_down_assign.Icon  = ability_resource.icon;}
		
		foreach(AbilityCategory ability_category in categories.GetChildren())
		{
			ability_category.AbilityAssigned(ability_resource, button_name);
		}
		
	}

	public void AbilityRemoved(string button_name)
	{
		this_ui.hud.AbilityRemoved(button_name);
		GD.Print("removing ability from " + button_name);
		if(button_name == "LCrossPrimaryUpAssign"){l_cross_primary_up_assign.Icon  = null;}
		if(button_name == "LCrossPrimaryRightAssign"){l_cross_primary_right_assign.Icon  = null;}
		if(button_name == "LCrossPrimaryLeftAssign"){l_cross_primary_left_assign.Icon  = null;}
		if(button_name == "LCrossPrimaryDownAssign"){l_cross_primary_down_assign.Icon  = null;}
		
		if(button_name == "LCrossSecondaryUpAssign"){l_cross_secondary_up_assign.Icon  = null;}
		if(button_name == "LCrossSecondaryRightAssign"){l_cross_secondary_right_assign.Icon  = null;}
		if(button_name == "LCrossSecondaryLeftAssign"){l_cross_secondary_left_assign.Icon  = null;}
		if(button_name == "LCrossSecondaryDownAssign"){l_cross_secondary_down_assign.Icon  = null;}

		if(button_name == "RCrossPrimaryUpAssign"){r_cross_primary_up_assign.Icon  = null;}
		if(button_name == "RCrossPrimaryRightAssign"){r_cross_primary_right_assign.Icon  = null;}
		if(button_name == "RCrossPrimaryLeftAssign"){r_cross_primary_left_assign.Icon  = null;}
		if(button_name == "RCrossPrimaryDownAssign"){r_cross_primary_down_assign.Icon  = null;}

		if(button_name == "RCrossSecondaryUpAssign"){r_cross_secondary_up_assign.Icon  = null;}
		if(button_name == "RCrossSecondaryRightAssign"){r_cross_secondary_right_assign.Icon  = null;}
		if(button_name == "RCrossSecondaryLeftAssign"){r_cross_secondary_left_assign.Icon  = null;}
		if(button_name == "RCrossSecondaryDownAssign"){r_cross_secondary_down_assign.Icon  = null;}
		
	}

	// private void HandleAbilityAccept()
    // {
    //     current_ui = ability_binds;
	// 	previous_ui = melee_abilities;
	// 	NavigateAbilities();
    // }

	// private void HandleAbilityCancel()
    // {
    //     current_ui = ability_binds;
	// 	previous_ui = melee_abilities;
	// 	NavigateAbilities();
    // }

	// public void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
	// {
	// 	if(button_name == "LCrossPrimaryUpAssign"){l_cross_primary_up_assign.Icon  = icon;}
	// 	if(button_name == "LCrossPrimaryRightAssign"){l_cross_primary_right_assign.Icon  = icon;}
	// 	if(button_name == "LCrossPrimaryLeftAssign"){l_cross_primary_left_assign.Icon  = icon;}
	// 	if(button_name == "LCrossPrimaryDownAssign"){l_cross_primary_down_assign.Icon  = icon;}
		
	// 	if(button_name == "LCrossSecondaryUpAssign"){l_cross_secondary_up_assign.Icon  = icon;}
	// 	if(button_name == "LCrossSecondaryRightAssign"){l_cross_secondary_right_assign.Icon  = icon;}
	// 	if(button_name == "LCrossSecondaryLeftAssign"){l_cross_secondary_left_assign.Icon  = icon;}
	// 	if(button_name == "LCrossSecondaryDownAssign"){l_cross_secondary_down_assign.Icon  = icon;}

	// 	if(button_name == "RCrossPrimaryUpAssign"){r_cross_primary_up_assign.Icon  = icon;}
	// 	if(button_name == "RCrossPrimaryRightAssign"){r_cross_primary_right_assign.Icon  = icon;}
	// 	if(button_name == "RCrossPrimaryLeftAssign"){r_cross_primary_left_assign.Icon  = icon;}
	// 	if(button_name == "RCrossPrimaryDownAssign"){r_cross_primary_down_assign.Icon  = icon;}

	// 	if(button_name == "RCrossSecondaryUpAssign"){r_cross_secondary_up_assign.Icon  = icon;}
	// 	if(button_name == "RCrossSecondaryRightAssign"){r_cross_secondary_right_assign.Icon  = icon;}
	// 	if(button_name == "RCrossSecondaryLeftAssign"){r_cross_secondary_left_assign.Icon  = icon;}
	// 	if(button_name == "RCrossSecondaryDownAssign"){r_cross_secondary_down_assign.Icon  = icon;}
	// }
	
	// public void HandleAbilityRemoved(string ability, string button_name)
	// {
	// 	if(button_name == "LCrossPrimaryUpAssign"){l_cross_primary_up_assign.Icon  = null;}
	// 	if(button_name == "LCrossPrimaryRightAssign"){l_cross_primary_right_assign.Icon  = null;}
	// 	if(button_name == "LCrossPrimaryLeftAssign"){l_cross_primary_left_assign.Icon  = null;}
	// 	if(button_name == "LCrossPrimaryDownAssign"){l_cross_primary_down_assign.Icon  = null;}
		
	// 	if(button_name == "LCrossSecondaryUpAssign"){l_cross_secondary_up_assign.Icon  = null;}
	// 	if(button_name == "LCrossSecondaryRightAssign"){l_cross_secondary_right_assign.Icon  = null;}
	// 	if(button_name == "LCrossSecondaryLeftAssign"){l_cross_secondary_left_assign.Icon  = null;}
	// 	if(button_name == "LCrossSecondaryDownAssign"){l_cross_secondary_down_assign.Icon  = null;}

	// 	if(button_name == "RCrossPrimaryUpAssign"){r_cross_primary_up_assign.Icon  = null;}
	// 	if(button_name == "RCrossPrimaryRightAssign"){r_cross_primary_right_assign.Icon  = null;}
	// 	if(button_name == "RCrossPrimaryLeftAssign"){r_cross_primary_left_assign.Icon  = null;}
	// 	if(button_name == "RCrossPrimaryDownAssign"){r_cross_primary_down_assign.Icon  = null;}

	// 	if(button_name == "RCrossSecondaryUpAssign"){r_cross_secondary_up_assign.Icon  = null;}
	// 	if(button_name == "RCrossSecondaryRightAssign"){r_cross_secondary_right_assign.Icon  = null;}
	// 	if(button_name == "RCrossSecondaryLeftAssign"){r_cross_secondary_left_assign.Icon  = null;}
	// 	if(button_name == "RCrossSecondaryDownAssign"){r_cross_secondary_down_assign.Icon  = null;}
	// }
}
