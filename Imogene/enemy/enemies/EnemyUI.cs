using Godot;
using System;

public partial class EnemyUI : Node
{
	[Export] public Sprite3D status_bar;
	[Export] public Sprite3D hard_target_icon;
	[Export] public Sprite3D soft_target_icon;
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
