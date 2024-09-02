using Godot;
using System;

public partial class Categories : Control
{

	[Export] public ClassCategory class_category;
	[Export] public GeneralCategory general_category;
	[Export] public Label ability_type_title;
	[Export] public Control passives;
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
		class_category.Hide();
		foreach(Control control in class_category.GetChildren())
		{
			if(control is AbilityPage ability_page)
			{
				ability_page.Hide();
			}
			else
			{
				control.Show();
			}
		}
		general_category.Show();
	}

	public void _on_class_button_down()
	{
		general_category.Hide();
		foreach(Control control in general_category.GetChildren())
		{
			if(control is AbilityPage ability_page)
			{
				ability_page.Hide();
			}
			else
			{
				control.Show();
			}
		}
		class_category.Show();
	}
	
}
 