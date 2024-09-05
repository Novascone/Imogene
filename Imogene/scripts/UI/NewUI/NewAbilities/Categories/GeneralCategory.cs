using Godot;
using System;

public partial class GeneralCategory : Control
{
	[Export] public AbilityPage melee;
	[Export] public AbilityPage ranged;
	[Export] public AbilityPage defensive;
	[Export] public AbilityPage movement;
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

	public void _on_melee_button_down()
	{
		melee.Show();
		buttons_container.Hide();
	}

	public void _on_ranged_button_down()
	{
		ranged.Show();
		buttons_container.Hide();
	}

	public void _on_defensive_button_down()
	{
		defensive.Show();
		buttons_container.Hide();
	}

	public void _on_movement_button_down()
	{
		movement.Show();
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
		GD.Print("reset general categories");
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
