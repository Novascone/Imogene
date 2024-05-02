using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;


public partial class player : Entity
{
	
	public player this_player;

	// Abilities - Functionalities
	Roll roll_ability;
	Target target_ability;
	BasicAttack basic_attack_ability;

	// Player Direction and animation variables
	public Vector3 player_position; // Position of the player
	public Vector3 velocity;
	public Vector3 direction;
	public float prev_y_rotation;
	public float current_y_rotation;
	public Vector2 blend_direction = Vector2.Zero;

	// Player bools
	public bool rolling; 
	public bool enemy_in_vision = false;
	public bool targeting = false;
	private bool max_health_changed = true;
	private bool stats_updated = true;
	private bool in_interact_area;
	private bool interacting;
	private bool entered_interact;
	private bool left_interact;


	// Player attached areas
	private Area3D hurtbox;
	public Area3D hitbox;
	private Area3D vision;
	public Node3D head_slot;

	// Player animation
	public AnimationTree tree;

	// Mob variables
	public Vector3 mob_to_LookAt_pos;
	private List<Vector3> mob_distance_from_player;

	// Mob sorting variables
	private	Dictionary<Area3D, Vector3> mob_pos; 
	private Dictionary<Area3D,Vector3> sorted_mob_pos; 
	private List<Area3D> mobs_in_order;
	private int mob_index = 0; 

	// Signal Variables
	private CustomSignals _customSignals;
	private Area3D target; 
	private MeshInstance3D targeting_icon;

	public string resource_path;
	private bool remove_equipped = false;

	MeshInstance3D helm;

	Node3D main_node;

	public Area3D interact_area;
	

	
	
	public override void _Ready()
	{
		this_player = this;
		
		roll_ability = (Roll)LoadAbility("Roll");
		target_ability = (Target)LoadAbility("Target");
		basic_attack_ability = (BasicAttack)LoadAbility("BasicAttack");

		damage = 2;
		health = 20;

		hurtbox = GetNode<Area3D>("Hurtbox");
		hurtbox.AreaEntered += OnHurtboxEntered;
		hurtbox.AreaExited += OnHurtboxExited;

		vision  = (Area3D)GetNode("Vision");
		vision.AreaEntered += OnVisionEntered;
		vision.AreaExited += OnVisionExited;

		head_slot = GetNode<Node3D>("Skeleton3D/Head/Head_Slot");
		helm = new MeshInstance3D();

		tree = GetNode<AnimationTree>("AnimationTree");
		tree.AnimationFinished += OnAnimationFinsihed;

		hitbox = (Area3D)GetNode("Skeleton3D/WeaponRight/axe/Hitbox");
		hitbox.AreaEntered += OnHitboxEntered;

		
		mob_distance_from_player = new List<Vector3>();
		mob_pos = new Dictionary<Area3D, Vector3>();
		
		
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandlePlayerDamage;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.PlayerPosition += HandlePlayerPosition;
		_customSignals.Targeting += HandleTargeting;
		_customSignals.UIHealthUpdate += HandleUIHealth;
		_customSignals.UIHealthUpdate += HandleUIResource;
		_customSignals.UIHealthUpdate += HandleUIHealthUpdate;
		_customSignals.UIHealthUpdate += HandleUIResourceUpdate;
		_customSignals.ItemInfo += HandleItemInfo;
		_customSignals.ConsumableInfo += HandleConsumableInfo;
		_customSignals.EquipableInfo += HandleEquipableInfo;
		_customSignals.RemoveEquipped += HandleRemoveEquipped;
		
	}

  


    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		SignalEmitter();
		direction = Vector3.Zero;
		player_position = GlobalPosition;

		
		resource = 0;
		
