using Godot;
using System;
using System.Linq;
using System.Runtime.Intrinsics.X86;


public partial class Slash : Ability
{

	public override void _Ready()
	{
		
		RotateOnSoft = true;
		RotateOnHeld = true;
		DamageModifier = 1;
    }


	public override void Execute(Player player)
	{
		base.Execute(player);
		GD.Print("Execute slash");
		if(UseTimer.TimeLeft == 0)
		{
			UseTimer.Start();
			
		}
	}


	public void _on_swing_timer_timeout()
	{
		if(ButtonReleased && !AbilityExecuted)
		{
			EmitSignal(nameof(AbilityFinished),this);
			AbilityExecuted = true;
		}
	}




    
    
}
