using Godot;
using System;

public partial class Skills : UI
{
	private Button l_cross_primary_up_assign;
	private Button r_cross_primary_right_assign;
	private PanelContainer assign_skill;
	private Button selected_button;
	private CustomSignals _customSignals; // Instance of CustomSignals
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		

		l_cross_primary_up_assign = GetNode<Button>("PanelContainer/VBoxContainer/AbilityContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryUpAssign");

		r_cross_primary_right_assign = GetNode<Button>("PanelContainer/VBoxContainer/AbilityContainer/SkillBinds/PanelContainer/HBoxContainer/GridContainer/VBoxRightCrossPrimary/RCrossPrimary/RCrossPrimaryRightAssign");
		assign_skill = GetNode<PanelContainer>("PanelContainer/VBoxContainer/AbilityContainer/SkillBinds/PanelContainer/AssignSkill");

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_l_cross_primary_up_button_down()
	{
		selected_button = l_cross_primary_up_assign;
		assign_skill.Show();
	}

	public void _on_r_cross_primary_right_assign_button_down()
	{
		selected_button = r_cross_primary_right_assign;
		assign_skill.Show();
	}

	public void _on_add_skill_button_down()
	{
		
		AbilityResource ability = ResourceLoader.Load<AbilityResource>("res://resources/roll.tres");
		selected_button.Icon = ability.icon;
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), ability.name, selected_button.Name , ability.icon);
	}
}
