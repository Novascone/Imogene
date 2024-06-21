using Godot;
using System;

public partial class ResourceSystem : EntitySystem
{
	public Timer posture_regen_timer;
	private CustomSignals _customSignals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		posture_regen_timer = GetNode<Timer>("PostureRegenTimer");
		posture_regen_timer.Timeout += OnPostureRegenTickTimeout;

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

	public void Posture(float posture_damage)
	{
		if(entity.posture < entity.maximum_posture)
		{
			GD.Print(entity.identifier + " taking posture damage of " + posture_damage);
			entity.posture += posture_damage;
			if(entity.posture >= entity.maximum_posture)
			{
				entity.posture_broken = true;
				entity.damage_system.Stun();
			}
			GD.Print("posture " + entity.posture);
			
			if(entity is Enemy enemy)
			{
				enemy.posture_bar.Value += posture_damage;
				_customSignals.EmitSignal(nameof(CustomSignals.EnemyPostureChangedUI), entity.posture);
			
			}
		}
		PostureRegen();
		
	}

	public void PostureRegen()
	{
		posture_regen_timer.Start();
	}

	private void OnPostureRegenTickTimeout()
    {
		GD.Print("posture regenerating");
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
				enemy.posture_bar.Value = entity.posture;
				_customSignals.EmitSignal(nameof(CustomSignals.EnemyPostureChangedUI), entity.posture);
			}
		if(entity.posture == 0)
		{
			posture_regen_timer.Stop();
		}
    }
}
