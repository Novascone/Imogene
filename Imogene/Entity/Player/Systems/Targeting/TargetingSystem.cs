using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime;
using System.Runtime.CompilerServices;

public partial class TargetingSystem : Node
{

	[Export] public DirectionalRayCast RayCast { get; set; }

	// Enemies
	public bool EnemyNear  { get; set; } = false;
	public bool EnemyFar { get; set; } = false;
	public bool FacingEnemy { get; set; } = false;
	public Enemy NearestEnemy { get; set; } = null;
	public Enemy EnemyPointedToward { get; set; } = null;
	public	Dictionary<Enemy, Vector3> Mobs { get; set; } = new Dictionary<Enemy, Vector3>();  // Dictionary of mob positions
	public Dictionary<Enemy,Vector3> SortedMobs { get; set; } = new Dictionary<Enemy, Vector3>(); // Sorted Dictionary of mob positions
	public List<Enemy> MobsInOrder { get; set; } = new List<Enemy> (); // List of mobs in order
	public Dictionary<Enemy, Vector3> MobsToLeft { get; set; } = new();
	public Dictionary<Enemy,Vector3> SortedLeft { get; set; } = new Dictionary<Enemy, Vector3> ();
	public List<Enemy> MobsInOrderToLeft { get; set; } = new List<Enemy> ();
	public Dictionary<Enemy, Vector3> MobsToRight { get; set; } = new Dictionary<Enemy, Vector3>();
	public Dictionary<Enemy,Vector3> SortedRight { get; set; } = new Dictionary<Enemy, Vector3> ();
	public List<Enemy> MobsInOrderToRight { get; set; } = new List<Enemy> ();
	public Enemy MobLookingAt { get; set; } = null;
	public Vector3 MobToLookAtPosition { get; set; } = Vector3.Zero; // Position of the mob that the player wants to face 

	// Targeting
	public bool RotatingToSoftTarget { get; set; } = false;
	public bool TargetPressed { get; set; } = false;
	public bool TargetReleased { get; set; } = false;
	private bool UITargetSignalEmitted { get; set; } = false;
	public int FramesHeld { get; set; } = 0;
	public int HeldThreshold { get; set; } = 20;
	public bool Targeting { get; set; } = false;
	public bool SoftTargetOn { get; set; } = true;
	public bool PlayerRotating { get; set; } = false;
	public bool SoftTargeting { get; set; } = false;
	public bool EnemyToSoftTarget { get; set; } = false;
	public float MaxXRotation { get; set; }  = 0.4f;
	public float MinXRotation { get; set; }  = -0.4f;
	float FacingSimilarity { get; set; }  = 0.0f;
	float CurrentTargetHealth { get; set; }  = 0.0f;
	Vector3 DirectionToEnemy { get; set; } = Vector3.Zero;
	
	[Signal] public delegate void ShowSoftTargetIconEventHandler(Enemy enemy);
	[Signal] public delegate void HideSoftTargetIconEventHandler(Enemy enemy);
	[Signal] public delegate void PlayerTargetingEventHandler(bool isTargeting);
	[Signal] public delegate void TargetHealthChangedEventHandler(Entity entity, float health);
	[Signal] public delegate void EnemyTargetedEventHandler(Enemy enemy);
	[Signal] public delegate void EnemyUntargetedEventHandler();
	[Signal] public delegate void BrightenSoftTargetHUDEventHandler();
	[Signal] public delegate void DimSoftTargetHUDEventHandler();
	[Signal] public delegate void RotatingEventHandler();
	[Signal] public delegate void RotationForAbilityFinishedEventHandler(bool finished);
	[Signal] public delegate void RotationForInputFinishedEventHandler(Player player);

	public override void _Ready()
	{
		RayCast.RemoveSoftTargetIcon += HandleRemoveSoftTargetIcon;
	}


