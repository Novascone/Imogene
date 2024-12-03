using Godot;
using System;


public partial class EnemyAbilityController : Node
{

	public bool can_use_abilities;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		
	}

	public bool StatusEffectPreventingAbilities(Enemy enemy)
	{
		if(enemy.EntityControllers.status_effect_controller.abilities_prevented)
		{
			return true;
		}
		return false;
	}

	public void CheckCanUseAbility(Enemy enemy)
	{
		if(StatusEffectPreventingAbilities(enemy))
		{
			can_use_abilities = false;
			// GD.Print("enemy can not use abilities because of a status effect");
		}
		else
		{
			can_use_abilities = true;
		}
	}
}
