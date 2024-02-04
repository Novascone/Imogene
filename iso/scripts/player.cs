using Godot;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class player : CharacterBody3D
{
	
	private float speed = 5.0f;

	private NavigationAgent3D navigation_agent;
	private AnimationPlayer animation_player;
	private Area3D hitbox;
	private Area3D area_hit;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		navigation_agent = (NavigationAgent3D)GetNode("NavigationAgent3D");
		animation_player = (AnimationPlayer)GetNode("AnimationPlayer");
		hitbox = (Area3D)GetNode("WeaponPivot/WeaponMesh/Hitbox");
			
		hitbox.AreaEntered += OnHitboxAreaEntered;
		
		
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Attack"))
		{
			animation_player.Play("attack");
			hitbox.Monitoring = true;

		}
		
		if(Input.IsActionJustReleased("Attack"))
		{
			hitbox.Monitoring = false;

		}


		if(navigation_agent.IsNavigationFinished())
		{
			return;
		}
		
		move_to_point(delta, speed);

		
	}

	public void move_to_point(double delta, float speed)
	{
		var target_position = navigation_agent.GetNextPathPosition();
		var direction = GlobalPosition.DirectionTo(target_position);
		face_direction(target_position);
		Velocity = direction * speed;
		MoveAndSlide();
	}

	

	public void face_direction(Vector3 direction)
	{
		LookAt(direction with { Y = GlobalPosition.Y }, Vector3.Up);
	}

	public override void _Input(InputEvent @event)
	{
		
		if (Input.IsActionPressed("LeftMouse"))
		{
			Camera3D camera = (Camera3D)GetTree().GetNodesInGroup("Camera")[0];
			Vector2 mouse_pos = GetViewport().GetMousePosition();
			var ray_length = 100;
			var from = camera.ProjectRayOrigin(mouse_pos);
			var to = from + camera.ProjectRayNormal(mouse_pos) * ray_length;
			var space = GetWorld3D().DirectSpaceState;
			var ray_query = PhysicsRayQueryParameters3D.Create(from, to);
			ray_query.From = from;
			ray_query.To = to;
			ray_query.CollideWithAreas = true;
			var result = space.IntersectRay(ray_query);
			// GD.Print(result);

			navigation_agent.TargetPosition = result["position"].AsVector3();

		}
		
	}

	private void OnHitboxAreaEntered(Area3D area_hit)
	{
		if(area_hit.IsInGroup("enemy"))
		{
			GD.Print("enemy hit");
		}
	}


}
