using Godot;
using System;

public partial class PassiveBindButton : Control
{
	[Signal] public delegate void PassiveBindButtonPressedEventHandler(PassiveBindButton passive_bind_button_);

	public void _on_button_down()
	{
		EmitSignal(nameof(PassiveBindButtonPressed), this); // Emits signal along with the information that the passive button carries
	}
}
