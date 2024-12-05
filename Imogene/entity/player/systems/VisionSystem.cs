using Godot;
using System;

public partial class VisionSystem : Node
{
	
	private static void OnBodyEnteredFar(Node3D body_, Player player_)
    {
        if(body_ is Enemy _enemy)
		{
			player_.PlayerSystems.targeting_system.EnemyEnteredFar(_enemy);
		}
    }

    private static void OnBodyExitedFar(Node3D body_, Player player_)
    {
        if(body_ is Enemy _enemy)
		{
			player_.PlayerSystems.targeting_system.EnemyExitedFar(_enemy);
		}
    }

	private static void OnBodyEnteredNear(Node3D body_, Player player_)
    {
         if(body_ is Enemy _enemy)
		{
			player_.PlayerSystems.targeting_system.EnemyEnteredNear(_enemy);
		}
    }

    private static void OnBodyExitedNear(Node3D body_, Player player_)
    {
        if(body_ is Enemy _enemy)
		{
			player_.PlayerSystems.targeting_system.EnemyExitedNear(_enemy);
		}
    }

	public static void Subscribe(Player player_)
	{
		player_.PlayerAreas.near.BodyEntered += (body_) => OnBodyEnteredNear(body_, player_);
		player_.PlayerAreas.near.BodyExited += (body_) => OnBodyExitedNear(body_, player_);

		player_.PlayerAreas.far.BodyEntered += (body_) => OnBodyEnteredFar(body_, player_);
		player_.PlayerAreas.far.BodyExited += (body_) => OnBodyExitedFar(body_, player_);
		
	}

	public static void unsubscribe(Player player_)
	{
		player_.PlayerAreas.near.BodyEntered -= (body_) => OnBodyEnteredNear(body_, player_);
		player_.PlayerAreas.near.BodyExited -= (body_) => OnBodyExitedNear(body_, player_);

		player_.PlayerAreas.far.BodyEntered -= (body_) => OnBodyEnteredFar(body_, player_);
		player_.PlayerAreas.far.BodyExited -= (body_) => OnBodyExitedFar(body_, player_);
		
	}

}
