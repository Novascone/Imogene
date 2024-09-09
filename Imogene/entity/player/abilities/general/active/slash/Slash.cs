using Godot;
using System;
using System.Linq;
using System.Runtime.Intrinsics.X86;


public partial class Slash : Ability
{
	Timer swing_timer;
	Timer secondary_swing_timer;
	Timer held_timer;
	Timer release_timer;
	private bool held = false;

	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
    {
		swing_timer = GetNode<Timer>("SwingTimer");
		secondary_swing_timer = GetNode<Timer>("SecondarySwingTimer"); // Timer for swinging not used 
		held_timer = GetNode<Timer>("HeldTimer"); // Timer to see how long attack is held handles if the play is holding down the button
		release_timer = GetNode<Timer>("ReleaseTimer");	// Timer that starts when the attack button is released
		_customSignals = _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		rotate_on_soft = true;
		rotate_on_held = false;
		rotate_on_soft_close = true;
		rotate_on_soft_far = false;
		// _customSignals.AbilityAssigned += HandleAbilityAssigned;
		// _customSignals.AbilityRemoved += HandleAbilityRemoved;
    }

    public override void _PhysicsProcess(double delta)
    {
		// GD.Print("action 2 set "  + player.action_2_set);
		
		// if(CheckHeld())
        // {
        //     player.movementController.rotation_only = true;
        //     GD.Print("Ability is making player only able to rotate");
        // }
        // else
        // {
        //     player.movementController.rotation_only = false;
        // }
		// GD.Print("slash held " + button_held);
		// if(player.can_move == false)
		// {
		// 	player.velocity.X = 0;
		// 	player.velocity.Z = 0;
		// }
		// if(player.recovery_1 || player.recovery_2) // If player is in recovery slow down the speed of the player
		// {
		// 	player.speed = 3.0f;
		// }
		// // else
		// // {
		// // 	player.speed = player.walk_speed;
		// // }
		// if(swing_timer.TimeLeft == 0 && pressed >= 2 && !held) // Resets the amount of times pressed if the button is not held and pressed is greater than 2
		// {
		// 	// GD.Print("Pressed was greater than two and the swing timer expired it has been reset");
		// 	pressed = 0;
		// 	// GD.Print("Pressed: " + pressed);
		// 	player.can_move = true;
		// 	player.attacking = false;
		// }
		// if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued)
		// {
		// 	QueueAbility();
		// }
		// else if (CheckHeld())
		// {
		// 	if(swing_timer.TimeLeft == 0)
		// 	{
		// 		QueueAbility();
		// 		CheckCanUseAbility();
		// 		GD.Print("using and holding ability");
		// 	}		
		// }
		// if(swing_timer.TimeLeft == 0)
		// {
		// 	CheckCanUseAbility();
		// }		
		// if(CheckHeld())
		// {
		// 	player.movementController.rotation_only = true;
		// 	GD.Print("Ability is making player only able to rotate");
		// }
		// if(Input.IsActionJustReleased(assigned_button))
		// {
		// 	player.movementController.rotation_only = false;
		// }
		// GD.Print("Projectile held " + button_held);
		// if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued)
		// {
		// 	QueueAbility();
		// }
		// else if (CheckHeld())
		// {
		// 	if(swing_timer.TimeLeft == 0)
		// 	{
		// 		QueueAbility();
		// 		CheckCanUseAbility();
		// 		GD.Print("using and holding ability");
		// 	}		
		// }
		// if(swing_timer.TimeLeft == 0)
		// {
		// 	CheckCanUseAbility();
		// }		
		// if(Input.IsActionJustPressed(assigned_button) && !ready_to_use)
		// {
		// 	ready_to_use = true;
		// }
		// if(ready_to_use)
		// {
		// 	if(player.can_use_abilities && useable && CheckCross() && swing_timer.TimeLeft == 0)
		// 	{
		// 		if(!player.targeting && player.targeting_system.closest_enemy_soft != null && player.targeting_system.soft_target_on && player.targeting_system.enemy_in_soft_small)
		// 		{
		// 			player.targeting_system.SoftTargetRotation();
		// 			if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
		// 			{
		// 				Execute();
		// 			}
		// 		}
		// 		else
		// 		{
		// 			Execute();
		// 		}
				// if(held == false && pressed == 0) // Starts timer to see if the action is being held down
				// {
				// 	held_timer.Start();
				// }
				// if(held)
				// {
				// 	pressed += 1;
				// }
				// else if(Input.IsActionJustPressed(assigned_button))  // Executes the ability, and increments how many times it is pressed
				// {
				// 	if(swing_timer.TimeLeft == 0)
				// 	{
				// 		pressed += 1;
				// 		Execute();
				// 	}
				// 	else if(swing_timer.TimeLeft > 0)
				// 	{
				// 		pressed += 1;
				// 		// GD.Print("Pressed while swinging");
				// 		Execute();
				// 	}
				// 	if(secondary_swing_timer.TimeLeft == 0 && pressed > 1)
				// 	{
				// 		secondary_swing_timer.Start();
				// 		// GD.Print("Secondary timer started");
				// 		held = true;
				// 	}
				// 	{

				// 	}
				// 	if(swing_timer.TimeLeft == 0)
				// 	{
				// 		// GD.Print("Timer started");
				// 		swing_timer.Start();
				// 	}

				// }
				
		// 	}
			
		
		// }		
		// if(player.can_use_abilities && useable && button_pressed && CheckCross()) 
		// {
		// 	// GD.Print(held);
		// 	AddToAbilityList(this); // Adds ability to list of abilities the player is using
		// 	// GD.Print("adding slash to list");
			
		// 	if(held == false && pressed == 0) // Starts timer to see if the action is being held down
		// 	{
		// 		held_timer.Start();
		// 	}
		// 	if(held)
		// 	{
		// 		pressed += 1;
		// 	}
		// 	else if(Input.IsActionJustPressed(assigned_button))  // Executes the ability, and increments how many times it is pressed
		// 	{
		// 		if(swing_timer.TimeLeft == 0)
		// 		{
		// 			pressed += 1;
		// 			Execute();
		// 		}
		// 		else if(swing_timer.TimeLeft > 0)
		// 		{
		// 			pressed += 1;
		// 			// GD.Print("Pressed while swinging");
		// 			Execute();
		// 		}
		// 		if(secondary_swing_timer.TimeLeft == 0 && pressed > 1)
		// 		{
		// 			secondary_swing_timer.Start();
		// 			// GD.Print("Secondary timer started");
		// 			held = true;
		// 		}
		// 		{

		// 		}
		// 		if(swing_timer.TimeLeft == 0)
		// 		{
		// 			// GD.Print("Timer started");
		// 			swing_timer.Start();
		// 		}

		// 	}
			
			
		// }
		if(Input.IsActionJustReleased(assigned_button)) // Resets timers if button is released
		{
			// held = false;\
			release_timer.Start();
			held_timer.Stop();
		}
    }
    
