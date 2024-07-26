using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Transactions;

public partial class PlayerEntity : Entity
{

	public bool stats_changed = true; // Has the entities heath changed?
	public bool stats_updated = true; // Have the entities stats changed?
	
	// Equipment
	public string resource_path;
	public string secondary_resource_path;
	public string weapon_type = "one_handed_axe";
	public Node3D main_node; // Temp helm node
	public Node3D right_node;
	public Node3D left_node;
	public Node3D head_slot; // Head slot for the player
	public MeshInstance3D helm; // Temp helm
	public Node3D shoulder_right_slot; 
	public MeshInstance3D shoulder_right;
	public Node3D shoulder_left_slot;
	public MeshInstance3D shoulder_left;
	public Node3D chest_slot;
	public MeshInstance3D chest;
	public Node3D mark_slot;
	public MeshInstance3D mark;
	public Node3D belt_slot;
	public MeshInstance3D belt; 
	public Node3D glove_right_slot; 
	public MeshInstance3D glove_right; 
	public Node3D glove_left_slot;
	public MeshInstance3D glove_left;
	public Node3D main_hand_slot;
	public MeshInstance3D main_hand;
	public Node3D off_hand_slot;
	public MeshInstance3D off_hand;
	public Node3D leg_right_slot;
	public MeshInstance3D leg_right;
	public Node3D leg_left_slot;
	public MeshInstance3D leg_left;
	public Node3D foot_right_slot;
	public MeshInstance3D foot_right;
	public Node3D foot_left_slot;
	public MeshInstance3D foot_left;

	private Enemy targeted_enemy;
	private bool target_enemy_set;

	// Timers

	// public Timer health_regen_timer;
	// public Timer resource_regen_timer;
	

	
	
	
	public bool remove_equipped = false;
	public Vector2 blend_direction = Vector2.Zero; // Blend Direction of the player for changing animation
	
	
	// Objects to exclude from ray casting
	public Godot.Collections.Array<Rid> exclude = new Godot.Collections.Array<Rid>();
	

	// Targeting variables
	public Area3D vision; // Area where the player can target enemies
	public bool targeting = false; // Is the entity targeting?= 1 - (50 * level / (50 * level + poison_resistance));
	public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	private int mob_index = 0; // Index of mobs in list
	private	Dictionary<Enemy, Vector3> mob_pos = new Dictionary<Enemy, Vector3>();  // Dictionary of mob positions
	private Dictionary<Enemy,Vector3> sorted_mob_pos; // Sorted Dictionary of mob positions
	private List<Enemy> mobs_in_order; // List of mobs in order
	public Vector3 mob_to_LookAt_pos; // Position of the mob that the player wants to face 
	private List<Vector3> mob_distance_from_player; // Distance from targeted mob to player

	// Interact variables
	public Area3D interact_area; // Radius of where the player can interact
	public Area3D area_interacting;
	public bool in_interact_area; // Is the entity in an interact area
	public bool entered_interact; // Has the entity entered the an interact area?
	public bool left_interact; // has the entity left the interact area?
	public bool interacting; // Is the entity interacting?
	public bool is_climbing;
	public bool is_clambering;
	

	// Ability Variables
	public bool recovery_1 = false;
	public bool recovery_2 = false;
	public bool action_1_set;
	public bool action_2_set;
	public bool using_ability; // Is the entity using an ability?
	public bool can_use_abilities = true;


	// Attached objects
	// public Area3D hurtbox; // Area where the player takes damage
	// public Area3D hitbox; // Area where the player does damage
	public MeshInstance3D land_point;
	public Vector3 land_point_position;

	public UI ui;	
	public CustomSignals _customSignals; // Custom signal instance

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		dr_lvl_scale = 50 * (float)level;
		rec_lvl_scale = 100 * (float)level;
		main_hand_hitbox = GetNode<Hitbox>("Character_GameRig/Skeleton3D/MainHand/MainHandSlot/Weapon/Hitbox");
		ui = GetNode<UI>("UI/UI");
		
		// Timer health_regen_timer = GetNode<Timer>("Systems/HealthRegenTimer");
		
		vision  = (Area3D)GetNode("Areas/Vision");
		interact_area = GetNode<Area3D>("Areas/InteractArea");

		land_point = GetNode<MeshInstance3D>("UI/LandPoint");
		
		vision.BodyEntered += OnVisionEntered;
		vision.AreaExited += OnVisionExited;
		interact_area.AreaEntered += OnInteractAreaEntered;
		interact_area.AreaExited += OnInteractAreaExited;

