using Godot;
using System;
using System.Threading.Tasks;

public partial class State3: State
{
    public override void _Ready()
	{
      name = "State3";
	}
   public override  void Enter()
   {
        entity = fsm.this_entity;
       
        GD.Print("Hello from state 3");

        // SceneTreeTimer timer = GetTree().CreateTimer(2.0);
        // await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
        
    
   }

    public override void _PhysicsProcess(double delta)
    {
        if(entity.switch_to_state2)
        {
            Exit("State2");
        }
    }

    public override void Exit(string next_state)
   {
        fsm.Back();
   }

}
