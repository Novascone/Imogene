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

// Player class, handles movement, abilities, sends signals to the UI

public partial class Player : PlayerEntity
{

	// Player reference
	public Player this_player; // Player
	public RayCast3D raycast;

	// Abilities
	private Target target_ability; // Target enemies
	private List<AbilityResource> ability_resources = new List<AbilityResource>(); // List of Ability Resources to load abilities from. Each AbilityResource Contains int id, string name, string ability_path, Texture2D icon, string type as well as 5 PackedScenes containing modifiers for the ability
	private List<Ability> abilities = new List<Ability>(); // List of abilities the player has access to. The Abilities are loaded from a PackedScene which is a Node3D with a script attached to it
	private bool abilities_loaded = false; // Bool to check if abilities are loaded and to load them/ send out the proper signals
	public Ability ability_in_use; // The ability that the player is currently using
	public LinkedList<Ability> abilities_in_use = new LinkedList<Ability>();
	public bool test_abilities_assigned = false;
	
	public bool l_cross_primary_selected; // Bool that tracks which left cross the player is using 
	public bool r_cross_primary_selected; // Bool that tracks which right cross the player is using 

	//Player consumables
	public int consumable = 1;
	public ConsumableResource[] consumables = new ConsumableResource[4];


	// Player animation
	public AnimationTree tree; // Animation control

	private MeshInstance3D targeting_icon; // Targeting icon

	// Ability Resources
	AbilityResource roll = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Roll/roll.tres");
	AbilityResource slash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Slash/Slash.tres");
	AbilityResource thrust = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Thrust/Thrust.tres");
	AbilityResource bash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Bash/Bash.tres");
	AbilityResource jump = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Jump/Jump.tres");
	
	Vector3 ray_origin;
	Vector3 ray_target = new  Vector3(0, -4, 0);
	int ray_range = 2000;
	

	public override void _Ready()
	{
		
		// ray_target = ;
		base._Ready();
		this_player = this;
		ability_resources.Add(roll);
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

		raycast = GetNode<RayCast3D>("RayCast3D");
		

		hitbox = (Area3D)GetNode("Skeleton3D/WeaponRight/axe/Hitbox");
		hitbox.AreaEntered += OnHitboxEntered;

		_customSignals.PlayerDamage += HandlePlayerDamage;
		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.ConsumableInfo += HandleConsumableInfo;
		_customSignals.EquipableInfo += HandleEquipableInfo;
		_customSignals.RemoveEquipped += HandleRemoveEquipped;
		_customSignals.UIPreventingMovement += HandleUIPreventingMovement;
		_customSignals.EquipConsumable += HandleEquipConsumable;
		GD.Print("max health ", maximum_health);
		GD.Print("max resource ", maximum_resource);
		GD.Print("physical resistance", physical_resistance);
		GD.Print("spell resistance ", spell_resistance);
		exclude.Add(vision.GetRid());
		exclude.Add(hitbox.GetRid());
		GD.Print("exclude: " + exclude);
		
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		// ray_origin = GlobalTransform.Origin;
		
		// GD.Print("ray origin" + ray_origin);
		LoadAbilities(); // Loads abilities into players ability list
		SignalEmitter(); // Emits signals to other parts of the game
		AssignAbilities();
		player_position = GlobalPosition;
		// if(raycast.IsColliding())
		// {
		// 	GD.Print("Collision point on RayCast Node: " + raycast.GetCollisionPoint());
		// 	land_point_position = raycast.GetCollisionPoint();
		// }
		
		
			ray_origin = GlobalTransform.Origin;
			ray_target = GlobalTransform.Origin + new Vector3(0, -20, 0);
			var spaceState = GetWorld3D().DirectSpaceState;
			var ray_query = PhysicsRayQueryParameters3D.Create(ray_origin, ray_target);
			ray_query.CollideWithAreas = true;
			ray_query.Exclude = exclude;
			var ray = spaceState.IntersectRay(ray_query);
			
				// if((string)ray["collider"] == "floor")
				if(ray.Count > 0)
				{
					
				// GD.Print("collision point of created raycast: " + ray["position"]);
				// GD.Print("player position: " + player_position);
				// GD.Print("collision: " + ray["collider"]);
				// GD.Print("Land point Position: " + land_point.Position);
				// GD.Print("Player position: " + player_position);
				// GD.Print("collision point: " + ray["position"]);
				land_point_position = ray["position"].AsVector3();
				
				// GD.Print("Land point position", land_point_position);
				land_point.Show();
				
				// GD.Print(land_point.Visible);
				// GD.Print("land point position: " + land_point_position);
				}
				
		
		
		
		
		
		
		
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
				consumables[consumable]?.UseItem();
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
		land_point.GlobalPosition = land_point_position;
		tree.Set("parameters/Master/Main/IW/blend_position", blend_direction);
		tree.Set("parameters/Master/Ability/Ability_1/Recovery_1/Walk_Recovery/blend_position", blend_direction);
		tree.Set("parameters/Master/Ability/Ability_1/Melee_Recovery_1/Slash/One_Handed_Slash_recovery_1/One_Handed_Medium_Recovery/Walk_Recovery/blend_position", blend_direction); // Set blend position
		// tree.Set("parameters/Master/Attack/AttackSpeed/scale", attack_speed);
		MoveAndSlide();

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
		GD.Print("Ability Assigned");
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

	public void SignalEmitter() // Emit signals
	{
		if(max_health_changed)
		{
			_customSignals.EmitSignal(nameof(CustomSignals.UIHealth), health);
			max_health_changed = false;
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

	// UI

	 private void HandleEquipableInfo(ArmsResource arm) // Gets info from equipable items
    {
		// GD.Print("Item name: "  + arm.name);
        resource_path = arm.resource_path;
		var scene_to_load = GD.Load<PackedScene>(resource_path);
		// if(arm.slot == "head")
		// {
			GD.Print("Helmet equipped");
			
			main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
			GD.Print(main_node);
			head_slot.AddChild(main_node);
			// if(equpipable.equipable_type is "Arm")
			// {
				GD.Print("Adding stats");
				// Arm item_to_add = equpipable.arm_item;
				// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
				AddEquipableStats(arm);
				UpdateStats();
				PrintStats();
			// }
			
		// }
		
    }

    private void HandleConsumableInfo(ConsumableResource item)// Gets info from consumable items
    {
        // GD.Print(item.heal_amount);
    }

	  private void HandleRemoveEquipped() // Removes equiped items
    {
		GD.Print("remove equipped");
        head_slot.RemoveChild(main_node);
    }

	private void HandleEquipConsumable(ConsumableResource item, int consumable_slot)
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
	
	public void PrintStats()
	{
		GD.Print("Strength: " + strength);
		GD.Print("Dexterity: " + dexterity);
		GD.Print("intellect: " + intellect);
		GD.Print("Physical Resistance: " + physical_resistance);
	}

	
}
