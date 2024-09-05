using Godot;
using System;

public partial class Binds : Control
{
	[Export] public CrossBinds l_cross_primary_assignment;
	[Export] public CrossBinds r_cross_primary_assignment;
	[Export] public CrossBinds l_cross_secondary_assignment;
	[Export] public CrossBinds r_cross_secondary_assignment;
	[Export] public Control passives;

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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_close_button_down()
	{
		
	}

	public void AssignAbility(string cross, string level, string bind, string ability_name, Texture2D icon)
	{
		if(CheckAssignment(cross, level, bind) == "LPrimeCrossUp"){
			l_cross_primary_assignment.up.Icon = icon;
			l_cross_primary_assignment.up.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LPrimeCrossLeft")
		{
			l_cross_primary_assignment.left.Icon = icon;
			l_cross_primary_assignment.left.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LPrimeCrossRight")
		{
			l_cross_primary_assignment.right.Icon = icon;
			l_cross_primary_assignment.right.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LPrimeCrossDown")
		{
			l_cross_primary_assignment.down.Icon = icon;
			l_cross_primary_assignment.down.ability_name = ability_name;
		}

		if(CheckAssignment(cross, level, bind) == "LSecondCrossUp"){
			l_cross_secondary_assignment.up.Icon = icon;
			l_cross_secondary_assignment.up.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LSecondCrossLeft")
		{
			l_cross_secondary_assignment.left.Icon = icon;
			l_cross_secondary_assignment.left.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LSecondCrossRight")
		{
			l_cross_secondary_assignment.right.Icon = icon;
			l_cross_secondary_assignment.right.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "LSecondCrossDown")
		{
			l_cross_secondary_assignment.down.Icon = icon;
			l_cross_secondary_assignment.down.ability_name = ability_name;
		}

		if(CheckAssignment(cross, level, bind) == "RPrimeCrossUp"){
			r_cross_primary_assignment.up.Icon = icon;
			r_cross_primary_assignment.up.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RPrimeCrossLeft")
		{
			r_cross_primary_assignment.left.Icon = icon;
			r_cross_primary_assignment.left.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RPrimeCrossRight")
		{
			r_cross_primary_assignment.right.Icon = icon;
			r_cross_primary_assignment.right.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RPrimeCrossDown")
		{
			r_cross_primary_assignment.down.Icon = icon;
			r_cross_primary_assignment.down.ability_name = ability_name;
		}

		if(CheckAssignment(cross, level, bind) == "RSecondCrossUp"){
			r_cross_secondary_assignment.up.Icon = icon;
			r_cross_secondary_assignment.up.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RSecondCrossLeft")
		{
			r_cross_secondary_assignment.left.Icon = icon;
			r_cross_secondary_assignment.left.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RSecondCrossRight")
		{
			r_cross_secondary_assignment.right.Icon = icon;
			r_cross_secondary_assignment.right.ability_name = ability_name;
		}
		else if(CheckAssignment(cross, level, bind) == "RSecondCrossDown")
		{
			r_cross_secondary_assignment.down.Icon = icon;
			r_cross_secondary_assignment.down.ability_name = ability_name;
		}


		// if(cross == "Left")
		// {
		// 	if(level == "Primary")
		// 	{
		// 		if(bind == left_up)
		// 		{
		// 			l_cross_primary_assignment.up.Icon = icon;
		// 			l_cross_primary_assignment.up.ability_name = ability_name;
		// 		}
		// 		else if(bind == left_left)
		// 		{
		// 			l_cross_primary_assignment.left.Icon = icon;
		// 			l_cross_primary_assignment.left.ability_name = ability_name;
		// 		}
		// 		else if(bind == left_right)
		// 		{
		// 			l_cross_primary_assignment.right.Icon = icon;
		// 			l_cross_primary_assignment.right.ability_name = ability_name;
		// 		}
		// 		else if(bind == left_down)
		// 		{
		// 			l_cross_primary_assignment.down.Icon = icon;
		// 			l_cross_primary_assignment.down.ability_name = ability_name;
		// 		}

		// 	}
		// 	else if(level == "Secondary")
		// 	{
		// 		if(bind == left_up)
		// 		{
		// 			l_cross_secondary_assignment.up.Icon = icon;
		// 			l_cross_secondary_assignment.up.ability_name = ability_name;
		// 		}
		// 		else if(bind == left_left)
		// 		{
		// 			l_cross_secondary_assignment.left.Icon = icon;
		// 			l_cross_secondary_assignment.left.ability_name = ability_name;
		// 		}
		// 		else if(bind == left_right)
		// 		{
		// 			l_cross_secondary_assignment.right.Icon = icon;
		// 			l_cross_secondary_assignment.right.ability_name = ability_name;
		// 		}
		// 		else if(bind == left_down)
		// 		{
		// 			l_cross_secondary_assignment.down.Icon = icon;
		// 			l_cross_secondary_assignment.down.ability_name = ability_name;
		// 		}
		// 	}
		// }
		// else if(cross== "Right")
		// {
		// 	if(level == "Primary")
		// 	{
		// 		if(bind == right_up)
		// 		{
		// 			r_cross_primary_assignment.up.Icon = icon;
		// 			r_cross_primary_assignment.up.ability_name = ability_name;
		// 		}
		// 		else if(bind == right_left)
		// 		{
		// 			r_cross_primary_assignment.left.Icon = icon;
		// 			r_cross_primary_assignment.left.ability_name = ability_name;
		// 		}
		// 		else if(bind == right_right)
		// 		{
		// 			r_cross_primary_assignment.right.Icon = icon;
		// 			r_cross_primary_assignment.right.ability_name = ability_name;
		// 		}
		// 		else if(bind == right_down)
		// 		{
		// 			r_cross_primary_assignment.down.Icon = icon;
		// 			r_cross_primary_assignment.down.ability_name = ability_name;
		// 		}

		// 	}
		// 	else if(level == "Secondary")
		// 	{
		// 		if(bind == right_up)
		// 		{
		// 			r_cross_secondary_assignment.up.Icon = icon;
		// 			r_cross_secondary_assignment.up.ability_name = ability_name;
		// 		}
		// 		else if(bind == right_left)
		// 		{
		// 			r_cross_secondary_assignment.left.Icon = icon;
		// 			r_cross_secondary_assignment.left.ability_name = ability_name;
		// 		}
		// 		else if(bind == right_right)
		// 		{
		// 			r_cross_secondary_assignment.right.Icon = icon;
		// 			r_cross_secondary_assignment.right.ability_name = ability_name;
		// 		}
		// 		else if(bind == right_down)
		// 		{
		// 			r_cross_secondary_assignment.down.Icon = icon;
		// 			r_cross_secondary_assignment.down.ability_name = ability_name;
		// 		}
		// 	}
		// }
	}

	public void ClearAbility(string ability_name_old, string ability_name_new)
	{
		foreach(Control control in l_cross_primary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind_button)
			{
				
				
				if(cross_bind_button.ability_name == ability_name_old || cross_bind_button.ability_name == ability_name_new)
				{
					GD.Print("Looking in L Cross " + cross_bind_button.ability_name);
					cross_bind_button.Icon = null;
					cross_bind_button.ability_name = "";
				}
			}
		}

		foreach(Control control in l_cross_secondary_assignment.GetChildren())
		{
			if(control is CrossBindButton cross_bind_button)
			{
				if(cross_bind_button.ability_name == ability_name_old || cross_bind_button.ability_name == ability_name_new)
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
				if(cross_bind_button.ability_name == ability_name_old || cross_bind_button.ability_name == ability_name_new)
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
				if(cross_bind_button.ability_name == ability_name_old || cross_bind_button.ability_name == ability_name_new)
				{
					cross_bind_button.Icon = null;
					cross_bind_button.ability_name = "";
				}
			}
		}
	}

	public string CheckAssignment(string cross, string level, string bind)
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
