using Godot;
using System;



public partial class XPSystem : Node
{
	public int xp_to_level { get; set; } = 0;

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
			if( entity.XP.Amount >= xp_to_level)
			{
				entity.XP.Amount -= xp_to_level;
				xp_to_level *= 2;
			}
		}
	}
}
