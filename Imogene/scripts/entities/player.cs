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
	List<Ability> abilities = new List<Ability>();

	public bool l_cross_primary_selected;
	public bool r_cross_primary_selected;

	public Dictionary<string, Ability> primary_RB = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> primary_LB = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> primary_RT = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> primary_LT = new Dictionary<string, Ability>();

	public Dictionary<string, Ability> primary_A = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> primary_B = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> primary_X = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> primary_Y = new Dictionary<string, Ability>();

	public Dictionary<string, Ability> secondary_RB = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> secondary_LB = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> secondary_RT = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> secondary_LT = new Dictionary<string, Ability>();

	public Dictionary<string, Ability> secondary_A = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> secondary_B = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> secondary_X = new Dictionary<string, Ability>();
	public Dictionary<string, Ability> secondary_Y = new Dictionary<string, Ability>();

	// Player Direction and animation variables
	public Vector3 player_position; // Position of the player
	public Vector3 velocity;
	public Vector3 direction;
	public float prev_y_rotation;
	public float current_y_rotation;
	public Vector2 blend_direction = Vector2.Zero;

	// Stats

	public int strength = 1;
    public int dexterity = 1;
    public int intellect = 1;
    public int vitality = 1;
    public int stamina = 1;
    public int wisdom = 1;
    public int charisma = 1;

	
	public int physical_melee_attack_abilities;
    public int physical_ranged_attack_abilities;
    public int spell_melee_attack_abilities;
    public int spell_ranged_attack_abilities;
	public int wisdom_attack_abilities;

	public float physical_melee_attack_ability_ratio;
    public float physical_ranged_attack_ability_ratio;
    public float spell_melee_attack_ability_ratio;
    public float spell_ranged_attack_ability_ratio;
	public float wisdom_attack_ability_ratio;
    public int total_attack_abilities;

	public float critical_hit_chance;
    public float critical_hit_damage;

	public float physical_melee_damage;
    public float physical_ranged_damage;
    public float spell_melee_damage;
    public float spell_ranged_damage;
	public float wisdom_scaler;

	

	
	public float physical_melee_power;
    public float physical_ranged_power;
    public float spell_melee_power;
    public float spell_ranged_power;


	// Player bools
	public bool using_movement_ability; 
	public Ability movement_ability;
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
		
		// roll_ability = (Roll)LoadAbility("Roll");
		// abilities.Add(roll_ability);
		// target_ability = (Target)LoadAbility("Target");
		// basic_attack_ability = (BasicAttack)LoadAbility("BasicAttack");
		// abilities.Add(basic_attack_ability);
		l_cross_primary_selected = true;
		r_cross_primary_selected = true;

		
	
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
		_customSignals.AbilityAssigned += HandleAbilityAssigned;
		_customSignals.LCrossPrimaryOrSecondary += HandleLCrossPrimaryOrSecondary;
		_customSignals.RCrossPrimaryOrSecondary += HandleRCrossPrimaryOrSecondary;
		_customSignals.UIPreventingMovement += HandleUIPreventingMovement;
		_customSignals.AvailableAbilities += HandleAvailableAbilities;
		
	}

    private void HandleAvailableAbilities(string ability)
    {
       	Ability new_ability = (Ability)LoadAbility(ability);
		abilities.Add(new_ability);
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

		// if(Input.IsActionJustPressed("Attack"))
		// {
		// 	attacking = true;
		// 	basic_attack_ability.Execute(this);
		// }
		// else
		// {
		// 	attacking = false;
		// 	basic_attack_ability.Execute(this);
		// }

		
		
		ExecuteAbility();

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

		
		if(using_movement_ability)
		{
			MovementAbility(movement_ability);
		}
		else
		{
			movement_ability = null;
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}
		Velocity = velocity;
		tree.Set("parameters/IW/blend_position", blend_direction);
		MoveAndSlide();

    }

	public void ExecuteAbility()
	{
		if(l_cross_primary_selected)
		{
			// Use ability assigned to primary RB
			if(Input.IsActionJustPressed("RB"))
			{

				if(primary_RB.Count > 0)
				{
					Ability current_ability = primary_RB.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
				
			}
			if(Input.IsActionJustPressed("LB"))
			{
				// Use ability assigned to primary LB
				if(primary_LB.Count > 0)
				{
					Ability current_ability = primary_LB.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
				
				
			}
			if(Input.IsActionJustPressed("RT"))
			{
				// Use ability assigned to primary RT
				if(primary_RT.Count > 0)
				{
					Ability current_ability = primary_RT.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
				
			}
			if(Input.IsActionJustPressed("LT"))
			{
				// Use ability assigned to primary LT
				if(primary_LT.Count > 0)
				{
					Ability current_ability = primary_LT.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
				
			}
		}
		else
		{	
			if(Input.IsActionJustPressed("RB"))
			{
				// Use ability assigned to secondary RB
				if(secondary_RB.Count > 0)
				{
					Ability current_ability = secondary_RB.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
				
			}
			if(Input.IsActionJustPressed("LB"))
			{
				// Use ability assigned to secondary LB
				if(secondary_LB.Count > 0)
				{
					Ability current_ability = secondary_LB.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
				
			}
			if(Input.IsActionJustPressed("RT"))
			{
				// Use ability assigned to secondary RT
				if(secondary_RT.Count > 0)
				{
					Ability current_ability = secondary_RT.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
				
			}
			if(Input.IsActionJustPressed("LT"))
			{
				// Use ability assigned to secondary LT
				if(secondary_LT.Count > 0)
				{
					Ability current_ability = secondary_LT.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
		}
		if(r_cross_primary_selected)
		{
			if(Input.IsActionJustPressed("A"))
			{
				// Use ability assigned to primary A
				if(primary_A.Count > 0)
				{
					Ability current_ability = primary_A.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
			if(Input.IsActionJustPressed("B"))
			{
				// Use ability assigned to primary B
				if(primary_B.Count > 0)
				{
					Ability current_ability = primary_B.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
			if(Input.IsActionJustPressed("X"))
			{
				// Use ability assigned to primary X
				if(primary_X.Count > 0)
				{
					Ability current_ability = primary_X.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
			if(Input.IsActionJustPressed("Y"))
			{
				// Use ability assigned to primary Y
				if(primary_Y.Count > 0)
				{
					Ability current_ability = primary_Y.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
		}
		else
		{
			if(Input.IsActionJustPressed("A"))
			{
				// Use ability assigned to secondary A
				if(secondary_A.Count > 0)
				{
					Ability current_ability = secondary_A.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
			if(Input.IsActionJustPressed("B"))
			{
				// Use ability assigned to secondary B
				if(secondary_B.Count > 0)
				{
					Ability current_ability = secondary_B.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
			if(Input.IsActionJustPressed("X"))
			{
				// Use ability assigned to secondary X
				if(secondary_X.Count > 0)
				{
					Ability current_ability = secondary_X.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
			if(Input.IsActionJustPressed("Y"))
			{
				// Use ability assigned to secondary Y
				if(secondary_Y.Count > 0)
				{
					Ability current_ability = secondary_Y.ElementAt(0).Value;
					if(current_ability.ability_type == "Movement")
					{
						using_movement_ability = true;
						movement_ability = current_ability;
					}
					else
					{
						current_ability.Execute(this);
					}
				}
			}
		}
	}

	public void MovementAbility(Ability ability)
	{
		ability.Execute(this);
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

	public void UpdateStats()
	{
		level = 0;
		strength = 10;
		dexterity = 10;
		intellect = 5;
		vitality = 1;
		stamina = 1;
		wisdom = 20;
		charisma = 1;

		weapon_damage = 10;
		attack_speed = 1.3f;

		critical_hit_chance = 0.05f;
		critical_hit_damage = 1.2f;

		physical_melee_attack_abilities = 3;
		physical_ranged_attack_abilities = 3;
		spell_melee_attack_abilities = 3;
		spell_ranged_attack_abilities = 3;
		wisdom_attack_abilities = 3;
		total_attack_abilities = physical_melee_attack_abilities + physical_ranged_attack_abilities + spell_melee_attack_abilities + spell_ranged_attack_abilities;

		physical_melee_attack_ability_ratio = (float)physical_melee_attack_abilities / total_attack_abilities;
		physical_ranged_attack_ability_ratio = (float)physical_ranged_attack_abilities / total_attack_abilities;
		spell_melee_attack_ability_ratio = (float)spell_melee_attack_abilities / total_attack_abilities;
		spell_ranged_attack_ability_ratio = (float)spell_ranged_attack_abilities / total_attack_abilities;
		wisdom_attack_ability_ratio = (float)wisdom_attack_abilities / total_attack_abilities;


		physical_melee_power = (2 * strength) + dexterity;
		physical_ranged_power = strength + (3 * dexterity);
		spell_melee_power = strength + dexterity + (3 * intellect);
		spell_ranged_power = (2 * dexterity) + (3 * intellect);

		physical_melee_damage = physical_melee_power/15;
		physical_ranged_damage = physical_ranged_power/15;
		spell_melee_damage = spell_melee_power/15;
		spell_ranged_damage = spell_ranged_power/15;
		wisdom_scaler = wisdom/20;

		damage = ((physical_melee_attack_ability_ratio * physical_melee_damage) + 
				 (physical_ranged_attack_ability_ratio * physical_ranged_damage) + 
				 (spell_melee_attack_ability_ratio * spell_melee_damage) + 
				 (spell_ranged_attack_ability_ratio * spell_ranged_damage) +
				 (wisdom_attack_ability_ratio * wisdom_scaler) +
				 weapon_damage) * 
				 attack_speed * 
				 (1 + (critical_hit_chance * critical_hit_damage));

		damage = (float)Math.Round(damage,2);
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
			UpdateStats();
			_customSignals.EmitSignal(nameof(CustomSignals.PlayerInfo), this_player);
			stats_updated = false;
		}
		
		_customSignals.EmitSignal(nameof(CustomSignals.PlayerPosition), GlobalPosition); // Sends player position to enemy
		_customSignals.EmitSignal(nameof(CustomSignals.Targeting), targeting, mob_to_LookAt_pos);
	}


	private void HandlePlayerDamage(float DamageAmount) // Sends damage amount to enemy
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

	private void HandleRCrossPrimaryOrSecondary(bool r_cross_primary_selected_signal)
    {
		r_cross_primary_selected = r_cross_primary_selected_signal;
        if(r_cross_primary_selected_signal == true)
		{
			GD.Print("RCross Primary Selected");
			
		}
		else
		{
			GD.Print("RCross Secondary Selected");
		}
    }

    private void HandleLCrossPrimaryOrSecondary(bool l_cross_primary_selected_signal)
    {
		l_cross_primary_selected = l_cross_primary_selected_signal;
        if(l_cross_primary_selected)
		{
			GD.Print("LCross Primary Selected");
		}
		else
		{
			GD.Print("LCross Secondary Selected");
		}
    }


	 private void HandleAbilityAssigned(string ability_to_assign, string button_name, Texture2D icon)
    {

		// Left Cross Primary
		if(button_name == "LCrossPrimaryUpAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_RB.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "LCrossPrimaryDownAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_LT.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "LCrossPrimaryLeftAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_LB.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "LCrossPrimaryRightAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_RT.Add(ability_to_assign, ability);
				}
			}
		}


		// Right Cross Primary
		if(button_name == "RCrossPrimaryUpAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_Y.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "RCrossPrimaryDownAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_A.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "RCrossPrimaryLeftAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_X.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "RCrossPrimaryRightAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_B.Add(ability_to_assign, ability);
				}
			}
		}


		// Left Cross Secondary
		if(button_name == "LCrossSecondaryUpAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_RB.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "LCrossSecondaryDownAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_LT.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "LCrossSecondaryLeftAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_LB.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "LCrossSecondaryRightAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_RT.Add(ability_to_assign, ability);
				}
			}
		}

		// Right Cross Secondary
		if(button_name == "RCrossSecondaryUpAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_RB.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "RCrossSecondaryDownAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_LT.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "RCrossSecondaryLeftAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_LB.Add(ability_to_assign, ability);
				}
			}
			
		}
		if(button_name == "RCrossSecondaryRightAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign)
				{
					primary_RT.Add(ability_to_assign, ability);
				}
			}
		}
    }

	private void HandleUIPreventingMovement(bool ui_preventing_movement)
    {
        can_move = !ui_preventing_movement;
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
