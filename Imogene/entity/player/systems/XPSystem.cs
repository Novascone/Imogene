using Godot;
using System;



public partial class XPSystem : Node
{
	public int xp_to_level { get; set; } = 0;

	public void GainXP(Entity entity_, float xp_gained_)
	{
		if(entity_ is Player player)
		{
			entity_.xp.amount += xp_gained_;
			LevelUp(entity_);
		}
	}

	public void LevelUp(Entity entity_)
	{
		if(entity_ is Player _player)
		{
			if( entity_.xp.amount >= xp_to_level)
			{
				entity_.xp.amount -= xp_to_level;
				xp_to_level *= 2;
			}
		}
	}
}
