using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class DirectionalRayCast : Node3D
{

	Vector3 position { get; set; } = Vector3.Zero;
	public float previous_y_rotation { get; set; } = 0.0f;
	public float current_y_rotation { get; set; } = 0.0f;
	public int range { get; set; } = 100;
	public bool enemy_hit { get; set; } = false;
	public Node3D collider { get; set; } = null;
	public Vector3 ray_origin { get; set; } = Vector3.Zero;
	public Vector2 DirectionRawVector { get; set; } = Vector2.Zero;
	public Vector2 AimRawVector { get; set; } = Vector2.Zero;
	public Vector2 DirectionDeadzonedVector { get; set; } = Vector2.Zero;
	public Vector2 AimDeadzonedVector { get; set; } = Vector2.Zero;
	public float DirectionInputStrength { get; set; } = 0.0f;
	public float AimInputStrength { get; set; } = 0.0f;
	public float deadzone { get; set; } = 0.25f;
	public Dictionary<Enemy,int> enemies { get; set; } = new();

	public Vector3 ray_cast_input = Vector3.Zero;

	[Signal] public delegate void RemoveSoftTargetIconEventHandler(Enemy enemy_);

    public void SetTargetingRayCastDirection()
	{
		ray_cast_input.X = 0.0f;
		ray_cast_input.Z = 0.0f;
		
		DirectionRawVector = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
		
		DirectionDeadzonedVector = DirectionRawVector;
		if(DirectionDeadzonedVector.Length() < deadzone)
		{
			
			DirectionDeadzonedVector = Vector2.Zero;
		}
		else
		{
			
			DirectionDeadzonedVector = DirectionDeadzonedVector.Normalized() * ((DirectionDeadzonedVector.Length() - deadzone) / (1 - deadzone));
		}
		ray_cast_input.X = -DirectionDeadzonedVector.X;
		ray_cast_input.Z = -DirectionDeadzonedVector.Y;
		
	}

	public void SetTargetingRayCastAim()
	{
		ray_cast_input.X = 0.0f;
		ray_cast_input.Z = 0.0f;
		
		AimRawVector = Input.GetVector("AimLeft", "AimRight", "AimForward", "AimBackward");
		
		AimDeadzonedVector = AimRawVector;
		if(AimDeadzonedVector.Length() < deadzone)
		{
			
			AimDeadzonedVector = Vector2.Zero;
		}
		else
		{
			
			AimDeadzonedVector = AimDeadzonedVector.Normalized() * ((AimDeadzonedVector.Length() - deadzone) / (1 - deadzone));
		}
		ray_cast_input.X = -AimDeadzonedVector.X;
		ray_cast_input.Z = -AimDeadzonedVector.Y;
		
	}

	public void GetInputStrength()
	{
		DirectionInputStrength = Input.GetActionStrength("MoveRight") + Input.GetActionStrength("MoveForward") + Input.GetActionStrength("MoveLeft") + Input.GetActionStrength("MoveBackward");
		AimInputStrength = Input.GetActionStrength("AimRight") + Input.GetActionStrength("AimForward") + Input.GetActionStrength("AimLeft") + Input.GetActionStrength("AimBackward");
		// GD.Print("Input strength " + input_strength);
	}

   


    public void GetRayCastCollisions()
	{
		
		foreach(Node3D _node3D in GetChildren().Cast<Node3D>())
		{
			foreach(TargetingRaycast _ray_cast in _node3D.GetChildren().Cast<TargetingRaycast>())
			{
				if (_ray_cast.IsColliding() && _ray_cast.enemy == null)
				{
					_ray_cast.enemy = (Enemy)_ray_cast.GetCollider();
					if(!enemies.ContainsKey(_ray_cast.enemy))
					{
						enemies.Add(_ray_cast.enemy, 1);
					}
					else
					{
						enemies[_ray_cast.enemy] += 1;
					}

				}
				else if (!_ray_cast.IsColliding() && _ray_cast.enemy != null)
				{
					enemies[_ray_cast.enemy] -= 1;
					if(enemies[_ray_cast.enemy] == 0)
					{
						enemies.Remove(_ray_cast.enemy);
						EmitSignal(nameof(RemoveSoftTargetIcon), _ray_cast.enemy);
					}
					
					_ray_cast.enemy = null;
				}
			}
		}
	}

	

	public Enemy GetEnemyWithMostCollisions()
	{
		if(enemies.Count > 0)
		{
			return enemies.MaxBy(entry => entry.Value).Key;
		}
		return null;
	}

	public void FollowPlayer(Player player_)
	{
		position = player_.GlobalPosition;
		GlobalPosition = position;
		LookForward();
	}

	public void LookForward() // Rotates the player character smoothly with lerp
	{
		previous_y_rotation = GlobalRotation.Y;
		if (GlobalTransform.Origin != GlobalPosition + ray_cast_input with {Y = 0}) // looks at direction the player is moving
		{
			LookAt(GlobalPosition + ray_cast_input with { Y = 0 });
		}
		current_y_rotation = GlobalRotation.Y;
		if(previous_y_rotation != current_y_rotation)
		{
			GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(previous_y_rotation, current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
		}
		
	}
	
}