	public override void _UnhandledInput(InputEvent @event) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
		if(@event.IsActionPressed("RS"))
		{
			TargetPressed = true;
			TargetReleased = false;
		}
		if(@event.IsActionReleased("RS"))
		{
			TargetPressed = false;
			TargetReleased = true;
		}
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void Target(Player player)
	{
		
		if(MobLookingAt != null && EnemyPointedToward != null)
		{
			if(MobLookingAt != EnemyPointedToward)
			{
				EmitSignal(nameof(RotationForAbilityFinished),false);
			}
		}

		if(MobLookingAt != null)
		{
		}
	
		RayCast.GetInputStrength();
		if(RayCast.AimInputStrength <= 0)
		{
			RayCast.SetTargetingRayCastDirection();
		}
		else
		{
			RayCast.SetTargetingRayCastAim();
		}
		
		RayCast.GetRayCastCollisions();
		EnemyPointedToward = RayCast.GetEnemyWithMostCollisions();
		
		EnemyCheck();
		SoftTargetToggle();
		
		if(!RotatingToSoftTarget) // If player is not rotation toward the soft target
		{
			Sort(player); // sorts the enemies by position
		}
		else if(RotatingToSoftTarget)
		{
			SoftTargetRotation(player);
		}
		if(EnemyPointedToward != null)
		{
			if(SoftTargetOn && !Targeting)
			{
				EnemyPointedToward.SoftTarget = true;
				EmitSignal(nameof(ShowSoftTargetIcon), EnemyPointedToward);
				if(MobsInOrder.Count > 1)
				{
					foreach(Enemy enemy in MobsInOrder)
					{
						if(enemy != EnemyPointedToward)
						{
							if(enemy.UI.SoftTargetIcon.Visible)
							{
								enemy.SoftTarget = false;
								EmitSignal(nameof(HideSoftTargetIcon), enemy);
							}
							
						}
					}
				}
			}
		}
		else if(EnemyNear)
		{
			NearestEnemy = MobsInOrder[0];
			NearestEnemy.SoftTarget = true;
			
			EmitSignal(nameof(ShowSoftTargetIcon), NearestEnemy);
			
			if(MobsInOrder.Count > 1)
			{
				foreach(Enemy enemy in MobsInOrder)
				{
					if(enemy != NearestEnemy)
					{
						if(enemy.UI.SoftTargetIcon.Visible)
						{
							enemy.SoftTarget = false;
							EmitSignal(nameof(HideSoftTargetIcon), enemy);
						}
						
					}
				}
			}
		}

		LookAtEnemy(player);
	
	}

	public void SoftTargetToggle()
	{
		if(TargetPressed)
		{
			FramesHeld += 1;
		}
		else if(TargetReleased)
		{
			if(FramesHeld > 0)
			{
			}
			if(FramesHeld >= HeldThreshold)
			{
				SoftTargetOn = !SoftTargetOn;
				if(!SoftTargetOn)
				{
					EmitSignal(nameof(DimSoftTargetHUD));
					if(NearestEnemy != null)
					{
						if(NearestEnemy.UI.SoftTargetIcon.Visible)
						{
							EmitSignal(nameof(HideSoftTargetIcon), NearestEnemy);
						}
					}
				}
				else
				{
					EmitSignal(nameof(BrightenSoftTargetHUD));
				}
			}
			
			FramesHeld = 0;
		}
	}

	public void EnemyEnteredNear(Enemy enemy) // Called when enemy enters the small soft target zone
	{
		enemy.InSoftTargetSmall = true;
		EnemyNear = true;
	}

	public void EnemyExitedNear(Enemy enemy) // Called when enemy exits the small soft target zone, checks to see if anymore enemies remain in the small soft zone
	{
		
		enemy.InSoftTargetSmall = false;
		enemy.SoftTarget = false;
		EmitSignal(nameof(HideSoftTargetIcon), enemy);
		var mobsInSmall = 0;

		foreach(Enemy enemyInMobs in Mobs.Keys)
		{
			if(enemyInMobs.InSoftTargetSmall)
			{
				mobsInSmall += 1;
			}
		}
		if(mobsInSmall == 0)
		{
			EnemyNear = false;
			NearestEnemy = null;
		}
	}

