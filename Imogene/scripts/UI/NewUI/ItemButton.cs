using Godot;
using System;

public partial class ItemButton : Button
{
	[Signal] public delegate void CursorHoveringEventHandler(ItemButton item_button);
	[Signal] public delegate void CursorLeftEventHandler(ItemButton item_button);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_2d_area_entered(Area2D area)
	{
		if(area.IsInGroup("cursor"))
		{
			GrabFocus();
		}
	}

	public void _on_area_2d_area_exited(Area2D area)
	{
		if(area.IsInGroup("cursor"))
		{
			ReleaseFocus();
		}
	}
}
