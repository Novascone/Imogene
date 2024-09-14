using Godot;
using System;

public partial class ResourceSystem : Node
{
	[Export] public Timer posture_regen_timer;
	[Export] public Timer resource_regen_timer;

	[Signal] public delegate void ResourceChangeEventHandler(float resource);

	StatModifier change_resource = new(StatModifier.ModificationType.Add);
	StatModifier change_posture = new(StatModifier.ModificationType.Add);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

    

    public void Resource(Player player, float resource_change)
	{
		change_resource.value_to_add = resource_change;
		player.resource.modifiers.Add(change_resource);
		GD.Print("Cost " + resource_change);
		EmitSignal(nameof(ResourceChange), player.resource.current_value);
		
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
		if(entity.posture.current_value < entity.posture.max_value)
		{
			// GD.Print(entity.identifier + " taking posture damage of " + posture_damage);
			
			change_posture.value_to_add = posture_damage;
			entity.posture.modifiers.Add(change_posture);
			
			if(entity.posture.current_value >= entity.posture.max_value)
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
		if(player.resource.current_value< player.resource.max_value)
		{
			change_resource.value_to_add = player.resource_regeneration.current_value;
			player.resource.modifiers.Add(change_resource);
		}

		// if(entity is Player player)
		// {
		// 	player.ui.hud.resource.Value = entity.resource;
		// }
    }

	

	private void OnPostureRegenTickTimeout(Entity entity)
    {
		// GD.Print("posture regenerating");
		if(entity.posture.current_value > 0)
		{
			change_posture.value_to_add = entity.resource_regeneration.current_value;
		}
        else
		{
			entity.posture.current_value = 0;
		}
		if(entity is Enemy enemy)
		{
			enemy.ui.posture_bar.Value = entity.posture.current_value;
		}
		// if(entity is Player player)
		// {
		// 	player.ui.hud.posture.Value = entity.posture;
		// }
		if(entity.posture.current_value == 0)
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
