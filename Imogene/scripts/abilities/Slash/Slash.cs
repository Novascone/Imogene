using Godot;
using System;
using System.Linq;
using System.Runtime.Intrinsics.X86;


public partial class Slash : Ability
{
	Ability jump;
	Timer swing_timer;
	Timer held_timer;
	player player;
	private int frames_held = 0;
	private bool held = false;
	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
    {
		swing_timer = GetNode<Timer>("SwingTimer");
		held_timer = GetNode<Timer>("HeldTimer");
		cross_type = "primary";
		assigned_button = "RB";
		_customSignals = _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;
		_customSignals.AnimationFinished += HandleAnimationFinished;
		
    }

  

    public override void _PhysicsProcess(double delta)
    {
		// GD.Print("pressed: " + pressed);
		GD.Print(held);
		GD.Print("held time: " + held_timer.TimeLeft);
		if(player.can_move == false)
		{
			player.velocity.X = 0;
			player.velocity.Z = 0;
		}
		if(Input.IsActionPressed(assigned_button))
		{
			if(held == false && pressed == 0)
			{
				held_timer.Start();	
			}
			
			if(held)
			{
				pressed += 1;
			}
			else if(Input.IsActionJustPressed(assigned_button))
			{
				pressed += 1;
			}
			GD.Print(assigned_button);
			GD.Print(frames_held);
			Execute_1();
		}
		if(Input.IsActionJustReleased(assigned_button))
		{
			held = false;
		}
    }
    
    public  void Execute_1()
	{
		if(player.attack_ready)
		{
			swing_timer.Start();
			player.attack_ready = false;
		}
		
	
		// GD.Print("In use " + in_use);
		// GD.Print(s.ability_in_use.pressed);
		if(player.weapon_type == "one_handed_axe" || player.weapon_type == "two_handed_axe" || player.weapon_type == "one_handed_sword" || player.weapon_type == "two_handed_sword" ||  player.weapon_type == "fist")
		{
			if(player.jumping) // Need to do this for other attacks
			{
				GD.Print("jump attack");
				if(jump == null)
				{
					foreach(Ability ability in player.abilities_in_use)
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
					player.UseAbility(jump);
				}
				
			}
			player.attacking = true;
			player.animation_triggered = true;
			player.slash_damage = player.weapon_damage;
			// GD.Print("weapon damage: " + s.weapon_damage);
			// GD.Print("slash damage: " + s.slash_damage);
			player.hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
			player.hitbox.Monitoring = true;
			if(player.weapon_type == "one_handed_axe") // play one handed axe animation
			{
				if(pressed == 1)
				{
					GD.Print("here");
					if(!player.attack_1_set)
					{
						GD.Print("Setting swing 1");
						player.tree.Set("parameters/Master/conditions/attacking", player.attacking);
						player.tree.Set("parameters/Master/Attacking/conditions/not_attacking", true);
						player.tree.Set("parameters/Master/Attacking/conditions/no_second", true);
						player.tree.Set("parameters/Master/Attacking/conditions/second_swing", false);
						player.tree.Set("parameters/Master/Attacking/conditions/loop", false);
						player.attack_2_set = false;
						
					}
					if(!player.recovery_1)
					{
						player.can_move = false;
						
					}
					animation_finished = false;
				}
				if(pressed == 2)
				{
					if(!player.attack_2_set)
					{
						GD.Print("Setting swing 2");
						player.tree.Set("parameters/Master/conditions/attacking", player.attacking);
						player.tree.Set("parameters/Master/Attacking/conditions/not_attacking", true);
						player.tree.Set("parameters/Master/Attacking/conditions/no_second", false);
						player.tree.Set("parameters/Master/Attacking/conditions/second_swing", true);
						player.tree.Set("parameters/Master/Attacking/conditions/loop", false);
						player.attack_2_set = true;
						player.attack_1_set = false;
						
					}
				
					if(!player.recovery_2)
					{
						player.can_move = false;
					}
					animation_finished = false;
				}
				if(pressed > 2)
				{
					pressed = 1;
				}
			}
			if(player.weapon_type == "one_handed_sword") // play one handed sword animation
			{
				player.tree.Set("parameters/PlayerState/conditions/attacking", player.attacking);
			}
			if(player.weapon_type == "two_handed_axe") // play two handed axe animation
			{
				player.tree.Set("parameters/PlayerState/conditions/attacking", player.attacking);
			}
			if(player.weapon_type == "two_handed_sword") // play two handed sword animation
			{
				player.tree.Set("parameters/PlayerState/conditions/attacking", player.attacking);
			}
			if(player.weapon_type == "fist") // play fist animation
			{
				player.tree.Set("parameters/PlayerState/conditions/attacking", player.attacking);
			}
	
			if(player.jumping)
			{
				in_use = true;
			}
		}
		else
		{
			GD.Print("can not use that ability with equipped weapon");
			player.can_move = true;
			in_use = false;
		}
		
	}

	 private void HandlePlayerInfo(player s)
    {
        GD.Print("Player info received in slash");
        player = s;
        GD.Print(player.Name);
    }

	  private void HandleAnimationFinished(string animation)
    {
        if(animation == "attack_1")
        {
			GD.Print("Swing 1 Finished");
			player.can_move = true;
			player.recovery_1 = true;
        }
        if(animation == "recovery_1")
        {
            if(pressed == 1)
			{
				GD.Print("Recovery 1 Finished");
				player.attacking = false;
				player.animation_finished = true;
				player.tree.Set("parameters/Master/conditions/attacking", false);
				player.tree.Set("parameters/Master/Attacking/conditions/not_attacking", true);
				player.tree.Set("parameters/Master/Attack/conditions/no_second", true);
				player.tree.Set("parameters/Master/Attack/conditions/second_swing", false);
				player.tree.Set("parameters/Master/Attack/conditions/loop", false);
				player.hitbox.Monitoring = false;
				player.recovery_1 = false;
				player.can_move = true;
				pressed -= 1;
				GD.Print("one presses");
				player.hitbox.RemoveFromGroup("player_hitbox");
				player.ability_in_use.animation_finished = true;
				player.attack_1_set = false;
				if(!player.attack_2_set)
				{
					in_use = false;
				}
			}
        }
        if(animation == "attack_2")
        {
            player.recovery_2 = true;
			player.attacking = false;
			player.animation_finished = true;
			player.tree.Set("parameters/Master/conditions/attacking", false);
			player.tree.Set("parameters/Master/Attacking/conditions/not_attacking", true);
			player.tree.Set("parameters/Master/Attacking/conditions/no_second", true);
			player.tree.Set("parameters/Master/Attacking/conditions/second_swing", false);
			player.tree.Set("parameters/Master/Attacking/conditions/loop", false);
			player.hitbox.Monitoring = false;
			player.can_move = true;
			player.hitbox.RemoveFromGroup("player_hitbox");
			GD.Print("Swing 2 Finished");
			GD.Print("two presses");
			if(pressed != 1)
			{
				pressed = 0;
			}
			
			player.ability_in_use.animation_finished = true;
			
			
        }
        if(animation == "recovery_2")
        {
            player.attack_1_set = false;
			player.recovery_1 = false;
			player.recovery_2 = false;
			player.attack_2_set = false;
			if(!player.attack_1_set)
			{
				in_use = false;
			}
			
        }
    }
	
	public void _on_swing_timer_timeout()
	{
		GD.Print("timeout");
	}

	public void _on_held_timer_timeout()
	{
		held = true;
	}
    
}
