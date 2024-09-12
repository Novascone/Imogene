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
		// Hide labels on secondary crosses
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

	public void SwitchCrosses(string cross) // switches between crosses
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
		
		health.hit_points.MaxValue = player.depth_stats["maximum_health"];
		health.hit_points.Value = player.general_stats["health"];
		resource.resource_points.MaxValue = player.depth_stats["maximum_resource"];
		resource.resource_points.Value = player.general_stats["resource"];
		
	}

	public void MoveCrosses(string cross) // checks which cross to switch and then changes the positioning and color of the crosses
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

	public void AssignAbility(string cross, string level, string bind, string ability_name, Texture2D icon) // Checks the assignment of an ability and then assigns the hud cross button that ability's icon and name
	{
		if(CheckAssignment(cross, level, bind) == "LPrimeCrossUp"){
			l_cross_primary.up.Icon = icon;
			l_cross_primary.up.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LPrimeCrossLeft")
		{
			l_cross_primary.left.Icon = icon;
			l_cross_primary.left.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LPrimeCrossRight")
		{
			l_cross_primary.right.Icon = icon;
			l_cross_primary.right.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LPrimeCrossDown")
		{
			l_cross_primary.down.Icon = icon;
			l_cross_primary.down.ability_name = ability_name;
		}

		if(CheckAssignment(cross, level, bind) == "LSecondCrossUp"){
			l_cross_secondary.up.Icon = icon;
			l_cross_secondary.up.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LSecondCrossLeft")
		{
			l_cross_secondary.left.Icon = icon;
			l_cross_secondary.left.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LSecondCrossRight")
		{
			l_cross_secondary.right.Icon = icon;
			l_cross_secondary.right.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LSecondCrossDown")
		{
			l_cross_secondary.down.Icon = icon;
			l_cross_secondary.down.ability_name = ability_name;
		}

		if(CheckAssignment(cross, level, bind) == "RPrimeCrossUp"){
			r_cross_primary.up.Icon = icon;
			r_cross_primary.up.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RPrimeCrossLeft")
		{
			r_cross_primary.left.Icon = icon;
			r_cross_primary.left.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RPrimeCrossRight")
		{
			r_cross_primary.right.Icon = icon;
			r_cross_primary.right.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RPrimeCrossDown")
		{
			r_cross_primary.down.Icon = icon;
			r_cross_primary.down.ability_name = ability_name;
		}

		if(CheckAssignment(cross, level, bind) == "RSecondCrossUp"){
			r_cross_secondary.up.Icon = icon;
			r_cross_secondary.up.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RSecondCrossLeft")
		{
			r_cross_secondary.left.Icon = icon;
			r_cross_secondary.left.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RSecondCrossRight")
		{
			r_cross_secondary.right.Icon = icon;
			r_cross_secondary.right.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RSecondCrossDown")
		{
			r_cross_secondary.down.Icon = icon;
			r_cross_secondary.down.ability_name = ability_name;
		}
	}

	public void ClearAbility(string ability_name_old, string ability_name_new) // Looks through each cross to find the old and new abilities, and then removes their icon and name
	{
		foreach(Control control in l_cross_primary.GetChildren())
		{
			if(control is HUDButton hud_button)
			{
				if(hud_button.ability_name == ability_name_old || hud_button.ability_name == ability_name_new)
				{
					GD.Print("Looking in L Cross " + hud_button.ability_name);
					hud_button.Icon = null;
					hud_button.ability_name = "";
				}
			}
		}

		foreach(Control control in l_cross_secondary.GetChildren())
		{
			if(control is HUDButton hud_button)
			{
				
				
				if(hud_button.ability_name == ability_name_old || hud_button.ability_name == ability_name_new)
				{
					GD.Print("Looking in L Cross " + hud_button.ability_name);
					hud_button.Icon = null;
					hud_button.ability_name = "";
				}
			}
		}

		foreach(Control control in r_cross_primary.GetChildren())
		{
			if(control is HUDButton hud_button)
			{
				
				
				if(hud_button.ability_name == ability_name_old || hud_button.ability_name == ability_name_new)
				{
					GD.Print("Looking in L Cross " + hud_button.ability_name);
					hud_button.Icon = null;
					hud_button.ability_name = "";
				}
			}
		}

		foreach(Control control in r_cross_secondary.GetChildren())
		{
			if(control is HUDButton hud_button)
			{
				
				
				if(hud_button.ability_name == ability_name_old || hud_button.ability_name == ability_name_new)
				{
					GD.Print("Looking in L Cross " + hud_button.ability_name);
					hud_button.Icon = null;
					hud_button.ability_name = "";
				}
			}
		}
	}

	public string CheckAssignment(string cross, string level, string bind) // Checks which button an ability is assigned to
	{
		if(cross == "Left")
		{
			if(level == "Primary")
			{
				if(bind == left_up)
				{return "LPrimeCrossUp";}
				else if(bind == left_left){return "LPrimeCrossLeft";}
				else if(bind == left_right){return "LPrimeCrossRight";}
				else if(bind == left_down){return "LPrimeCrossDown";}
				else return "";

			}
			else if(level == "Secondary")
			{
				if(bind == left_up)
				{return "LSecondCrossUp";}
				else if(bind == left_left){return "LSecondCrossLeft";}
				else if(bind == left_right){return "LSecondCrossRight";}
				else if(bind == left_down){return "LSecondCrossDown";}
				else return "";
			}
			else return "";
		}
		else if(cross== "Right")
		{
			if(level == "Primary")
			{
				if(bind == right_up){return "RPrimeCrossUp";}
				else if(bind == right_left){return "RPrimeCrossLeft";}
				else if(bind == right_right){return "RPrimeCrossRight";}
				else if(bind == right_down){return "RPrimeCrossDown";}
				else return "";
			}
			else if(level == "Secondary")
			{
				if(bind == right_up){return "RSecondCrossUp";}
				else if(bind == right_left){return "RPrimeCrossLeft";}
				else if(bind == right_right){return "RPrimeCrossRight";}
				else if(bind == right_down){return "RPrimeCrossDown";}
				else return "";
			}
			else return "";
		}
		else return "";
	}
}
