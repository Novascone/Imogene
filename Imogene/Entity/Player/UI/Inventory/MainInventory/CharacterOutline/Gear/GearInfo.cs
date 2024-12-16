using Godot;
using System;

public partial class GearInfo : Button
{

	[Export] private Control info { get; set; }

	public void _on_focus_entered()
	{
		
		info.Show();
	}
	public void _on_focus_exited()
	{
		
		info.Hide();
	}

	public void _on_area_2d_area_entered(Area2D area_)
	{
		if(area_.IsInGroup("cursor"))
		{
			GrabFocus();
		}
	}
	public void _on_area_2d_area_exited(Area2D area_)
	{
		if(area_.IsInGroup("cursor"))
		{
			ReleaseFocus();
		}
	}

	public override void _GuiInput(InputEvent @event_)
	{
		if(@event_ is InputEventJoypadButton eventJoypadButton)
		{
			if(eventJoypadButton.ButtonIndex == JoyButton.A){GD.Print("event accepted"); AcceptEvent();}
			if(eventJoypadButton.ButtonIndex == JoyButton.B){GD.Print("event accepted"); AcceptEvent();}
		}
	}


}