    public override void Execute(Player player) // checks weapon type sets animations enables the hitbox 
	{
		// GD.Print("execute");
		// GD.Print("Pressed in execute: " + pressed);
		state = States.not_queued;
		stop_movement_input = true;
		
		// player.targeting_system.SoftTargetRotation();
		if(player.weapon_type == "one_handed")
		{
			OneHanded(player);
		}

		// player.hitbox.AddToGroup("player_hitbox"); // Adds weapon to attacking group
		player.main_hand_hitbox.AddToGroup("ActiveHitbox");
		
		// player.hitbox.Monitoring = true;
		// player.main_hand_hitbox.damage_type = "Slash";
		player.main_hand_hitbox.damage_type = "Cold";
		// player.main_hand_hitbox.damage_type = "Lightning";
		// GD.Print("Main Hand damage type: " + player.main_hand_hitbox.damage_type);
		if(player.entity_systems.damage_system.Crit(player))
		{
			GD.Print("Critical!");
			player.main_hand_hitbox.damage = MathF.Round(player.damage * (1 + player.critical_hit_damage), 2);
			player.main_hand_hitbox.posture_damage = player.posture_damage;
			player.main_hand_hitbox.is_critical = true;
			// GD.Print("Main Hand damage: " + player.main_hand_hitbox.damage);
		}
		else
		{
			player.main_hand_hitbox.damage = player.damage;
			player.main_hand_hitbox.posture_damage = player.posture_damage;
			player.main_hand_hitbox.is_critical = false;
			// GD.Print("Main Hand damage: " + player.main_hand_hitbox.damage);
		}
		
		
		
	
		if(player.weapon_type == "one_handed_axe") // play one handed axe animation
		{
			OneHanded(player);

		}
	

		if(player.jumping)
		{
			in_use = true;
		}
	}