		mob_distance_from_player = new List<Vector3>();

		

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
	}

    private void OnInteractAreaExited(Area3D area)
    {
		
        left_interact = true;
		in_interact_area = false;
		area_interacting = null;
    }

    private void OnInteractAreaEntered(Area3D area)
    {
        entered_interact = true;
		in_interact_area = true;
		area_interacting = area;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		
		direction = Vector3.Zero;
		if(mob_pos.Count == 0) // Reset enemy_in_vision
		{	
			// GD.Print("no enemy in sight");
			enemy_in_vision = false;
			mob_index = 0;
		}
		if(Fall(delta))
		{
			// GD.Print("falling");
			land_point.Show();
		}
		else
		{
			land_point.Hide();
		}
	}


	public void SmoothRotation() // Rotates the player character smoothly with lerp
	{
		if(!targeting && !is_climbing)
		{
			prev_y_rotation = GlobalRotation.Y;
			if (!GlobalTransform.Origin.IsEqualApprox(GlobalPosition + direction)) // looks at direction the player is moving
			{
				LookAt(GlobalPosition + direction);
			}
			current_y_rotation = GlobalRotation.Y;
			if(prev_y_rotation != current_y_rotation)
			{
				GlobalRotation = GlobalRotation with {Y = Mathf.LerpAngle(prev_y_rotation, current_y_rotation, 0.15f)}; // smoothly rotates between the previous angle and the new angle!
			}
		}
		else if(is_climbing) // Use the rotation that is calculated in MovementController when the player is climbing
		{
			GlobalRotation = GlobalRotation with {Y = current_y_rotation};
		}
	}

	private void OnVisionEntered(Node3D body) // handler for area entered signal
	{
		if(body is Enemy enemy)
		{
			GD.Print("Entity entered vision");
		
			if(body.IsInGroup("enemy")) 
			{
				GD.Print(body.Name + " entered vision ");
				enemy_in_vision = true;
				Vector3 dist_vec = position - body.GlobalPosition;
				if(targeting && mob_pos.Count > 0)
				{
					mob_index += 1; // increments index when enemy enters so the player stays looking at the current enemy
				}
				if(!mob_pos.ContainsKey(enemy))
				{
					mob_pos.Add(enemy, dist_vec); // adds mob to list and how close it is to the player
					Sort(); // sorts the enemies by position
				}
		
			}
		}
	}

	private void OnVisionExited(Node3D body) // handler for area exited signal
	{
		if (body is Enemy enemy)
		{
			if(enemy.IsInGroup("enemy")) 
		{
			if (mob_pos.Count == 1)
			{
				mob_index = 0; // resets index when all enemies leave
				mob_pos.Remove(enemy);
				sorted_mob_pos.Clear();
				mobs_in_order.Clear();
			}
			else if(mob_pos.Count > 0)
			{
				if(mob_index > 0) 
				{
					mob_index -= 1; // decrements index when enemy leaves so the player keeps looking at the current enemy
				}
				
				mob_pos.Remove(enemy);
				
			}
			
		}
		}
		
		
	}
	
	public void LookAtOver() // Look at enemy and switch
	{
		if(targeting && enemy_in_vision && (mobs_in_order.Count > 0))
		{
			
			// target_ability.Execute(this);
			if(Input.IsActionJustPressed("TargetNext"))
			{
				target_enemy_set = false;
				if(mob_index < mob_pos.Count - 1)
				{
					mob_index += 1;
				}
				
			}
			else if (Input.IsActionJustPressed("TargetLast"))
			{
				target_enemy_set = false;
				if(mob_index > 0)
				{
					mob_index -= 1;
				}
				
			}
			
			mob_to_LookAt_pos = mobs_in_order[mob_index].GlobalPosition;
			LookAt(mob_to_LookAt_pos with {Y = GlobalPosition.Y});
			if(!target_enemy_set)
			{
				targeted_enemy = mobs_in_order[mob_index];
				GD.Print("Targeted enemy " + targeted_enemy.identifier);
				target_enemy_set = true;
				_customSignals.EmitSignal(nameof(CustomSignals.EnemyTargetedUI), targeted_enemy);
				ui.hud.enemy_health.EnemyTargeted(targeted_enemy);
				targeted_enemy.targeted = true;
				GD.Print( identifier + " is targeting " + targeted_enemy.identifier);
			}
			
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

	public void CheckInteract() // Checks if within interact and handles input
	{
		if(in_interact_area)
		{
			if(entered_interact)
			{
				// _customSignals.EmitSignal(nameof(CustomSignals.Interact), area_interacting, in_interact_area, interacting);
				ui.GetInteract(area_interacting, in_interact_area, interacting);
				entered_interact = false;
			}
			
			if(Input.IsActionJustPressed("Interact") && !interacting)
			{
				interacting = true;
				// _customSignals.EmitSignal(nameof(CustomSignals.Interact), area_interacting, in_interact_area, interacting);
				ui.GetInteract(area_interacting, in_interact_area, interacting);
			}
			else if(Input.IsActionJustPressed("Interact") && interacting)
			{
				interacting = false;
				// _customSignals.EmitSignal(nameof(CustomSignals.Interact), area_interacting, in_interact_area, interacting);
				ui.GetInteract(area_interacting, in_interact_area, interacting);
			}
		}
		else if(left_interact)
		{
			interacting = false;
			// _customSignals.EmitSignal(nameof(CustomSignals.Interact), area_interacting, in_interact_area, interacting);
			ui.GetInteract(area_interacting, in_interact_area, interacting);
			left_interact = false;
		}

	}

	public void EnemyCheck() // Emits signal when enemy is targeted/ untargeted
	{
		if(enemy_in_vision)
		{
			if(Input.IsActionJustPressed("Target"))
			{
				target_enemy_set = false;
				if(!targeting)
				{
					targeting = true;
				}
				else if(targeting)
				{
					targeting = false;
					// _customSignals.EmitSignal(nameof(CustomSignals.EnemyUntargetedUI));
					ui.hud.enemy_health.EnemyUntargeted();
					targeted_enemy.targeted = false;
					
				}
				
			}
		}
	}
	private void Sort() // Sort mobs by distance
	{
		sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, position);
		mobs_in_order = new List<Enemy>(sorted_mob_pos.Keys);
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
