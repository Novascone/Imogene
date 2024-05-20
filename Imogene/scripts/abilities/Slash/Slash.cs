using Godot;
using System;
using System.Linq;

public partial class Slash : Ability
{
	Ability jump;
	public override void Execute(player s)
	{
		GD.Print("In use");
		if(s.weapon_type == "one_handed_axe" || s.weapon_type == "two_handed_axe" || s.weapon_type == "one_handed_sword" || s.weapon_type == "two_handed_sword" ||  s.weapon_type == "fist" ||  s.weapon_type == "katana")
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
					GD.Print("Playing one");
					s.tree.Set("parameters/PlayerState/conditions/attacking", s.attacking);
					s.tree.Set("parameters/PlayerState/Attack/AttackState/conditions/not_attacking", true);
					s.tree.Set("parameters/PlayerState/Attack/AttackState/conditions/no_second", true);
					s.tree.Set("parameters/PlayerState/Attack/AttackState/conditions/second_attack", false);
					s.tree.Set("parameters/PlayerState/Attack/AttackState/conditions/loop", false);
					animation_finished = false;
				}
				if(s.ability_in_use.pressed == 2)
				{
					GD.Print("Playing two");
					s.tree.Set("parameters/PlayerState/conditions/attacking", s.attacking);
					s.tree.Set("parameters/PlayerState/Attack/AttackState/conditions/not_attacking", true);
					s.tree.Set("parameters/PlayerState/Attack/AttackState/conditions/no_second", false);
					s.tree.Set("parameters/PlayerState/Attack/AttackState/conditions/second_attack", true);
					s.tree.Set("parameters/PlayerState/Attack/AttackState/conditions/loop", false);
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
			s.can_move = false;
			if(s.jumping)
			{
				in_use = true;
			}
			if(animation_finished && s.ability_in_use.pressed == 0)
			{
				GD.Print("finished");
				in_use = false;
				s.can_move = true;
			}

			
		}
		else
		{
			GD.Print("can not use that ability with equipped weapon");
			s.can_move = true;
			in_use = false;
		}
	}
}
