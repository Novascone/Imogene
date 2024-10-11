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
	[Export] public Assignment new_assignment;
	public CrossBindButton cross_bind_selected;
	public bool active;
	public bool passive;

	// [Signal] public delegate void AbilityReassignedEventHandler(string ability_name, string ability_bind, string cross, string level);
	[Signal] public delegate void ClearAbilityBindEventHandler(string ability_name);
	[Signal] public delegate void ClearAbilityIconEventHandler(string ability_name_old, string ability_name_new);
	[Signal] public delegate void AbilityReassignedEventHandler(Ability.Cross cross_, Ability.Tier tier_, string bind, string ability_name, Texture2D icon);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(Button button in class_category.buttons.GetChildren()){button.ButtonDown += HandleCategoryButtonDown;}
		foreach(Button button in general_category.buttons.GetChildren()){button.ButtonDown += HandleCategoryButtonDown;}

		foreach(AbilityButton ability_button in general_category.melee.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in general_category.ranged.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in general_category.defensive.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in general_category.movement.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in general_category.unique.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in general_category.toy.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}


		foreach(AbilityButton ability_button in class_category.basic.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in class_category.kernel.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in class_category.defensive.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in class_category.mastery.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in class_category.movement.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in class_category.specialized.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in class_category.unique.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}
		foreach(AbilityButton ability_button in class_category.toy.ability_button_container_1.GetChildren()){ability_button.ButtonDown += () => OnAbilityButtonDown(ability_button);}

		new_assignment.accept.ButtonDown += OnAssignedAcceptAccept;
		new_assignment.cancel.ButtonDown += OnAssignedAcceptCancel;

	}

    private void OnAssignedAcceptCancel()
    {
        throw new NotImplementedException();
    }

    private void OnAssignedAcceptAccept()
    {
		GD.Print("Ability " + cross_bind_selected.new_ability_name + " is now assigned to " + cross_bind_selected.button_bind + " On " + cross_bind_selected.cross + " " + cross_bind_selected.tier);
		EmitSignal(nameof(ClearAbilityBind), new_assignment.old_ability_name);
		EmitSignal(nameof(ClearAbilityIcon), new_assignment.old_ability_name, new_assignment.new_ability_name);
		EmitSignal(nameof(AbilityReassigned), (int)new_assignment.new_cross, (int)new_assignment.new_tier, new_assignment.new_button_bind, new_assignment.new_ability_name, new_assignment.assigned.Icon);
		
		ResetPage();
		new_assignment.Hide();

    }

  

    private void HandleCategoryButtonDown()
    {
		new_assignment.assigned_label.Text = cross_bind_selected.button_bind + " " + cross_bind_selected.cross + " " + cross_bind_selected.tier;
		new_assignment.assigned.Icon = cross_bind_selected.Icon;
		new_assignment.new_ability_name = cross_bind_selected.ability_name;
		new_assignment.new_button_bind = cross_bind_selected.button_bind;
		new_assignment.new_cross = cross_bind_selected.cross;
		new_assignment.new_tier = cross_bind_selected.tier;
        new_assignment.Show();
    }

    private void OnAbilityButtonDown(AbilityButton ability_button) // When an ability button is pressed, set the icon of the action bar to be assigned to that of the ability button, set the action bar new name to the ability buttons name
    {
        new_assignment.assigned.Icon = ability_button.Icon;
		new_assignment.new_ability_name = ability_button.ability_name;
		// assigned_accepted.old_cross = ability_button.cross;
		// assigned_accepted.old_level = ability_button.level;
		// assigned_accepted.old_button_bind = ability_button.button_bind;
		cross_bind_selected.new_ability_name = ability_button.ability_name;
		
		GD.Print("ability " + new_assignment.new_ability_name + " at bindings " + new_assignment.new_button_bind + " " + new_assignment.new_cross + " " + new_assignment.new_tier);
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
			new_assignment.Hide();
		}
		else if(passive)
		{
			assigned_passives.Show();
			class_passives.Hide();
			class_passives.ResetPage();
			general_passives.Show();
			new_assignment.Hide();
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
			new_assignment.Hide();
		}
		else if(passive)
		{
			assigned_passives.Show();
			general_passives.Hide();
			general_passives.ResetPage();
			class_passives.Show();
			new_assignment.Hide();
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
			if(control == new_assignment)
			{
				new_assignment.Hide();
			}
		}
	}

	
}
 