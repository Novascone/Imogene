using Godot;
using System;
using System.Net.Http;

// Movement Controller
// Moves the player character around the world, sets basic animations, implements the landing icon, applies smooth rotation, handles looking at enemies, and prevents movement when UI is open
public partial class MovementController : Controller
{
	Vector3 ray_origin;
	Vector3 ray_target = new  Vector3(0, -4, 0);
	int ray_range = 2000;
	private float climb_speed = 4.0f;
	private float clamber_speed = 10.0f;
	
	// Called when the node enters the scene tree for the first time.
	private CustomSignals _customSignals; // Custom signal instance
	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.UIPreventingMovement += HandleUIPreventingMovement;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _PhysicsProcess(double delta)
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
			if(!player.is_climbing)
			{
				player.speed = player.walk_speed;
				if (Input.IsActionPressed("Right"))
				{
					player.direction.X -= 1.0f;	
					// GD.Print("Action strength right " + Input.GetActionStrength("Right"));	
				}
				if (Input.IsActionPressed("Left"))
				{
					player.direction.X += 1.0f;
					// GD.Print("Action strength left " + Input.GetActionStrength("Left"));	
				}
				if (Input.IsActionPressed("Backward"))
				{
					player.direction.Z -= 1.0f;
					// GD.Print("Action strength back " + Input.GetActionStrength("Backward"));	
				}
				if (Input.IsActionPressed("Forward"))
				{
					player.direction.Z += 1.0f;
					// GD.Print("Action strength forward " + Input.GetActionStrength("Forward"));	
				}
			}
			else
			{
				player.direction = Vector3.Zero;
				if(!player.is_clambering)
				{
					player.speed = climb_speed;
				}
				else
				{
					player.speed = clamber_speed;
					// GD.Print("Setting tween");
					// var vertical_movement = player.GlobalTransform.Origin + new Vector3(0,1.85f,0);
					// var vertical_move_time = 0.4;
					// var vm_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.In);

					// vm_tween.TweenProperty(player, "GlobalTransform:Origin", vertical_movement, vertical_move_time);

					// await ToSignal(vm_tween, Tween.SignalName.Finished);

					// var forward_movement = player.GlobalTransform.Origin + -player.Basis.Z;
					// var horizontal_move_time = 0.2;
					// var fm_tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Linear);

					// fm_tween.TweenProperty(player, "GlobalTransform:Origin", forward_movement, horizontal_move_time);

				}
				
				if (Input.IsActionPressed("Right"))
				{
					player.direction.X -= 1.0f;	
					// GD.Print("Action strength right " + Input.GetActionStrength("Right"));	
				}
				if (Input.IsActionPressed("Left"))
				{
					player.direction.X += 1.0f;
					// GD.Print("Action strength left " + Input.GetActionStrength("Left"));	
				}
				if (Input.IsActionPressed("Backward"))
				{
					player.direction.Y -= 1.0f;
					GD.Print("player moving down");
					GD.Print("player velocity y " + player.velocity.Y);
					// GD.Print("Action strength back " + Input.GetActionStrength("Backward"));	
				}
				if (Input.IsActionPressed("Forward"))
				{
					player.direction.Y += 1.0f;
					GD.Print("player moving up");
					GD.Print("player velocity y " + player.velocity.Y);

					// GD.Print("Action strength forward " + Input.GetActionStrength("Forward"));	
				}
				
				var rot = -(MathF.Atan2(player.near_wall.GetCollisionNormal().Z, player.near_wall.GetCollisionNormal().X) - MathF.PI/2); // Get the angle of rotation needed to face the object climbing
				
				
					player.vertical_input = Input.GetActionStrength("Forward") - Input.GetActionStrength("Backward");
				
				
				var horizontal_input = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
				if(!player.is_clambering)
				{
					player.direction = new Vector3(horizontal_input, player.vertical_input, player.move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***

				}
				player.direction = new Vector3(horizontal_input, player.vertical_input, player.move_forward_clamber).Rotated(Vector3.Up, rot).Normalized(); // Rotate the input so it is relative to the wall *** Might want to use this for playing animations when targeting an enemy ***
			}

			if(player.direction != Vector3.Zero && player.is_climbing)
			{
				player.current_y_rotation = -(MathF.Atan2(player.near_wall.GetCollisionNormal().Z, player.near_wall.GetCollisionNormal().X) - MathF.PI/2); // Set the player y rotation to the rotation needed to face the wall
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
			if(player.is_climbing)
			{
				player.velocity.Y = player.direction.Y * player.speed;
			}
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
