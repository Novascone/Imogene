using Godot;
using System;

public partial class XPSystem : EntitySystem
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void GainXP(float xp_gained)
	{
		if(entity is Player player)
		{
			player.xp += xp_gained;
			// player.ui.hud.xp.Value = player.xp;
			LevelUp();
		}
	}

	public void LevelUp()
	{
		if(entity is Player player)
		{
			if( entity.xp >= entity.xp_to_level)
			{
				entity.xp -= entity.xp_to_level;
				entity.xp_to_level *= 2;
				// player.ui.hud.xp.MaxValue = player.xp_to_level;
				// player.ui.hud.xp.Value = player.xp;
				GD.Print("Leveled up");
				GD.Print("New xp to level " + entity.xp_to_level);
				GD.Print("current xp " + entity.xp);
			}
		}
	}
}
