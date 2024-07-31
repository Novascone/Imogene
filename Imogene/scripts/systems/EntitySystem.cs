using Godot;
using System;

public partial class EntitySystem : Node
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
