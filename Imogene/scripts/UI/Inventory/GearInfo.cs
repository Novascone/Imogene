using Godot;
using System;

public partial class GearInfo : Button
{
	private CustomSignals _customSignals;
	private Control info;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		info = GetNode<Control>("Info");
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_focus_entered()
	{
		if(this.Name == "Head")
		{
			_customSignals.EmitSignal(nameof(CustomSignals.OverSlot), "Head");
			GD.Print(this.Name);
		}
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
