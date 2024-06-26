using Godot;
using System;
using System.Threading.Tasks;

public partial class State2 : State
{
    public override void _Ready()
	{
      name = "State2";
	}
   public override async void Enter()
   {
       GD.Print("Hello from state 2");

		SceneTreeTimer timer = GetTree().CreateTimer(2.0);
		await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
		Exit("State1");
   }

   public override void Exit(string next_state)
   {
        fsm.Back();
   }

}
