using Godot;
using System;

public partial class ClassCategory : Control
{
	[Export] public Control basic;
	[Export] public Control kernel;
	[Export] public Control defensive;
	[Export] public Control mastery;
	[Export] public Control movement;
	[Export] public Control specialized;
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

	public void _on_basic_button_down()
	{
		basic.Show();
		buttons.Hide();
	}
	
	public void _on_kernel_button_down()
	{
		kernel.Show();
		buttons.Hide();
	}

	public void _on_defensive_button_down()
	{
		defensive.Show();
		buttons.Hide();
	}

	public void _on_mastery_button_down()
	{
		mastery.Show();
		buttons.Hide();
	}

	public void _on_movement_button_down()
	{
		movement.Show();
		buttons.Hide();
	}

	public void _on_specialized_button_down()
	{
		specialized.Show();
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
