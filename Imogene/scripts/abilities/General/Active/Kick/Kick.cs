using Godot;
using System;

public partial class Kick : Ability
{

	private Timer charge_timer_1;
	private Timer charge_timer_2;
	private Timer cast_timer;
	public PackedScene hitbox_to_load;
	public PackedScene mesh_to_load;
	KickHitbox kick_hitbox;
	MeshInstance3D kick_mesh;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hitbox_to_load = GD.Load<PackedScene>("res://scenes/abilities/kick/kick_hitbox.tscn");
		mesh_to_load = GD.Load<PackedScene>("res://scenes/abilities/kick/kick_mesh.tscn");
		charge_timer_1 = GetNode<Timer>("ChargeTimer1");
		charge_timer_2 = GetNode<Timer>("ChargeTimer2");
		cast_timer = GetNode<Timer>("CastTimer");
		charges = 2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		{
			
			// GD.Print("Queuing kick");
			QueueAbility();	
		}
		else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(cast_timer. TimeLeft == 0)
			{
				QueueAbility();
				CheckCanUseAbility();
			}
			
		
		}
		if(cast_timer.TimeLeft == 0)
		{
			CheckCanUseAbility();
		}
		
		
	}

	public override void Execute()
	{

		state = States.not_queued;
		charges_used += 1;
		AddToAbilityList(this); // Add ability to list
		cast_timer.Start();
		if(charge_timer_1.TimeLeft == 0)
		{
			charge_timer_1.Start();
		}
		else
		{
			charge_timer_2.Start();
		}
		
		
		AddHitbox();
		
		if(player.damage_system.Crit()) // check if the play will crit
		{
			kick_hitbox.damage = MathF.Round(player.damage * (1 + player.critical_hit_damage), 2) / 2; // Set projectile damage
			kick_hitbox.posture_damage = player.posture_damage * 3; // Set projectile posture damage 
			kick_hitbox.is_critical = true;
		}
		else
		{
			
			kick_hitbox.damage = player.damage / 2; // Set projectile damage
			kick_hitbox.posture_damage = player.posture_damage * 2; // Set projectile posture damage 
			kick_hitbox.is_critical = false;
		}
		kick_hitbox.damage_type = "physical"; // Set projectile damage type
		
	}

	public void AddHitbox()
	{
		kick_hitbox = (KickHitbox)hitbox_to_load.Instantiate(); // Instantiate the projectile
		player.belt_slot.AddChild(kick_hitbox);
		if(kick_mesh == null)
		{
			// GD.Print("Add mesh");
			kick_mesh = (MeshInstance3D)mesh_to_load.Instantiate();
			player.belt_slot.AddChild(kick_mesh);
		}
	}

	public void RemoveHitbox()
	{
		if(kick_hitbox != null)
		{
			kick_hitbox.QueueFree();
			kick_hitbox= null;
		}
		
		RemoveFromAbilityList(this);
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
		
		if(charges_used > 0)
		{
			// GD.Print("charge refilled");
			charges_used -= 1;
		}
		
	}

	public void _on_charge_timer_2_timeout()
	{
		
		if(charges_used > 0)
		{
			// GD.Print("charge refilled");
			charges_used -= 1;
		}
		
	}
	public void _on_cast_timer_timeout()
	{
		// GD.Print("cast timer timeout");
		RemoveHitbox();
		RemoveMesh();
	}
}
