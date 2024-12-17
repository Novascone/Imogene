using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime;
using System.Runtime.CompilerServices;

public partial class TargetingSystem : Node
{

	[Export] public DirectionalRayCast ray_cast { get; set; }

	// Enemies
	public bool enemy_near  { get; set; } = false;
	public bool enemy_far { get; set; } = false;
	public bool facing_enemy { get; set; } = false;
	public Enemy nearest_enemy { get; set; } = null;
	public Enemy enemy_pointed_toward { get; set; } = null;
	public	Dictionary<Enemy, Vector3> mobs { get; set; } = new Dictionary<Enemy, Vector3>();  // Dictionary of mob positions
	public Dictionary<Enemy,Vector3> sorted_mobs { get; set; } = new Dictionary<Enemy, Vector3>(); // Sorted Dictionary of mob positions
	public List<Enemy> mobs_in_order { get; set; } = new List<Enemy> (); // List of mobs in order
	public Dictionary<Enemy, Vector3> mobs_to_left { get; set; } = new();
	public Dictionary<Enemy,Vector3> sorted_left { get; set; } = new Dictionary<Enemy, Vector3> ();
	public List<Enemy> mobs_in_order_to_left { get; set; } = new List<Enemy> ();
	public Dictionary<Enemy, Vector3> mobs_to_right { get; set; } = new Dictionary<Enemy, Vector3>();
	public Dictionary<Enemy,Vector3> sorted_right { get; set; } = new Dictionary<Enemy, Vector3> ();
	public List<Enemy> mobs_in_order_to_right { get; set; } = new List<Enemy> ();
	public Enemy mob_looking_at { get; set; } = null;
	public Vector3 mob_to_LookAt_pos { get; set; } = Vector3.Zero; // Position of the mob that the player wants to face 

	// Targeting
	public bool rotating_to_soft_target { get; set; } = false;
	public bool target_pressed { get; set; } = false;
	public bool target_released { get; set; } = false;
	private bool ui_target_signal_emitted { get; set; } = false;
	public int frames_held { get; set; } = 0;
	public int held_threshold { get; set; } = 20;
	public bool targeting { get; set; } = false;
	public bool soft_target_on { get; set; } = true;
	public bool player_rotating { get; set; } = false;
	public bool soft_targeting { get; set; } = false;
	public bool enemy_to_soft_target { get; set; } = false;
	public float max_x_rotation { get; set; }  = 0.4f;
	public float min_x_rotation { get; set; }  = -0.4f;
	float facing_similarity { get; set; }  = 0.0f;
	float current_target_health { get; set; }  = 0.0f;
	Vector3 direction_to_enemy { get; set; } = Vector3.Zero;
	
	[Signal] public delegate void ShowSoftTargetIconEventHandler(Enemy enemy_);
	[Signal] public delegate void HideSoftTargetIconEventHandler(Enemy enemy_);
	[Signal] public delegate void PlayerTargetingEventHandler(bool is_targeting_);
	[Signal] public delegate void TargetHealthChangedEventHandler(Entity entity_, float health_);
	[Signal] public delegate void EnemyTargetedEventHandler(Enemy enemy_);
	[Signal] public delegate void EnemyUntargetedEventHandler();
	[Signal] public delegate void BrightenSoftTargetHUDEventHandler();
	[Signal] public delegate void DimSoftTargetHUDEventHandler();
	[Signal] public delegate void RotatingEventHandler();
	[Signal] public delegate void RotationForAbilityFinishedEventHandler(bool finished_);
	[Signal] public delegate void RotationForInputFinishedEventHandler(Player player_);

	public override void _Ready()
	{
		ray_cast.RemoveSoftTargetIcon += HandleRemoveSoftTargetIcon;
	}


