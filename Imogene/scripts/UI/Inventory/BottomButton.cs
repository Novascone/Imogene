using Godot;
using System;

public partial class BottomButton : Button
{
	private Control info;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		info = GetNode<Control>("Info");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
			if(eventJoypadButton.Pressed && eventJoypadButton.ButtonIndex == JoyButton.B)
			{
				GD.Print("event accepted ");
				AcceptEvent();
			}
		}
		
	}


}
