using Godot;
using System;

public partial class State5 : State
{

	public float[] interest_to_herd; // Interest weight, how interested the entity is in moving toward a location
	public float[] interest_to_interest_point; // Interest weight, how interested the entity is in moving toward a location
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		
        name = "State5";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(enemy != null)
      {
		 if(enemy.switch_to_state2)
        {
            Exit("State2");
        }

		// ************************** Change behavior here ************************************

        
		Array.Resize(ref interest_to_interest_point, enemy.num_rays);
		Array.Resize(ref interest_to_herd, enemy.num_rays);
		if(enemy.near_herd_mate == true) // If the entity can see the box calculate the interest for moving away from it
		{
			if(entity.GlobalPosition.DistanceTo(enemy.herd_mate_position) > 2)
			{
				SetInterestToHerd();
			}
			else // If the entity can't see the box remove the interest in moving away from it
			{
				GD.Print("Close to herd mate");
				for(int i = 0; i < enemy.num_rays; i++)
				{
					interest_to_herd[i] = 0;
				}
				
			}
		}
		
		if(entity.GlobalPosition.DistanceTo(enemy.interest_position) < 2)
		{
			GD.Print("distance less than 2");
			for(int i = 0; i < enemy.num_rays; i++)
			{
				interest_to_interest_point[i] = 0;
			}
		}
		
		SetInterestToInterestPoint();
		// CombineInterests();
		SetDanger();
		ChooseDirection();
			
        
	  }
	}

	public override  void Enter()
    {
        entity = fsm.this_entity;
		if (entity is Enemy this_enemy)
		{
			enemy = this_enemy;
		}
        GD.Print("Hello from state 5 (Herd interest state)");

        // SceneTreeTimer timer = GetTree().CreateTimer(2.0);
        // await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
    
    }

	public void SetInterestToInterestPoint()
    {
		// GD.Print("Setting interest to interest point");
        // entity.navigation_agent.TargetPosition = entity.box_position;
        // target_position = entity.navigation_agent.GetNextPathPosition();

		// ******************************************** Set up interest point ************************************************************************
		target_position_1 = enemy.interest_position;

        for(int i = 0; i < enemy.num_rays; i++)
            {
                // GD.Print(ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) + " dot " + Transform.Basis.Z);

                // Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move
                // The interest in moving away from the object
                var d = enemy.ray_directions[i].Rotated(entity.GlobalTransform.Basis.Y.Normalized(), entity.Rotation.Y).Dot(entity.GlobalPosition.DirectionTo(target_position_1).Normalized()); 
				d *= 1.2f; // Making the entity more interested in running away from the chaser than moving toward the center
                // If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
                interest_to_interest_point[i] = MathF.Max(0, d);
                // GD.Print(interest[i]);
            }
    }

	public void SetInterestToHerd()
    {
		// GD.Print("Setting interest to herd mate");
        // entity.navigation_agent.TargetPosition = 

		// ************************************* Set herd_mate position ***********************************************************
        target_position_2 = enemy.herd_mate_position;

        for(int i = 0; i < enemy.num_rays; i++)
            {
                // GD.Print(ray_directions[i].Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y) + " dot " + Transform.Basis.Z);

                // Get the dot product of ray directions (rotated with the player hence the .Rotated(GlobalTransform.Basis.Y.Normalized(), Rotation.Y)) and the direction the entity wants to move
                // (in this case along the vector between Transform.Basis.X and the vector from this entity to the object in contact with)
                var d = enemy.ray_directions[i].Rotated(entity.GlobalTransform.Basis.Y.Normalized(), entity.Rotation.Y).Dot(entity.GlobalPosition.DirectionTo(target_position_2).Normalized()); 
                // If d is less that zero, replace it with 0 in the interest array, this is to ignore weight in the opposite direction the entity wants to go
                interest_to_herd[i] = MathF.Max(0, d);
                // GD.Print(interest[i]);
            }
    }

	public override void SetDanger()
    {
        var space_state = entity.GetWorld3D().DirectSpaceState;
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
				
				enemy.danger[i] = 5.0f;	
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
			if(enemy.danger[i] > 0.0f)
			{
				enemy.interest[i] = 0.0f;
			}
			enemy.interest[i] = interest_to_interest_point[i] + interest_to_herd[i];

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
