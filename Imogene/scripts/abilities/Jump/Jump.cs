using Godot;
using System;

public partial class Jump : Ability
{
	bool off_floor;
	private Timer coyote = new Timer();
	private bool coyote_elapsed = false;
	int timer = 0;
	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
    {
		coyote = GetNode<Timer>("Coyote");
		
		_customSignals = _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;
		// _customSignals.KeyBind += HandleKeyBind;
		_customSignals.AbilityAssigned += HandleAbilityAssigned;

		
    }


	private void HandleAbilityAssigned(string ability, string button_name, Texture2D icon)
    {
        if(this.Name == ability)
		{
			CheckAssignment(button_name);
		}
    }

    private void HandlePlayerInfo(player s)
    {
        player = s;
    }
	  public override void _PhysicsProcess(double delta)
    {
		if(!player.IsOnFloor() && !coyote_elapsed)
		{
			GD.Print("coyote timer started");
			coyote.Start();
			coyote_elapsed = true;
		}
		GD.Print(coyote.TimeLeft);
		// GD.Print(in_use);
		if(player.can_move == false)
		{
			player.velocity.X = 0;
			player.velocity.Z = 0;
		}
		if(player.can_use_abilities && Input.IsActionPressed(assigned_button) && CheckCross() || player.jumping)
		{
			AddToAbilityList(this);
			Execute();
		}
		
    }

    // Called when the node enters the scene tree for the first time.
    public override void Execute()
    {	
		
		if((player.IsOnFloor() || coyote.TimeLeft > 0) && !player.jumping) // If player is on the floor and not jumping (add double jump later) set the players velocity to its jump speed 
		{
			// GD.Print("start jumping");
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
			
			RemoveFromAbilityList(this);
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
