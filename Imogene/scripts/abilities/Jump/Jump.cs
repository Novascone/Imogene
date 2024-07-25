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

    private void OnClamberTimeout()
    {
		GD.Print("clamber timeout");
        player.is_climbing = false;
		player.is_clambering = false;
		player.move_forward_clamber = 0;
		held_threshold = 0;
		button_held = false;
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
		// GD.Print("climbing " + player.is_climbing);
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
			held_threshold += 1;
			if(held.TimeLeft == 0)
			{
				held.Start();
				GD.Print("held started");
			}
		}

		if(held_threshold > 10)
		{
			button_held = true;
		}
		
		
		
		Climb();
		if(player.can_use_abilities && CheckCross() || player.jumping)
		{
			
			AddToAbilityList(this);
			if(!player.is_climbing && held_threshold < 10 && button_released)
			{
				// GD.Print("Player is not climbing");
				GD.Print("held threshold going into jump " + held_threshold);
				Execute();
				held_threshold = 0;
			}
			else if(held_threshold > 10)
			{
				GD.Print("button held");
				if(!player.on_wall.IsColliding())
				{
					held_threshold = 0;
					button_held = false;
				}
			}
			
		}
		GD.Print(held_threshold);
		GD.Print(button_pressed);
		
    }

	public void Climb() // Checks if player is near wall and sets state to climb if the player presses the climb button
	{
		if(player.near_wall.IsColliding())
		{
			if(player.on_wall.IsColliding())
			{
				if(button_held && !player.is_climbing)
				{
					GD.Print("setting climbing to true");
					player.is_climbing = true;
				}
				else if(Input.IsActionJustPressed(assigned_button) && player.is_climbing)
				{
					GD.Print("Setting climbing to false");
					held_threshold = 0;
					button_held = false;
					player.is_climbing = false;
				}
				
			}
			else
			{
				Clamber();
			}
		}
	}

	public void Clamber() // Need to write to boost the player over the edge when climbing, and for use clambering over other objects
	{
		GD.Print("Clamber now");
		player.is_clambering = true;
		player.velocity.Y = 20;
		player.move_forward_clamber = -1;
		GD.Print(player.Velocity.Y);
		clamber.Start();
		GD.Print("Clamber start");
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
			GD.Print("stop jumping");
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
			GD.Print("still jumping");
			player.jumping = true;
		}
	}
		
    
	
}
