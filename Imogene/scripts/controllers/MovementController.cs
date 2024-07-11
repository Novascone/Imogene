using Godot;
using System;

// Movement Controller
// Moves the player character around the world, sets basic animations, implements the landing icon, applies smooth rotation, handles looking at enemies, and prevents movement when UI is open
public partial class MovementController : Controller
{
	Vector3 ray_origin;
	Vector3 ray_target = new  Vector3(0, -4, 0);
	int ray_range = 2000;
	
	// Called when the node enters the scene tree for the first time.
	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.UIPreventingMovement += HandleUIPreventingMovement;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		player.Fall(delta);

		ray_origin = player.GlobalTransform.Origin;
		ray_target = player.GlobalTransform.Origin + new Vector3(0, -20, 0);
		var spaceState = player.GetWorld3D().DirectSpaceState;
		var ray_query = PhysicsRayQueryParameters3D.Create(ray_origin, ray_target);
		ray_query.CollideWithAreas = true;
		ray_query.Exclude = player.exclude;
		var ray = spaceState.IntersectRay(ray_query);
		
		if(player.ui.inventory.Visible || player.ui.abilities_open && !player.ui.abilities_secondary_ui_open || player.attacking) 
		{
			player.can_move = false;
		}
		else if (!player.ui.inventory_open || !player.ui.abilities_open && player.ui.abilities_secondary_ui_open)
		{
			player.can_move = true;
		}
		
		if(ray.Count > 0)
		{
			player.land_point_position = ray["position"].AsVector3();
			player.land_point.Show();
		}

		if(player.velocity == Vector3.Zero) // If not moving return to Idle slowly (hence the lerp)
		{
			player.blend_direction.X = Mathf.Lerp(player.blend_direction.X, 0, 0.1f);
			player.blend_direction.Y = Mathf.Lerp(player.blend_direction.Y, 0, 0.1f);
		}

		if(player.can_move) // Basic movement controller
		{
			if (Input.IsActionPressed("Right"))
			{
				player.direction.X -= 1.0f;		
			}
			if (Input.IsActionPressed("Left"))
			{
				player.direction.X += 1.0f;
			}
			if (Input.IsActionPressed("Backward"))
			{
				player.direction.Z -= 1.0f;
			}
			if (Input.IsActionPressed("Forward"))
			{
				player.direction.Z += 1.0f;
			}
		}
		if(player.can_move == false)
		{
			player.velocity.X = 0;
			player.velocity.Z = 0;
		}

		player.SmoothRotation(); // Rotate the player character smoothly
		player.LookAtOver(); // Look at mob and handle switching

		if(!player.using_movement_ability)
		{
			player.velocity.X = player.direction.X * player.speed;
			player.velocity.Z = player.direction.Z * player.speed;
		}
		player.Velocity = player.velocity;
		player.land_point.GlobalPosition = player.land_point_position;
		player.tree.Set("parameters/Master/Main/IW/blend_position", player.blend_direction);
		player.tree.Set("parameters/Master/Ability/Ability_1/Recovery_1/Walk_Recovery/blend_position", player.blend_direction);
		player.tree.Set("parameters/Master/Ability/Ability_1/Melee_Recovery_1/Slash/One_Handed_Slash_recovery_1/One_Handed_Medium_Recovery/Walk_Recovery/blend_position", player.blend_direction); // Set blend position
		// tree.Set("parameters/Master/Attack/AttackSpeed/scale", attack_speed);
		player.MoveAndSlide();
	}
	// private void HandleUIPreventingMovement(bool ui_preventing_movement) // Check if UI is preventing movement
    // {
	// 	if(player != null)
	// 	{
	// 		player.can_move = !ui_preventing_movement;
	// 		player.can_use_abilities = !ui_preventing_movement;
	// 		player.velocity = Vector3.Zero;
	// 	}
        
    // }
}
