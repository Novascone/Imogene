using Godot;
using System;

public partial class GenericInventoryButton : Button
{
	[Export] public Control info;
	[Export] public RichTextLabel info_text;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = Name;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_2d_area_entered(Area2D area)
	{
		if (area.IsInGroup("cursor"))
		{
			GrabFocus();
		}
	}

	public void _on_area_2d_area_exited(Area2D area)
	{
		if (area.IsInGroup("cursor"))
		{
			ReleaseFocus();
		}
	}

	public void _on_button_down()
	{
		AcceptEvent();
	}

	public void _on_focus_entered()
	{
		info.Show();
	}

	public void _on_focus_exited()
	{
		info.Hide();
	}
	public override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(eventJoypadButton.ButtonIndex == JoyButton.A){GD.Print("event accepted"); AcceptEvent();}
			if(eventJoypadButton.ButtonIndex == JoyButton.B){GD.Print("event accepted"); AcceptEvent();}
		}
	}

}
