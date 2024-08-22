using Godot;
using System;

public partial class Whirlwind : Ability
{

	private Timer tick_timer;
	public PackedScene hitbox_to_load;
	public PackedScene mesh_to_load;
	WhirlwindHitbox whirlwind_hitbox;
	MeshInstance3D whirlwind_mesh;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tick_timer = GetNode<Timer>("TickTimer");
		hitbox_to_load = GD.Load<PackedScene>("res://scenes/abilities/whirlwind/whirlwind_hitbox.tscn");
		mesh_to_load = GD.Load<PackedScene>("res://scenes/abilities/whirlwind/whirlwind_mesh.tscn");
		resource_cost = 10;
		rotate_on_soft = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// if(CheckHeld()) // Check if button is held and only allow the player to rotate if it is
		// {
		// 	player.movement_controller.rotation_only = true;
		// }
		// if(Input.IsActionJustReleased(assigned_button)) // Allow the player to move fully if the button is released
		// {
		// 	player.movement_controller.rotation_only = false;
		// }
		
		if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		{
			
			GD.Print("Queuing whirlwind");
			QueueAbility();	
		}
		else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(tick_timer.TimeLeft == 0)
			{
				QueueAbility();
				CheckCanUseAbility();
			}		
		}
		if(tick_timer.TimeLeft == 0) // If not held check if ability can be used
		{
			CheckCanUseAbility();
		}		
		if(Input.IsActionJustReleased(assigned_button) || player.resource - resource_cost <= 0 && player.ability_in_use == this)
		{
			GD.Print("Remove hit box");
			RemoveHitbox();
			RemoveMesh();
		}	
		
	}

	public override void Execute()
	{
		
		GD.Print("execute");
		state = States.not_queued;
		AddToAbilityList(this); // Add ability to list
		
			// player.resource_system.Resource(channel_resource_cost);
		tick_timer.Start();
		
		if(whirlwind_hitbox == null)
		{
			AddHitbox();
		}
		
		player.resource_system.Resource(resource_cost);
		if(player.damage_system.Crit()) // check if the play will crit
		{
			whirlwind_hitbox.damage = MathF.Round(player.damage * (1 + player.critical_hit_damage), 2); // Set projectile damage
			whirlwind_hitbox.posture_damage = player.posture_damage / 3; // Set projectile posture damage 
			whirlwind_hitbox.is_critical = true;
		}
		else
		{
			
			whirlwind_hitbox.damage = player.damage; // Set projectile damage
			whirlwind_hitbox.posture_damage = player.posture_damage / 3; // Set projectile posture damage 
			whirlwind_hitbox.is_critical = false;
		}
		whirlwind_hitbox.damage_type = "physical"; // Set projectile damage type
		
	}

	public void AddHitbox()
	{
		whirlwind_hitbox = (WhirlwindHitbox)hitbox_to_load.Instantiate(); // Instantiate the projectile
		player.surrounding_hitbox.AddChild(whirlwind_hitbox);
		if(whirlwind_mesh == null)
		{
			GD.Print("Add mesh");
			whirlwind_mesh = (MeshInstance3D)mesh_to_load.Instantiate();
			player.surrounding_hitbox.AddChild(whirlwind_mesh);
		}
	}

	public void RemoveHitbox()
	{
		if(whirlwind_hitbox != null)
		{
			whirlwind_hitbox.QueueFree();
			whirlwind_hitbox = null;
		}
		
		RemoveFromAbilityList(this);
		tick_timer.Stop();
	}

	public void RemoveMesh()
	{
		GD.Print("remove mesh");
		if(whirlwind_mesh != null)
		{
			player.surrounding_hitbox.RemoveChild(whirlwind_mesh);
			whirlwind_mesh = null;
		}
		
	}

	

	public void _on_tick_timer_timeout()
	{
		RemoveHitbox();
	}
}
