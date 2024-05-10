using Godot;
using System;

public partial class BasicAttack : Ability
{
	public override void Execute(player s)
	{
		if(s.attacking)
		{
			s.attacking = true;
			s.hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
			s.hitbox.Monitoring = true;
			s.tree.Set("parameters/conditions/attacking", s.attacking);
			s.can_move = false;
			
		}
		else
		{
			s.attacking = false;
			s.tree.Set("parameters/conditions/attacking", s.attacking);
		}
	}
}
