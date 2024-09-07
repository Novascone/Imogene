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
		player.resource += resource_change;
		GD.Print("Cost " + resource_change);
		
		EmitSignal(nameof(ResourceChange), player.resource);
		
		ResourceRegen(player);
	}

	public void ResourceRegen(Player player)
	{
		resource_regen_timer.Timeout += () => OnResourceRegenTickTimeout(player);
		resource_regen_timer.Start();
	}

	public void Posture(Entity entity, float posture_damage)
	{
		if(entity.posture < entity.maximum_posture)
		{
			// GD.Print(entity.identifier + " taking posture damage of " + posture_damage);
			entity.posture += posture_damage;
			if(entity.posture >= entity.maximum_posture)
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
        // GD.Print("resource regenerating");
		if(player.resource < player.maximum_resource)
		{
			player.resource += player.resource_regen;
			GD.Print("Resource " + player.resource);
		}

		// if(entity is Player player)
		// {
		// 	player.ui.hud.resource.Value = entity.resource;
		// }
    }

	

	private void OnPostureRegenTickTimeout(Entity entity)
    {
		// GD.Print("posture regenerating");
		if(entity.posture > 0)
		{
			entity.posture -= entity.posture_regen;
		}
        else
		{
			entity.posture = 0;
		}
		if(entity is Enemy enemy)
		{
			enemy.ui.posture_bar.Value = entity.posture;
		}
		// if(entity is Player player)
		// {
		// 	player.ui.hud.posture.Value = entity.posture;
		// }
		if(entity.posture == 0)
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
