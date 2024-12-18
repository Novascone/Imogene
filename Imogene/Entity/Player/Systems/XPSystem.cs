using Godot;
using System;



public partial class XPSystem : Node
{
	public int XPToLevel { get; set; } = 0;

	public void GainXP(Entity entity, float xpGained)
	{
		if(entity is Player player)
		{
			entity.XP.Amount += xpGained;
			LevelUp(entity);
		}
	}

	public void LevelUp(Entity entity)
	{
		if(entity is Player player)
		{
			if( entity.XP.Amount >= XPToLevel)
			{
				entity.XP.Amount -= XPToLevel;
				XPToLevel *= 2;
			}
		}
	}
}
