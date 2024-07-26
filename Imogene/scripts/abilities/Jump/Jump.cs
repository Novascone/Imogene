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
	private bool button_held;
	int held_threshold = 0;
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
		held_threshold = 0;
		button_held = false;
		button_released = false;
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
		GD.Print("jumping " + player.jumping);
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
			held_threshold += 1; // If the button is pressed start counting the amount of frames its pressed
			// if(held.TimeLeft == 0)
			// {
			// 	held.Start();
			// 	GD.Print("held started");
			// }
		}

		if(held_threshold > 10)
		{
			button_held = true;
		}
		
		
		
		Climb();
		if(player.can_use_abilities && CheckCross() || player.jumping)
		{
			
			AddToAbilityList(this);
			if(!player.is_climbing && held_threshold < 10 && button_released) // If the player is not climbing and the button has been held for less than 10 frames, and the button has been released
			{
				// GD.Print("Player is not climbing");
				// GD.Print("held threshold going into jump " + held_threshold);
				Execute(); // Jump
				held_threshold = 0; // Reset the frames the button has been held
			}
			else if(held_threshold > 10) // If the button has been held for more than 10 frames
			{
				GD.Print("button held");
				if(!player.on_wall.IsColliding()) // If the player is not near a wall reset how many frames the button has been held
				{
					held_threshold = 0;
					button_held = false;
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
				}
				else if(Input.IsActionJustPressed(assigned_button) && player.is_climbing) // If the player pushes the button assigned to jump while climbing, stop climbing
				{
					GD.Print("Setting climbing to false");
					held_threshold = 0;
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
		
	}

	public async void Clamber()
	{
		GD.Print("Clamber now");
		player.speed = 10f;
		player.is_clambering = true;
		var vertical_movement = player.GlobalTransform.Origin + new Vector3(0,1.85f,0);
		var vertical_move_time = 0.2;
		var vm_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.In);

		vm_tween.TweenProperty(player, "global_transform:origin", vertical_movement, vertical_move_time);

		await ToSignal(vm_tween, Tween.SignalName.Finished);

		var forward_movement = player.GlobalTransform.Origin + (-player.Transform.Basis.Z * 2f);
		var horizontal_move_time = 0.4;
		var fm_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Linear);

		fm_tween.TweenProperty(player, "global_transform:origin", forward_movement, horizontal_move_time);
		// player.velocity.Y = 20; // give the player extra Y velocity
		player.move_forward_clamber = -1; // Make the player move in the direction they are facing
		// GD.Print(player.Velocity.Y);
		clamber.Start(); // Start the clamber timer
		// GD.Print("Clamber start");
		// player.velocity.Z = 10;
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
