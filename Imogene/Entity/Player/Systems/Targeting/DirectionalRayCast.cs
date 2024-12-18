using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class DirectionalRayCast : Node3D
{

	Vector3 PositionVector { get; set; } = Vector3.Zero;
	public float PreviousYRotation { get; set; } = 0.0f;
	public float CurrentYRotation { get; set; } = 0.0f;
	public int Range { get; set; } = 100;
	public bool EnemyHit { get; set; } = false;
	public Node3D Collider { get; set; } = null;
	public Vector3 RayOrigin { get; set; } = Vector3.Zero;
	public Vector2 DirectionRawVector { get; set; } = Vector2.Zero;
	public Vector2 AimRawVector { get; set; } = Vector2.Zero;
	public Vector2 DirectionDeadZonedVector { get; set; } = Vector2.Zero;
	public Vector2 AimDeadZonedVector { get; set; } = Vector2.Zero;
	public float DirectionInputStrength { get; set; } = 0.0f;
	public float AimInputStrength { get; set; } = 0.0f;
	public float DeadZone { get; set; } = 0.25f;
	public Dictionary<Enemy,int> Enemies { get; set; } = new();

	public Vector3 RayCastInput = Vector3.Zero;

	[Signal] public delegate void RemoveSoftTargetIconEventHandler(Enemy enemy);

    public void SetTargetingRayCastDirection()
	{
		RayCastInput.X = 0.0f;
		RayCastInput.Z = 0.0f;
		
		DirectionRawVector = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
		
		DirectionDeadZonedVector = DirectionRawVector;
		if(DirectionDeadZonedVector.Length() < DeadZone)
		{
			
			DirectionDeadZonedVector = Vector2.Zero;
		}
		else
		{
			
			DirectionDeadZonedVector = DirectionDeadZonedVector.Normalized() * ((DirectionDeadZonedVector.Length() - DeadZone) / (1 - DeadZone));
		}
		RayCastInput.X = -DirectionDeadZonedVector.X;
		RayCastInput.Z = -DirectionDeadZonedVector.Y;
		
	}

	public void SetTargetingRayCastAim()
	{
		RayCastInput.X = 0.0f;
		RayCastInput.Z = 0.0f;
		
		AimRawVector = Input.GetVector("AimLeft", "AimRight", "AimForward", "AimBackward");
		
		AimDeadZonedVector = AimRawVector;
		if(AimDeadZonedVector.Length() < DeadZone)
		{
			
			AimDeadZonedVector = Vector2.Zero;
		}
		else
		{
			
			AimDeadZonedVector = AimDeadZonedVector.Normalized() * ((AimDeadZonedVector.Length() - DeadZone) / (1 - DeadZone));
		}
		RayCastInput.X = -AimDeadZonedVector.X;
		RayCastInput.Z = -AimDeadZonedVector.Y;
		
	}

	public void GetInputStrength()
	{
		DirectionInputStrength = Input.GetActionStrength("MoveRight") + Input.GetActionStrength("MoveForward") + Input.GetActionStrength("MoveLeft") + Input.GetActionStrength("MoveBackward");
		AimInputStrength = Input.GetActionStrength("AimRight") + Input.GetActionStrength("AimForward") + Input.GetActionStrength("AimLeft") + Input.GetActionStrength("AimBackward");
		// GD.Print("Input strength " + input_strength);
	}

   


    public void GetRayCastCollisions()
	{
		
		foreach(Node3D node3D in GetChildren().Cast<Node3D>())
		{
			foreach(TargetingRaycast rayCast in node3D.GetChildren().Cast<TargetingRaycast>())
			{
				if (rayCast.IsColliding() && rayCast.Enemy == null)
				{
					rayCast.Enemy = (Enemy)rayCast.GetCollider();
					if(!Enemies.ContainsKey(rayCast.Enemy))
					{
						Enemies.Add(rayCast.Enemy, 1);
					}
					else
					{
						Enemies[rayCast.Enemy] += 1;
					}

				}
				else if (!rayCast.IsColliding() && rayCast.Enemy != null)
				{
					Enemies[rayCast.Enemy] -= 1;
					if(Enemies[rayCast.Enemy] == 0)
					{
						Enemies.Remove(rayCast.Enemy);
						EmitSignal(nameof(RemoveSoftTargetIcon), rayCast.Enemy);
					}
					
					rayCast.Enemy = null;
				}
			}
		}
	}

	

	public Enemy GetEnemyWithMostCollisions()
	{
		if(Enemies.Count > 0)
		{
			return Enemies.MaxBy(entry => entry.Value).Key;
		}
		return null;
	}

	public void FollowPlayer(Player player)
	{
		Position = player.GlobalPosition;
		GlobalPosition = Position;
		LookForward();
	}

	public void LookForward() // Rotates the player character smoothly with lerp
	{
		PreviousYRotation = GlobalRotation.Y;
		if (GlobalTransform.Origin != GlobalPosition + RayCastInput with {Y = 0}) // looks at direction the player is moving
		{
			LookAt(GlobalPosition + RayCastInput with { Y = 0 });
		}
		CurrentYRotation = GlobalRotation.Y;
		if(PreviousYRotation != CurrentYRotation)
		{
			GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(PreviousYRotation, CurrentYRotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
		}
		
	}
	
}