	public override void _UnhandledInput(InputEvent @event_) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
		if(@event_.IsActionPressed("RS"))
		{
			target_pressed = true;
			target_released = false;
		}
		if(@event_.IsActionReleased("RS"))
		{
			target_pressed = false;
			target_released = true;
		}
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void Target(Player player_)
	{
		
		if(mob_looking_at != null && enemy_pointed_toward != null)
		{
			if(mob_looking_at != enemy_pointed_toward)
			{
				EmitSignal(nameof(RotationForAbilityFinished),false);
			}
		}

		if(mob_looking_at != null)
		{
		}
	
		ray_cast.GetInputStrength();
		if(ray_cast.AimInputStrength <= 0)
		{
			ray_cast.SetTargetingRayCastDirection();
		}
		else
		{
			ray_cast.SetTargetingRayCastAim();
		}
		
		ray_cast.GetRayCastCollisions();
		enemy_pointed_toward = ray_cast.GetEnemyWithMostCollisions();
		
		EnemyCheck();
		SoftTargetToggle();
		
		if(!rotating_to_soft_target) // If player is not rotation toward the soft target
		{
			Sort(player_); // sorts the enemies by position
		}
		else if(rotating_to_soft_target)
		{
			SoftTargetRotation(player_);
		}
		if(enemy_pointed_toward != null)
		{
			if(soft_target_on && !targeting)
			{
				enemy_pointed_toward.SoftTarget = true;
				EmitSignal(nameof(ShowSoftTargetIcon), enemy_pointed_toward);
				if(mobs_in_order.Count > 1)
				{
					foreach(Enemy _enemy in mobs_in_order)
					{
						if(_enemy != enemy_pointed_toward)
						{
							if(_enemy.UI.soft_target_icon.Visible)
							{
								_enemy.SoftTarget = false;
								EmitSignal(nameof(HideSoftTargetIcon), _enemy);
							}
							
						}
					}
				}
			}
		}
		else if(enemy_near)
		{
			nearest_enemy = mobs_in_order[0];
			nearest_enemy.SoftTarget = true;
			
			EmitSignal(nameof(ShowSoftTargetIcon), nearest_enemy);
			
			if(mobs_in_order.Count > 1)
			{
				foreach(Enemy _enemy in mobs_in_order)
				{
					if(_enemy != nearest_enemy)
					{
						if(_enemy.UI.soft_target_icon.Visible)
						{
							_enemy.SoftTarget = false;
							EmitSignal(nameof(HideSoftTargetIcon), _enemy);
						}
						
					}
				}
			}
		}

		LookAtEnemy(player_);
	
	}

	public void SoftTargetToggle()
	{
		if(target_pressed)
		{
			frames_held += 1;
		}
		else if(target_released)
		{
			if(frames_held > 0)
			{
			}
			if(frames_held >= held_threshold)
			{
				soft_target_on = !soft_target_on;
				if(!soft_target_on)
				{
					EmitSignal(nameof(DimSoftTargetHUD));
					if(nearest_enemy != null)
					{
						if(nearest_enemy.UI.soft_target_icon.Visible)
						{
							EmitSignal(nameof(HideSoftTargetIcon), nearest_enemy);
						}
					}
				}
				else
				{
					EmitSignal(nameof(BrightenSoftTargetHUD));
				}
			}
			
			frames_held = 0;
		}
	}

	public void EnemyEnteredNear(Enemy enemy_) // Called when enemy enters the small soft target zone
	{
		
		enemy_.InSoftTargetSmall = true;
		enemy_near = true;
	}

	public void EnemyExitedNear(Enemy enemy_) // Called when enemy exits the small soft target zone, checks to see if anymore enemies remain in the small soft zone
	{
		
		enemy_.InSoftTargetSmall = false;
		enemy_.SoftTarget = false;
		EmitSignal(nameof(HideSoftTargetIcon), enemy_);
		var mobs_in_small = 0;

		foreach(Enemy _enemy_in_mobs in mobs.Keys)
		{
			if(_enemy_in_mobs.InSoftTargetSmall)
			{
				mobs_in_small += 1;
			}
		}
		if(mobs_in_small == 0)
		{
			enemy_near = false;
			nearest_enemy = null;
		}
	}

	public void EnemyEnteredFar(Enemy enemy_) // Called when enemy enters the large soft zone, adds enemy to the dictionary of enemies
	{
		enemy_.InSoftTargetLarge = true;
		enemy_far = true;
		Vector3 enemy_position = enemy_.GlobalTransform.Origin;
		if(!mobs.ContainsKey(enemy_))
		{
			mobs.Add(enemy_, enemy_position); // adds mob to list and how close it is to the player
		}
	}

