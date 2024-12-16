using Godot;
using System;

public partial class CrossBindButton : Button
{
	[Export] public string button_bind { get; set; }
	[Export] public Label label { get; set; }
	[Export] public Ability.Cross cross { get; set; }
	[Export] public Ability.Tier tier { get; set; }
	public string ability_name { get; set; }
	public string new_ability_name { get; set; }
	
	[Signal] public delegate void CrossButtonPressedEventHandler(CrossBindButton cross_button_);
	
	public override void _Ready()
	{
		label.Text = button_bind;
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
