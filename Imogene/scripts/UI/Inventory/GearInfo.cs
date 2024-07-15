using Godot;
using System;

public partial class GearInfo : Button
{
	private CustomSignals _customSignals;
	private Control info;
	private UI this_ui;
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

	public void GetUIInfo(UI i)
	{
		this_ui = i;
	}

	public void _on_focus_entered()
	{
		if(this.Name == "Head")
		{
			// _customSignals.EmitSignal(nameof(CustomSignals.OverSlot), "Head");
			this_ui.over_head = true;
			GD.Print(this.Name);
		}
		info.Show();
	}
	public void _on_focus_exited()
	{
		if(this.Name == "Head")
		{
			// _customSignals.EmitSignal(nameof(CustomSignals.OverSlot), "Head");
			this_ui.over_head = false;
			GD.Print(this.Name);
		}
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
