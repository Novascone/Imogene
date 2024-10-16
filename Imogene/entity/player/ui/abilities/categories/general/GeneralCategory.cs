using Godot;
using System;
using System.Linq;

public partial class GeneralCategory : Control
{
	[Export] public AbilityPage melee { get; set; }
	[Export] public AbilityPage ranged { get; set; }
	[Export] public AbilityPage defensive { get; set; }
	[Export] public AbilityPage movement { get; set; }
	[Export] public AbilityPage unique { get; set; }
	[Export] public AbilityPage toy { get; set; }
	[Export] public Control buttons_container { get; set; }
	[Export] public Control buttons { get; set; }

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
		foreach(Control _control in GetChildren().Cast<Control>())
		{
			if(_control is AbilityPage ability_page)
			{
				ability_page.Hide();
			}
			else
			{
				_control.Show();
			}
		}
	}
	
}
