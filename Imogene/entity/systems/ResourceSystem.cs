using Godot;
using System;

public partial class ResourceSystem : Node
{
	[Export] public Timer posture_regen_timer;
	[Export] public Timer resource_regen_timer;
	

	[Signal] public delegate void ResourceChangeEventHandler(float resource);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

    

    public void Resource(Player player, float resource_change)
	{
		player.general_stats["resource"] += resource_change;
		GD.Print("Cost " + resource_change);
		EmitSignal(nameof(ResourceChange), player.general_stats["resource"]);
		
		ResourceRegen(player);
	}

	public void ResourceRegen(Player player)
	{
		if(resource_regen_timer.TimeLeft == 0)
		{
			resource_regen_timer.Timeout += () => OnResourceRegenTickTimeout(player);
			resource_regen_timer.Start();
		}
		
		
		
	}

	public void Posture(Entity entity, float posture_damage)
	{
		if(entity.general_stats["posture"] < entity.calculation_stats["maximum_posture"])
		{
			// GD.Print(entity.identifier + " taking posture damage of " + posture_damage);
			entity.general_stats["posture"] += posture_damage;
			if(entity.general_stats["posture"] >= entity.calculation_stats["maximum_posture"])
			{
				entity.posture_broken = true;
			}
			// GD.Print("posture " + entity.posture);
			
			if(entity is Enemy enemy)
			{
				enemy.ui.posture_bar.Value += posture_damage;

			}
			// if(entity is Player player)
			// {
			// 	player.ui.hud.posture.Value += posture_damage;
			// }
		}
		PostureRegen(entity);
		
	}

	public void PostureRegen(Entity entity)
	{
		posture_regen_timer.Timeout += () => OnPostureRegenTickTimeout(entity);
		posture_regen_timer.Start();
	}

	private void OnResourceRegenTickTimeout(Player player)
    {
        GD.Print("resource regenerating");
		if(player.general_stats["resource"] < player.depth_stats["maximum_resource"])
		{
			player.general_stats["resource"] += player.depth_stats["resource_regeneration"];
			GD.Print("Resource of " + player.Name + " " + player.general_stats["resource"]);
		}

		// if(entity is Player player)
		// {
		// 	player.ui.hud.resource.Value = entity.resource;
		// }
    }

	

	private void OnPostureRegenTickTimeout(Entity entity)
    {
		// GD.Print("posture regenerating");
		if(entity.general_stats["posture"] > 0)
		{
			entity.general_stats["posture"] -= entity.depth_stats["resource_regeneration"];
		}
        else
		{
			entity.general_stats["posture"] = 0;
		}
		if(entity is Enemy enemy)
		{
			enemy.ui.posture_bar.Value = entity.general_stats["posture"];
		}
		// if(entity is Player player)
		// {
		// 	player.ui.hud.posture.Value = entity.posture;
		// }
		if(entity.general_stats["posture"] == 0)
		{
			posture_regen_timer.Stop();
		}
    }

    internal void HandleResourceEffect(Player player, float resource_change)
    {
        GD.Print("resource system has received resource change");
        Resource(player, resource_change);
    }
}
