using Godot;
using System;
using System.Linq;

public partial class MainHUD : Control
{
	[Export] public Control useable_1 { get; set; }
	[Export] public Control useable_2 { get; set; }
	[Export] public Control useable_3 { get; set; }
	[Export] public Control useable_4 { get; set; }
	[Export] public Control posture { get; set; }
	[Export] public Control xp { get; set; }
	[Export] public HUDHealth health { get; set; }
	[Export] public HUDCross l_cross_primary { get; set; }
	[Export] public HUDCross l_cross_secondary { get; set; }
	[Export] public HUDCross r_cross_primary { get; set; }
	[Export] public HUDCross r_cross_secondary { get; set; }
	[Export] public HUDResource resource { get; set; }

	public bool l_cross_primary_selected { get; set; } = true;
	public bool r_cross_primary_selected { get; set; } = true;

	public string left_up { get; set; } = "RB";
	public string left_left { get; set; } = "LB";
	public string left_right { get; set; } = "RT";
	public string left_down { get; set; } = "LT";

	public string right_up { get; set; } = "Y";
	public string right_left { get; set; } = "X";
	public string right_right { get; set; } = "B";
	public string right_down { get; set; } = "A";



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hide labels on secondary crosses
		foreach(Control _control in l_cross_secondary.GetChildren().Cast<Control>())
		{
			if (_control is HUDButton hud_button)
			{
				hud_button.label.Hide();
			}
		}

