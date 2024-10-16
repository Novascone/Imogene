using Godot;
using System;

public partial class Binds : Control
{
	[Export] public CrossBinds l_cross_primary_assignment { get; set; }
	[Export] public CrossBinds r_cross_primary_assignment { get; set; }
	[Export] public CrossBinds l_cross_secondary_assignment { get; set; }
	[Export] public CrossBinds r_cross_secondary_assignment { get; set; }
	[Export] public Control passives { get; set; }

	public string left_up { get; set; } = "RB";
	public string left_left { get; set; } = "LB";
	public string left_right { get; set; } = "RT";
	public string left_down { get; set; } = "LT";

	public string right_up { get; set; } = "Y";
	public string right_left { get; set; } = "X";
	public string right_right { get; set; } = "B";
	public string right_down { get; set; } = "A";

	public void _on_close_button_down()
	{
		
	}

	public void AssignAbility(Ability.Cross cross_, Ability.Tier tier_, string bind_, string ability_name_, Texture2D icon_)
	{
		if(CheckAssignment(cross_, tier_, bind_) == "LPrimeCrossUp") // Finds the assignment of an ability and sets the name and Icon for it
		{
			l_cross_primary_assignment.up.Icon = icon_;
			l_cross_primary_assignment.up.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "LPrimeCrossLeft")
		{
			l_cross_primary_assignment.left.Icon = icon_;
			l_cross_primary_assignment.left.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "LPrimeCrossRight")
		{
			l_cross_primary_assignment.right.Icon = icon_;
			l_cross_primary_assignment.right.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "LPrimeCrossDown")
		{
			l_cross_primary_assignment.down.Icon = icon_;
			l_cross_primary_assignment.down.ability_name = ability_name_;
		}

		if(CheckAssignment(cross_, tier_, bind_) == "LSecondCrossUp"){
			l_cross_secondary_assignment.up.Icon = icon_;
			l_cross_secondary_assignment.up.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "LSecondCrossLeft")
		{
			l_cross_secondary_assignment.left.Icon = icon_;
			l_cross_secondary_assignment.left.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "LSecondCrossRight")
		{
			l_cross_secondary_assignment.right.Icon = icon_;
			l_cross_secondary_assignment.right.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "LSecondCrossDown")
		{
			l_cross_secondary_assignment.down.Icon = icon_;
			l_cross_secondary_assignment.down.ability_name = ability_name_;
		}

		if(CheckAssignment(cross_, tier_, bind_) == "RPrimeCrossUp"){
			r_cross_primary_assignment.up.Icon = icon_;
			r_cross_primary_assignment.up.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "RPrimeCrossLeft")
		{
			r_cross_primary_assignment.left.Icon = icon_;
			r_cross_primary_assignment.left.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "RPrimeCrossRight")
		{
			r_cross_primary_assignment.right.Icon = icon_;
			r_cross_primary_assignment.right.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "RPrimeCrossDown")
		{
			r_cross_primary_assignment.down.Icon = icon_;
			r_cross_primary_assignment.down.ability_name = ability_name_;
		}

		if(CheckAssignment(cross_, tier_, bind_) == "RSecondCrossUp"){
			r_cross_secondary_assignment.up.Icon = icon_;
			r_cross_secondary_assignment.up.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "RSecondCrossLeft")
		{
			r_cross_secondary_assignment.left.Icon = icon_;
			r_cross_secondary_assignment.left.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "RSecondCrossRight")
		{
			r_cross_secondary_assignment.right.Icon = icon_;
			r_cross_secondary_assignment.right.ability_name = ability_name_;
		}
		else if(CheckAssignment(cross_, tier_, bind_) == "RSecondCrossDown")
		{
			r_cross_secondary_assignment.down.Icon = icon_;
			r_cross_secondary_assignment.down.ability_name = ability_name_;
		}
	}

	public void ClearAbility(string ability_name_old_, string ability_name_new_) // Clears Icons and names from both the old and the new ability, the clear ability signal is fired first, so both abilities are cleared before reassignment
	{
		foreach(Control control in l_cross_primary_assignment.GetChildren()) // Goes through each cross and clears the name and icon if the ability name matches
		{
			if(control is CrossBindButton cross_bind_button)
			{
				if(cross_bind_button.ability_name == ability_name_old_ || cross_bind_button.ability_name == ability_name_new_)
				{
					cross_bind_button.Icon = null;
					cross_bind_button.ability_name = "";
				}
			}
		}

		foreach(Control control in l_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind_button)
			{
				if(cross_bind_button.ability_name == ability_name_old_ || cross_bind_button.ability_name == ability_name_new_)
				{
					cross_bind_button.Icon = null;
					cross_bind_button.ability_name = "";
				}
			}
		}

		foreach(Control control in r_cross_primary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind_button)
			{
				if(cross_bind_button.ability_name == ability_name_old_ || cross_bind_button.ability_name == ability_name_new_)
				{
					cross_bind_button.Icon = null;
					cross_bind_button.ability_name = "";
				}
			}
		}

		foreach(Control control in r_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind_button)
			{
				if(cross_bind_button.ability_name == ability_name_old_ || cross_bind_button.ability_name == ability_name_new_)
				{
					cross_bind_button.Icon = null;
					cross_bind_button.ability_name = "";
				}
			}
		}
	}

	public string CheckAssignment(Ability.Cross cross_, Ability.Tier tier_, string bind_) // Checks the assignment of an ability
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
}
