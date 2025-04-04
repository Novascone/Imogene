using Godot;
using System;

public partial class EnemyMovementController : Node
{
	public bool SpeedAlteredCheck;
	public bool MovementPreventedCheck;
	public bool MovementCheckCompleted;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GD.Print("Enemy movement controller loaded");
	}

	public void MoveEnemy(Enemy enemy, double delta)
	{
		
			
		StandardMovement(enemy, delta);
			

		enemy.VelocityVector.X = enemy.VelocityVector.X * enemy.MovementSpeed.CurrentValue;
		enemy.VelocityVector.Z = enemy.DirectionVector.Z * enemy.MovementSpeed.CurrentValue;			
		enemy.Velocity = enemy.VelocityVector;
			
	}

	private void StandardMovement(Enemy enemy, double delta)
    {
        
		// else
		// {
		// 	player.movement_stats["speed"] = Mathf.Lerp(player.movement_stats["speed"], 2.0f, 0.1f);
		// }

		if(!enemy.IsOnFloor())
		{
		
			enemy.VelocityVector.Y += (float)(SetGravity(enemy) * delta);
		}
		else
		{
			enemy.VelocityVector.Y = 0;
		}
		
		// LookForward(player,player.direction);
    }

	public float SetGravity(Enemy enemy)
	{
		if(enemy.Velocity.Y > 0)
		{

			return enemy.JumpGravity;
		}
		else
		{
			return enemy.FallGravity;
		}
	}

	
	public bool StatusEffectsPreventingMovement(Enemy enemy)
	{
		if(enemy.EntityControllers.EntityStatusEffectsController.EntityMovementPrevented)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool StatusEffectsAffectingSpeed(Enemy enemy)
	{
		if (enemy.EntityControllers.EntityStatusEffectsController.EntityAbilitiesPrevented)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	
}