	public void EnemyEnteredFar(Enemy enemy) // Called when enemy enters the large soft zone, adds enemy to the dictionary of enemies
	{
		enemy.InSoftTargetLarge = true;
		EnemyFar = true;
		Vector3 enemyPosition = enemy.GlobalTransform.Origin;
		if(!Mobs.ContainsKey(enemy))
		{
			Mobs.Add(enemy, enemyPosition); // adds mob to list and how close it is to the player
		}
	}

	public void EnemyExitedFar(Enemy enemy) // Called when enemy exits the large soft zone, removes enemy from dictionary, and clears it if it's the last mob
	{
		enemy.SoftTarget = false;
		if(enemy.IsInGroup("enemy")) 
		{
			if(enemy == MobLookingAt)
			{
				MobLookingAt = null;
				Targeting = false;
				UITargetSignalEmitted = false;
				EmitSignal(nameof(PlayerTargeting), Targeting);
			}
			if (Mobs.Count == 1)
			{
				Mobs.Remove(enemy);
				SortedMobs.Clear();
				MobsInOrder.Clear();
				EnemyFar = false;
			}
			else if(Mobs.Count > 0)
			{
				Mobs.Remove(enemy);
			}
			var mobsInLarge = 0;

			foreach(Enemy enemyInMobs in Mobs.Keys)
			{
				if(enemyInMobs.InSoftTargetSmall)
				{
					mobsInLarge += 1;
				}
			}
			if(mobsInLarge == 0)
			{
				// player.ui.hud.enemy_health.SetSoftTargetIcon(enemy);
				EmitSignal(nameof(HideSoftTargetIcon), enemy);
				EnemyNear = false;
				NearestEnemy = null;
			}
			{
				// player.ui.hud.enemy_health.SetSoftTargetIcon(enemy);
				EmitSignal(nameof(HideSoftTargetIcon), enemy);
			}
		}
	}
	
	public void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		
		if(FramesHeld > 1)
		{
			if(FramesHeld < HeldThreshold && TargetReleased)
			{
				if((!Targeting && EnemyFar) || (!Targeting && EnemyPointedToward != null)) // has player look at the closest enemy when targeting
				{
					Targeting = true;
					EmitSignal(nameof(PlayerTargeting), Targeting);
					
					if(MobsInOrder.Count > 0 && EnemyPointedToward == null && MobLookingAt == null)
					{
						MobToLookAtPosition = MobsInOrder[0].GlobalPosition;
						MobLookingAt = MobsInOrder[0];
						CurrentTargetHealth = MobLookingAt.Health.CurrentValue;
					}
					else if (EnemyPointedToward != null)
					{
						MobToLookAtPosition = EnemyPointedToward.GlobalPosition;
						MobLookingAt = EnemyPointedToward;
						CurrentTargetHealth = MobLookingAt.Health.CurrentValue;
					}
					
				}
				else if(Targeting)
				{
					Targeting = false;
					UITargetSignalEmitted = false;
					EmitSignal(nameof(PlayerTargeting), Targeting);
					MobLookingAt = null;
					EmitSignal(nameof(EnemyUntargeted));
				}
				
			}
		}
		if (FramesHeld > HeldThreshold && EnemyNear)
		{
			NearestEnemy.SoftTarget = false;
			EmitSignal(nameof(ShowSoftTargetIcon), NearestEnemy);
		}
		
