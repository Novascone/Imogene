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

	// Player reference
	public player this_player; // Player

	// Abilities
	private Target target_ability; // Target enemies
	private List<AbilityResource> ability_resources = new List<AbilityResource>(); // List of Ability Resources to load abilities from. Each AbilityResource Contains int id, string name, string ability_path, Texture2D icon, string type as well as 5 PackedScenes containing modifiers for the ability
	private List<Ability> abilities = new List<Ability>(); // List of abilities the player has access to. The Abilities are loaded from a PackedScene which is a Node3D with a script attached to it
	private bool abilities_loaded = false; // Bool to check if abilities are loaded and to load them/ send out the proper signals
	private Ability ability_in_use; // The ability that the player is currently using
	public List<Ability> abilities_in_use = new List<Ability>();

	public bool l_cross_primary_selected; // Bool that tracks which left cross the player is using 
	public bool r_cross_primary_selected; // Bool that tracks which right cross the player is using 

	public Ability primary_RB; // Ability assigned to Primary RB
	public Ability primary_LB;  // Ability assigned to Primary LB
	public Ability primary_RT;  // Ability assigned to Primary RT
	public Ability primary_LT;  // Ability assigned to Primary LT

	public Ability primary_A;  // Ability assigned to Primary A
	public Ability primary_B;  // Ability assigned to Primary B
	public Ability primary_X;  // Ability assigned to Primary X
	public Ability primary_Y;  // Ability assigned to Primary Y

	public Ability secondary_RB;  // Ability assigned to Secondary RB
	public Ability secondary_LB;  // Ability assigned to Secondary RB
	public Ability secondary_RT;  // Ability assigned to Secondary RB
	public Ability secondary_LT;  // Ability assigned to Secondary RB

	public Ability secondary_A;  // Ability assigned to Secondary A
	public Ability secondary_B;  // Ability assigned to Secondary B
	public Ability secondary_X;  // Ability assigned to Secondary X
	public Ability secondary_Y;  // Ability assigned to Secondary Y



	// Player Direction and animation variables
	public Vector3 player_position; // Position of the player
	public Vector3 velocity; // Velocity of the player
	public Vector3 direction; // Direction of the player
	public float prev_y_rotation; // Rotation of the player before current rotation
	public float current_y_rotation; // Rotation of the player
	public Vector2 blend_direction = Vector2.Zero; // Blend Direction of the player for changing animations



	// Stats
	public int strength = 1; // Strength: A primary stat for melee damage. Contributes to Physical Melee Power, Physical Ranged Power, and Spell Melee Power
    public int dexterity = 1; // Dexterity: A primary stat for melee damage. Contributes to all Power stats
    public int intellect = 1; // Intellect: Primary stat spell damage for. Contributes to Spell Melee Power and Spell Ranged Power
    public int vitality = 1; // Vitality: Primary stat for health
    public int stamina = 1; // Primary stat for resource and regeneration
    public int wisdom = 1; // Increases the damage of abilities that use wisdom, also used for interactions
    public int charisma = 1; // Primary Stat for character interaction

	public float physical_melee_power; // Increases physical melee DPS by 1 every 15 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_melee_power; // Increases melee magic DPS by 1 every 15 points + 3 for every point of intellect + 1 for every point of dexterity + 1 for every point strength
    public float physical_ranged_power; // Increases physical ranged DPS by 1 every 15 points + 2 for every point of strength + 1 for every point of dexterity
    public float spell_ranged_power;  // Increases physical ranged DPS by 1 every 15 points + 3 for every point of dexterity + 1 for every point of strength
	public float wisdom_scaler; // Increases by one every 20 wisdom. Increases how powerful attacks that scale with wisdom are

	public float critical_hit_chance; // Percentage change for hit to be a critical hit
    public float critical_hit_damage; // Multiplier applied to base damage if a hit is critical
 	public float physical_melee_damage; 
    public float physical_ranged_damage; 
    public float spell_melee_damage;
    public float spell_ranged_damage;
	

	public int physical_melee_attack_abilities; // Total number of attack abilities in each category
    public int physical_ranged_attack_abilities;
    public int spell_melee_attack_abilities;
    public int spell_ranged_attack_abilities;
	public int wisdom_attack_abilities;

	public float physical_melee_attack_ability_ratio; // ratio of attack abilities
    public float physical_ranged_attack_ability_ratio;
    public float spell_melee_attack_ability_ratio;
    public float spell_ranged_attack_ability_ratio;
	public float wisdom_attack_ability_ratio;
    public int total_attack_abilities;

	public float slash_damage;
	public float thrust_damage;
	public float blunt_damage;


	
	//Player equipment details
	public string weapon_type = "one_handed_axe";



	//Player consumables
	public int consumable = 1;
	public Consumable[] consumables = new Consumable[4];



	// Player bools   																									*** Switch Some of these to Entity ***
	public bool using_ability; // Is the entity using an ability?
	public bool animation_finished = false; // Is the entity's animation finished?
	public bool enemy_in_vision = false; // Is there an enemy in the entity's vision?
	public bool targeting = false; // Is the entity targeting?
	private bool max_health_changed = true; // Has the entities heath changed?
	private bool stats_updated = true; // Have the entities stats changed?
	private bool in_interact_area; // Is the entity in an interact area
	private bool interacting; // Is the entity interacting?
	private bool entered_interact; // Has the entity entered the an interact area?
	private bool left_interact; // has the entity left the interact area?
	private bool can_use_abilities;

	// Player attached areas
	private Area3D hurtbox; // Area where the player takes damage
	public Area3D hitbox; // Area where the player does damage
	private Area3D vision; // Area where the player can target enemies
	public Node3D head_slot; // Head slot for the player
	public Area3D interact_area; // Radius of where the player can interact

	// Player animation
	public AnimationTree tree; // Animation control

	// Mob variables
	public Vector3 mob_to_LookAt_pos; // Position of the mob that the player wants to face 
	private List<Vector3> mob_distance_from_player; // Distance from targeted mob to player

	// Mob sorting variables
	private	Dictionary<Area3D, Vector3> mob_pos;  // Dictionary of mob positions
	private Dictionary<Area3D,Vector3> sorted_mob_pos; // Sorted Dictionary of mob positions
	private List<Area3D> mobs_in_order; // List of mobs in order
	private int mob_index = 0; // Index of mobs in list

	// Signal Variables
	private CustomSignals _customSignals; // Custom signal instance
	private MeshInstance3D targeting_icon; // Targeting icon

	// Ability Resources
	AbilityResource roll = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Roll/roll.tres");
	AbilityResource basic_attack = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/BasicAttack/basic_attack.tres");
	AbilityResource slash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Slash/Slash.tres");
	AbilityResource thrust = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Thrust/Thrust.tres");
	AbilityResource bash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Bash/Bash.tres");
	AbilityResource jump = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Jump/Jump.tres");
	
	// Misc
	public string resource_path;
	private bool remove_equipped = false;

	public MeshInstance3D helm; // Temp helm

	public Node3D main_node; // Temp helm node
	
	
	public override void _Ready()
	{
		
		this_player = this;
		ability_resources.Add(roll);
		ability_resources.Add(basic_attack);
		ability_resources.Add(slash);
		ability_resources.Add(thrust);
		ability_resources.Add(bash);
		ability_resources.Add(jump);

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
		tree.AnimationFinished += OnAnimationFinished;
		

		hitbox = (Area3D)GetNode("Skeleton3D/WeaponRight/axe/Hitbox");
		hitbox.AreaEntered += OnHitboxEntered;

		
		mob_distance_from_player = new List<Vector3>();
		mob_pos = new Dictionary<Area3D, Vector3>();
		
		
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandlePlayerDamage;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		// _customSignals.PlayerPosition += HandlePlayerPosition;
		// _customSignals.Targeting += HandleTargeting;
		// _customSignals.UIHealthUpdate += HandleUIHealth;
		// _customSignals.UIHealthUpdate += HandleUIResource;
		// _customSignals.UIHealthUpdate += HandleUIHealthUpdate;
		// _customSignals.UIHealthUpdate += HandleUIResourceUpdate;
		// _customSignals.ItemInfo += HandleItemInfo;
		_customSignals.ConsumableInfo += HandleConsumableInfo;
		_customSignals.EquipableInfo += HandleEquipableInfo;
		_customSignals.RemoveEquipped += HandleRemoveEquipped;
		_customSignals.AbilityAssigned += HandleAbilityAssigned;
		// _customSignals.LCrossPrimaryOrSecondary += HandleLCrossPrimaryOrSecondary;
		// _customSignals.RCrossPrimaryOrSecondary += HandleRCrossPrimaryOrSecondary;
		_customSignals.UIPreventingMovement += HandleUIPreventingMovement;
		_customSignals.EquipConsumable += HandleEquipConsumable;		
		
		
	}

    

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		// GD.Print("Can move: " + can_move);
		LoadAbilities(); // Loads abilities into players ability list
		ResetAnimationTriggers(); // Resets animation triggers so animations don't play twice
		SignalEmitter(); // Emits signals to other parts of the game

		direction = Vector3.Zero;
		player_position = GlobalPosition;
		// GD.Print("Jumping " + jumping);
		resource = 0;
		if(velocity == Vector3.Zero) // If not moving return to Idle slowly (hence the lerp)
		{
			blend_direction.X = Mathf.Lerp(blend_direction.X, 0, 0.1f);
			blend_direction.Y = Mathf.Lerp(blend_direction.Y, 0, 0.1f);
		}
		
		Fall(delta);
		
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
		if(can_use_abilities)
		{
			if(Input.IsActionJustPressed("D-PadLeft")) // Select crosses 
			{
				l_cross_primary_selected = !l_cross_primary_selected;
				_customSignals.EmitSignal(nameof(CustomSignals.LCrossPrimaryOrSecondary), l_cross_primary_selected);
			}
			if(Input.IsActionJustPressed("D-PadRight"))
			{
				r_cross_primary_selected = !r_cross_primary_selected;
				_customSignals.EmitSignal(nameof(CustomSignals.	RCrossPrimaryOrSecondary), r_cross_primary_selected);
			}
			if(Input.IsActionJustPressed("D-PadUp"))
			{
				// switch consumable
				if(consumable < 4)
				{
					consumable += 1;
				}
				else if(consumable == 4)
				{
					consumable = 1;
				}

				_customSignals.EmitSignal(nameof(CustomSignals.WhichConsumable), consumable);
			}
			if(Input.IsActionJustPressed("D-PadDown"))
			{
				// use consumable
				consumables[consumable].UseItem();
			}
		}
		

		SmoothRotation(); // Rotate the player character smoothly
		GrabAbility(); // Grab ability player wants to use
		UseAbility(ability_in_use); // Use the ability the player has just grabbed
		CheckInteract(); // Check if the player can interact with anything
		EnemyCheck(); // Check for enemy
		LookAtOver(); // Look at mod and handle switching

		
		if(mob_pos.Count == 0) // Reset enemy_in_vision
		{	
			// GD.Print("no enemy in sight");
			enemy_in_vision = false;
			mob_index = 0;
		}

		
		
		Velocity = velocity;
		
		tree.Set("parameters/PlayerState/IW/blend_position", blend_direction); // Set blend position
		tree.Set("parameters/PlayerState/Attack/AttackSpeed/scale", attack_speed);
		MoveAndSlide();

    }

	private void Fall(double delta) // bring the player back to the ground
	{
		if(!IsOnFloor())
		{
			velocity.Y -= fall_speed * (float)delta;
			on_floor = false;
		}
		else
		{
			velocity.Y = 0;
			on_floor = true;
		}
	}


	 private void LoadAbilities() // Loads abilities
   {
		if(!abilities_loaded)
		{
			_customSignals.EmitSignal(nameof(CustomSignals.LCrossPrimaryOrSecondary), l_cross_primary_selected);
			_customSignals.EmitSignal(nameof(CustomSignals.RCrossPrimaryOrSecondary), r_cross_primary_selected);
			foreach(AbilityResource ability_resource in ability_resources)
			{
				LoadAbilitiesHelper(ability_resource);
			}

		}
		abilities_loaded = true;
   }

    private void LoadAbilitiesHelper(AbilityResource ability_resource) // Adds ability to abilities list
    {
       	Ability new_ability = (Ability)LoadAbility(ability_resource.name);
		abilities.Add(new_ability);
		_customSignals.EmitSignal(nameof(CustomSignals.AvailableAbilities), ability_resource);
    }

	public void ResetAnimationTriggers() // Resets the animation triggers
	{
		if(animation_triggered)
		{
			tree.Set("parameters/PlayerState/conditions/attacking", false);
			animation_triggered = false;
		}
	}

	public void GrabAbility() // Grabs ability based on input
	{
		if(can_use_abilities)
		{
			if(l_cross_primary_selected)
			{
				// Use ability assigned to primary RB
				if(Input.IsActionJustPressed("RB"))
				{
					if(primary_RB != null) {abilities_in_use.Add(primary_RB); primary_RB.in_use = true; ability_in_use = primary_RB;}
				}
				// Use ability assigned to primary LB
				if(Input.IsActionJustPressed("LB"))
				{
					if(primary_LB != null) {abilities_in_use.Add(primary_LB); primary_LB.in_use = true; ability_in_use = primary_LB; }
				}
				// Use ability assigned to primary RT
				if(Input.IsActionJustPressed("RT"))
				{
					if(primary_RT != null) {abilities_in_use.Add(primary_RT); primary_RT.in_use = true; ability_in_use = primary_RT; }
				}
				// Use ability assigned to primary LT
				if(Input.IsActionJustPressed("LT"))
				{
					if(primary_LT != null) {abilities_in_use.Add(primary_LT); primary_LT.in_use = true; ability_in_use = primary_LT; }
				}
			}
			else
			{	// Use ability assigned to secondary RB
				if(Input.IsActionJustPressed("RB"))
				{
					if(secondary_RB != null) {abilities_in_use.Add(secondary_RB); secondary_RB.in_use = true; ability_in_use = secondary_RB; }
				}
				// Use ability assigned to secondary LB
				if(Input.IsActionJustPressed("LB"))
				{
					if(secondary_LB != null) {abilities_in_use.Add(secondary_LB); secondary_LB.in_use = true; ability_in_use = secondary_LB; }
				}
				// Use ability assigned to secondary RT
				if(Input.IsActionJustPressed("RT"))
				{
					if(secondary_RT != null) {abilities_in_use.Add(secondary_RT); secondary_RT.in_use = true; ability_in_use = secondary_RT; }
				}
				// Use ability assigned to secondary LT
				if(Input.IsActionJustPressed("LT"))
				{
					if(secondary_LT != null) {abilities_in_use.Add(secondary_LT); secondary_LT.in_use = true; ability_in_use = secondary_LT; }
				}
			}
			if(r_cross_primary_selected)
			{
				// Use ability assigned to primary A
				if(Input.IsActionJustPressed("A"))
				{
					if(primary_A != null) {abilities_in_use.Add(primary_A); primary_A.in_use = true; ability_in_use = primary_A;}
				}
				// Use ability assigned to primary B
				if(Input.IsActionJustPressed("B"))
				{
					if(primary_B != null) {abilities_in_use.Add(primary_B); primary_B.in_use = true; ability_in_use = primary_B; }
				}
				// Use ability assigned to primary X
				if(Input.IsActionJustPressed("X"))
				{
					if(primary_X != null) {abilities_in_use.Add(primary_X); primary_X.in_use = true; ability_in_use = primary_X; }
				}
				// Use ability assigned to primary Y
				if(Input.IsActionJustPressed("Y"))
				{
					if(primary_Y != null) {abilities_in_use.Add(primary_Y); primary_Y.in_use = true; ability_in_use = primary_Y;}
				}
			}
			else
			{
				// Use ability assigned to secondary A
				if(Input.IsActionJustPressed("A"))
				{
					if(secondary_A != null) {abilities_in_use.Add(secondary_A); secondary_A.in_use = true; ability_in_use = secondary_A;}
				}
				// Use ability assigned to secondary B
				if(Input.IsActionJustPressed("B"))
				{
					if(secondary_B != null) {abilities_in_use.Add(secondary_B); secondary_B.in_use = true; ability_in_use = secondary_B;}
				}
				// Use ability assigned to secondary X
				if(Input.IsActionJustPressed("X"))
				{
					if(secondary_X != null) {abilities_in_use.Add(secondary_X); secondary_X.in_use = true; ability_in_use = secondary_X;}
				}
				// Use ability assigned to secondary Y
				if(Input.IsActionJustPressed("Y"))
				{
					if(secondary_Y != null) {abilities_in_use.Add(secondary_Y); secondary_Y.in_use = true; ability_in_use = secondary_Y;}
				}
			}

		}
		
	}

	public void UseAbility(Ability ability) // Uses ability
	{
		if(can_use_abilities)
		{
			if(ability != null && ability.in_use)
			{
				ability.Execute(this);
			}
			else
			{
				ability_in_use = null;
				velocity.X = direction.X * speed;
				velocity.Z = direction.Z * speed;
			}
		}
	}
    
	public void SmoothRotation() // Rotates the player character smoothly with lerp
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

	public void UpdateStats() // Updates stats 															*** NEEDS ADDITIONS ***
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

		// Calculates stats
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

	private void CheckInteract() // Checks if within interact and handles input
	{
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
	public void LookAtOver() // Look at enemy and switch
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

	private void OnAnimationFinished(StringName animName) // when animation is finished
    {
	
		if(animName == "Attack")
		{
			attacking = false;
			animation_finished = true;
			tree.Set("parameters/PlayerState/conditions/attacking", false);
			hitbox.Monitoring = false;
			can_move = true;
			hitbox.RemoveFromGroup("player_hitbox");
		}
		if(animName == "Roll_Forward")
		{

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
			entered_interact = true;
			in_interact_area = true;
			interact_area = area;
		}
		
	}
	private void OnHurtboxExited(Area3D area) // Check if interact area has come in contact with the player
    {
        if(area.IsInGroup("interactive"))
		{
			left_interact = true;
			in_interact_area = false;
			interact_area = null;
		}
    }

	private void Sort() // Sort mobs by distance
	{
		sorted_mob_pos = Vector3DictionarySorter.SortByDistance(mob_pos, player_position);
		mobs_in_order = new List<Area3D>(sorted_mob_pos.Keys);
	}

	public void SignalEmitter() // Emit signals
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

	 private void HandleEquipableInfo(Equipable item) // Gets info from equipable items
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

    private void HandleConsumableInfo(Consumable item)// Gets info from consumable items
    {
        // GD.Print(item.heal_amount);
    }

	  private void HandleRemoveEquipped() // Removes equiped items
    {
		GD.Print("remove equipped");
        head_slot.RemoveChild(main_node);
    }


	 private void HandleAbilityAssigned(string ability_to_assign, string button_name, Texture2D icon) // Gets signal from the UI where an ability has just been assigned
    {
		GD.Print("got assignment");
		// Left Cross Primary
		if(button_name == "LCrossPrimaryUpAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){primary_RB = ability;}
			}
		}
		if(button_name == "LCrossPrimaryRightAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){primary_RT = ability;}
			}
		}
		if(button_name == "LCrossPrimaryLeftAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){primary_LB = ability;}
			}
		}
		if(button_name == "LCrossPrimaryDownAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){primary_LT = ability;}
			}
		}


		// Right Cross Primary
		if(button_name == "RCrossPrimaryUpAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){primary_Y = ability;}
			}
		}
		if(button_name == "RCrossPrimaryRightAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){primary_B = ability;}
			}
		}
		if(button_name == "RCrossPrimaryLeftAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){primary_X = ability;}
			}
		}
		if(button_name == "RCrossPrimaryDownAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){primary_A = ability;}
			}
		}


		// Left Cross Secondary
		if(button_name == "LCrossSecondaryUpAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){secondary_RB = ability;}
			}
		}
		if(button_name == "LCrossSecondaryRightAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){secondary_RT = ability;}
			}
		}
		if(button_name == "LCrossSecondaryLeftAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){secondary_LB = ability;}
			}
		}
		if(button_name == "LCrossSecondaryDownAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){secondary_LT = ability;}
			}
		}

		// Right Cross Secondary
		if(button_name == "RCrossSecondaryUpAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){secondary_Y = ability;}
			}
		}
		if(button_name == "RCrossSecondaryRightAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){secondary_B = ability;}
			}
		}
		if(button_name == "RCrossSecondaryLeftAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){secondary_X = ability;}
			}
		}
		if(button_name == "RCrossSecondaryDownAssign")
		{
			foreach(Ability ability in abilities)
			{
				if(ability.Name == ability_to_assign){secondary_Y = ability;}
			}
		}
    	
	}

	private void HandleEquipConsumable(Consumable item, int consumable_slot)
    {
        consumables[consumable_slot] = item;
		GD.Print(consumables[consumable_slot].name);
    }	

	private void HandleUIPreventingMovement(bool ui_preventing_movement) // Check if UI is preventing movement
    {
        can_move = !ui_preventing_movement;
		can_use_abilities = !ui_preventing_movement;
		velocity = Vector3.Zero;
    }

	public static class Vector3DictionarySorter // Sorts mobs by distance
	{
		public static Dictionary<Area3D, Vector3> SortByDistance(Dictionary<Area3D, Vector3> dict, Vector3 point)
		{
			var sortedList = dict.ToList();

			sortedList.Sort((pair1, pair2) => pair1.Value.DistanceTo(point).CompareTo(pair2.Value.DistanceTo(point)));

			return sortedList.ToDictionary(pair => pair.Key, pair => pair.Value);
		}
	}
}
