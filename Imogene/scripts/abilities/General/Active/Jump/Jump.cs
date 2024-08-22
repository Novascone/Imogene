using Godot;
using System;

public partial class Jump : Ability
{
	bool off_floor;
	private Timer coyote_timer = new Timer();
	private Timer clamber = new Timer();
	private bool is_climbing;

	public override void _Ready()
    {
		coyote_timer = GetNode<Timer>("Coyote"); // Timer for coyote jump
		clamber = GetNode<Timer>("Clamber");
		rotate_on_soft = false;
		clamber.Timeout += OnClamberTimeout;
    }

    private void OnClamberTimeout() // The player is forced to move forward and up until the Clamber timer times out
    {
		GD.Print("clamber timeout");
        player.is_climbing = false;
		player.is_clambering = false;
		player.jumping = false;
		player.move_forward_clamber = 0;
		frames_held = 0;
		button_held = false;
		player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", true); // Set animation to land
		
    }

    public override void _PhysicsProcess(double delta)
    {
		if(!player.IsOnFloor() && !player.jumping && coyote_timer.TimeLeft == 0) // Start the coyote timer if it hasn't started, and the play is not on the floor and is not jumping
		{
			coyote_timer.Start();
		}
	
		Climb();
		if(player.jumping)
		{
			CheckGround();
		}
		

		if(player.can_use_abilities && CheckCross() || player.jumping)
		{
			if(!player.near_wall.IsColliding())
			{
				if(!player.is_climbing && !CheckHeld() && Input.IsActionJustPressed(assigned_button) && state == States.not_queued) // If the player is not climbing and the button is not held, and the button has been released
				{
					if(CanJump()){QueueAbility();}
				}
				CheckCanUseAbility();
			}
			else if (player.near_wall.IsColliding())
			{
				if(!player.is_climbing && !CheckHeld() && Input.IsActionJustReleased(assigned_button) && state == States.not_queued) // If the player is not climbing and the button is not held, and the button has been released
				{
					if(CanJump()){QueueAbility();}
				}
				CheckCanUseAbility();
			}
			
		}
    }

	public void Climb() // Checks if player is near wall and sets state to climb if the player presses the climb button
	{
		if(player.near_wall.IsColliding())
		{
			if(player.on_wall.IsColliding())
			{
				// GD.Print(player.near_wall.GetCollider());
				if(CheckHeld() && !player.is_climbing)
				{
					GD.Print("setting climbing to true");
					player.is_climbing = true;
				}
				else if(Input.IsActionJustPressed(assigned_button) && player.is_climbing) // If the player pushes the button assigned to jump while climbing, stop climbing
				{
					GD.Print("Setting climbing to false");
					player.is_climbing = false;
				}
				
			}
			else
			{
				if(!player.is_clambering)
				{
					Clamber(); // If the to ray cast is no longer making contact, clamber
				}
			}
		}
		else
		{
			player.is_climbing = false;
		}
		
	}

	public async void Clamber()
	{
		GD.Print("Player can clamber");
		if(CheckHeld())
		{
			player.is_clambering = true;
			var upward_movement = player.GlobalTransform.Origin + new Vector3(0,1.85f,0); // Move the plater up bt 1.85 units
			var upward_move_time = 0.4; // set how long the tween to take to move upward
			var upward_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.In); // Create the tween and sets its transition and ease
			upward_tween.SetProcessMode(0);
			upward_tween.TweenProperty(player, "global_transform:origin", upward_movement, upward_move_time); // Tell the tween which object should be moved and what property of that object should be changed, how it should be changed, and how long it should take

			await ToSignal(upward_tween, Tween.SignalName.Finished); // Wait for vertical movement tween to complete

			var forward_movement = player.GlobalTransform.Origin + (-player.Transform.Basis.Z * 2f); // Get the players forward vector and multiply it by 2 for the forward movement
			var forward_move_time = 0.2; // Set forward move time
			var forward_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Linear); // Create tween set transition type
			forward_tween.SetProcessMode(0);
			forward_tween.TweenProperty(player, "global_transform:origin", forward_movement, forward_move_time); // Same as the upward tween

			player.move_forward_clamber = -1; // Make the player move in the direction they are facing
			clamber.Start(); // Start the clamber timer

			}
	}

    public override void Execute()
    {	
		AddToAbilityList(this);
		state = States.not_queued;
		if((player.IsOnFloor() || coyote_timer.TimeLeft > 0) && !player.jumping) // If player is on the floor and not jumping (add double jump later) set the players velocity to its jump speed 
		{
			GD.Print("start jumping");
			player.tree.Set("parameters/Master/Main/conditions/jumping", true); // Set animation to jumping
			player.velocity.Y = player.jump_speed;			
			player.jumping = true;
		}
		
	}

	public bool CanJump()
	{
		if(player.ability_in_use != null)
		{
			if(player.ability_in_use.resource.type != "movement")
			{
				GD.Print("Can not jump because the ability in use is not movement");
				return false;
			}
			else
			{
				GD.Print("Can jump because the ability in use is movement");
				return true;
			}
		}
		GD.Print("Can jump because no other ability is being used");
		return true;

	}

	public void CheckGround()
	{
		if(player.IsOnFloor())
		{
			GD.Print("stop jumping");
			player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", true); // Set animation to land
			off_floor = false;
			player.jumping = false;
			player.velocity.Y = 0;
			RemoveFromAbilityList(this);
		}
		else if(!player.IsOnFloor())
		{
			player.tree.Set("parameters/Master/Main/conditions/jumping", false);
			player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", false); // Set animation to fall
			// GD.Print("still jumping");
			player.jumping = true;
		}
	}
}
