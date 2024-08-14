using Godot;
using System;

public partial class Hitscan : Ranged
{
	
	public Sprite3D cast_marker;
	public PackedScene scene;
	public Timer cast_timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scene = GD.Load<PackedScene>("res://scenes/debug/cast_marker.tscn");
		cast_timer = GetNode<Timer>("CastTimer");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsActionJustPressed(assigned_button) && !ready_to_use)
		{
			ready_to_use = true;
		}
		if(ready_to_use)
		{
			if(player.can_use_abilities && useable && CheckCross() && cast_timer.TimeLeft == 0)
			{
				if(!player.targeting && player.targeting_system.closest_enemy_soft != null && player.targeting_system.soft_target_on)
				{
					player.targeting_system.SoftTargetRotation();
					if(MathF.Round(player.current_y_rotation - player.prev_y_rotation, 1) == 0)
					{
						Execute();
					}
				}
				else
				{
					GD.Print("execute with out targeting");
					Execute();
				}
			}
		
		}		
	}

	public override void Execute()
	{
		// GD.Print("Casting");
		ready_to_use = false;
		AddToAbilityList(this);
		cast_timer.Start();
		Vector3 collision = GetPlayerCollision();
		HitscanCollision(collision);
	}

	

	public void HitscanCollision(Vector3 collision_point)
	{
		Vector3 cast_direction = (collision_point - player.cast_point.GlobalTransform.Origin).Normalized();
		var new_intersection = PhysicsRayQueryParameters3D.Create(player.cast_point.GlobalTransform.Origin, collision_point + cast_direction * 2);
		new_intersection.CollisionMask = 16;
		new_intersection.CollideWithAreas = true;
		new_intersection.Exclude = player.exclude;
		var cast_collision = GetWorld3D().DirectSpaceState.IntersectRay(new_intersection);

		if(cast_collision.Count > 0)
		{
			HitscanDamage((Node3D)cast_collision["collider"]);
			Node3D collider = (Node3D)cast_collision["collider"];
			var scene_node = scene.Instantiate();
			cast_marker = (Sprite3D)scene_node;
			var world = GetTree().Root;
			world.AddChild(cast_marker);
			cast_marker.GlobalTranslate(cast_collision["position"].AsVector3());
			GD.Print("collided with" + collider.Name);
			
			
		}
	}

	public void HitscanDamage(Node3D collider)
	{
		if(collider is	Hurtbox hurtbox)
		{
			
			if(hurtbox.Owner is Enemy enemy)
			{
				enemy.damage_system.TakeDamage("cold",10.0f,false);
			}
			
		}
	}

	public void _on_cast_timer_timeout()
	{
		RemoveFromAbilityList(this);
	}
}
