using Godot;
using System;

public partial class Kick : Ability
{

	private Timer charge_timer_1;
	private Timer charge_timer_2;
		[Export] public PackedScene hitbox_to_load;
	[Export] public PackedScene mesh_to_load;
	KickHitbox kick_hitbox;
	MeshInstance3D kick_mesh;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MeleeHitbox = kick_hitbox;
		charge_timer_1 = GetNode<Timer>("ChargeTimer1");
		charge_timer_2 = GetNode<Timer>("ChargeTimer2");
		UseTimer = GetNode<Timer>("CastTimer");
		Charges = 2;
		DamageModifier = 0.5f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		// {
			
		// 	// GD.Print("Queuing kick");
		// 	QueueAbility();	
		// }
		// else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		// {
		// 	if(cast_timer. TimeLeft == 0)
		// 	{
		// 		QueueAbility();
		// 		CheckCanUseAbility();
		// 	}
			
		
		// }
		// if(cast_timer.TimeLeft == 0)
		// {
		// 	CheckCanUseAbility();
		// }
		
		
	}

	public override void Execute(Player player)
	{
		base.Execute(player);
		ChargesUsed += 1;
		UseTimer.Start();
		if(charge_timer_1.TimeLeft == 0)
		{
			charge_timer_1.Start();
		}
		else
		{
			charge_timer_2.Start();
		}
		
		
		AddHitbox(player);
		DealDamage(player, DamageModifier);
		
			
		kick_hitbox.damage = player.CombinedDamage  / 2; // Set projectile damage
		kick_hitbox.posture_damage = 0; // Set projectile posture damage 
		kick_hitbox.is_critical = false;
		
		kick_hitbox.type = MeleeHitbox.DamageType.Physical; // Set projectile damage type
		
	}

    public override void FrameCheck(Player player)
    {
        if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(UseTimer. TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityQueue), this);
				EmitSignal(nameof(AbilityCheck), this);
			}
			
		
		}
		if(UseTimer.TimeLeft == 0)
		{
			EmitSignal(nameof(AbilityCheck), player, this);
		}
    }

    public void AddHitbox(Player player)
	{
		kick_hitbox = (KickHitbox)hitbox_to_load.Instantiate(); // Instantiate the projectile
		player.SurroundingHitbox.AddChild(kick_hitbox);
		if(kick_mesh == null)
		{
			// GD.Print("Add mesh");
			kick_mesh = (MeshInstance3D)mesh_to_load.Instantiate();
			player.SurroundingHitbox.AddChild(kick_mesh);
		}
	}

	public void RemoveHitbox()
	{
		if(kick_hitbox != null)
		{
			kick_hitbox.QueueFree();
			kick_hitbox= null;
		}
		
		EmitSignal(nameof(AbilityFinished), this);
	}

	public void RemoveMesh()
	{
		// GD.Print("remove mesh");
		if(kick_mesh != null)
		{
			kick_mesh.QueueFree();
			kick_mesh = null;
		}
	}

	public void _on_charge_timer_1_timeout()
	{
		
		if(ChargesUsed > 0)
		{
			// GD.Print("charge refilled");
			ChargesUsed -= 1;
		}
		
	}

	public void _on_charge_timer_2_timeout()
	{
		
		if(ChargesUsed > 0)
		{
			// GD.Print("charge refilled");
			ChargesUsed -= 1;
		}
		
	}
	public void _on_cast_timer_timeout()
	{
		// GD.Print("cast timer timeout");
		RemoveHitbox();
		RemoveMesh();
		EmitSignal(nameof(AbilityFinished), this);
	}
}
