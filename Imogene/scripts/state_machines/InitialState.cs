using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class InitialState : State
{
   public override void _Ready()
	{
      base._Ready();
      name = "InitialState";
	}
   public override async void Enter()
   {
     
      GD.Print("Hello from Initial state");
		SceneTreeTimer timer = GetTree().CreateTimer(2.0);
		await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
      if(fsm.this_entity is not StandardEnemy)
      {
         Exit("ForwardState");
      }
      
      GD.Print("Exiting initial state");
      
   }


}
