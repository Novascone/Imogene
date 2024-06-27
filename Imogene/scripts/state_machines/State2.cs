using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class State2 : State
{
   public override void _Ready()
	{
      name = "State2";
	}
   public override  void Enter()
   {
        entity = fsm.this_entity;
      GD.Print("Hello from state 2");
      GD.Print(entity.Name);
    
     

      
      
      
		// SceneTreeTimer timer = GetTree().CreateTimer(2.0);
		// await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
      
		
   }

    public override void _PhysicsProcess(double delta)
    {
        if(entity.in_contact_with_rotate_box)
        {
            Exit("State3");
        }
    }

    

    public override void Exit(string next_state)
   {
      fsm.ChangeTo(next_state);
   }


}
