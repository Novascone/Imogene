using Godot;
using System;


public partial class EnemyAbilityController : Node
{

	public bool CanUseAbilities;
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
		if(enemy.EntityControllers.EntityStatusEffectsController.EntityAbilitiesPrevented)
		{
			return true;
		}
		return false;
	}

	public void CheckCanUseAbility(Enemy enemy)
	{
		if(StatusEffectPreventingAbilities(enemy))
		{
			CanUseAbilities = false;
			// GD.Print("enemy can not use abilities because of a status effect");
		}
		else
		{
			CanUseAbilities = true;
		}
	}
}
