using Godot;
using System;

public partial class Whirlwind : Ability
{

	[Export] private Timer tick_timer;
	[Export] public PackedScene hitbox_to_load;
	[Export] public PackedScene mesh_to_load;
	WhirlwindHitbox whirlwind_hitbox;
	MeshInstance3D whirlwind_mesh;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		resource_change = -10;
		rotate_on_soft = false;
		DamageModifier = 1.25f;
	}

	public override void Execute(Player player)
	{
		
		base.Execute(player);
		
		tick_timer.Start();
		
		if(whirlwind_hitbox == null)
		{
			AddHitbox(player);
		}
		
		melee_hitbox = whirlwind_hitbox;
		whirlwind_hitbox.type = MeleeHitbox.DamageType.Physical; // Set projectile damage type		
		DealDamage(player, DamageModifier);
	}

    public override void FrameCheck(Player player)
    {
		
        if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(tick_timer.TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityQueue), this);
				EmitSignal(nameof(AbilityCheck), this);
			}		
		}
		if(tick_timer.TimeLeft == 0) // If not held check if ability can be used
		{
			EmitSignal(nameof(AbilityCheck),this);
		}		
		if(Input.IsActionJustReleased(assigned_button) || player.Resource.CurrentValue + resource_change < 0 && player.ability_in_use == this)
		{
			// GD.Print("Remove hit box");
			
			RemoveHitbox();
			RemoveMesh(player);
			EmitSignal(nameof(AbilityFinished),this);
		}	
    }

    public void AddHitbox(Player player)
	{
		whirlwind_hitbox = (WhirlwindHitbox)hitbox_to_load.Instantiate(); // Instantiate the projectile
		player.surrounding_hitbox.AddChild(whirlwind_hitbox);
		if(whirlwind_mesh == null)
		{
			// GD.Print("Add mesh");
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
		tick_timer.Stop();
	}

	public void RemoveMesh(Player player)
	{
		// GD.Print("remove mesh");
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
