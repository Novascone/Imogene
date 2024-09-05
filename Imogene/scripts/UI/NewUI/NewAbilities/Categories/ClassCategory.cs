using Godot;
using System;

public partial class ClassCategory : Control
{
	[Export] public AbilityPage basic;
	[Export] public AbilityPage kernel;
	[Export] public AbilityPage defensive;
	[Export] public AbilityPage mastery;
	[Export] public AbilityPage movement;
	[Export] public AbilityPage specialized;
	[Export] public AbilityPage unique;
	[Export] public AbilityPage toy;
	[Export] public Control buttons_container;
	[Export] public Control buttons;
	
	
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_basic_button_down()
	{
		basic.Show();
		buttons_container.Hide();
	}
	
	public void _on_kernel_button_down()
	{
		kernel.Show();
		buttons_container.Hide();
	}

	public void _on_defensive_button_down()
	{
		defensive.Show();
		buttons_container.Hide();
	}

	public void _on_mastery_button_down()
	{
		mastery.Show();
		buttons_container.Hide();
	}

	public void _on_movement_button_down()
	{
		movement.Show();
		buttons_container.Hide();
	}

	public void _on_specialized_button_down()
	{
		specialized.Show();
		buttons_container.Hide();
	}

	public void _on_unique_button_down()
	{
		unique.Show();
		buttons_container.Hide();
	}

	public void _on_toy_button_down()
	{
		toy.Show();
		buttons_container.Hide();
	}

	public void ResetPage()
	{
		GD.Print("reset class categories");
		foreach(Control control in GetChildren())
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
	}
	
}
