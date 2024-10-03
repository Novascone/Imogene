using Godot;
using System;

public partial class Dash : Ability
{

	private Timer dash_timer;
	private float dash_speed = 15.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dash_timer = GetNode<Timer>("DashTimer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued)
		// {
		// 	QueueAbility();
		// }
		// else if (CheckHeld())
		// {
		// 	if(dash_timer.TimeLeft == 0)
		// 	{
		// 		QueueAbility();
		// 		CheckCanUseAbility();
		// 	}		
		// }
		// if(dash_timer.TimeLeft == 0)
		// {
		// 	CheckCanUseAbility();
		// }		
	}

    public override void Execute(Player player)
    {
		
        state = States.not_queued;
		// player.using_movement_ability = true;
		EmitSignal(nameof(MovementAbilityExecuted), true);
		dash_timer.Start();
		if(player._direction != Vector3.Zero) // If the player is moving, dash in that direction
		{
			player._velocity.X = player._direction.X * dash_speed; 
			player._velocity.Z = player._direction.Z * dash_speed;
		} 
		else // If the player is not moving dash backwards
		{
			// GD.Print("Direction behind player " + player.GlobalTransform.Basis.Z);
			player._velocity = player.GlobalTransform.Basis.Z * dash_speed;
		}
		
		
    }

    public override void FrameCheck(Player player)
    {
        if (CheckHeld())
		{
			if(dash_timer.TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityQueue), this);
				EmitSignal(nameof(AbilityCheck), this);
			}		
		}
		if(dash_timer.TimeLeft == 0)
		{
			EmitSignal(nameof(AbilityCheck), this);
		}		
    }

    public void _on_dash_timer_timeout() // When the dash timer times out remove the ability and reset the player velocity
	{
		EmitSignal(nameof(AbilityFinished), this);
		EmitSignal(nameof(MovementAbilityExecuted), false);
		// player.using_movement_ability = false;
		// player.velocity.X = player.direction.X * player.run_speed; 
		// player.velocity.Z = player.direction.Z * player.run_speed;
	}
}
