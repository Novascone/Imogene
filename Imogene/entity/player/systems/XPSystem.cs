using Godot;
using System;



public partial class XPSystem : EntitySystem
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

	public void GainXP(float xp_gained)
	{
		if(entity is Player player)
		{
			entity.accumulation_stats["xp"] += xp_gained;
			// player.ui.hud.xp.Value = player.xp;
			LevelUp();
		}
	}

	public void LevelUp()
	{
		if(entity is Player player)
		{
			if( entity.accumulation_stats["xp"] >= entity.accumulation_stats["xp"])
			{
				entity.accumulation_stats["xp"]-= xp_to_level;
				xp_to_level *= 2;
				// player.ui.hud.xp.MaxValue = player.xp_to_level;
				// player.ui.hud.xp.Value = player.xp;
				GD.Print("Leveled up");
				GD.Print("New xp to level " + xp_to_level);
				GD.Print("current xp " + entity.accumulation_stats["xp"]);
			}
		}
	}
}
