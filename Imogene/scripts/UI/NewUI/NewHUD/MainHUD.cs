using Godot;
using System;

public partial class MainHUD : Control
{
	[Export] public Control useable_1;
	[Export] public Control useable_2;
	[Export] public Control useable_3;
	[Export] public Control useable_4;
	[Export] public Control posture;
	[Export] public Control xp;
	[Export] public Control health;
	[Export] public HUDCross l_cross_primary;
	[Export] public HUDCross l_cross_secondary;
	[Export] public HUDCross r_cross_primary;
	[Export] public HUDCross r_cross_secondary;
	
	[Export] public Control resource;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach(Control control in l_cross_secondary.GetChildren())
		{
			if (control is HUDButton hud_button)
			{
				hud_button.label.Hide();
			}
		}

		foreach(Control control in r_cross_secondary.GetChildren())
		{
			if (control is HUDButton hud_button)
			{
				hud_button.label.Hide();
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
