using Godot;
using System;



public partial class XPSystem : Node
{
	public int xp_to_level;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GainXP(Entity entity_, float xp_gained)
	{
		if(entity_ is Player player)
		{
			entity_.xp.amount += xp_gained;
			// player.ui.hud.xp.Value = player.xp;
			LevelUp(entity_);
		}
	}

	public void LevelUp(Entity entity_)
	{
		if(entity_ is Player player)
		{
			if( entity_.xp.amount >= xp_to_level)
			{
				entity_.xp.amount -= xp_to_level;
				xp_to_level *= 2;
				// player.ui.hud.xp.MaxValue = player.xp_to_level;
				// player.ui.hud.xp.Value = player.xp;
				GD.Print("Leveled up");
				GD.Print("New xp to level " + xp_to_level);
				GD.Print("current xp " + entity_.xp.amount);
			}
		}
	}
}
