using Godot;
using System;

public partial class Skills : UI
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

	


	public VBoxContainer skill_binds;
	public VBoxContainer skill_types;
	public PanelContainer melee_skills;
	public PanelContainer ranged_skills;
	public PanelContainer movement_skills;
	public PanelContainer defense_skills;
	public PanelContainer class_skills;
	public PanelContainer Special_skills;
	public PanelContainer passives;
	public VBoxContainer accept_cancel;
	public PanelContainer button_to_be_assigned_container;
	public Button button_to_be_assigned;
	
	public bool is_action_assignment_open;
	public bool is_non_action_assignment_open;

	private PanelContainer assign_skill;
	private Button selected_button;
	private CustomSignals _customSignals; // Instance of CustomSignals
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		skill_binds = GetNode<VBoxContainer>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds");
		skill_types = GetNode<VBoxContainer>("PanelContainer/AbilityContainer/PanelContainer/SkillTypes");
		melee_skills = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/MeleeSkills");
		ranged_skills = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/RangedSkills");
		movement_skills = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/MovementSkills");
		defense_skills = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/DefenseSkills");
		class_skills = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/ClassSkills");
		Special_skills = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/SpecialSkills");
		passives = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/Passives");
		accept_cancel = GetNode<VBoxContainer>("PanelContainer/AbilityContainer/PanelContainer/AcceptChangesContainer");
		button_to_be_assigned_container = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/ButtonToBeAssigned");
		button_to_be_assigned = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/ButtonToBeAssigned/VBoxContainer/ButtonToBeAssigned");

		l_cross_primary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryUpAssign");
		l_cross_primary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryDownAssign");
		l_cross_primary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryLeftAssign");
		l_cross_primary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryRightAssign");

		l_cross_secondary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryUpAssign");
		l_cross_secondary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryDownAssign");
		l_cross_secondary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryLeftAssign");
		l_cross_secondary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossSecondary/LCrossSecondary/LCrossSecondaryRightAssign");

		r_cross_primary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryUpAssign");
		r_cross_primary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryDownAssign");
		r_cross_primary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryLeftAssign");
		r_cross_primary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryRightAssign");

		r_cross_secondary_up_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryUpAssign");
		r_cross_secondary_down_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryDownAssign");
		r_cross_secondary_left_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryLeftAssign");
		r_cross_secondary_right_assign = GetNode<Button>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossSecondary/RCrossSecondary/RCrossSecondaryRightAssign");

		assign_skill = GetNode<PanelContainer>("PanelContainer/AbilityContainer/PanelContainer/SkillBinds/PanelContainer/AssignSkill");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		WhichSkillsUIOpen();
		GoBackSkills();
	}

	public void WhichSkillsUIOpen()
	{
		if(melee_skills.Visible || ranged_skills.Visible || movement_skills.Visible || defense_skills.Visible || class_skills.Visible || Special_skills.Visible)
		{
			is_action_assignment_open = true;
			is_non_action_assignment_open = false;
			_customSignals.EmitSignal(nameof(CustomSignals.SkillsUISecondaryOpen),true);
		}
		else if(passives.Visible || skill_types.Visible)
		{
			is_action_assignment_open = false;
			is_non_action_assignment_open = true;
			_customSignals.EmitSignal(nameof(CustomSignals.SkillsUISecondaryOpen),true);
		}
		else
		{
			_customSignals.EmitSignal(nameof(CustomSignals.SkillsUISecondaryOpen),false);
		}
	}

	public void GoBackSkills()
	{
		if(this.Visible)
		{
			if(is_action_assignment_open)
			{
				if(Input.IsActionJustPressed("B"))
				{
					melee_skills.Hide();
					ranged_skills.Hide();
					movement_skills.Hide();
					defense_skills.Hide();
					class_skills.Hide();
					Special_skills.Hide();
					accept_cancel.Hide();
					button_to_be_assigned_container.Hide();
					skill_types.Show();
				}
			}
			if(skill_types.Visible)
			{
				if(Input.IsActionJustPressed("B"))
				{
					skill_types.Hide();
					button_to_be_assigned_container.Hide();
					skill_binds.Show();
				}
			}
			if(passives.Visible)
			{
				if(Input.IsActionJustPressed("B"))
				{
					passives.Hide();
					skill_binds.Show();
				}
			}
		}
	}


	// Left Cross Primary Assignment Buttons
	public void _on_l_cross_primary_up_assign_button_down()
	{
		selected_button = l_cross_primary_up_assign;
		button_to_be_assigned.Text = "Primary RB";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
		// assign_skill.Show();
	}

	public void _on_l_cross_primary_down_assign_button_down()
	{
		selected_button = l_cross_primary_down_assign;
		button_to_be_assigned.Text = "Primary LT";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_l_cross_primary_left_assign_button_down()
	{
		selected_button = l_cross_primary_left_assign;
		button_to_be_assigned.Text = "Primary LB";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_l_cross_primary_right_assign_button_down()
	{
		selected_button = l_cross_primary_right_assign;
		button_to_be_assigned.Text = "Primary RT";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
		// assign_skill.Show();
	}

	// Left Cross Secondary Assignment Buttons
	public void _on_l_cross_secondary_up_assign_button_down()
	{
		selected_button = l_cross_secondary_up_assign;
		button_to_be_assigned.Text = "Secondary RB";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
		// assign_skill.Show();
	}

	public void _on_l_cross_secondary_down_assign_button_down()
	{
		selected_button = l_cross_secondary_down_assign;
		button_to_be_assigned.Text = "Secondary LT";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_l_cross_secondary_left_assign_button_down()
	{
		selected_button = l_cross_secondary_left_assign;
		button_to_be_assigned.Text = "Secondary LB";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_l_cross_secondary_right_assign_button_down()
	{
		selected_button = l_cross_secondary_right_assign;
		button_to_be_assigned.Text = "Secondary RT";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
		// assign_skill.Show();
	}


	// Right Cross Primary Assignment Buttons
	public void _on_r_cross_primary_up_assign_button_down()
	{
		selected_button = r_cross_primary_up_assign;
		button_to_be_assigned.Text = "Primary Y";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_primary_down_assign_button_down()
	{
		selected_button = r_cross_primary_down_assign;
		button_to_be_assigned.Text = "Primary A";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_primary_left_assign_button_down()
	{
		selected_button = r_cross_primary_left_assign;
		button_to_be_assigned.Text = "Primary X";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_primary_right_assign_button_down()
	{
		selected_button = r_cross_primary_right_assign;
		button_to_be_assigned.Text = "Primary B";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	// Right Cross Secondary Assignment Buttons
	public void _on_r_cross_secondary_up_assign_button_down()
	{
		selected_button = r_cross_secondary_up_assign;
		button_to_be_assigned.Text = "Secondary Y";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_secondary_down_assign_button_down()
	{
		selected_button = r_cross_secondary_down_assign;
		button_to_be_assigned.Text = "Secondary A";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_secondary_left_assign_button_down()
	{
		selected_button = r_cross_secondary_left_assign;
		button_to_be_assigned.Text = "Secondary X";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_r_cross_secondary_right_assign_button_down()
	{
		selected_button = r_cross_secondary_right_assign;
		button_to_be_assigned.Text = "Secondary B";
		skill_binds.Hide();
		skill_types.Show();
		button_to_be_assigned_container.Show();
	}

	public void _on_add_skill_button_down()
	{
		
		AbilityResource ability = ResourceLoader.Load<AbilityResource>("res://resources/roll.tres");
		selected_button.Icon = ability.icon;
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), ability.name, selected_button.Name , ability.icon);
	}
}
