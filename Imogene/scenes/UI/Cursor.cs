using Godot;
using System;

public partial class Cursor : Sprite2D
{

	public InventoryButton cursor_button;
	public Area2D cursor_button_area;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cursor_button = GetNode<InventoryButton>("CursorArea2D/CursorButton");
		cursor_button.Disabled = true;
		cursor_button_area = (Area2D)cursor_button.GetChild(2);
		cursor_button_area.Monitoring = false;
		cursor_button.AddToGroup("cursorbutton");
		GD.Print("cursro button disabled " + cursor_button.Disabled);
		cursor_button.Hide();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