	 public override void OnAnimationFinished(StringName animName) // Handles logic needed when animations finish
    {

        if(animName == "Slash_And_Bash_Dual_Wield_Swing_2")
        {
		
            // player.recovery_2 = true;
			// player.tree.Set("parameters/Master/conditions/using_ability", false);
			// player.tree.Set("parameters/Master/Ability/conditions/not_attacking", true);
			// player.tree.Set("parameters/Master/Ability/conditions/no_second", true);
			// player.tree.Set("parameters/Master/Ability/conditions/second_action", false);
			// player.hitbox.Monitoring = false;
			// player.can_move = true;
			// player.hitbox.RemoveFromGroup("player_hitbox");
			// GD.Print(pressed);
			
			if(pressed != 2)
			{
				// GD.Print("Pressed set to zero");
				pressed = 0;
			}
			else
			{
				// GD.Print("Pressed set to 1");
				pressed = 1;
			}
			
        }
        if(animName == "Slash_And_Bash_Dual_Wield_Recovery_2")
        {
            // player.action_1_set = false;
			// player.recovery_1 = false;
			// player.recovery_2 = false;
			// player.action_2_set = false;
			
			EmitSignal(nameof(AbilityFinished),this);
			
			
        }
		if(animName == "Slash_And_Bash_Dual_Wield_Swing_1")
		{
			
			// player.recovery_1 = true;
			// // player.can_move = true;
			// stop_movement_input = true;
			// player.attacking = false;
			// // player.hitbox.Monitoring = false;
			// player.action_1_set = false;
			// player.main_hand_hitbox.RemoveFromGroup("ActiveHitbox");
		}
		if(animName == "Slash_And_Bash_Dual_Wield_Recovery_1")
        {
            if(pressed <= 1 && release_timer.TimeLeft == 0)
			{
				// GD.Print("slash 1 recovery finished");
				// player.tree.Set("parameters/Master/conditions/using_ability", false);
				// player.tree.Set("parameters/Master/Attacking/Ability/conditions/melee", false);
				// player.tree.Set("parameters/Master/Attacking/Ability/Melee_1/conditions/Slash", false);
				// player.tree.Set("parameters/Master/Attacking/Ability/Melee_1/Slash/conditions/One_Handed", false);
				// player.tree.Set("parameters/Master/Attacking/Ability/Melee_1/Slash/One_Handed_Slash_1/conditions/Medium", false);
				// player.hitbox.Monitoring = false;
				// player.recovery_1 = false;
				// player.main_hand_hitbox.RemoveFromGroup("ActiveHitbox");
				// player.action_1_set = false;
				// if(player.action_2_set)
				// {
				// 	// player.can_move = false;
				// }
				// else
				// {
					// player.can_move = true;
					EmitSignal(nameof(AbilityFinished),this);
				// }
			}
        }
    }
	public void DualWield()
	{

	}

	public void OneHanded(Player player)
	{
		if(!player.action_1_set) // Sets swing 1 animation for one handed
		{
			// GD.Print(pressed);
			// GD.Print("Setting swing 1");
			player.action_1_set = true;
			// GD.Print("Setting animations");
			// GD.Print("Playing hitbox animation");
			// player.animation_player.Play("Attack_1_Hitbox_Activation");
			
			player.tree.Set("parameters/Master/conditions/using_ability", true);
			player.tree.Set("parameters/Master/Ability/Ability_1/conditions/melee", true);
			player.tree.Set("parameters/Master/Ability/Ability_1/Melee_1/conditions/Slash", true);
			player.tree.Set("parameters/Master/Ability/Ability_1/Melee_1/Slash/conditions/One_Handed", true);
			player.tree.Set("parameters/Master/Ability/Ability_1/Melee_1/Slash/One_Handed_Slash_1/conditions/Medium", true);
			// pressed -= 1;
			// player.attack_2_set = false;
			// player.can_move = false;
			player.attacking = true;
			// GD.Print(player.can_move);
			// if(pressed > 1)
			// {
			// 	player.action_2_set = true;
			// 	// GD.Print("Attack 2 set");
			// }
			
		}
	}

	public void TwoHanded()
	{

	}

	public void _on_swing_timer_timeout() // decrements presses on primary swing timer timeout
	{
		// GD.Print("timeout");
		
		if(pressed > 0)
		{
			// GD.Print("Pressed was at: " + pressed);
			pressed -= 1;
			// GD.Print("Press decremented to: " + pressed);
		}
		
	}

	public void _on_secondary_swing_timer_timeout() // decrements presses on secondary swing timer timeout
	{
		if(pressed > 0)
		{
			// GD.Print("Pressed was at: " + pressed);
			pressed -= 1;
			// GD.Print("Press decremented to: " + pressed);
		}
	}

	public void _on_held_timer_timeout() // If the button has been held for the duration of the held timer set held to true
	{
		held = true;
	}

	public void _on_release_timer_timeout() // Tracks time after release of input, it is the same length of time as the attack animation. It is used to check when the player is finished with inputs, and its time to reset animations to the normal states
	{
		held = false;
	}

	// private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    // {
    //     if(this.Name == ability)
	// 	{
	// 		useable = true;
	// 		CheckAssignment(button_name);
	// 	}
    // }

	// private void HandleAbilityRemoved(string ability, string button_removed)
    // {
    //     if(this.Name == ability)
	// 	{
	// 		useable = false;
	// 		assigned_button = null;
	// 		cross_type = null;
	// 		cross = null;
	// 	}
    // }

    
    
}