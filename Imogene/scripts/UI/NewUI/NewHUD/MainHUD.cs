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

	public string left_up = "RB";
	public string left_left = "LB";
	public string left_right = "RT";
	public string left_down = "LT";

	public string right_up = "Y";
	public string right_left = "X";
	public string right_right = "B";
	public string right_down = "A";



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

	public void AssignAbility(Ability ability)
	{
		if(ability.cross == "Left")
		{
			if(ability.level == "Primary")
			{
				if(ability.assigned_button == left_up)
				{
					l_cross_primary.up.Icon = ability.icon;
				}
				else if(ability.assigned_button == left_left)
				{
					l_cross_primary.left.Icon = ability.icon;
				}
				else if(ability.assigned_button == left_right)
				{
					l_cross_primary.right.Icon = ability.icon;
				}
				else if(ability.assigned_button == left_down)
				{
					l_cross_primary.down.Icon = ability.icon;
				}

			}
			else if(ability.level == "Secondary")
			{
				if(ability.assigned_button == left_up)
				{
					l_cross_secondary.up.Icon = ability.icon;
				}
				else if(ability.assigned_button == left_left)
				{
					l_cross_secondary.left.Icon = ability.icon;
				}
				else if(ability.assigned_button == left_right)
				{
					l_cross_secondary.right.Icon = ability.icon;
				}
				else if(ability.assigned_button == left_down)
				{
					l_cross_secondary.down.Icon = ability.icon;
				}
			}
		}
		else if(ability.cross == "Right")
		{
			if(ability.level == "Primary")
			{
				if(ability.assigned_button == right_up)
				{
					r_cross_primary.up.Icon = ability.icon;
				}
				else if(ability.assigned_button == right_left)
				{
					r_cross_primary.left.Icon = ability.icon;
				}
				else if(ability.assigned_button == right_right)
				{
					r_cross_primary.right.Icon = ability.icon;
				}
				else if(ability.assigned_button == right_down)
				{
					r_cross_primary.down.Icon = ability.icon;
				}

			}
			else if(ability.level == "Secondary")
			{
				if(ability.assigned_button == right_up)
				{
					r_cross_secondary.up.Icon = ability.icon;
				}
				else if(ability.assigned_button == right_left)
				{
					r_cross_secondary.left.Icon = ability.icon;
				}
				else if(ability.assigned_button == right_right)
				{
					r_cross_secondary.right.Icon = ability.icon;
				}
				else if(ability.assigned_button == right_down)
				{
					r_cross_secondary.down.Icon = ability.icon;
				}
			}
		}
	}
}
