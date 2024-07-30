using Godot;
using System;

public partial class Jump : Ability
{
	bool off_floor;
	private Timer coyote = new Timer();
	private Timer clamber = new Timer();
	private Timer held = new Timer();
	private bool coyote_elapsed = false;
	private bool is_climbing;
	// private bool button_held;
	// int frames_held_threshold = 10;
	// int frames_held = 0;
	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
    {
		coyote = GetNode<Timer>("Coyote");

		clamber = GetNode<Timer>("Clamber");
		clamber.Timeout += OnClamberTimeout;

		held = GetNode<Timer>("Held");
		// held.Timeout += OnHeldTimeout;
		
		_customSignals = _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.PlayerInfo += HandlePlayerInfo;
		// _customSignals.KeyBind += HandleKeyBind;
		// _customSignals.AbilityAssigned += HandleAbilityAssigned;

		
    }

    // private void OnHeldTimeout()
    // {
		
		// if(held_threshold > 10)
		// {
		// 	GD.Print("timeout Button held");
		// 	button_held = true;
		// }
		// GD.Print("timeout");
		
		
        
   // }

    private void OnClamberTimeout() // The player is forced to move forward and up until the Clamber timer times out
    {
		GD.Print("clamber timeout");
        player.is_climbing = false;
		player.is_clambering = false;
		player.jumping = false;
		player.move_forward_clamber = 0;
		frames_held = 0;
		button_held = false;
		button_released = false;
		player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", true); // Set animation to land
    }


    // private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    // {
    //     if(this.Name == ability)
    // 	{
    // 		CheckAssignment(button_name);
    // 	}
    // }

    // private void HandlePlayerInfo(player s)
    // {
    //     player = s;
    // }
    public override void _PhysicsProcess(double delta)
    {

		if(!player.IsOnFloor() && !coyote_elapsed)
		{
			// GD.Print("coyote timer started");
			coyote.Start();
			coyote_elapsed = true;
		}
		// GD.Print(coyote.TimeLeft);
		// GD.Print(in_use);
		if(player.can_move == false)
		{
			player.velocity.X = 0;
			player.velocity.Z = 0;
		}
		
		if(button_pressed)
		{
			frames_held += 1; // If the button is pressed start counting the amount of frames its pressed
		}
		if(frames_held > frames_held_threshold)
		{
			button_held = true;
		}
		
		
		
		Climb();

		if(player.can_use_abilities && CheckCross() || player.jumping)
		{
			
			AddToAbilityList(this);
			if(!player.is_climbing && frames_held < frames_held_threshold && button_released) // If the player is not climbing and the button has been held for less than 10 frames, and the button has been released
			{
				// GD.Print("Player is not climbing");
				// GD.Print("held threshold going into jump " + held_threshold);
				Execute(); // Jump
				frames_held = 0; // Reset the frames the button has been held
			}
			else if(frames_held > frames_held_threshold) // If the button has been held for more than 10 frames
			{
				GD.Print("button held");
				
				if(!player.on_wall.IsColliding()) // If the player is not near a wall reset how many frames the button has been held
				{
					
					if(!player.is_climbing)
					{
						button_released = false;
						button_held = false;
						// frames_held = 0;
					}
					
				}

			}
			
		}
		// GD.Print(held_threshold);
		// GD.Print(button_pressed);
		
    }

	public void Climb() // Checks if player is near wall and sets state to climb if the player presses the climb button
	{
		if(player.near_wall.IsColliding())
		{
			if(player.on_wall.IsColliding())
			{
				// GD.Print(player.near_wall.GetCollider());
				if(button_held && !player.is_climbing)
				{
					GD.Print("setting climbing to true");
					player.is_climbing = true;
					button_held = false;
					frames_held = 0;
				}
				else if(Input.IsActionJustPressed(assigned_button) && player.is_climbing) // If the player pushes the button assigned to jump while climbing, stop climbing
				{
					GD.Print("Setting climbing to false");
					frames_held = 0;
					button_held = false;
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
		if(frames_held >= frames_held_threshold)
		{
			GD.Print("Clamber now");
			player.speed = 10f;
			player.is_clambering = true;
			var upward_movement = player.GlobalTransform.Origin + new Vector3(0,1.85f,0); // Move the plater up bt 1.85 units
			var upward_move_time = 0.2; // set how long the tween to take to move upward
			var upward_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.In); // Create the tween and sets its transition and ease

			upward_tween.TweenProperty(player, "global_transform:origin", upward_movement, upward_move_time); // Tell the tween which object should be moved and what property of that object should be changed, how it should be changed, and how long it should take

			await ToSignal(upward_tween, Tween.SignalName.Finished); // Wait for vertical movement tween to complete

			var forward_movement = player.GlobalTransform.Origin + (-player.Transform.Basis.Z * 2f); // Get the players forward vector and multiply it by 2 for the forward movement
			var forward_move_time = 0.4; // Set forward move time
			var forward_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Linear); // Create tween set transition type

			forward_tween.TweenProperty(player, "global_transform:origin", forward_movement, forward_move_time); // Same as the upward tween
			// player.velocity.Y = 20; // give the player extra Y velocity
			player.move_forward_clamber = -1; // Make the player move in the direction they are facing
			// GD.Print(player.Velocity.Y);
			clamber.Start(); // Start the clamber timer
			// GD.Print("Clamber start");
			// player.velocity.Z = 10;
			}
	}

    // Called when the node enters the scene tree for the first time.
    public override void Execute()
    {	
		
		if((player.IsOnFloor() || coyote.TimeLeft > 0) && !player.jumping) // If player is on the floor and not jumping (add double jump later) set the players velocity to its jump speed 
		{
			GD.Print("start jumping");
			player.tree.Set("parameters/Master/Main/conditions/jumping", true); // Set animation to jumping
			player.velocity.Y = player.jump_speed;			
			player.jumping = true;
		}
		else if(player.IsOnFloor())
		{
			// GD.Print("stop jumping");
			coyote_elapsed = false;
			player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", true); // Set animation to land
			off_floor = false;
			player.jumping = false;
			player.velocity.Y = 0;
			RemoveFromAbilityList(this);
			button_released = false;
		}
		if(!player.IsOnFloor())
		{
			player.tree.Set("parameters/Master/Main/conditions/jumping", false);
			player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", false); // Set animation to fall
			// GD.Print("still jumping");
			player.jumping = true;
		}
	}
		
    
	
}
