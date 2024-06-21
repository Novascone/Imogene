using Godot;
using System;

public partial class EntitySystem : Node
{
	public Entity entity; 
	
	public void GetEntityInfo(Entity s)
	{
		entity = s;
	}
}
