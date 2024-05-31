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


public partial class player : PlayerEntity
{

	// Player reference
	public player this_player; // Player

	// Abilities
	private Target target_ability; // Target enemies
	private List<AbilityResource> ability_resources = new List<AbilityResource>(); // List of Ability Resources to load abilities from. Each AbilityResource Contains int id, string name, string ability_path, Texture2D icon, string type as well as 5 PackedScenes containing modifiers for the ability
	private List<Ability> abilities = new List<Ability>(); // List of abilities the player has access to. The Abilities are loaded from a PackedScene which is a Node3D with a script attached to it
	private bool abilities_loaded = false; // Bool to check if abilities are loaded and to load them/ send out the proper signals
	public Ability ability_in_use; // The ability that the player is currently using
	public LinkedList<Ability> abilities_in_use = new LinkedList<Ability>();
	public bool test_abilities_assigned = false;
	public bool recovery_1 = false;
	public bool recovery_2 = false;
	public bool attack_1_set;
	public bool attack_2_set;

	public bool l_cross_primary_selected; // Bool that tracks which left cross the player is using 
	public bool r_cross_primary_selected; // Bool that tracks which right cross the player is using 

	// Player Direction and animation variables
	
	public Vector3 velocity; // Velocity of the player
	
	
	




	// Stats
	



	//Player consumables
	public int consumable = 1;
	public Consumable[] consumables = new Consumable[4];



	// Player bools   																									*** Switch Some of these to Entity ***
	public bool using_ability; // Is the entity using an ability?

	
	
	private bool max_health_changed = true; // Has the entities heath changed?
	private bool stats_updated = true; // Have the entities stats changed?
	private bool in_interact_area; // Is the entity in an interact area
	private bool interacting; // Is the entity interacting?
	private bool entered_interact; // Has the entity entered the an interact area?
	private bool left_interact; // has the entity left the interact area?
	public bool can_use_abilities;

	// Player attached areas
	private Area3D hurtbox; // Area where the player takes damage
	public Area3D hitbox; // Area where the player does damage
	private Area3D vision; // Area where the player can target enemies
	public Node3D head_slot; // Head slot for the player
	public Area3D interact_area; // Radius of where the player can interact

	// Player animation
	public AnimationTree tree; // Animation control

	// Mob variables
	
	

	// Mob sorting variables
	

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
		base._Ready();
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

		
		

		head_slot = GetNode<Node3D>("Skeleton3D/Head/Head_Slot");
		helm = new MeshInstance3D();

		tree = GetNode<AnimationTree>("AnimationTree");
		// tree.AnimationFinished += OnAnimationFinished;
		

		hitbox = (Area3D)GetNode("Skeleton3D/WeaponRight/axe/Hitbox");
		hitbox.AreaEntered += OnHitboxEntered;


		
		
		

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerDamage += HandlePlayerDamage;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.ConsumableInfo += HandleConsumableInfo;
		_customSignals.EquipableInfo += HandleEquipableInfo;
		_customSignals.RemoveEquipped += HandleRemoveEquipped;
		_customSignals.UIPreventingMovement += HandleUIPreventingMovement;
		_customSignals.EquipConsumable += HandleEquipConsumable;		
		
	}

    

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		
		// foreach(Ability ability in abilities_in_use)
		// {
		// 	GD.Print(ability.Name);
		// }
		// if(ability_in_use != null)
		// {
		// 	GD.Print(ability_in_use.Name);
		// }
		// GD.Print("Can use abilities: " + can_use_abilities);
		
		
		// GD.Print("X velocity from player: " + velocity.X);
		// GD.Print("Z velocity from player: " + velocity.Z);
		LoadAbilities(); // Loads abilities into players ability list
		// ResetAnimationTriggers(); // Resets animation triggers so animations don't play twice
		SignalEmitter(); // Emits signals to other parts of the game
		AssignAbilities();
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
		if(can_move == false)
		{
			velocity.X = 0;
			velocity.Z = 0;
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
		// GrabAbility(); // Grab ability player wants to use
		if(abilities_in_use != null)
		{
			// UseAbility(ability_in_use); 
		}
		// Use the ability the player has just grabbed
		CheckInteract(); // Check if the player can interact with anything
		EnemyCheck(); // Check for enemy
		LookAtOver(); // Look at mod and handle switching

		
		

		if(!using_movement_ability)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}

		Velocity = velocity;
		
		tree.Set("parameters/Master/Main/IW/blend_position", blend_direction);
		tree.Set("parameters/Master/Ability/Ability_1/Recovery_1/Walk_Recovery/blend_position", blend_direction);
		tree.Set("parameters/Master/Ability/Ability_1/Melee_Recovery_1/Slash/One_Handed_Slash_recovery_1/One_Handed_Medium_Recovery/Walk_Recovery/blend_position", blend_direction); // Set blend position
		// tree.Set("parameters/Master/Attack/AttackSpeed/scale", attack_speed);
		MoveAndSlide();

    }

	private void Fall(double delta) // bring the player back to the ground
	{
		if(!IsOnFloor())
		{
			velocity.Y -= fall_speed * (float)delta;
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
		AddChild(new_ability);
		new_ability.GetPlayerInfo(this);
		_customSignals.EmitSignal(nameof(CustomSignals.AvailableAbilities), ability_resource);
    }

	private void AssignAbilities()
	{	
		if(!test_abilities_assigned)
		{
			AssignAbilityHelper("RCrossPrimaryRightAssign", roll);
			AssignAbilityHelper("LCrossPrimaryUpAssign", slash);
			AssignAbilityHelper("RCrossPrimaryDownAssign", jump);
		}
		test_abilities_assigned = true;
	}
	 private void AssignAbilityHelper(string button_name, AbilityResource abilityResource)
   {
		_customSignals.EmitSignal(nameof(CustomSignals.AbilityAssigned), abilityResource.name, button_name, abilityResource.icon);
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
	

	

	private void OnHitboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("enemy"))
		{
			_customSignals.EmitSignal(nameof(CustomSignals.PlayerDamage), damage); // Sends how much damage the player does to the enemy
			hitbox.RemoveFromGroup("player_hitbox"); // Removes weapon from attacking group
			// GD.Print("enemy hit");
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
	

	
}
