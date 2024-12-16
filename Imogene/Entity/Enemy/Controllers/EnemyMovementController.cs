using Godot;
using System;

public partial class EnemyMovementController : Node
{
	public bool speed_altered_check;
	public bool movement_stopped_check;
	public bool movement_check_completed;
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

			return enemy.jump_gravity;
		}
		else
		{
			return enemy.fall_gravity;
		}
	}

	
	public bool StatusEffectsPreventingMovement(Enemy enemy)
	{
		if(enemy.EntityControllers.status_effect_controller.movement_prevented)
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
		if (enemy.EntityControllers.status_effect_controller.abilities_prevented)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	
}
