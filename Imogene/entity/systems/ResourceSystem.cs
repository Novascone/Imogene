using Godot;
using System;

public partial class ResourceSystem : Node
{
	StatModifier change_resource  { get; set; } = new (StatModifier.ModificationType.AddCurrent);
	StatModifier change_posture  { get; set; } = new (StatModifier.ModificationType.AddCurrent);
	public float resource_timer_duration { get; set; } = 1;
	public SceneTreeTimer resource_timer { get; set; } = null;
	public float posture_timer_duration { get; set; } = 1;
	public SceneTreeTimer posture_timer { get; set; } = null;
	public bool posture_broken { get; set; } = false;

	[Signal] public delegate void ResourceChangeEventHandler(float resource_);

    public void ChangeResource(Player player_, float resource_change_)
	{
		change_resource.value_to_add = resource_change_;
		player_.resource.AddModifier(change_resource);
		EmitSignal(nameof(ResourceChange), player_.resource.current_value);
		ResourceRegeneration(player_);
	}

	public void ResourceRegeneration(Player player_)
	{
		if(resource_timer == null || resource_timer.TimeLeft == 0)
		{
			resource_timer = GetTree().CreateTimer(resource_timer_duration);
			resource_timer.Timeout += () => ResourceTimerTimeout(player_);
		}
	}

	private void ResourceTimerTimeout(Player player_)
    {
		if(player_.resource.current_value < player_.resource.max_value)
		{
			if(resource_timer == null || resource_timer.TimeLeft == 0)
			{
				resource_timer = GetTree().CreateTimer(resource_timer_duration);
				resource_timer.Timeout += () => ResourceTimerTimeout(player_);
			}
			change_resource.value_to_add = player_.resource_regeneration.current_value;
			player_.resource.AddModifier(change_resource);
			EmitSignal(nameof(ResourceChange), player_.resource.current_value);
		}
    }

	public void ChangePosture(Entity entity_, float posture_damage_)
	{
		GD.Print("posture damage");
		if(entity_.posture.current_value < entity_.posture.max_value)
		{
			
			change_posture.value_to_add = posture_damage_;
			entity_.posture.AddModifier(change_posture);
			
			if(entity_.posture.current_value >= entity_.posture.max_value)
			{
				posture_broken = true;
			}

			
			if(entity_ is Enemy enemy)
			{
				enemy.ui.posture_bar.Value += posture_damage_;
			}
		}
		PostureRegeneration(entity_);
		
	}

	public void PostureRegeneration(Entity entity_)
	{
		
		if(posture_timer == null || posture_timer.TimeLeft == 0)
		{
			posture_timer = GetTree().CreateTimer(posture_timer_duration);
			posture_timer.Timeout += () => PostureTimerTimeout(entity_);
		}
	}

	private void PostureTimerTimeout(Entity entity_)
    {
		if(entity_.posture.current_value > 0)
		{
			if(posture_timer == null || posture_timer.TimeLeft == 0)
			{
				posture_timer = GetTree().CreateTimer(posture_timer_duration);
				posture_timer.Timeout += () => PostureTimerTimeout(entity_);
			}
			change_posture.value_to_add = -entity_.posture_regeneration.current_value;
			entity_.posture.AddModifier(change_posture);
		}
        else
		{
			entity_.posture.current_value = 0;
		}
		if(entity_ is Enemy _enemy)
		{
			_enemy.ui.posture_bar.Value = entity_.posture.current_value;
		}

    }

    internal void HandleResourceEffect(Player player_, float resource_change_)
    {
        ChangeResource(player_, resource_change_);
    }

	public void Subscribe(Entity entity_)
	{
		if(entity_ is Player _player)
		{
			_player.controllers.ability_controller.ResourceEffect += HandleResourceEffect;
		}
		entity_.entity_systems.damage_system.ChangePosture += ChangePosture;
	}

    private void HandleChangePosture()
    {
        GD.Print("change posture");
    }
}
