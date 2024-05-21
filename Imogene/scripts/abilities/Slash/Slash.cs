using Godot;
using System;
using System.Linq;
using System.Runtime.Intrinsics.X86;


public partial class Slash : Ability
{
	Ability jump;

	
	public override void Execute(player s)
	{
		GD.Print(s.ability_in_use.pressed);
		if(s.weapon_type == "one_handed_axe" || s.weapon_type == "two_handed_axe" || s.weapon_type == "one_handed_sword" || s.weapon_type == "two_handed_sword" ||  s.weapon_type == "fist")
		{
			if(s.jumping) // Need to do this for other attacks
			{
				GD.Print("jump attack");
				if(jump == null)
				{
					foreach(Ability ability in s.abilities_in_use)
					{
						if(ability.Name == "Jump")
						{
							GD.Print("found jump");
							
							jump = ability;
						}
					}
					
				}
				else
				{
					s.UseAbility(jump);
				}
				
			}
			s.attacking = true;
			s.animation_triggered = true;
			s.slash_damage = s.weapon_damage;
			// GD.Print("weapon damage: " + s.weapon_damage);
			// GD.Print("slash damage: " + s.slash_damage);
			s.hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
			s.hitbox.Monitoring = true;
			if(s.weapon_type == "one_handed_axe") // play one handed axe animation
			{
				if(s.ability_in_use.pressed == 1)
				{
					
					if(!s.attack_1_set)
					{
						GD.Print("Setting swing 1");
						s.tree.Set("parameters/Master/conditions/attacking", s.attacking);
						s.tree.Set("parameters/Master/Attacking/conditions/not_attacking", true);
						s.tree.Set("parameters/Master/Attacking/conditions/no_second", true);
						s.tree.Set("parameters/Master/Attacking/conditions/second_swing", false);
						s.tree.Set("parameters/Master/Attacking/conditions/loop", false);
						s.attack_1_set = true;
						s.attack_2_set = false;
						
					}
					s.velocity.X = Mathf.Lerp(s.velocity.X, 0, 0.5f);
					s.velocity.Z = Mathf.Lerp(s.velocity.Z, 0, 0.5f);
					if(!s.recovery_1)
					{
						s.can_move = false;
						s.velocity.X = s.direction.X * s.speed;
						s.velocity.Z = s.direction.Z * s.speed;
					}
					animation_finished = false;
				}
				if(s.ability_in_use.pressed == 2)
				{
					if(!s.attack_2_set)
					{
						GD.Print("Setting swing 2");
						s.tree.Set("parameters/Master/conditions/attacking", s.attacking);
						s.tree.Set("parameters/Master/Attacking/conditions/not_attacking", true);
						s.tree.Set("parameters/Master/Attacking/conditions/no_second", false);
						s.tree.Set("parameters/Master/Attacking/conditions/second_swing", true);
						s.tree.Set("parameters/Master/Attacking/conditions/loop", false);
						s.attack_2_set = true;
						
					}
					s.velocity.X = Mathf.Lerp(s.velocity.X, 0, 0.5f);
					s.velocity.Z = Mathf.Lerp(s.velocity.Z, 0, 0.5f);
					if(!s.recovery_2)
					{
						s.can_move = false;
						s.velocity.X = s.direction.X * s.speed;
						s.velocity.Z = s.direction.Z * s.speed;
					}
					animation_finished = false;
				}
				if(s.ability_in_use.pressed > 2)
				{
					s.ability_in_use.pressed = 1;
				}
			}
			if(s.weapon_type == "one_handed_sword") // play one handed sword animation
			{
				s.tree.Set("parameters/PlayerState/conditions/attacking", s.attacking);
			}
			if(s.weapon_type == "two_handed_axe") // play two handed axe animation
			{
				s.tree.Set("parameters/PlayerState/conditions/attacking", s.attacking);
			}
			if(s.weapon_type == "two_handed_sword") // play two handed sword animation
			{
				s.tree.Set("parameters/PlayerState/conditions/attacking", s.attacking);
			}
			if(s.weapon_type == "fist") // play fist animation
			{
				s.tree.Set("parameters/PlayerState/conditions/attacking", s.attacking);
			}
			
			
			
			
			
			if(s.jumping)
			{
				in_use = true;
			}
			// if(s.ability_finished && s.ability_in_use.pressed == 0)
			// {
			// 	GD.Print("finished");
			// 	in_use = false;
			// 	s.can_move = true;
			// }

			
		}
		else
		{
			GD.Print("can not use that ability with equipped weapon");
			s.can_move = true;
			in_use = false;
		}
		
	}

	public override void AnimationHandler(player s, string animation)
	{
		
		if(animation == "attack_1")
        {
			GD.Print("Swing 1 Finished");
			s.can_move = true;
			s.recovery_1 = true;
        }
        if(animation == "recovery_1")
        {
            if(s.ability_in_use.pressed == 1)
			{
				GD.Print("Recovery 1 Finished");
				s.attacking = false;
				s.animation_finished = true;
				s.tree.Set("parameters/Master/conditions/attacking", false);
				s.tree.Set("parameters/Master/Attacking/conditions/not_attacking", true);
				s.tree.Set("parameters/Master/Attack/conditions/no_second", true);
				s.tree.Set("parameters/Master/Attack/conditions/second_swing", false);
				s.tree.Set("parameters/Master/Attack/conditions/loop", false);
				s.hitbox.Monitoring = false;
				s.recovery_1 = false;
				s.can_move = true;
				s.ability_in_use.pressed -= 1;
				GD.Print("one presses");
				s.hitbox.RemoveFromGroup("player_hitbox");
				s.ability_in_use.animation_finished = true;
				s.attack_1_set = false;
			}
        }
        if(animation == "attack_2")
        {
            s.recovery_2 = true;
			s.attacking = false;
			s.animation_finished = true;
			s.tree.Set("parameters/Master/conditions/attacking", false);
			s.tree.Set("parameters/Master/Attacking/conditions/not_attacking", true);
			s.tree.Set("parameters/Master/Attacking/conditions/no_second", true);
			s.tree.Set("parameters/Master/Attacking/conditions/second_swing", false);
			s.tree.Set("parameters/Master/Attacking/conditions/loop", false);
			s.hitbox.Monitoring = false;
			s.can_move = true;
			s.hitbox.RemoveFromGroup("player_hitbox");
			GD.Print("Swing 2 Finished");
			GD.Print("two presses");
			if(s.ability_in_use.pressed != 1)
			{
				s.ability_in_use.pressed = 0;
			}
			
			s.ability_in_use.animation_finished = true;
			
        }
        if(animation == "recovery_2")
        {
            s.attack_1_set = false;
			s.recovery_1 = false;
			s.recovery_2 = false;
			s.attack_2_set = false;
        }
	}
	

    
}
