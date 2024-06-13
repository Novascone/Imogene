using Godot;
using System;

// Base Class for the controllers, contains a player reference, and implements GetPlayerInfo for the controllers 
public partial class Controller : Node
{
	public Player player;

	public void GetPlayerInfo(Player s)
	{
		player = s;
	}
}
