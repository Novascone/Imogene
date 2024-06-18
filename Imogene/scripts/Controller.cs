using Godot;
using System;

// Base Class for the controllers, contains a player reference, and implements GetPlayerInfo for the controllers 
public partial class Controller : Node
{
	public Entity entity;
	public Player player;

	public void GetEntityInfo(Entity s)
	{
		entity = s;
	}
	public void GetPlayerInfo(Player s)
	{
		player = s;
	}
}
