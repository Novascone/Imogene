using Godot;
using System;

public partial class Categories : Control
{

	[Export] public ClassCategory class_category;
	[Export] public GeneralCategory general_category;
	[Export] public Label ability_type_title;
	[Export] public Control assigned_passives;
	[Export] public Passives class_passives;
	[Export] public Passives general_passives;
	[Export] public Control page_container;
	public bool active;
	public bool passive;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		}
		else if(passive)
		{
			assigned_passives.Show();
			class_passives.Hide();
			class_passives.ResetPage();
			general_passives.Show();
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
		}
		else if(passive)
		{
			assigned_passives.Show();
			general_passives.Hide();
			general_passives.ResetPage();
			class_passives.Show();
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
		}
	}
	
}
 