		foreach(Control _control in r_cross_secondary.GetChildren().Cast<Control>())
		{
			if (_control is HUDButton hud_button)
			{
				hud_button.label.Hide();
			}
		}
	}

	public void SwitchCrosses(string cross_) // switches between crosses
	{
		if(cross_ == "Left")
		{
			MoveCrosses(cross_);
			l_cross_primary_selected = !l_cross_primary_selected;
		}
		else if(cross_ == "Right")
		{
			MoveCrosses(cross_);
			r_cross_primary_selected = !r_cross_primary_selected;
		}

	}

	public void UpdateHUDStats(Player player)
	{
		
		health.hit_points.MaxValue = player.Health.max_value;
		health.hit_points.Value = player.Health.current_value;
		resource.resource_points.MaxValue = player.Resource.max_value;
		resource.resource_points.Value = player.Resource.current_value;
		
	}

	public void MoveCrosses(string cross_) // checks which cross to switch and then changes the positioning and color of the crosses
	{
		if(cross_ == "Left")
		{
			if(l_cross_primary_selected)
			{
				l_cross_primary.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
				l_cross_primary.SizeFlagsVertical = SizeFlags.ShrinkCenter;
				l_cross_primary.Modulate = new Color(Colors.White, 0.1f);
				l_cross_primary.up.label.Hide();
				l_cross_primary.left.label.Hide();
				l_cross_primary.right.label.Hide();
				l_cross_primary.down.label.Hide();
				l_cross_primary.ZIndex = 0;

				l_cross_secondary.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
				l_cross_secondary.SizeFlagsVertical = SizeFlags.ShrinkEnd;
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
				l_cross_primary.SizeFlagsVertical = SizeFlags.ShrinkEnd;
				l_cross_primary.Modulate = new Color(Colors.White, 1f);
				l_cross_primary.up.label.Show();
				l_cross_primary.left.label.Show();
				l_cross_primary.right.label.Show();
				l_cross_primary.down.label.Show();
				l_cross_primary.ZIndex = 1;

				l_cross_secondary.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
				l_cross_secondary.SizeFlagsVertical = SizeFlags.ShrinkCenter;
				l_cross_secondary.Modulate = new Color(Colors.White, 0.1f);
				l_cross_secondary.up.label.Hide();
				l_cross_secondary.left.label.Hide();
				l_cross_secondary.right.label.Hide();
				l_cross_secondary.down.label.Hide();
				l_cross_secondary.ZIndex = 0;
			}
		}
		else if (cross_ == "Right")
		{
			if(r_cross_primary_selected)
			{
				r_cross_primary.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
				r_cross_primary.SizeFlagsVertical = SizeFlags.ShrinkCenter;
				r_cross_primary.Modulate = new Color(Colors.White, 0.1f);
				r_cross_primary.up.label.Hide();
				r_cross_primary.left.label.Hide();
				r_cross_primary.right.label.Hide();
				r_cross_primary.down.label.Hide();
				r_cross_primary.ZIndex = 0;

				r_cross_secondary.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
				r_cross_secondary.SizeFlagsVertical = SizeFlags.ShrinkEnd;
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
				r_cross_primary.SizeFlagsVertical = SizeFlags.ShrinkEnd;
				r_cross_primary.Modulate = new Color(Colors.White, 1f);
				r_cross_primary.up.label.Show();
				r_cross_primary.left.label.Show();
				r_cross_primary.right.label.Show();
				r_cross_primary.down.label.Show();
				r_cross_primary.ZIndex = 1;

				r_cross_secondary.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
				r_cross_secondary.SizeFlagsVertical = SizeFlags.ShrinkCenter;
				r_cross_secondary.Modulate = new Color(Colors.White, 0.1f);
				r_cross_secondary.up.label.Hide();
				r_cross_secondary.left.label.Hide();
				r_cross_secondary.right.label.Hide();
				r_cross_secondary.down.label.Hide();
				r_cross_secondary.ZIndex = 0;
			}
		}
		
	}

	public void AssignAbility(Ability.Cross cross_, Ability.Tier tier_, string bind, string ability_name_, Texture2D icon_) // Checks the assignment of an ability and then assigns the hud cross button that ability's icon and name
	{
		if(CheckAssignment(cross_, tier_, bind) == "LPrimeCrossUp"){
			l_cross_primary.up.Icon = icon_;
			l_cross_primary.up.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "LPrimeCrossLeft")
		{
			l_cross_primary.left.Icon = icon_;
			l_cross_primary.left.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "LPrimeCrossRight")
		{
			l_cross_primary.right.Icon = icon_;
			l_cross_primary.right.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "LPrimeCrossDown")
		{
			l_cross_primary.down.Icon = icon_;
			l_cross_primary.down.ability_name = ability_name_;
		}

		if(CheckAssignment(cross_, tier_, bind) == "LSecondCrossUp"){
			l_cross_secondary.up.Icon = icon_;
			l_cross_secondary.up.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "LSecondCrossLeft")
		{
			l_cross_secondary.left.Icon = icon_;
			l_cross_secondary.left.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "LSecondCrossRight")
		{
			l_cross_secondary.right.Icon = icon_;
			l_cross_secondary.right.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "LSecondCrossDown")
		{
			l_cross_secondary.down.Icon = icon_;
			l_cross_secondary.down.ability_name = ability_name_;
		}

		if(CheckAssignment(cross_, tier_, bind) == "RPrimeCrossUp"){
			r_cross_primary.up.Icon = icon_;
			r_cross_primary.up.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "RPrimeCrossLeft")
		{
			r_cross_primary.left.Icon = icon_;
			r_cross_primary.left.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "RPrimeCrossRight")
		{
			r_cross_primary.right.Icon = icon_;
			r_cross_primary.right.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "RPrimeCrossDown")
		{
			r_cross_primary.down.Icon = icon_;
			r_cross_primary.down.ability_name = ability_name_;
		}

		if(CheckAssignment(cross_, tier_, bind) == "RSecondCrossUp"){
			r_cross_secondary.up.Icon = icon_;
			r_cross_secondary.up.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "RSecondCrossLeft")
		{
			r_cross_secondary.left.Icon = icon_;
			r_cross_secondary.left.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "RSecondCrossRight")
		{
			r_cross_secondary.right.Icon = icon_;
			r_cross_secondary.right.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind) == "RSecondCrossDown")
		{
			r_cross_secondary.down.Icon = icon_;
			r_cross_secondary.down.ability_name = ability_name_;
		}
	}

	public void ClearAbility(string ability_name_old_, string ability_name_new_) // Looks through each cross to find the old and new abilities, and then removes their icon and name
	{
		foreach(Control control in l_cross_primary.GetChildren().Cast<Control>())
		{
			if(control is HUDButton hud_button)
			{
				if(hud_button.ability_name == ability_name_old_ || hud_button.ability_name == ability_name_new_)
				{
					hud_button.Icon = null;
					hud_button.ability_name = "";
				}
			}
		}

		foreach(Control _control in l_cross_secondary.GetChildren().Cast<Control>())
		{
			if(_control is HUDButton hud_button)
			{
				
				
				if(hud_button.ability_name == ability_name_old_ || hud_button.ability_name == ability_name_new_)
				{
					hud_button.Icon = null;
					hud_button.ability_name = "";
				}
			}
		}

		foreach(Control _control in r_cross_primary.GetChildren().Cast<Control>())
		{
			if(_control is HUDButton hud_button)
			{
				
				
				if(hud_button.ability_name == ability_name_old_ || hud_button.ability_name == ability_name_new_)
				{
					hud_button.Icon = null;
					hud_button.ability_name = "";
				}
			}
		}

		foreach(Control _control in r_cross_secondary.GetChildren().Cast<Control>())
		{
			if(_control is HUDButton hud_button)
			{
				
				
				if(hud_button.ability_name == ability_name_old_ || hud_button.ability_name == ability_name_new_)
				{
					hud_button.Icon = null;
					hud_button.ability_name = "";
				}
			}
		}
	}

	public string CheckAssignment(Ability.Cross cross_, Ability.Tier tier_, string bind_) // Checks which button an ability is assigned to
	{
		if(cross_ == Ability.Cross.Left)
		{
			if(tier_ == Ability.Tier.Primary)
			{
				if(bind_ == left_up)
				{return "LPrimeCrossUp";}
				else if(bind_ == left_left){return "LPrimeCrossLeft";}
				else if(bind_ == left_right){return "LPrimeCrossRight";}
				else if(bind_ == left_down){return "LPrimeCrossDown";}
				else return "";

			}
			else if(tier_ == Ability.Tier.Secondary)
			{
				if(bind_ == left_up)
				{return "LSecondCrossUp";}
				else if(bind_ == left_left){return "LSecondCrossLeft";}
				else if(bind_ == left_right){return "LSecondCrossRight";}
				else if(bind_ == left_down){return "LSecondCrossDown";}
				else return "";
			}
			else return "";
		}
		else if(cross_ == Ability.Cross.Right)
		{
			if(tier_ == Ability.Tier.Primary)
			{
				if(bind_ == right_up){return "RPrimeCrossUp";}
				else if(bind_ == right_left){return "RPrimeCrossLeft";}
				else if(bind_ == right_right){return "RPrimeCrossRight";}
				else if(bind_ == right_down){return "RPrimeCrossDown";}
				else return "";
			}
			else if(tier_ == Ability.Tier.Secondary)
			{
				if(bind_ == right_up){return "RSecondCrossUp";}
				else if(bind_ == right_left){return "RPrimeCrossLeft";}
				else if(bind_ == right_right){return "RPrimeCrossRight";}
				else if(bind_ == right_down){return "RPrimeCrossDown";}
				else return "";
			}
			else return "";
		}
		else return "";
	}

    internal void HandleResourceChange(float incoming_resource_value_)
    {
        
		resource.resource_points.Value = incoming_resource_value_;
    }
}
