using Godot;
using System;
using System.Linq;
using System.Runtime.Intrinsics.X86;


public partial class Slash : Ability
{

	public override void _Ready()
	{
		
		rotate_on_soft = true;
		rotate_on_held = true;
		rotate_on_soft_far = true;
		rotate_on_soft_close = true;
		ability_damage_modifier = 1;
    }


	public override void Execute(Player player)
	{
		base.Execute(player);
		GD.Print("Execute slash");
		
		state = States.not_queued;
		if(use_timer.TimeLeft == 0)
		{
			use_timer.Start();
			
		}
	}


	public void _on_swing_timer_timeout()
	{
		if(button_released && !ability_finished)
		{
			EmitSignal(nameof(AbilityFinished),this);
			ability_finished = true;
		}
	}




    
    
}