using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class ForwardState : State
{


   public override void _Ready()
	{
      base._Ready();
      name = "ForwardState";
	}
   
   public void SwitchState(Enemy enemy)
   {
      if(enemy.entity_in_alert_area)
         {
            Exit("CircleState");
         }
         
         SetInterest(enemy);
         SetDanger(enemy);
         ChooseDirection(enemy);
      
   }

   public override  void Enter(Enemy enemy)
   {
      enemy = fsm.enemy;
      GD.Print("Enemy set");
      GD.Print(enemy.Name);
      
     
      SetInterest(enemy);
      
      
      GD.Print("Hello from forward state");
      
      // SceneTreeTimer timer = GetTree().CreateTimer(2.0);
      // await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
   }

   public override void SetInterest(Enemy enemy)
   {
        enemy.navigation_agent.TargetPosition = Vector3.Forward;
        target_position_1 = enemy.navigation_agent.GetNextPathPosition();
        

         for(int i = 0; i < enemy.num_rays; i++)
            {
                // GD.Print(ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) + " dot " + Transform.Basis.Z);

                // Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move
                // (in this case along the vector between Transform.Basis.X and the vector from this entity to the object in contact with)
                var d = enemy.ray_directions[i].Rotated(enemy.GlobalTransform.Basis.Y.Normalized(), enemy.Rotation.Y).Dot(target_position_1.Normalized()); 
                // If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
                enemy.interest[i] = MathF.Max(0, d);
                // GD.Print(interest[i]);
            }
    }

   public override void SetDanger(Enemy enemy)
   {
      var space_state = enemy.GetWorld3D().DirectSpaceState;
      for(int i = 0; i < enemy.num_rays; i++)
      {
         // Cast a ray from the ray origin, in the ray direction(rotated with player .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) with a magnitude of our look_ahead variable
         var ray_query = PhysicsRayQueryParameters3D.Create(enemy.ray_origin, enemy.ray_origin + enemy.ray_directions[i].Rotated(enemy.GlobalTransform.Basis.Y.Normalized(), enemy.Rotation.Y) * enemy.look_ahead);
         var ray_target = enemy.ray_origin + enemy.ray_directions[i].Rotated(enemy.GlobalTransform.Basis.Y.Normalized(), enemy.Rotation.Y) * enemy.look_ahead; // Used in SetRayCastLines
         var result = space_state.IntersectRay(ray_query); // Result dictionary from the ray cast
      
         // Uncomment to show ray casts before collision
         // SetRayCastLines(ray_lines,ray_target);

         // *************** Comment/ Uncomment for test behavior ***************
         if(result.Count > 0)
         {
            collider = (Node3D)result["collider"];
            enemy.SetCollisionLines(enemy.debug.collision_lines, result);
            enemy.danger[i] = 1.0f;	
         }
         else
         {
            enemy.danger[i] = 0;
         }
      }
   }

   public override void ChooseDirection(Enemy enemy)
	{
		for(int i = 0; i < enemy.num_rays; i++)
		{
			// If there is danger where the ray was cast, set the interest to zero
			// Need to change this to make the changing direction more versatile
			if(enemy.danger[i] == 1.0f)
			{
				enemy.interest[i] = 0.0f;
			}
		}

		enemy.chosen_dir = Vector3.Zero;
		
		for(int i = 0; i < enemy.num_rays; i++)
		{

			// Sum up all of the directions where there is interest (if the interest is zero at a given direction that direction will not factor into the chosen direction)
			enemy.chosen_dir += enemy.ray_directions[i] * enemy.interest[i];
			// GD.Print("directions: " + ray_directions[i] * interest[i]);
			// GD.Print("Interest[i] " + interest[i]);

			// Uncomment to show lines the represent the weight of the directions the entity can move in
			enemy.SetDirectionLines(enemy.debug.direction_lines, enemy.ray_directions[i] * enemy.interest[i] * enemy.direction_lines_mag);
		}

		// Normalize the chosen direction
		enemy.chosen_dir = enemy.chosen_dir.Normalized();

		// Uncomment to show a line representing the direction the entity is moving in
		enemy.SetDirectionMovingLine(enemy.debug.direction_moving_line, enemy.chosen_dir * enemy.direction_line_mag);

		// GD.Print("chosen dir " + chosen_dir);
	}
   

   





}
