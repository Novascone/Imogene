using Godot;
using System;

// 																********************************************** MARKED FOR REWORK ************************************************************


public partial class Bash : GeneralAbility
{

	public override void _PhysicsProcess(double delta)
    {
        
    }
    // publ
	// public override void Execute(player s)
	// {
	// 	if(s.weapon_type == "one_handed_hammer" || s.weapon_type == "two_handed_hammer" ||  s.weapon_type == "fist")
	// 	{	
	// 		s.attacking = true;
	// 		s.animation_triggered = true;
	// 		s.blunt_damage = s.weapon_damage;
	// 		GD.Print("weapon damage: " + s.weapon_damage);
	// 		GD.Print("blunt damage: " + s.blunt_damage);
	// 		s.hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
	// 		s.hitbox.Monitoring = true;
	// 		if(s.weapon_type == "one_handed_hammer") // play one handed hammer animation
	// 		{
	// 			s.tree.Set("parameters/conditions/attacking", s.attacking);
	// 		}
	// 		if(s.weapon_type == "two_handed_hammer") // play two handed hammer animation
	// 		{
	// 			s.tree.Set("parameters/conditions/attacking", s.attacking);
	// 		}
	// 		if(s.weapon_type == "fist") // play fist animation
	// 		{
	// 			s.tree.Set("parameters/conditions/attacking", s.attacking);
	// 		}
	// 		s.can_move = false;
	// 		s.using_ability = false;
	// 	}
	// 	else
	// 	{
	// 		GD.Print("can not use that ability with equipped weapon");
	// 		s.can_move = true;
	// 		s.using_ability = false;
	// 	}
	// }
}
