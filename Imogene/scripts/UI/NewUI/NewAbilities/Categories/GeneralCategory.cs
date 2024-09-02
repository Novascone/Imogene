using Godot;
using System;

public partial class GeneralCategory : Control
{
	[Export] public Control melee;
	[Export] public Control ranged;
	[Export] public Control defensive;
	[Export] public Control movement;
	[Export] public Control unique;
	[Export] public Control toy;
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
		buttons.Hide();
	}

	public void _on_ranged_button_down()
	{
		ranged.Show();
		buttons.Hide();
	}

	public void _on_defensive_button_down()
	{
		defensive.Show();
		buttons.Hide();
	}

	public void _on_movement_button_down()
	{
		movement.Show();
		buttons.Hide();
	}

	public void _on_unique_button_down()
	{
		unique.Show();
		buttons.Hide();
	}

	public void _on_toy_button_down()
	{
		toy.Show();
		buttons.Hide();
	}
	
}
