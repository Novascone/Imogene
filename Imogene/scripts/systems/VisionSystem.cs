using Godot;
using System;

public partial class VisionSystem : Node
{
	
	public void SubscribeToAreaSignals(Player player)
	{
		player.areas.vision.BodyEntered += (body) => OnBodyEnteredPlayerVision(body, player);
		player.areas.vision.BodyExited += (body) => OnBodyExitedPlayerVision(body, player);

		player.areas.near.BodyEntered += (body) => OnBodyEnteredNear(body, player);
		player.areas.near.BodyExited += (body) => OnBodyExitedNear(body, player);

		player.areas.far.BodyEntered += (body) => OnBodyEnteredFar(body, player);
		player.areas.far.BodyExited += (body) => OnBodyExitedFar(body, player);
		
	}

    private void OnBodyExitedFar(Node3D body, Player player)
    {
        if(body is Enemy enemy)
		{
			player.targeting_system.EnemyExitedFar(enemy);
		}
    }

    private void OnBodyEnteredFar(Node3D body, Player player)
    {
        if(body is Enemy enemy)
		{
			player.targeting_system.EnemyEnteredFar(enemy);
		}
    }

    private void OnBodyExitedNear(Node3D body, Player player)
    {
        if(body is Enemy enemy)
		{
			player.targeting_system.EnemyExitedNear(enemy);
		}
    }

    private void OnBodyEnteredNear(Node3D body, Player player)
    {
         if(body is Enemy enemy)
		{
			player.targeting_system.EnemyEnteredNear(enemy);
		}
    }
	private void OnBodyExitedPlayerVision(Node3D body, Player player)
    {
        if (body is Enemy enemy)
		{
			player.targeting_system.EnemyExitedVision(enemy);
		}
    }

    private void OnBodyEnteredPlayerVision(Node3D body, Player player)
    {
        if(body is Enemy enemy)
		{
			player.targeting_system.EnemyEnteredVision(enemy);
		}
    }
}
