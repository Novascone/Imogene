using Godot;
using System;

public partial class Jump : Ability
{
	bool off_floor;
	int timer = 0;
	player player;
	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
    {
		
		cross_type = "primary";
		assigned_button = "A";
		_customSignals = _customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;

		
    }

    private void HandlePlayerInfo(player s)
    {
        player = s;
    }
	  public override void _PhysicsProcess(double delta)
    {
		
		if(player.can_move == false)
		{
			player.velocity.X = 0;
			player.velocity.Z = 0;
		}
		if(Input.IsActionPressed(assigned_button) || player.jumping)
		{
			Execute();
		}
		
    }

    // Called when the node enters the scene tree for the first time.
    public void Execute()
    {	
		
		if(player.IsOnFloor() && !player.jumping)
		{
			GD.Print("start jumping");
			player.tree.Set("parameters/Master/Main/conditions/jumping", true);
			player.velocity.Y = player.jump_speed;			
			player.jumping = true;
			timer += 1;
		}
		else if(player.IsOnFloor())
		{
			GD.Print("stop jumping");
			player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", true);
			off_floor = false;
			player.jumping = false;
			in_use = false;
			timer = 0;
		}
        if(!player.IsOnFloor())
		{
			player.tree.Set("parameters/Master/Main/conditions/jumping", false);
			player.tree.Set("parameters/Master/Main/Jump/JumpState/conditions/on_ground", false);
			GD.Print("still jumping");
			player.jumping = true;
			timer += 1;
		}
		
    }
	
}
