using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class State1 : State
{
   public override void _Ready()
	{
      name = "State1";
	}
   public override async void Enter()
   {
     
      GD.Print("Hello from state 1");
		SceneTreeTimer timer = GetTree().CreateTimer(2.0);
		await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
      Exit("State2");
   }

   public override void Exit(string next_state)
   {
      fsm.ChangeTo(next_state);
   }

}
