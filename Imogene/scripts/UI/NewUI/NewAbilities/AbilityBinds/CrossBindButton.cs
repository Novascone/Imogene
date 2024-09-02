using Godot;
using System;

public partial class CrossBindButton : Button
{
	[Export] public string button_bind;
	[Export] public Label label;
	[Export] public string side;
	[Export] public string level;
	[Signal] public delegate void CrossButtonPressedEventHandler(CrossBindButton cross_button);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label.Text = button_bind;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_button_down()
	{
		GD.Print("Cross bind button down");
		EmitSignal(nameof(CrossButtonPressed),this);
	}

}
