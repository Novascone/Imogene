using Godot;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

public partial class ChaseState: State
{
    Timer circle_timer;
    public override void _Ready()
	{
        base._Ready();
        name = "ChaseState";
	}
  
    public override void _PhysicsProcess(double delta)
    {
        if(enemy != null)
        {
            if(enemy.switch_to_state2)
            {
                Exit("State2");
            }
            if(enemy.player_in_alert != null)
            {
                SetInterest();
                SetDanger();
                ChooseDirection();
            }
            
        
        }
    }

    public override  void Enter()
    {
        entity = fsm.this_entity;
        if (entity is StandardEnemy this_enemy)
        {
            GD.Print("enemy is standard enemy");
            enemy = this_enemy;
        }
        GD.Print("Hello from chase state");

        // SceneTreeTimer timer = GetTree().CreateTimer(2.0);
        // await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
    
    }
    public override void SetInterest()
   {
        enemy.navigation_agent.TargetPosition = enemy.player_in_alert.GlobalPosition;
        GD.Print("Target postition " + enemy.navigation_agent.TargetPosition);
        target_position_1 = enemy.navigation_agent.GetNextPathPosition();
        

         for(int i = 0; i < enemy.num_rays; i++)
            {
                // GD.Print(ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) + " dot " + Transform.Basis.Z);

                // Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move
                // (in this case along the vector between Transform.Basis.X and the vector from this entity to the object in contact with)
                var d = enemy.ray_directions[i].Rotated(enemy.GlobalTransform.Basis.Y.Normalized(), enemy.Rotation.Y).Dot(enemy.GlobalPosition.DirectionTo(target_position_1).Normalized()); 
                // If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
                enemy.interest[i] = MathF.Max(0, d);
                // GD.Print(interest[i]);
            }
    }

     public override void SetDanger()
   {
      var space_state = enemy.GetWorld3D().DirectSpaceState;
      for(int i = 0; i < enemy.num_rays; i++)
      {
         // Cast a ray from the ray origin, in the ray direction(rotated with player .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) with a magnitude of our look_ahead variable
         var ray_query = PhysicsRayQueryParameters3D.Create(enemy.ray_origin, enemy.ray_origin + enemy.ray_directions[i].Rotated(entity.GlobalTransform.Basis.Y.Normalized(), entity.Rotation.Y) * enemy.look_ahead);
         var ray_target = enemy.ray_origin + enemy.ray_directions[i].Rotated(entity.GlobalTransform.Basis.Y.Normalized(), entity.Rotation.Y) * enemy.look_ahead; // Used in SetRayCastLines
         var result = space_state.IntersectRay(ray_query); // Result dictionary from the ray cast
      
         // Uncomment to show ray casts before collision
         // SetRayCastLines(ray_lines,ray_target);

         // *************** Comment/ Uncomment for test behavior ***************
         if(result.Count > 0)
         {
            collider = (Node3D)result["collider"];
            enemy.SetCollisionLines(enemy.collision_lines, result);
            enemy.danger[i] = 1.0f;	
         }
         else
         {
            enemy.danger[i] = 0;
         }
      }
   }

    public override void ChooseDirection()
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
			enemy.SetDirectionLines(enemy.direction_lines, enemy.ray_directions[i] * enemy.interest[i] * enemy.direction_lines_mag);
		}

		// Normalize the chosen direction
		enemy.chosen_dir = enemy.chosen_dir.Normalized();

		// Uncomment to show a line representing the direction the entity is moving in
		enemy.SetDirectionMovingLine(enemy.direction_moving_line, enemy.chosen_dir * enemy.direction_line_mag);

		// GD.Print("chosen dir " + chosen_dir);
	}
}