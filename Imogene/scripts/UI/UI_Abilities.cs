using Godot;
using System;

public partial class UI_Abilities : UI
{
	// Left Cross Primary
	private Button l_cross_primary_up_assign;
	private Button l_cross_primary_down_assign;
	private Button l_cross_primary_left_assign;
	private Button l_cross_primary_right_assign;

	// Left Cross Secondary
	private Button l_cross_secondary_up_assign;
	private Button l_cross_secondary_down_assign;
	private Button l_cross_secondary_left_assign;
	private Button l_cross_secondary_right_assign;

	// Right Cross Primary
	private Button r_cross_primary_up_assign;
	private Button r_cross_primary_down_assign;
	private Button r_cross_primary_left_assign;
	private Button r_cross_primary_right_assign;

	// Right Cross Secondary
	private Button r_cross_secondary_up_assign;
	private Button r_cross_secondary_down_assign;
	private Button r_cross_secondary_left_assign;
	private Button r_cross_secondary_right_assign;

	


	public PanelContainer ability_binds;
	public PanelContainer ability_types;
	public PanelContainer melee_abilities;
	public PanelContainer ranged_abilities;
	public PanelContainer movement_abilities;
	public PanelContainer defense_abilities;
	public PanelContainer class_abilities;
	public PanelContainer Special_abilities;
	public PanelContainer passives;
	public PanelContainer accept_cancel;
	public PanelContainer button_to_be_assigned_container;
	public Button button_to_be_assigned;
	public Button button_to_be_assigned_icon;
	
	public bool is_action_assignment_open;
	public bool is_non_action_assignment_open;

	private PanelContainer current_ui;
	private PanelContainer previous_ui;
	private PanelContainer temp_ui;

	private PanelContainer assign_ability;
	private Button selected_button;
	private CustomSignals _customSignals; // Instance of CustomSignals
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		ability_binds = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds");
		ability_types = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/AbilityTypes");
		melee_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/MeleeAbilities");
		// ranged_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/RangedAbilities");
		// movement_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/MovementAbilities");
		// defense_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/DefenseAbilities");
		// class_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/ClassAbilities");
		// Special_abilities = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/SpecialAbilities");
		passives = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/Passives");
		accept_cancel = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/AssignedAndAccept/AcceptCancel");
		button_to_be_assigned_container = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/AssignedAndAccept/ButtonToBeAssigned");
		button_to_be_assigned = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AssignedAndAccept/ButtonToBeAssigned/VBoxContainer/ButtonToBeAssigned/");
		button_to_be_assigned_icon = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AssignedAndAccept/ButtonToBeAssigned/VBoxContainer/Button");

		l_cross_primary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryUpAssign");
		l_cross_primary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryDownAssign");
		l_cross_primary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryLeftAssign");
		l_cross_primary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryRightAssign");

		l_cross_secondary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryUpAssign");
		l_cross_secondary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryDownAssign");
		l_cross_secondary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryLeftAssign");
		l_cross_secondary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryRightAssign");

		r_cross_primary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryUpAssign");
		r_cross_primary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryDownAssign");
		r_cross_primary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryLeftAssign");
		r_cross_primary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryRightAssign");

		r_cross_secondary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryUpAssign");
		r_cross_secondary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryDownAssign");
		r_cross_secondary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryLeftAssign");
		r_cross_secondary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/AbilityBinds/AbilityBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryRightAssign");

