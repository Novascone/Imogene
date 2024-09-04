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
	[Export] public HUDHealth health;
	[Export] public HUDCross l_cross_primary;
	[Export] public HUDCross l_cross_secondary;
	[Export] public HUDCross r_cross_primary;
	[Export] public HUDCross r_cross_secondary;
	[Export] public HUDResource resource;

	public bool l_cross_primary_selected = true;
	public bool r_cross_primary_selected = true;

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

	public void SwitchCrosses(string cross)
	{
		if(cross == "Left")
		{
			MoveCrosses(cross);
			l_cross_primary_selected = !l_cross_primary_selected;
		}
		else if(cross == "Right")
		{
			MoveCrosses(cross);
			r_cross_primary_selected = !r_cross_primary_selected;
		}

	}

	public void UpdateHUDStats(Player player)
	{
		
		health.hit_points.MaxValue = player.maximum_health;
		health.hit_points.Value = player.health;
		resource.resource_points.MaxValue = player.maximum_resource;
		resource.resource_points.Value = player.resource;
		
	}

	public void MoveCrosses(string cross)
	{
		if(cross == "Left")
		{
			if(l_cross_primary_selected)
			{
				l_cross_primary.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
				l_cross_primary.Modulate = new Color(Colors.White, 0.1f);
				l_cross_primary.up.label.Hide();
				l_cross_primary.left.label.Hide();
				l_cross_primary.right.label.Hide();
				l_cross_primary.down.label.Hide();
				l_cross_primary.ZIndex = 0;

				l_cross_secondary.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
				l_cross_secondary.Modulate = new Color(Colors.White, 1f);
				l_cross_secondary.up.label.Show();
				l_cross_secondary.left.label.Show();
				l_cross_secondary.right.label.Show();
				l_cross_secondary.down.label.Show();
				l_cross_secondary.ZIndex = 1;
			}
			else
			{
				l_cross_primary.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
				l_cross_primary.Modulate = new Color(Colors.White, 1f);
				l_cross_primary.up.label.Show();
				l_cross_primary.left.label.Show();
				l_cross_primary.right.label.Show();
				l_cross_primary.down.label.Show();
				l_cross_primary.ZIndex = 1;

				l_cross_secondary.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
				l_cross_secondary.Modulate = new Color(Colors.White, 0.1f);
				l_cross_secondary.up.label.Hide();
				l_cross_secondary.left.label.Hide();
				l_cross_secondary.right.label.Hide();
				l_cross_secondary.down.label.Hide();
				l_cross_secondary.ZIndex = 0;
			}
		}
		else if (cross == "Right")
		{
			if(r_cross_primary_selected)
			{
				r_cross_primary.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
				r_cross_primary.Modulate = new Color(Colors.White, 0.1f);
				r_cross_primary.up.label.Hide();
				r_cross_primary.left.label.Hide();
				r_cross_primary.right.label.Hide();
				r_cross_primary.down.label.Hide();
				r_cross_primary.ZIndex = 0;

				r_cross_secondary.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
				r_cross_secondary.Modulate = new Color(Colors.White, 1f);
				r_cross_secondary.up.label.Show();
				r_cross_secondary.left.label.Show();
				r_cross_secondary.right.label.Show();
				r_cross_secondary.down.label.Show();
				r_cross_secondary.ZIndex = 1;
			}
			else
			{
				r_cross_primary.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
				r_cross_primary.Modulate = new Color(Colors.White, 1f);
				r_cross_primary.up.label.Show();
				r_cross_primary.left.label.Show();
				r_cross_primary.right.label.Show();
				r_cross_primary.down.label.Show();
				r_cross_primary.ZIndex = 1;

				r_cross_secondary.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
				r_cross_secondary.Modulate = new Color(Colors.White, 0.1f);
				r_cross_secondary.up.label.Hide();
				r_cross_secondary.left.label.Hide();
				r_cross_secondary.right.label.Hide();
				r_cross_secondary.down.label.Hide();
				r_cross_secondary.ZIndex = 0;
			}
		}
		
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
