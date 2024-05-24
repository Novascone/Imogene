using Godot;
using System;

public partial class Roll : Ability
{
	private Vector3 roll_velocity = Vector3.Zero; 
	private int roll_time = 13; // How many frames the player can roll for.
	private bool roll_right = false;
	private bool roll_left = false;
	private  bool roll_back = false;
	private bool roll_forward = false;
	public bool loaded = false;
	private bool rolling = false;
	private Vector3 temp_rotation = Vector3.Zero;
	Timer roll_timer;
	
	// public string description = "rolls";
	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
    {	
		roll_timer = GetNode<Timer>("RollTimer");
		
		_customSignals = _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;
		// _customSignals.AnimationFinished += HandleAnimationFinished;
		_customSignals.AbilityAssigned += HandleAbilityAssigned;
		_customSignals.AbilityRemoved += HandleAbilityRemoved;

		
    }

	public override void _PhysicsProcess(double delta)
	{
		if(player.can_use_abilities && useable && Input.IsActionPressed(assigned_button) && CheckCross())
		{
			player.using_movement_ability = true;
			roll_timer.Start(); // Starts roll timer which is exactly the same time as the roll animation
			Execute();
		}
		if(rolling)
		{
			player.Rotation = temp_rotation; // Gets the rotation of the player the moment they roll
			if(roll_timer.TimeLeft > 0.65) // Sets the speed of the player during the roll, depending on where they are in the roll
			{
				player.velocity = player.velocity.Lerp(roll_velocity, 0.3f);	
				GD.Print("Speeding up");
			}
			else
			{
				player.velocity = player.velocity.Lerp(Vector3.Zero, 0.2f);
				GD.Print("Slowing Down");
			}
			
		}
		else
		{
			temp_rotation = Vector3.Zero; // reset
		}
	}
	public override void Execute()
	{
		temp_rotation = player.Rotation;
		if(player.blend_direction.X > 0 && player.blend_direction.Y > 0) // Determines which roll animation to use
		{
			roll_right = true;
		}
		if(player.blend_direction.X < 0 && player.blend_direction.Y < 0)
		{
			roll_left = true;
		}
		if(player.blend_direction.X > 0 && player.blend_direction.Y < 0)
		{
			roll_right = true;
		}
		if(player.blend_direction.X < 0 && player.blend_direction.Y > 0)
		{
			roll_left = true;
		}
		if(player.blend_direction.X > 0 && player.blend_direction.Y == 0)
		{
			roll_right = true;
		}
		if(player.blend_direction.X < 0 && player.blend_direction.Y == 0)
		{
			roll_left = true;
		}
		if(player.blend_direction.X == 0 && player.blend_direction.Y > 0)
		{
			roll_forward = true;
		}
		if(player.blend_direction.X == 0 && player.blend_direction.Y < 0)
		{
			roll_back = true;
		}
		if(!rolling)
		{
			rolling = true;
			roll_velocity = Vector3.Zero; 
			// roll_velocity = Vector3.Zero; // resets dash_velocity so it always moves in the right direction
			roll_velocity += player.Velocity * 1.5f; // Set roll velocity
			player.tree.Set("parameters/conditions/roll_back", roll_back); // Set animations
			player.tree.Set("parameters/Master/Main/conditions/rolling", roll_forward);
			player.tree.Set("parameters/conditions/roll_left", roll_left);
			player.tree.Set("parameters/conditions/roll_right", roll_right);
		}
		
	}
	
	private void OnAnimationFinished(StringName animName)
    {
        if(animName == "Roll_Forward")
		{
			GD.Print("Roll finished");
			rolling = false;
			player.using_movement_ability = false;
			player.tree.Set("parameters/Master/Main/conditions/rolling", false);
		}
    }

    private void HandleAbilityRemoved(string ability, string button_removed)
    {
        if(this.Name == ability)
		{
			useable = false;
			assigned_button = null;
			cross_type = null;
			cross = null;
		}
    }

    private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    {
        if(this.Name == ability)
		{
			useable = true;
			CheckAssignment(button_name);
		}
    }

	private void HandlePlayerInfo(player s)
    {
        player = s;
		player.tree.AnimationFinished += OnAnimationFinished;
    }
	
}
