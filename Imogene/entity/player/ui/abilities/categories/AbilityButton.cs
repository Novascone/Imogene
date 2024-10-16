using Godot;
using System;

public partial class AbilityButton : Button
{

	[Export] public string ability_name { get; set; }

	public override void _GuiInput(InputEvent @event_)
	{
		if(@event_ is InputEventJoypadButton eventJoypadButton)
		{
			if(eventJoypadButton.ButtonIndex == JoyButton.A){GD.Print("event accepted"); AcceptEvent();}
			if(eventJoypadButton.ButtonIndex == JoyButton.B){GD.Print("event accepted"); AcceptEvent();}
		}
	}

	
}
