using Godot;
using System;

public partial class VisionSystem : Node
{
	
	private static void OnBodyEnteredFar(Node3D body, Player player)
    {
        if(body is Enemy enemy)
		{
			player.PlayerSystems.TargetingSystem.EnemyEnteredFar(enemy);
		}
    }

    private static void OnBodyExitedFar(Node3D body, Player player)
    {
        if(body is Enemy enemy)
		{
			player.PlayerSystems.TargetingSystem.EnemyExitedFar(enemy);
		}
    }

	private static void OnBodyEnteredNear(Node3D body, Player player)
    {
         if(body is Enemy enemy)
		{
			player.PlayerSystems.TargetingSystem.EnemyEnteredNear(enemy);
		}
    }

    private static void OnBodyExitedNear(Node3D body, Player player)
    {
        if(body is Enemy enemy)
		{
			player.PlayerSystems.TargetingSystem.EnemyExitedNear(enemy);
		}
    }

	public static void Subscribe(Player player)
	{
		player.PlayerAreas.Near.BodyEntered += (body) => OnBodyEnteredNear(body, player);
		player.PlayerAreas.Near.BodyExited += (body) => OnBodyExitedNear(body, player);

		player.PlayerAreas.Far.BodyEntered += (body) => OnBodyEnteredFar(body, player);
		player.PlayerAreas.Far.BodyExited += (body) => OnBodyExitedFar(body, player);
		
	}

	public static void unsubscribe(Player player)
	{
		player.PlayerAreas.Near.BodyEntered -= (body) => OnBodyEnteredNear(body, player);
		player.PlayerAreas.Near.BodyExited -= (body) => OnBodyExitedNear(body, player);

		player.PlayerAreas.Far.BodyEntered -= (body) => OnBodyEnteredFar(body, player);
		player.PlayerAreas.Far.BodyExited -= (body) => OnBodyExitedFar(body, player);
		
	}

}
