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
		
		ResourceChange = -10;
		RotateOnSoft = false;
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
		
		MeleeHitbox = whirlwind_hitbox;
		whirlwind_hitbox.Type = MeleeHitbox.DamageType.Physical; // Set projectile damage type		
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
		if(Input.IsActionJustReleased(AssignedButton) || player.Resource.CurrentValue + ResourceChange < 0 && player.AbilityInUse == this)
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
		player.SurroundingHitbox.AddChild(whirlwind_hitbox);
		if(whirlwind_mesh == null)
		{
			// GD.Print("Add mesh");
			whirlwind_mesh = (MeshInstance3D)mesh_to_load.Instantiate();
			player.SurroundingHitbox.AddChild(whirlwind_mesh);
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
			player.SurroundingHitbox.RemoveChild(whirlwind_mesh);
			whirlwind_mesh = null;
		}
		
	}

	public void _on_tick_timer_timeout()
	{
		RemoveHitbox();
	}
}