	public void EnemyExitedFar(Enemy enemy_) // Called when enemy exits the large soft zone, removes enemy from dictionary, and clears it if it's the last mob
	{
		enemy_.SoftTarget = false;
		if(enemy_.IsInGroup("enemy")) 
		{
			if(enemy_ == mob_looking_at)
			{
				mob_looking_at = null;
				targeting = false;
				ui_target_signal_emitted = false;
				EmitSignal(nameof(PlayerTargeting), targeting);
			}
			if (mobs.Count == 1)
			{
				mobs.Remove(enemy_);
				sorted_mobs.Clear();
				mobs_in_order.Clear();
				enemy_far = false;
			}
			else if(mobs.Count > 0)
			{
				mobs.Remove(enemy_);
			}
			var mobs_in_large = 0;

			foreach(Enemy _enemy_in_mobs in mobs.Keys)
			{
				if(_enemy_in_mobs.InSoftTargetSmall)
				{
					mobs_in_large += 1;
				}
			}
			if(mobs_in_large == 0)
			{
				// player.ui.hud.enemy_health.SetSoftTargetIcon(enemy);
				EmitSignal(nameof(HideSoftTargetIcon), enemy_);
				enemy_near = false;
				nearest_enemy = null;
			}
			{
				// player.ui.hud.enemy_health.SetSoftTargetIcon(enemy);
				EmitSignal(nameof(HideSoftTargetIcon), enemy_);
			}
		}
	}
	
	public void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		
		if(frames_held > 1)
		{
			if(frames_held < held_threshold && target_released)
			{
				if((!targeting && enemy_far) || (!targeting && enemy_pointed_toward != null)) // has player look at the closest enemy when targeting
				{
					targeting = true;
					EmitSignal(nameof(PlayerTargeting), targeting);
					
					if(mobs_in_order.Count > 0 && enemy_pointed_toward == null && mob_looking_at == null)
					{
						mob_to_LookAt_pos = mobs_in_order[0].GlobalPosition;
						mob_looking_at = mobs_in_order[0];
						current_target_health = mob_looking_at.Health.CurrentValue;
					}
					else if (enemy_pointed_toward != null)
					{
						mob_to_LookAt_pos = enemy_pointed_toward.GlobalPosition;
						mob_looking_at = enemy_pointed_toward;
						current_target_health = mob_looking_at.Health.CurrentValue;
					}
					
				}
				else if(targeting)
				{
					targeting = false;
					ui_target_signal_emitted = false;
					EmitSignal(nameof(PlayerTargeting), targeting);
					mob_looking_at = null;
					EmitSignal(nameof(EnemyUntargeted));
				}
				
			}
		}
		if (frames_held > held_threshold && enemy_near)
		{
			nearest_enemy.SoftTarget = false;
			EmitSignal(nameof(ShowSoftTargetIcon), nearest_enemy);
		}
		
