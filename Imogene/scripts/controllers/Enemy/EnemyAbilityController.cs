using Godot;
using System;


public partial class EnemyAbilityController : Controller
{

	public bool can_use_abilities;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(StatusEffectPreventingAbilities())
		{
			can_use_abilities = false;
			// GD.Print("enemy can not use abilities because of a status effect");
		}
		else
		{
			can_use_abilities = true;
		}
	}

	public bool StatusEffectPreventingAbilities()
	{
		if(entity.status_effect_controller.dazed || entity.status_effect_controller.frozen || entity.status_effect_controller.feared || entity.status_effect_controller.hexed || entity.status_effect_controller.staggered)
		{
			return true;
		}
		return false;
	}
}
