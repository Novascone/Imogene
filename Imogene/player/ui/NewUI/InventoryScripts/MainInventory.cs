using Godot;
using System;

public partial class MainInventory : Control
{
	[Export] public Control items;
	[Export] public GenericInventoryButton mats;
	[Export] public BottomButtons bottom_buttons;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_mats_button_down()
	{
		GD.Print("Mats button down");
		mats.Show();
	}
}
