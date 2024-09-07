using Godot;
using System;

public partial class Thrust : Ability
{

    public override void _PhysicsProcess(double delta)
    {
        
    }
    // public override void Execute(player s)
    // {
    // 	if(s.weapon_type == "one_handed_sword" || s.weapon_type == "two_handed_sword" ||  s.weapon_type == "fist" ||  s.weapon_type == "spear")
    // 	{
    // 		s.attacking = true;
    // 		s.animation_triggered = true;
    // 		s.thrust_damage = s.weapon_damage;
    // 		GD.Print("weapon damage: " + s.weapon_damage);
    // 		GD.Print("thrust damage: " + s.thrust_damage);
    // 		s.hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
    // 		s.hitbox.Monitoring = true;
    // 		if(s.weapon_type == "one_handed_sword") // play one handed sword animation
    // 		{
    // 			s.tree.Set("parameters/conditions/attacking", s.attacking);
    // 		}
    // 		if(s.weapon_type == "two_handed_sword") // play two handed sword animation
    // 		{
    // 			s.tree.Set("parameters/conditions/attacking", s.attacking);
    // 		}
    // 		if(s.weapon_type == "fist") // play fist animation
    // 		{
    // 			s.tree.Set("parameters/conditions/attacking", s.attacking);
    // 		}
    // 		if(s.weapon_type == "spear") // play spear animation
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
