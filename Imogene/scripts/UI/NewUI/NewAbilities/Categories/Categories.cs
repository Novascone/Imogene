using Godot;
using System;
using System.Collections.Generic;

public partial class Categories : Control
{

	[Export] public ClassCategory class_category;
	[Export] public GeneralCategory general_category;
	[Export] public Label ability_type_title;
	[Export] public Control assigned_passives;
	[Export] public Passives class_passives;
	[Export] public Passives general_passives;
	[Export] public Control page_container;
	[Export] public AssignedAccepted assigned_accepted;
	public CrossBindButton button_selected;
	public bool active;
	public bool passive;

	// [Signal] public delegate void AbilityReassignedEventHandler(string ability_name, string ability_bind, string cross, string level);
	[Signal] public delegate void ClearAbilityBindEventHandler(string ability_name);
	[Signal] public delegate void ClearAbilityIconEventHandler(string ability_name_old, string ability_name_new);
	[Signal] public delegate void AbilityReassignedEventHandler(string cross, string level, string bind, string ability_name, Texture2D icon);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(Button button in class_category.buttons.GetChildren()){button.ButtonDown += HandleCategoryButtonDown;}
		foreach(Button button in general_category.buttons.GetChildren()){button.ButtonDown += HandleCategoryButtonDown;}

		foreach(NewAbilityButton ability_button in general_category.melee.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in general_category.ranged.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in general_category.defensive.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in general_category.movement.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in general_category.unique.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in general_category.toy.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}


		foreach(NewAbilityButton ability_button in class_category.basic.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in class_category.kernel.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in class_category.defensive.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in class_category.mastery.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in class_category.movement.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in class_category.specialized.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in class_category.unique.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(NewAbilityButton ability_button in class_category.toy.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}

		assigned_accepted.accept.ButtonDown += OnAssignedAcceptAccept;
		assigned_accepted.cancel.ButtonDown += OnAssignedAcceptCancel;

	}

    private void OnAssignedAcceptCancel()
    {
        throw new NotImplementedException();
    }

    private void OnAssignedAcceptAccept()
    {
		GD.Print("Ability " + button_selected.new_ability_name + " is now assigned to " + button_selected.button_bind + " On " + button_selected.cross + " " + button_selected.level);
		EmitSignal(nameof(ClearAbilityBind), assigned_accepted.old_ability_name);
		EmitSignal(nameof(ClearAbilityIcon), assigned_accepted.old_ability_name, assigned_accepted.new_ability_name);
		EmitSignal(nameof(AbilityReassigned), assigned_accepted.new_cross, assigned_accepted.new_level, assigned_accepted.new_button_bind, assigned_accepted.new_ability_name, assigned_accepted.assigned.Icon);
		ResetPage();
		assigned_accepted.Hide();

    }

    private void OnAbilityButtonDown(NewAbilityButton ability_button)
    {
        assigned_accepted.assigned.Icon = ability_button.Icon;
		assigned_accepted.new_ability_name = ability_button.ability_name;
		assigned_accepted.old_cross = ability_button.cross;
		assigned_accepted.old_level = ability_button.level;
		assigned_accepted.old_button_bind = ability_button.button_bind;
		button_selected.new_ability_name = ability_button.ability_name;
		
		GD.Print("ability " + assigned_accepted.new_ability_name + " at bindings " + assigned_accepted.new_button_bind + " " + assigned_accepted.new_cross + " " + assigned_accepted.new_level);
    }

    private void HandleCategoryButtonDown()
    {
		assigned_accepted.assigned_label.Text = button_selected.button_bind + " " + button_selected.cross + " " + button_selected.level;
		assigned_accepted.assigned.Icon = button_selected.Icon;
		assigned_accepted.new_ability_name = button_selected.ability_name;
		assigned_accepted.new_button_bind = button_selected.button_bind;
		assigned_accepted.new_cross = button_selected.cross;
		assigned_accepted.new_level = button_selected.level;
        assigned_accepted.Show();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
	}

   
    public void _on_general_button_down()
	{
		if(active)
		{
			class_category.Hide();
			class_category.ResetPage();
			general_category.ResetPage();
			general_category.Show();
			assigned_accepted.Hide();
		}
		else if(passive)
		{
			assigned_passives.Show();
			class_passives.Hide();
			class_passives.ResetPage();
			general_passives.Show();
			assigned_accepted.Hide();
		}
		
	}

	public void _on_class_button_down()
	{
		if(active)
		{
			general_category.Hide();
			general_category.ResetPage();
			class_category.ResetPage();
			class_category.Show();
			assigned_accepted.Hide();
		}
		else if(passive)
		{
			assigned_passives.Show();
			general_passives.Hide();
			general_passives.ResetPage();
			class_passives.Show();
			assigned_accepted.Hide();
		}
	}

	public void ResetPage()
	{
		foreach(Control control in page_container.GetChildren())
		{
			if(control is ClassCategory class_category)
			{
				class_category.Hide();
				class_category.ResetPage();
			}
			if(control is GeneralCategory general_category)
			{
				general_category.Hide();
				general_category.ResetPage();
			}
			if(control is Passives passives)
			{
				passives.Hide();
				passives.ResetPage();
			}
			if(control is ActivePassives active_passives)
			{
				active_passives.Hide();
			}
			if(control == assigned_accepted)
			{
				assigned_accepted.Hide();
			}
		}
	}

	
}
 