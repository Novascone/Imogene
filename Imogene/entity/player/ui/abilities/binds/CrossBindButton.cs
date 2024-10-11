using Godot;
using System;

public partial class CrossBindButton : Button
{
	[Export] public string button_bind;
	[Export] public Label label;
	[Export] public Ability.Cross cross;
	[Export] public Ability.Tier tier;
	[Signal] public delegate void CrossButtonPressedEventHandler(CrossBindButton cross_button);
	public string ability_name;
	public string new_ability_name;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = button_bind;
		// ButtonDown += () => _on_button_down(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventJoypadButton eventJoypadButton)
		{
			if(eventJoypadButton.ButtonIndex == JoyButton.A){GD.Print("event accepted"); AcceptEvent();}
			if(eventJoypadButton.ButtonIndex == JoyButton.B){GD.Print("event accepted"); AcceptEvent();}
		}
	}

	// public void _on_button_down(CrossBindButton cross_bind_button)
	// {
	// 	// GD.Print("Cross bind button down");
	// 	// EmitSignal(nameof(CrossButtonPressed),this);
	// }

}
