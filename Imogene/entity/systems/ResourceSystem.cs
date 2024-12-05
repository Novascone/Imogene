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

    public void ChangeResource(Player player, float resourceChange)
	{
		change_resource.ValueToAdd = resourceChange;
		player.Resource.AddModifier(change_resource);
		EmitSignal(nameof(ResourceChange), player.Resource.CurrentValue);
		ResourceRegeneration(player);
	}

	public void ResourceRegeneration(Player player_)
	{
		if(resource_timer == null || resource_timer.TimeLeft == 0)
		{
			resource_timer = GetTree().CreateTimer(resource_timer_duration);
			resource_timer.Timeout += () => ResourceTimerTimeout(player_);
		}
	}

	private void ResourceTimerTimeout(Player player)
    {
		if(player.Resource.CurrentValue < player.Resource.MaxValue)
		{
			if(resource_timer == null || resource_timer.TimeLeft == 0)
			{
				resource_timer = GetTree().CreateTimer(resource_timer_duration);
				resource_timer.Timeout += () => ResourceTimerTimeout(player);
			}
			change_resource.ValueToAdd = player.ResourceRegeneration.CurrentValue;
			player.Resource.AddModifier(change_resource);
			EmitSignal(nameof(ResourceChange), player.Resource.CurrentValue);
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
			_player.PlayerControllers.ability_controller.ResourceEffect += HandleResourceEffect;
		}
		
	}

	public void unsubscribe(Entity entity_)
	{
		if(entity_ is Player _player)
		{
			_player.PlayerControllers.ability_controller.ResourceEffect -= HandleResourceEffect;
		}
		
	}

  
}
