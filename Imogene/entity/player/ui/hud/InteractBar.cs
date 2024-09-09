using Godot;
using System;

public partial class InteractBar : Control
{
	[Export] public Label button;
	[Export] public Label interact_object;
	[Export] public Control interact_inventory;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetInteractText(string object_name)
	{
		interact_object.Text = object_name;
	}
}
