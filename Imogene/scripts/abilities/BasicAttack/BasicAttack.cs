using Godot;
using System;

public partial class BasicAttack : Ability
{

	public override void Execute(player s)
	{

		if(s.attacking)

		{
			GD.Print("attacking");
			s.hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
			s.hitbox.Monitoring = true;
			s.tree.Set("parameters/conditions/attacking", true);
			s.can_move = false;
			s.using_ability = false;
		}
		else
		{
			GD.Print("made it to else");
			s.using_ability = false;
			s.hitbox.Monitoring = false;
			s.can_move = true;
			s.hitbox.RemoveFromGroup("player_hitbox");
			s.attacking = false;	
		}
			
	}
}
