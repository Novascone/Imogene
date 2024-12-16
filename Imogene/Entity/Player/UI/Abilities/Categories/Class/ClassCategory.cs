using Godot;
using System;
using System.Linq;

public partial class ClassCategory : Control
{
	[Export] public AbilityPage basic { get; set; }
	[Export] public AbilityPage kernel  { get; set; }
	[Export] public AbilityPage defensive  { get; set; }
	[Export] public AbilityPage mastery  { get; set; }
	[Export] public AbilityPage movement { get; set; }
	[Export] public AbilityPage specialized { get; set; }
	[Export] public AbilityPage unique { get; set; }
	[Export] public AbilityPage toy { get; set; }
	[Export] public Control buttons_container { get; set; }
	[Export] public Control buttons  { get; set; }
	

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
