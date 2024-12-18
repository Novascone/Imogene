using Godot;
using System;

public partial class EnemyUI : Node
{
	[Export] public Sprite3D StatusBar;
	[Export] public Sprite3D HardTargetIcon;
	[Export] public Sprite3D SoftTargetIcon;
	[Export] public ProgressBar HealthBar;
	[Export] public TextureProgressBar PostureBar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
