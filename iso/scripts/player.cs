using Godot;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public partial class player : CharacterBody3D
{
	private float speed = 5.0f; // speed of character

	private NavigationAgent3D navigation_agent; // navigation agent 
	private AnimationPlayer animation_player; // animation player
	private Area3D hitbox; // weapon hitbox
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		navigation_agent = (NavigationAgent3D)GetNode("NavigationAgent3D"); // get reference to NavigationAgent3D
		animation_player = (AnimationPlayer)GetNode("AnimationPlayer"); // get reference to AnimationPlater
		hitbox = (Area3D)GetNode("WeaponPivot/WeaponMesh/Hitbox"); // get reference to Hitbox
		hitbox.AreaEntered += OnHitboxAreaEntered; // subscribe hitbox to OnHitboxAreaEntered signal
		
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
	{
		attack_check();

		if(navigation_agent.IsNavigationFinished())
		{
			return;
		}
		
		move_to_point(delta, speed); // moves character to point

	}

	public void move_to_point(double delta, float speed) // moves character to point
	{
		var target_position = navigation_agent.GetNextPathPosition(); // use the information taken from the _Input function to set the target position
		var direction = GlobalPosition.DirectionTo(target_position); // move navigation agent toward the target position
		face_direction(target_position); // face direction moving
		Velocity = direction * speed; // set velocity
		MoveAndSlide(); // Godot physics
	}

	

	public void face_direction(Vector3 direction)
	{
		LookAt(direction with { Y = GlobalPosition.Y }, Vector3.Up); // rotates character to face direction moving
	}

	public override void _Input(InputEvent @event) // function to move character
	{
		
		if (Input.IsActionPressed("LeftMouse"))
		{
			Camera3D camera = (Camera3D)GetTree().GetNodesInGroup("Camera")[0]; // get reference to camera
			Vector2 mouse_pos = GetViewport().GetMousePosition(); // get 2d mouse position
			var ray_length = 100;
			var from = camera.ProjectRayOrigin(mouse_pos); // set origin of ray
			var to = from + camera.ProjectRayNormal(mouse_pos) * ray_length; // set destination of character
			var space = GetWorld3D().DirectSpaceState; // collision detection to see what we are clicking on
			var ray_query = PhysicsRayQueryParameters3D.Create(from, to); // creates an object that has a dictionary of location based on when the ray collides 
			ray_query.From = from;
			ray_query.To = to;
			ray_query.CollideWithAreas = true;
			var result = space.IntersectRay(ray_query); // get location and information of where the ray went
			/* result can be used to get information about what you are clicking on, for instance if you are clicking on an enemy you can use get the parent of
			   the collider to detect if it is in fact and enemy.
			GD.Print(result);
			*/ 

			navigation_agent.TargetPosition = result["position"].AsVector3(); // setting navigation agent target position

		}
		
	}

	public void attack_check() // changes weapon hitbox monitoring based on animation
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
	}

	private void OnHitboxAreaEntered(Area3D area_hit) // handler for area entered signal
	{
		if(area_hit.IsInGroup("enemy")) // checks if the area entered is in the enemy group
		{
			GD.Print("enemy hit");
		}
	}


}
