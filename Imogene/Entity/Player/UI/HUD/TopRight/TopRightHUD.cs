using Godot;
using System;

public partial class TopRightHUD : Control
{
	[Export] public Control soft_target_indicator;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void BrightenSoftTargetIndicator()
	{
		GD.Print("brightening soft target indicator");
		soft_target_indicator.Modulate = new Color(Colors.White, 1.0f);
	}
	public void DimSoftTargetIndicator()
	{
		GD.Print("dimming soft target indicator");
		soft_target_indicator.Modulate = new Color(Colors.White, 0.1f);
	}
}