		if(EnemyNear || EnemyPointedToward != null || Targeting)
		{
			EnemyToSoftTarget = true;
		}
		else
		{
			EnemyToSoftTarget = false;
		}
		
		
	}

	public void LookAtEnemy(Player player) // Look at enemy and switch between enemies
	{
		if(Targeting && MobsInOrder.Count > 0)
		{
			if(Input.IsActionJustPressed("RS+"))
			{
				TargetRight();
			}
			else if (Input.IsActionJustPressed("RS-"))
			{
				TargetLeft();
			}
			
			HardTargetRotation(player);
		}

	}

	public void TargetRight() // Check if there is an enemy to the right and switch to the enemy thats is closest to the current enemy targeted, and to the right of the player
	{
		UITargetSignalEmitted = false;
		if(MobsInOrderToRight.Count >= 1)
		{
			MobLookingAt.Targeted = false;
			EmitSignal(nameof(EnemyUntargeted));
			if(MobsInOrderToRight.Contains(MobLookingAt))
			{
				if(MobLookingAt == MobsInOrderToRight[0])
				{
					if(MobsInOrderToRight.Count >= MobsInOrderToRight.IndexOf(MobLookingAt) + 2)
					{
						MobToLookAtPosition = MobsInOrderToRight[MobsInOrderToRight.IndexOf(MobLookingAt) + 1].GlobalPosition;
						MobLookingAt = MobsInOrderToRight[MobsInOrderToRight.IndexOf(MobLookingAt) + 1];
						MobLookingAt.Targeted = true;
					}
				}
				else
				{
					MobToLookAtPosition = MobsInOrderToRight[0].GlobalPosition;
					MobLookingAt = MobsInOrderToRight[0];
					MobLookingAt.Targeted = true;
				}
			}
			else
			{
				if(MobsInOrderToRight.Count >= MobsInOrderToRight.IndexOf(MobLookingAt) + 1)
				{
					MobToLookAtPosition = MobsInOrderToRight[0].GlobalPosition;
					MobLookingAt = MobsInOrderToRight[0];
					MobLookingAt.Targeted = true;
				}
			}
		}
	}

	public void TargetLeft() // Check if there is an enemy to the left and switch to the enemy thats is closest to the current enemy targeted, and to the left of the player
	{
		UITargetSignalEmitted = false;
		if(MobsInOrderToLeft.Count >= 1)
		{
			MobLookingAt.Targeted = false;
			EmitSignal(nameof(EnemyUntargeted));
			if(MobsInOrderToLeft.Contains(MobLookingAt))
			{
				if(MobLookingAt == MobsInOrderToLeft[0])
				{
					if(MobsInOrderToLeft.Count >= MobsInOrderToLeft.IndexOf(MobLookingAt) + 2)
					{
						MobToLookAtPosition = MobsInOrderToLeft[MobsInOrderToLeft.IndexOf(MobLookingAt) + 1].GlobalPosition;
						MobLookingAt = MobsInOrderToLeft[MobsInOrderToLeft.IndexOf(MobLookingAt) + 1];
						MobLookingAt.Targeted = true;
					}
				}
				else
				{
					MobToLookAtPosition = MobsInOrderToLeft[0].GlobalPosition;
					MobLookingAt = MobsInOrderToLeft[0];
					MobLookingAt.Targeted = true;
				}
			}
			else
			{
				if(MobsInOrderToLeft.Count >= MobsInOrderToLeft.IndexOf(MobLookingAt) + 1)
				{
					MobToLookAtPosition = MobsInOrderToLeft[0].GlobalPosition;
					MobLookingAt = MobsInOrderToLeft[0];
					MobLookingAt.Targeted = true;
				}
			}
		}
	}

	public void HardTargetRotation(Player player) // Smoothly rotate to hard target
	{
		
		player.PreviousYRotation = player.GlobalRotation.Y;
		if(player.IsOnFloor())
		{
			player.LookAt(MobToLookAtPosition with {Y = player.GlobalPosition.Y});
		}
		else if (!SoftTargeting)
		{
			// player.previous_x_rotation = player.GlobalRotation.X;
			player.PreviousXRotation = Mathf.Clamp(player.GlobalRotation.X, MinXRotation, MaxXRotation);
			player.LookAt(MobToLookAtPosition with {Y = MobLookingAt.GlobalPosition.Y});
			// player.current_x_rotation = player.GlobalRotation.X;
			player.CurrentXRotation = Mathf.Clamp(player.GlobalRotation.X, MinXRotation, MaxXRotation);
			if(player.PreviousXRotation != player.CurrentXRotation)
			{
				player.GlobalRotation = player.GlobalRotation with {X = Mathf.LerpAngle(player.PreviousXRotation, player.CurrentXRotation, 0.2f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
		if (!UITargetSignalEmitted)
		{
			EmitSignal(nameof(EnemyTargeted), MobLookingAt);
			UITargetSignalEmitted = true;
		}

		if(CurrentTargetHealth != MobLookingAt.Health.CurrentValue)
		{
			CurrentTargetHealth = MobLookingAt.Health.CurrentValue;
			EmitSignal(nameof(TargetHealthChanged), MobLookingAt, CurrentTargetHealth);

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
		if(!Targeting)
		{
			if(EnemyPointedToward != null)
			{
				DirectionToEnemy = player.GlobalPosition.DirectionTo(EnemyPointedToward.GlobalPosition);
				if(MobLookingAt != EnemyPointedToward)
				{
					MobLookingAt = EnemyPointedToward;
					MobToLookAtPosition = EnemyPointedToward.GlobalPosition;
				}

			}
			else if(EnemyNear && (RayCast.DirectionInputStrength < 0.25f || RayCast.AimInputStrength < 0.25f))
			{
				
				MobLookingAt = NearestEnemy;
				MobToLookAtPosition = NearestEnemy.GlobalPosition;
				DirectionToEnemy = player.GlobalPosition.DirectionTo(NearestEnemy.GlobalPosition);
			}
		}
		else
		{
			DirectionToEnemy = player.GlobalPosition.DirectionTo(MobLookingAt.GlobalPosition);
		}
		
		if(EnemyPointedToward != null || Targeting || MobLookingAt != null)
		{
			player.PreviousYRotation = player.GlobalRotation.Y;
			if(player.IsOnFloor())
			{
				player.LookAt(MobToLookAtPosition with {Y = player.GlobalPosition.Y});
			}
			else
			{
				player.PreviousXRotation = Mathf.Clamp(player.GlobalRotation.X, MinXRotation, MaxXRotation);
				player.LookAt(MobToLookAtPosition with {Y = MobLookingAt.GlobalPosition.Y});
				player.CurrentXRotation = Mathf.Clamp(player.GlobalRotation.X, MinXRotation, MaxXRotation);
				
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


			if(EnemyPointedToward == null && NearestEnemy != null && !Targeting)
			{
				RayCast.LookAt(NearestEnemy.GlobalPosition with {Y = RayCast.GlobalPosition.Y});
			}

			
			if(-player.GlobalBasis.Z.Dot(DirectionToEnemy) > FacingSimilarity && CheckSimilarRotations(player))
			{
				EmitSignal(nameof(RotationForAbilityFinished), true);
				EmitSignal(nameof(RotationForInputFinished), player);
				RotatingToSoftTarget = false;
				FacingEnemy = true;

				if(!Targeting)
				{
					MobLookingAt = null;
				}
				
			}
			else
			{
				FacingEnemy = false;
				EmitSignal(nameof(RotationForAbilityFinished), false);
			}
		}
		else
		{
			EmitSignal(nameof(RotationForAbilityFinished), true);
			EmitSignal(nameof(RotationForInputFinished), player);
		}
		
	}

	public void SetMaxXRotation(Player player)
	{
		if(player.GlobalPosition.DistanceTo(MobLookingAt.GlobalPosition) > 13)
		{
			MinXRotation = -0.5f;
		}
		else if(player.GlobalPosition.DistanceTo(MobLookingAt.GlobalPosition) < 13 && player.GlobalPosition.DistanceTo(MobLookingAt.GlobalPosition) > 9)
		{
			MinXRotation = -0.7f;
		}
		else if(player.GlobalPosition.DistanceTo(MobLookingAt.GlobalPosition) < 9)
		{
			MinXRotation = -1f;
		}
	}

	public void SetSimilarity(Player player_)
	{
		if(player_.GlobalPosition.DistanceTo(MobLookingAt.GlobalPosition) > 10)
		{
			if(player_.IsOnFloor())
			{
				FacingSimilarity = 0.98f;
			}
			else
			{
				FacingSimilarity = 0.75f;
			}
			
		}
		else if(player_.GlobalPosition.DistanceTo(MobLookingAt.GlobalPosition) < 10 && player_.GlobalPosition.DistanceTo(MobLookingAt.GlobalPosition) > 6)
		{
			if(player_.IsOnFloor())
			{
				FacingSimilarity = 0.7f;
			}
			else
			{
				FacingSimilarity = 0.6f;
			}
			
		}
		else if(player_.GlobalPosition.DistanceTo(MobLookingAt.GlobalPosition) < 6)
		{
			if(player_.IsOnFloor())
			{
				FacingSimilarity = 0.6f;
			}
			else
			{
				FacingSimilarity = 0.5f;
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
		SortedMobs = Vector3DictionarySorter.SortByDistance(Mobs, player.GlobalTransform.Origin);
		MobsInOrder = new List<Enemy>(SortedMobs.Keys);

		foreach(Enemy enemy in SortedMobs.Keys)
		{
			AssignEnemySide(player, enemy);
		}
		if(MobLookingAt != null) // sort the enemies to the left and right of the player if the player is looking at an enemy
		{
			SortedRight = Vector3DictionarySorter.SortByDistance(MobsToRight, MobLookingAt.GlobalTransform.Origin);
			MobsInOrderToRight = new List<Enemy>(SortedRight.Keys);
			SortedLeft = Vector3DictionarySorter.SortByDistance(MobsToLeft, MobLookingAt.GlobalTransform.Origin);
			MobsInOrderToLeft = new List<Enemy>(SortedLeft.Keys);
		}
	}

	public void AssignEnemySide(Player player, Enemy enemy) // Determine if an enemy is to the left or right of the player
	{
		Vector3 enemyPosition = enemy.GlobalTransform.Origin;
		var angleBetweenPlayerAndEnemy = Mathf.RadToDeg(-(-player.Transform.Basis.X.AngleTo(player.GlobalPosition.DirectionTo(enemy.GlobalPosition))));
		if(angleBetweenPlayerAndEnemy >= 0 && angleBetweenPlayerAndEnemy < 90)
		{
			
			if(!MobsToRight.ContainsKey(enemy))
			{
				MobsToRight.Add(enemy, enemyPosition);
			}
			if(MobsToLeft.ContainsKey(enemy))
			{
				MobsToLeft.Remove(enemy);
			}
		}
		else if (angleBetweenPlayerAndEnemy >= 90 && angleBetweenPlayerAndEnemy < 180)
		{
			if(!MobsToLeft.ContainsKey(enemy))
			{
				MobsToLeft.Add(enemy, enemyPosition);
			}
			if(MobsToRight.ContainsKey(enemy))
			{
				MobsToRight.Remove(enemy);
			}
		}
		else
		{
			if(MobsToRight.ContainsKey(enemy))                                                                                                                                                                                                                                    
			{
				MobsToRight.Remove(enemy);
			}
			if(MobsToLeft.ContainsKey(enemy))
			{
				MobsToLeft.Remove(enemy);
			}
		}
	}

    internal void HandleRotatePlayer()
    {
        RotatingToSoftTarget = true;
    }

	private void HandleRemoveSoftTargetIcon(Enemy enemy)
    {
        EmitSignal(nameof(HideSoftTargetIcon), enemy);
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