		assign_ability = GetNode<PanelContainer>("AssignAbility");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		WhichAbilityUIOpen();
		// GoBackAbilities();
		NavigateAbilities();
	}

	public void WhichAbilityUIOpen()
	{
		if(melee_abilities.Visible) // || ranged_abilities.Visible || movement_abilities.Visible || defense_abilities.Visible || class_abilities.Visible || Special_abilities.Visible) // change this with groups
		{
			is_action_assignment_open = true;
			is_non_action_assignment_open = false;
			_customSignals.EmitSignal(nameof(CustomSignals.AbilityUISecondaryOpen),true);
		}
		else if(passives.Visible || ability_types.Visible)
		{
			is_action_assignment_open = false;
			is_non_action_assignment_open = true;
			_customSignals.EmitSignal(nameof(CustomSignals.AbilityUISecondaryOpen),true);
		}
		else
		{
			_customSignals.EmitSignal(nameof(CustomSignals.AbilityUISecondaryOpen),false);
		}
	}

	public void GoBackAbilities()
	{
		if(this.Visible)
		{
			if(is_action_assignment_open)
			{
				if(Input.IsActionJustPressed("B"))
				{
					melee_abilities.Hide();
					// ranged_abilities.Hide();
					// movement_abilities.Hide();
					// defense_abilities.Hide();
					// class_abilities.Hide();
					// Special_abilities.Hide();
					accept_cancel.Hide();
					button_to_be_assigned_container.Hide();
					ability_types.Show();
				}
			}
			if(ability_types.Visible)
			{
				if(Input.IsActionJustPressed("B"))
				{
					ability_types.Hide();
					button_to_be_assigned_container.Hide();
					ability_binds.Show();
				}
			}
			if(melee_abilities.Visible)
			{
				if(Input.IsActionJustPressed("B"))
				{
					melee_abilities.Hide();
					button_to_be_assigned_container.Hide();
					ability_binds.Show();
				}
			}
			if(passives.Visible)
			{
				if(Input.IsActionJustPressed("B"))
				{
					passives.Hide();
					ability_binds.Show();
				}
			}
		}
	}

	public void NavigateAbilities()
	{
		
		if(current_ui == null)
		{
			current_ui = ability_binds;
		}
		if(current_ui.IsInGroup("assignment"))
		{
			
			accept_cancel.Show();
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
			previous_ui.Hide();
			current_ui.Show();
			
			
		}
	}


	// Left Cross Primary Assignment Buttons
	public void _on_l_cross_primary_up_assign_button_down()
	{
		selected_button = l_cross_primary_up_assign;
		button_to_be_assigned.Text = "Primary RB";
		previous_ui = ability_binds;
		current_ui = ability_types;
		previous_ui.Hide();
		current_ui.Show();
		button_to_be_assigned_container.Show();
		_customSignals.EmitSignal(nameof(CustomSignals.ButtonToBeAssigned), selected_button, button_to_be_assigned_icon);
		// assign_ability.Show();
	}

	public void _on_l_cross_primary_down_assign_button_down()
	{
		selected_button = l_cross_primary_down_assign;
		button_to_be_assigned.Text = "Primary LT";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_l_cross_primary_left_assign_button_down()
	{
		selected_button = l_cross_primary_left_assign;
		button_to_be_assigned.Text = "Primary LB";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_l_cross_primary_right_assign_button_down()
	{
		selected_button = l_cross_primary_right_assign;
		button_to_be_assigned.Text = "Primary RT";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
		// assign_ability.Show();
	}

	// Left Cross Secondary Assignment Buttons
	public void _on_l_cross_secondary_up_assign_button_down()
	{
		selected_button = l_cross_secondary_up_assign;
		button_to_be_assigned.Text = "Secondary RB";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
		// assign_ability.Show();
	}

	public void _on_l_cross_secondary_down_assign_button_down()
	{
		selected_button = l_cross_secondary_down_assign;
		button_to_be_assigned.Text = "Secondary LT";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_l_cross_secondary_left_assign_button_down()
	{
		selected_button = l_cross_secondary_left_assign;
		button_to_be_assigned.Text = "Secondary LB";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_l_cross_secondary_right_assign_button_down()
	{
		selected_button = l_cross_secondary_right_assign;
		button_to_be_assigned.Text = "Secondary RT";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
		// assign_ability.Show();
	}


	// Right Cross Primary Assignment Buttons
	public void _on_r_cross_primary_up_assign_button_down()
	{
		selected_button = r_cross_primary_up_assign;
		button_to_be_assigned.Text = "Primary Y";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_primary_down_assign_button_down()
	{
		selected_button = r_cross_primary_down_assign;
		button_to_be_assigned.Text = "Primary A";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_primary_left_assign_button_down()
	{
		selected_button = r_cross_primary_left_assign;
		button_to_be_assigned.Text = "Primary X";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_primary_right_assign_button_down()
	{
		selected_button = r_cross_primary_right_assign;
		button_to_be_assigned.Text = "Primary B";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	// Right Cross Secondary Assignment Buttons
	public void _on_r_cross_secondary_up_assign_button_down()
	{
		selected_button = r_cross_secondary_up_assign;
		button_to_be_assigned.Text = "Secondary Y";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_secondary_down_assign_button_down()
	{
		selected_button = r_cross_secondary_down_assign;
		button_to_be_assigned.Text = "Secondary A";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_secondary_left_assign_button_down()
	{
		selected_button = r_cross_secondary_left_assign;
		button_to_be_assigned.Text = "Secondary X";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_secondary_right_assign_button_down()
	{
		selected_button = r_cross_secondary_right_assign;
		button_to_be_assigned.Text = "Secondary B";
		ability_binds.Hide();
		ability_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_melee_abilites_button_down()
	{
		previous_ui = ability_types;
		current_ui = melee_abilities;
		previous_ui.Hide();
		current_ui.Show();
		accept_cancel.Show();
	}

	
	public void _on_add_skill_button_down()
	{
		AbilityResource ability = ResourceLoader.Load<AbilityResource>("res://resources/roll.tres");
		// selected_button.Icon = ability.icon;
		// _customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), ability.name, selected_button.Name , ability.icon);
		_customSignals.EmitSignal(nameof(CustomSignals.AddToAbilitySelection), ability.name, ability.type_of_ability, ability.icon);
	}
}