		if(velocity == Vector3.Zero)
		{
			blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
			blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, 0.1f);
		}
		
		if(can_move) // Basic movement controller
		{
			if (Input.IsActionPressed("Right"))
			{
				direction.X -= 1.0f;		
			}
			if (Input.IsActionPressed("Left"))
			{
				direction.X += 1.0f;
			}
			if (Input.IsActionPressed("Backward"))
			{
				direction.Z -= 1.0f;
			}
			if (Input.IsActionPressed("Forward"))
			{
				direction.Z += 1.0f;
			}
		}

		if (Input.IsActionPressed("Roll"))
		{
			rolling = true;
		}
		if(rolling)
		{
			roll_ability.Execute(this);
		}
		else
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}


		if(Input.IsActionJustPressed("Attack"))
		{
			attacking = true;
			basic_attack_ability.Execute(this);
		}
		else
		{
			attacking = false;
			basic_attack_ability.Execute(this);
		}

		if(in_interact_area)
		{
			if(entered_interact)
			{
				_customSignals.EmitSignal(nameof(CustomSignals.Interact), interact_area, in_interact_area, interacting);
				entered_interact = false;
			}
			
			if(Input.IsActionJustPressed("Interact") && !interacting)
			{
				interacting = true;
				_customSignals.EmitSignal(nameof(CustomSignals.Interact), interact_area, in_interact_area, interacting);
			}
			else if(Input.IsActionJustPressed("Interact") && interacting)
			{
				interacting = false;
				_customSignals.EmitSignal(nameof(CustomSignals.Interact), interact_area, in_interact_area, interacting);
			}
		}
		else if(left_interact)
		{
			interacting = false;
			_customSignals.EmitSignal(nameof(CustomSignals.Interact), interact_area, in_interact_area, interacting);
			left_interact = false;
		}


		SmoothRotation();
		EnemyCheck();
		LookAtOver();

		
		if(mob_pos.Count == 0)
		{	
			// GD.Print("no enemy in sight");
			enemy_in_vision = false;
			mob_index = 0;
		}

		
	
		Velocity = velocity;
		tree.Set("parameters/IW/blend_position", blend_direction);
		MoveAndSlide();

    }
    
	public void SmoothRotation()
	{
		if(!targeting)
		{
			prev_y_rotation = GlobalRotation.Y;
			if (!GlobalTransform.Origin.IsEqualApprox(GlobalPosition + direction)) // looks at direction the player is moving
			{
				LookAt(GlobalPosition + direction);
			}
			current_y_rotation = GlobalRotation.Y;
			if(prev_y_rotation != current_y_rotation)
			{
				GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(prev_y_rotation, current_y_rotation, 0.2f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
	}

	private void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		if(enemy_in_vision)
		{
			if(Input.IsActionJustPressed("Target"))
			{
				if(!targeting)
				{
					targeting = true;
				}
				else if(targeting)
				{
					targeting = false;
				}
				
			}
		}
	}
	public void LookAtOver()
	{
		if(targeting && enemy_in_vision && (mobs_in_order.Count > 0))
		{
			
			target_ability.Execute(this);
			if(Input.IsActionJustPressed("TargetNext"))
			{
				if(mob_index < mob_pos.Count - 1)
				{
					mob_index += 1;
				}
				
			}
			else if (Input.IsActionJustPressed("TargetLast"))
			{
				if(mob_index > 0)
				{
					mob_index -= 1;
				}
				
			}
			
			mob_to_LookAt_pos = mobs_in_order[mob_index].GlobalPosition;
			LookAt(mob_to_LookAt_pos with {Y = GlobalPosition.Y});
			
		}
		else
		{
			
			targeting = false;
			// Sets the animation to walk forward when not targeting
			if(direction != Vector3.Zero)
			{
				blend_direction.X = 0;
				blend_direction.Y = 1;
				// GD.Print("Normal: ", blend_direction);
			}
			else
			{
				blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
				blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, 0.1f);
			}
		}
	}

	private void OnVisionEntered(Area3D interactable) // handler for area entered signal
	{
		if(interactable.IsInGroup("enemy")) 
		{
			enemy_in_vision = true;
			Vector3 dist_vec = player_position - interactable.GlobalPosition;
			if(targeting && mob_pos.Count > 0)
			{
				mob_index += 1; // increments index when enemy enters so the player stays looking at the current enemy
			}
			if(!mob_pos.ContainsKey(interactable))
			{
				mob_pos.Add(interactable, dist_vec); // adds mob to list and how close it is to the player
				Sort(); // sorts the enemies by position
			}
	
		}
		
	}

	private void OnVisionExited(Area3D interactable) // handler for area exited signal
	{
		if(interactable.IsInGroup("enemy")) 
		{
			if (mob_pos.Count == 1)
			{
				mob_index = 0; // resets index when all enemies leave
				mob_pos.Remove(interactable);
				sorted_mob_pos.Clear();
				mobs_in_order.Clear();
			}
			else if(mob_pos.Count > 0)
			{
				if(mob_index > 0) 
				{
					mob_index -= 1; // decrements index when enemy leaves so the player keeps looking at the current enemy
				}
				
				mob_pos.Remove(interactable);
				
			}
			
		}
		
	}

	private void OnHitboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("enemy"))
		{
			_customSignals.EmitSignal(nameof(CustomSignals.PlayerDamage), damage); // Sends how much damage the player does to the enemy
			hitbox.RemoveFromGroup("player_hitbox"); // Removes weapon from attacking group
			// GD.Print("enemy hit");
		}
		
		
	}

	private void OnAnimationFinsihed(StringName animName) // when animation is finished
    {
	
		if(animName == "Attack")
		{
			hitbox.Monitoring = false;
			can_move = true;
			hitbox.RemoveFromGroup("player_hitbox");
		}

    }

	private void OnHurtboxEntered(Area3D area) // handler for area entered signal
	{
		if(area.IsInGroup("enemy_hitbox"))
		{
			GD.Print("player hit");
			TakeDamage(1);
			GD.Print(health);
			_customSignals.EmitSignal(nameof(CustomSignals.UIHealthUpdate), 1);
		}
		else if(area.IsInGroup("interactive"))
		{
			GD.Print("entered");
			entered_interact = true;
			in_interact_area = true;
			interact_area = area;
		}
		
	}

	private void OnHurtboxExited(Area3D area)
    {
        if(area.IsInGroup("interactive"))
		{
			GD.Print("exited");
			left_interact = true;
			in_interact_area = false;
			interact_area = null;
		}
    }

	private void Sort()
	{
		sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player_position);
		mobs_in_order = new List<Area3D>(sorted_mob_pos.Keys);
	}

	public void SignalEmitter()
	{
		if(max_health_changed)
		{
			_customSignals.EmitSignal(nameof(CustomSignals.UIHealth), health);
			max_health_changed = false;
		}
		if(stats_updated)
		{
			_customSignals.EmitSignal(nameof(CustomSignals.PlayerInfo), this_player);
			stats_updated = false;
		}
		
		_customSignals.EmitSignal(nameof(CustomSignals.PlayerPosition), GlobalPosition); // Sends player position to enemy
		_customSignals.EmitSignal(nameof(CustomSignals.Targeting), targeting, mob_to_LookAt_pos);
	}


	private void HandlePlayerDamage(int DamageAmount) // Sends damage amount to enemy
	{
			DamageAmount += damage;
	}

	private void HandleEnemyPosition(Vector3 position) // Gets enemy position from enemy
    {
        enemy_position = position;
    }

	 private void HandleEquipableInfo(Equipable item)
    {
        resource_path = item.item_path;
		GD.Print("Plus " + item.strength + " Strength");
		GD.Print(item.item_path);
		var scene_to_load = GD.Load<PackedScene>(resource_path);
		if(item.slot == "head")
		{
			GD.Print("Helmet equipped");
			
			main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
			GD.Print(main_node);
			head_slot.AddChild(main_node);
		}
		
    }

    private void HandleConsumableInfo(Consumable item)
    {
        GD.Print(item.heal_amount);
    }

	  private void HandleRemoveEquipped()
    {
		GD.Print("remove equipped");
        head_slot.RemoveChild(main_node);
    }

    private void HandleItemInfo(Item item){}


	private void HandlePlayerPosition(Vector3 position){} // Sends player position to enemy
	private void HandleTargeting(bool targeting, Vector3 position){}
	private void HandleUIResource(int amount){}
    private void HandleUIHealth(int amount){}
	private void HandleUIHealthUpdate(int amount){}
	private void HandleUIResourceUpdate(int amount){}
	

	public static class Vector3DictionarySorter 
	{
		public static Dictionary<Area3D, Vector3> SortByDistance(Dictionary<Area3D, Vector3> dict, Vector3 point)
		{
			var sortedList = dict.ToList();

			sortedList.Sort((pair1, pair2) => pair1.Value.DistanceTo(point).CompareTo(pair2.Value.DistanceTo(point)));

			return sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}
}
