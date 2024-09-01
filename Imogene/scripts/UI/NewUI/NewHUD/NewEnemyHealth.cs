using Godot;
using System;

public partial class NewEnemyHealth : Control
{
	[Export] public VBoxContainer vBoxContainer;
	[Export] public Label label;
	[Export] public ProgressBar health_bar;
	[Export] public TextureProgressBar posture_bar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
