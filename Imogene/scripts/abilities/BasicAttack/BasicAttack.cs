using Godot;
using System;

public partial class BasicAttack : Ability
{
	public override void Execute(player s)
	{
			if(s.animation_finished)
			{
				s.using_ability = true;
				s.animation_finished = false;
				s.hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
				s.hitbox.Monitoring = true;
				s.tree.Set("parameters/conditions/attacking", s.attacking);
				s.can_move = false;
			}
			else
			{
				s.using_ability = false;
				s.hitbox.Monitoring = false;
				s.can_move = true;
				s.hitbox.RemoveFromGroup("player_hitbox");
				s.attacking = false;
				s.tree.Set("parameters/conditions/attacking", false);
			}
			
	}
}
