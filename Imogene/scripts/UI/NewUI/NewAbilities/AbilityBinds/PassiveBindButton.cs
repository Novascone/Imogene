using Godot;
using System;

public partial class PassiveBindButton : Control
{
	[Signal] public delegate void PassiveBindButtonPressedEventHandler(PassiveBindButton passive_bind_button);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_button_down()
	{
		EmitSignal(nameof(PassiveBindButtonPressed), this); // Emits signal along with the information that the passive button carries
	}
}