		if(enemy_near || enemy_pointed_toward != null || targeting)
		{
			enemy_to_soft_target = true;
		}
		else
		{
			enemy_to_soft_target = false;
		}
		
		
	}

	public void LookAtEnemy(Player player_) // Look at enemy and switch between enemies
	{
		if(targeting && mobs_in_order.Count > 0)
		{
			if(Input.IsActionJustPressed("RS+"))
			{
				TargetRight();
			}
			else if (Input.IsActionJustPressed("RS-"))
			{
				TargetLeft();
			}
			
			HardTargetRotation(player_);
		}

	}

	public void TargetRight() // Check if there is an enemy to the right and switch to the enemy thats is closest to the current enemy targeted, and to the right of the player
	{
		ui_target_signal_emitted = false;
		if(mobs_in_order_to_right.Count >= 1)
		{
			mob_looking_at.Targeted = false;
			EmitSignal(nameof(EnemyUntargeted));
			if(mobs_in_order_to_right.Contains(mob_looking_at))
			{
				if(mob_looking_at == mobs_in_order_to_right[0])
				{
					if(mobs_in_order_to_right.Count >= mobs_in_order_to_right.IndexOf(mob_looking_at) + 2)
					{
						mob_to_LookAt_pos = mobs_in_order_to_right[mobs_in_order_to_right.IndexOf(mob_looking_at) + 1].GlobalPosition;
						mob_looking_at = mobs_in_order_to_right[mobs_in_order_to_right.IndexOf(mob_looking_at) + 1];
						mob_looking_at.Targeted = true;
					}
				}
				else
				{
					mob_to_LookAt_pos = mobs_in_order_to_right[0].GlobalPosition;
					mob_looking_at = mobs_in_order_to_right[0];
					mob_looking_at.Targeted = true;
				}
			}
			else
			{
				if(mobs_in_order_to_right.Count >= mobs_in_order_to_right.IndexOf(mob_looking_at) + 1)
				{
					mob_to_LookAt_pos = mobs_in_order_to_right[0].GlobalPosition;
					mob_looking_at = mobs_in_order_to_right[0];
					mob_looking_at.Targeted = true;
				}
			}
		}
	}

	public void TargetLeft() // Check if there is an enemy to the left and switch to the enemy thats is closest to the current enemy targeted, and to the left of the player
	{
		ui_target_signal_emitted = false;
		if(mobs_in_order_to_left.Count >= 1)
		{
			mob_looking_at.Targeted = false;
			EmitSignal(nameof(EnemyUntargeted));
			if(mobs_in_order_to_left.Contains(mob_looking_at))
			{
				if(mob_looking_at == mobs_in_order_to_left[0])
				{
					if(mobs_in_order_to_left.Count >= mobs_in_order_to_left.IndexOf(mob_looking_at) + 2)
					{
						mob_to_LookAt_pos = mobs_in_order_to_left[mobs_in_order_to_left.IndexOf(mob_looking_at) + 1].GlobalPosition;
						mob_looking_at = mobs_in_order_to_left[mobs_in_order_to_left.IndexOf(mob_looking_at) + 1];
						mob_looking_at.Targeted = true;
					}
				}
				else
				{
					mob_to_LookAt_pos = mobs_in_order_to_right[0].GlobalPosition;
					mob_looking_at = mobs_in_order_to_right[0];
					mob_looking_at.Targeted = true;
				}
			}
			else
			{
				if(mobs_in_order_to_left.Count >= mobs_in_order_to_left.IndexOf(mob_looking_at) + 1)
				{
					mob_to_LookAt_pos = mobs_in_order_to_left[0].GlobalPosition;
					mob_looking_at = mobs_in_order_to_left[0];
					mob_looking_at.Targeted = true;
				}
			}
		}
	}

	public void HardTargetRotation(Player player) // Smoothly rotate to hard target
	{
		
		player.PreviousYRotation = player.GlobalRotation.Y;
		if(player.IsOnFloor())
		{
			player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
		}
		else if (!soft_targeting)
		{
			// player.previous_x_rotation = player.GlobalRotation.X;
			player.PreviousXRotation = Mathf.Clamp(player.GlobalRotation.X, min_x_rotation, max_x_rotation);
			player.LookAt(mob_to_LookAt_pos with {Y = mob_looking_at.GlobalPosition.Y});
			// player.current_x_rotation = player.GlobalRotation.X;
			player.CurrentXRotation = Mathf.Clamp(player.GlobalRotation.X, min_x_rotation, max_x_rotation);
			if(player.PreviousXRotation != player.CurrentXRotation)
			{
				player.GlobalRotation = player.GlobalRotation with {X = Mathf.LerpAngle(player.PreviousXRotation, player.CurrentXRotation, 0.2f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
		if (!ui_target_signal_emitted)
		{
			EmitSignal(nameof(EnemyTargeted), mob_looking_at);
			ui_target_signal_emitted = true;
		}

		if(current_target_health != mob_looking_at.Health.CurrentValue)
		{
			current_target_health = mob_looking_at.Health.CurrentValue;
			EmitSignal(nameof(TargetHealthChanged), mob_looking_at, current_target_health);

		}
		
		player.CurrentYRotation = player.GlobalRotation.Y;
		if(player.PreviousYRotation != player.CurrentYRotation)
		{
			player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.PreviousYRotation, player.CurrentYRotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
		}
	}

	public void SoftTargetRotation(Player player)
	{
		if(player.GlobalRotation.X == -0)
		{
			player.PreviousXRotation = 0f;
			player.CurrentXRotation = 0f;
		}
		EmitSignal(nameof(Rotating));
		if(!targeting)
		{
			if(enemy_pointed_toward != null)
			{
				direction_to_enemy = player.GlobalPosition.DirectionTo(enemy_pointed_toward.GlobalPosition);
				if(mob_looking_at != enemy_pointed_toward)
				{
					mob_looking_at = enemy_pointed_toward;
					mob_to_LookAt_pos = enemy_pointed_toward.GlobalPosition;
				}

			}
			else if(enemy_near && (ray_cast.DirectionInputStrength < 0.25f || ray_cast.AimInputStrength < 0.25f))
			{
				
				mob_looking_at = nearest_enemy;
				mob_to_LookAt_pos = nearest_enemy.GlobalPosition;
				direction_to_enemy = player.GlobalPosition.DirectionTo(nearest_enemy.GlobalPosition);
			}
		}
		else
		{
			direction_to_enemy = player.GlobalPosition.DirectionTo(mob_looking_at.GlobalPosition);
		}
		
		if(enemy_pointed_toward != null || targeting || mob_looking_at != null)
		{
			player.PreviousYRotation = player.GlobalRotation.Y;
			if(player.IsOnFloor())
			{
				player.LookAt(mob_to_LookAt_pos with {Y = player.GlobalPosition.Y});
			}
			else
			{
				player.PreviousXRotation = Mathf.Clamp(player.GlobalRotation.X, min_x_rotation, max_x_rotation);
				player.LookAt(mob_to_LookAt_pos with {Y = mob_looking_at.GlobalPosition.Y});
				player.CurrentXRotation = Mathf.Clamp(player.GlobalRotation.X, min_x_rotation, max_x_rotation);
				
				if(player.PreviousXRotation != player.CurrentXRotation)
				{
					player.GlobalRotation = player.GlobalRotation with {X = Mathf.LerpAngle(player.PreviousXRotation, player.CurrentXRotation, 0.33f)}; // smoothly rotates between the previous angle and the new angle!
				}
			}
			player.CurrentYRotation = player.GlobalRotation.Y;
			if(player.PreviousYRotation != player.CurrentYRotation)
			{
				player.GlobalRotation = player.GlobalRotation with {Y = Mathf.LerpAngle(player.PreviousYRotation, player.CurrentYRotation, 0.33f)}; // smoothly rotates between the previous angle and the new angle!
				
			}

			
			SetMaxXRotation(player);
			SetSimilarity(player);


			if(enemy_pointed_toward == null && nearest_enemy != null && !targeting)
			{
				ray_cast.LookAt(nearest_enemy.GlobalPosition with {Y = ray_cast.GlobalPosition.Y});
			}

			
			if(-player.GlobalBasis.Z.Dot(direction_to_enemy) > facing_similarity && CheckSimilarRotations(player))
			{
				EmitSignal(nameof(RotationForAbilityFinished), true);
				EmitSignal(nameof(RotationForInputFinished), player);
				rotating_to_soft_target = false;
				facing_enemy = true;

				if(!targeting)
				{
					mob_looking_at = null;
				}
				
			}
			else
			{
				facing_enemy = false;
				EmitSignal(nameof(RotationForAbilityFinished), false);
			}
		}
		else
		{
			EmitSignal(nameof(RotationForAbilityFinished), true);
			EmitSignal(nameof(RotationForInputFinished), player);
		}
		
	}

	public void SetMaxXRotation(Player player_)
	{
		if(player_.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) > 13)
		{
			min_x_rotation = -0.5f;
			
		}
		else if(player_.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) < 13 && player_.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) > 9)
		{
			min_x_rotation = -0.7f;
		}
		else if(player_.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) < 9)
		{
			min_x_rotation = -1f;
		}
	}

	public void SetSimilarity(Player player_)
	{
		if(player_.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) > 10)
		{
			if(player_.IsOnFloor())
			{
				facing_similarity = 0.98f;
			}
			else
			{
				facing_similarity = 0.75f;
			}
			
		}
		else if(player_.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) < 10 && player_.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) > 6)
		{
			if(player_.IsOnFloor())
			{
				facing_similarity = 0.7f;
			}
			else
			{
				facing_similarity = 0.6f;
			}
			
		}
		else if(player_.GlobalPosition.DistanceTo(mob_looking_at.GlobalPosition) < 6)
		{
			if(player_.IsOnFloor())
			{
				facing_similarity = 0.6f;
			}
			else
			{
				facing_similarity = 0.5f;
			}
			
		}
	}

	public static bool CheckSimilarRotations(Player player)
	{
		if(Mathf.IsEqualApprox(MathF.Round(player.PreviousYRotation - player.CurrentYRotation, 2), 0))
		{
			
			if(Mathf.IsEqualApprox(MathF.Round(player.PreviousXRotation - player.CurrentXRotation, 2), 0))
			{
				player.PreviousXRotation = 0f;
				player.CurrentXRotation = 0f;
				return true;
			}
		}
		return false;
	}

	public void Sort(Player player) // Sort mobs by distance
	{
		sorted_mobs = Vector3DictionarySorter.SortByDistance(mobs, player.GlobalTransform.Origin);
		mobs_in_order = new List<Enemy>(sorted_mobs.Keys);

		foreach(Enemy enemy in sorted_mobs.Keys)
		{
			AssignEnemySide(player, enemy);
		}
		if(mob_looking_at != null) // sort the enemies to the left and right of the player if the player is looking at an enemy
		{
			sorted_right = Vector3DictionarySorter.SortByDistance(mobs_to_right, mob_looking_at.GlobalTransform.Origin);
			mobs_in_order_to_right = new List<Enemy>(sorted_right.Keys);
			sorted_left = Vector3DictionarySorter.SortByDistance(mobs_to_left, mob_looking_at.GlobalTransform.Origin);
			mobs_in_order_to_left = new List<Enemy>(sorted_left.Keys);
		}
	}

	public void AssignEnemySide(Player player_, Enemy enemy_) // Determine if an enemy is to the left or right of the player
	{
		Vector3 enemy_position = enemy_.GlobalTransform.Origin;
		var angle_between_player_and_enemy = Mathf.RadToDeg(-(-player_.Transform.Basis.X.AngleTo(player_.GlobalPosition.DirectionTo(enemy_.GlobalPosition))));
		if(angle_between_player_and_enemy >= 0 && angle_between_player_and_enemy < 90)
		{
			
			if(!mobs_to_right.ContainsKey(enemy_))
			{
				mobs_to_right.Add(enemy_, enemy_position);
			}
			if(mobs_to_left.ContainsKey(enemy_))
			{
				mobs_to_left.Remove(enemy_);
			}
		}
		else if (angle_between_player_and_enemy >= 90 && angle_between_player_and_enemy < 180)
		{
			if(!mobs_to_left.ContainsKey(enemy_))
			{
				mobs_to_left.Add(enemy_, enemy_position);
			}
			if(mobs_to_right.ContainsKey(enemy_))
			{
				mobs_to_right.Remove(enemy_);
			}
		}
		else
		{
			if(mobs_to_right.ContainsKey(enemy_))                                                                                                                                                                                                                                    
			{
				mobs_to_right.Remove(enemy_);
			}
			if(mobs_to_left.ContainsKey(enemy_))
			{
				mobs_to_left.Remove(enemy_);
			}
		}
	}

    internal void HandleRotatePlayer()
    {
        rotating_to_soft_target = true;
    }

	private void HandleRemoveSoftTargetIcon(Enemy enemy_)
    {
        EmitSignal(nameof(HideSoftTargetIcon), enemy_);
    }

    

    public static class Vector3DictionarySorter // Sorts mobs by distance
	{
		public static Dictionary<Enemy, Vector3> SortByDistance(Dictionary<Enemy, Vector3> dict, Vector3 point)
		{
			var sortedList = dict.ToList();

			sortedList.Sort((pair1, pair2) => pair1.Value.DistanceTo(point).CompareTo(pair2.Value.DistanceTo(point)));

			return sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}
}
