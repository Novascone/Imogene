using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class State2 : State
{


   public override void _Ready()
	{
      base._Ready();
      name = "State2";
	}
   
   public override void _PhysicsProcess(double delta)
   {
     
      if(entity.entity_in_detection)
      {
         Exit("State3");
      }
      if(entity != null)
      {
         SetInterest();
         SetDanger();
         ChooseDirection();
      }
   }

      public override  void Enter()
   {
      entity = fsm.this_entity;
      SetInterest();
      GD.Print("Hello from state 2 (forward state)");
      
      // SceneTreeTimer timer = GetTree().CreateTimer(2.0);
      // await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
   }

   public override void SetInterest()
   {
        entity.navigation_agent.TargetPosition = Vector3.Forward;
        target_position_1 = entity.navigation_agent.GetNextPathPosition();
        

         for(int i = 0; i < entity.num_rays; i++)
            {
                // GD.Print(ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) + " dot " + Transform.Basis.Z);

                // Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move
                // (in this case along the vector between Transform.Basis.X and the vector from this entity to the object in contact with)
                var d = entity.ray_directions[i].Rotated(entity.GlobalTransform.Basis.Y.Normalized(), entity.Rotation.Y).Dot(target_position_1.Normalized()); 
                // If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
                entity.interest[i] = MathF.Max(0, d);
                // GD.Print(interest[i]);
            }
    }

   public override void SetDanger()
   {
      var space_state = entity.GetWorld3D().DirectSpaceState;
      for(int i = 0; i < entity.num_rays; i++)
      {
         // Cast a ray from the ray origin, in the ray direction(rotated with player .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) with a magnitude of our look_ahead variable
         var ray_query = PhysicsRayQueryParameters3D.Create(entity.ray_origin, entity.ray_origin + entity.ray_directions[i].Rotated(entity.GlobalTransform.Basis.Y.Normalized(), entity.Rotation.Y) * entity.look_ahead);
         var ray_target = entity.ray_origin + entity.ray_directions[i].Rotated(entity.GlobalTransform.Basis.Y.Normalized(), entity.Rotation.Y) * entity.look_ahead; // Used in SetRayCastLines
         var result = space_state.IntersectRay(ray_query); // Result dictionary from the ray cast
      
         // Uncomment to show ray casts before collision
         // SetRayCastLines(ray_lines,ray_target);

         // *************** Comment/ Uncomment for test behavior ***************
         if(result.Count > 0)
         {
            collider = (Node3D)result["collider"];
            entity.SetCollisionLines(entity.collision_lines, result);
            entity.danger[i] = 1.0f;	
         }
         else
         {
            entity.danger[i] = 0;
         }
      }
   }

   public override void ChooseDirection()
	{
		for(int i = 0; i < entity.num_rays; i++)
		{
			// If there is danger where the ray was cast, set the interest to zero
			// Need to change this to make the changing direction more versatile
			if(entity.danger[i] == 1.0f)
			{
				entity.interest[i] = 0.0f;
			}
		}

		entity.chosen_dir = Vector3.Zero;
		
		for(int i = 0; i < entity.num_rays; i++)
		{

			// Sum up all of the directions where there is interest (if the interest is zero at a given direction that direction will not factor into the chosen direction)
			entity.chosen_dir += entity.ray_directions[i] * entity.interest[i];
			// GD.Print("directions: " + ray_directions[i] * interest[i]);
			// GD.Print("Interest[i] " + interest[i]);

			// Uncomment to show lines the represent the weight of the directions the entity can move in
			entity.SetDirectionLines(entity.direction_lines, entity.ray_directions[i] * entity.interest[i] * entity.direction_lines_mag);
		}

		// Normalize the chosen direction
		entity.chosen_dir = entity.chosen_dir.Normalized();

		// Uncomment to show a line representing the direction the entity is moving in
		entity.SetDirectionMovingLine(entity.direction_moving_line, entity.chosen_dir * entity.direction_line_mag);

		// GD.Print("chosen dir " + chosen_dir);
	}
   

   





}
