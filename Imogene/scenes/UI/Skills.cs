using Godot;
using System;

public partial class Skills : UI
{
	private Button l_cross_primary_up_assign;
	private PanelContainer assign_skill;

	private CustomSignals _customSignals; // Instance of CustomSignals
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");

		l_cross_primary_up_assign = GetNode<Button>("PanelContainer/VBoxContainer/AbilityContainer/SkillBinds/HBoxContainer/GridContainer/VBoxLeftCrossPrimary/LCrossPrimary/LCrossPrimaryUpAssign");
		assign_skill = GetNode<PanelContainer>("PanelContainer/VBoxContainer/AbilityContainer/SkillBinds/AssignSkill");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_l_cross_primary_up_button_down()
	{
		assign_skill.Show();
	}

	public void _on_add_skill_button_down()
	{
		
		AbilityResource ability = ResourceLoader.Load<AbilityResource>("res://resources/roll.tres");
		l_cross_primary_up_assign.Icon = ability.icon;
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), ability.name, "l_cross_primary_up" , ability.icon);
	}

	// public void AddAbility(AbilityResource ability)
	// {

	// }
}
