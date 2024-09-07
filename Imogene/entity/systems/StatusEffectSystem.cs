using Godot;
using System;

public partial class StatusEffectSystem : EntitySystem
{
	// public Timer slow_timer;
	// public Timer stun_timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		// slow_timer = GetNode<Timer>("SlowTimer");
		// slow_timer.Timeout += OnSlowTickTimeout;

		// stun_timer = GetNode<Timer>("StunTimer");
		// stun_timer.Timeout += OnStunTickTimeout;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
	}

	// Crowd control de-buffs

	// public void Slow()
	// {
	// 	slow_timer.Start();
	// 	if(!entity.slowed)
	// 	{
	// 		entity.speed /= 2;
	// 	}
		
	// 	entity.slowed = true;
	// }

	//  private void OnSlowTickTimeout()
    // {
    //     GD.Print(entity.identifier + " is slowed for " + entity.slow_duration);
		
	// 	entity.slow_duration -= 1;
	// 	if(entity.slow_duration == 0)
	// 	{
	// 		slow_timer.Stop();
	// 		entity.slowed = false;
	// 		entity.speed = entity.speed *= 2;
	// 	}
    // }

	// public void Daze()
	// {

	// }

	// public void Chill()
	// {

	// }

	// public void Frozen()
	// {

	// }

	// public void Fear()
	// {

	// }

	// public void Hamstrung()
	// {

	// }

	// public void Tethered()
	// {

	// }



	// public void Stun()
	// {
	// 	stun_timer.Start();
	// 	entity.can_move = false;
	// 	entity.stunned = true;
	// 	if(entity.posture_broken)
	// 	{
	// 		GD.Print(entity.identifier + " posture broken");
	// 	}
	// }

	// private void OnStunTickTimeout()
    // {

    //    GD.Print(entity.identifier + " is stunned for " + entity.stun_duration);

	// 	if(entity.stun_duration > 0)
	// 	{
	// 		entity.stun_duration -= 1;
	// 	}
	// 	else
	// 	{
	// 		entity.stun_duration = 5;
	// 	}
	   
	//    if(entity.stun_duration == 0)
	//    {
	// 		stun_timer.Stop();
	// 		entity.stunned = false;
	// 		entity.can_move = true;
	//    }
    // }

	// public void Hex()
	// {

	// }

	// public


}
