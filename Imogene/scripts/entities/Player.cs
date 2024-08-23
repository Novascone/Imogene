using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;


// Player class, handles movement, abilities, sends signals to the UI

public partial class Player : PlayerEntity
{

	// Player reference
	public Player this_player; // Player
	public MeshInstance3D player_mesh;
	public RayCast3D raycast;
	public RayCast3D near_wall;
	public RayCast3D on_wall;
	public CameraRig camera_rig;
	public Area3D vision; // Area where the player can target enemies
	public Area3D soft_target_small;
	public Area3D soft_target_large;
	public CollisionShape3D collision;
	public Marker3D cast_point;
	public Node3D surrounding_hitbox;

	// Abilities
	// private Target target_ability; // Target enemies
	public List<AbilityResource> ability_resources = new List<AbilityResource>(); // List of Ability Resources to load abilities from. Each AbilityResource Contains int id, string name, string ability_path, Texture2D icon, string type as well as 5 PackedScenes containing modifiers for the ability
	public List<Ability> abilities = new List<Ability>(); // List of abilities the player has access to. The Abilities are loaded from a PackedScene which is a Node3D with a script attached to it
	public bool abilities_loaded = false; // Bool to check if abilities are loaded and to load them/ send out the proper signals
	public Ability ability_in_use; // The ability that the player is currently using
	public LinkedList<Ability> abilities_in_use = new LinkedList<Ability>();
	public List<Ability> ability_list = new List<Ability>();
	public bool test_abilities_assigned = false;
	
	// UI
	public bool l_cross_primary_selected; // Bool that tracks which left cross the player is using 
	public bool r_cross_primary_selected; // Bool that tracks which right cross the player is using 
	

	// Controllers
	public MovementController movement_controller;
	public AbilityController ability_controller;
	public StatController stat_controller;
	public EquipmentController equipment_controller;

	//Player consumables
	public int consumable = 1;
	public ConsumableResource[] consumables = new ConsumableResource[4];


	// Player animation
	public AnimationTree tree; // Animation control
	public AnimationPlayer animation_player;


	// Ability Resources
	public AbilityResource slash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/Slash/Slash.tres");
	public AbilityResource thrust = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/Thrust/Thrust.tres");
	public AbilityResource bash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/Bash/Bash.tres");
	public AbilityResource jump = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/Jump/Jump.tres");
	public AbilityResource hitscan = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/Hitscan/Hitscan.tres");
	public AbilityResource projectile = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/Projectile/Projectile.tres");
	public AbilityResource dash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/Dash/Dash.tres");
	public AbilityResource whirlwind = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Brigian/Active/Whirlwind/Whirlwind.tres");
	public AbilityResource small_fireball = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/SmallFireball/SmallFireball.tres");
	public AbilityResource kick = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/General/Active/Kick/Kick.tres");

	public float move_forward_clamber = 0;
	public float vertical_input;

	public bool targeting = false; // Is the entity targeting?= 1 - (50 * level / (50 * level + poison_resistance));

	public bool ability_preventing_movement;

	public CollisionShape3D hitbox_collision;
	

	

	public override void _Ready()
	{
		
		base._Ready();
		this_player = this;

		ability_resources.Add(small_fireball);
		ability_resources.Add(slash);
		ability_resources.Add(thrust);
		ability_resources.Add(bash);
		ability_resources.Add(jump);
		ability_resources.Add(hitscan);
		ability_resources.Add(projectile);
		ability_resources.Add(dash);
		ability_resources.Add(whirlwind);
		ability_resources.Add(kick);

		l_cross_primary_selected = true;
		r_cross_primary_selected = true;		

		cast_point = GetNode<Marker3D>("Character_GameRig/Skeleton3D/MainHand/CastPoint");
		

		camera_rig = GetNode<CameraRig>("CameraRig");
		camera_rig.TopLevel = true;

		movement_controller = GetNode<MovementController>("Controllers/MovementController");
		ability_controller = GetNode<AbilityController>("Controllers/AbilityController");
		stat_controller = GetNode<StatController>("Controllers/StatController");
		equipment_controller = GetNode<EquipmentController>("Controllers/EquipmentController");


		vision  = (Area3D)GetNode("Areas/Vision");

		soft_target_small = GetNode<Area3D>("Areas/SoftTargetSmall");
		soft_target_large = GetNode<Area3D>("Areas/SoftTargetLarge");

		vision.BodyEntered += OnVisionEntered;
		vision.BodyExited += OnVisionExited;

		soft_target_small.BodyEntered += OnSoftTargetSmallEntered;
		soft_target_small.BodyExited += OnSoftTargetSmallExited;

		soft_target_large.BodyEntered += OnSoftTargetLargeEntered;
		soft_target_large.BodyExited += OnSoftTargetLargeExited;

		hitbox_collision = GetNode<CollisionShape3D>("Character_GameRig/Skeleton3D/MainHand/MainHandSlot/Weapon/Hitbox/CollisionShape3D");

		player_mesh = GetNode<MeshInstance3D>("Character_GameRig/retop_prelim_pc");

		surrounding_hitbox = GetNode<Node3D>("Character_GameRig/Skeleton3D/Chest/ChestSlot/SurroundingHitbox");

		collision = GetNode<CollisionShape3D>("CollisionShape3D");

		


		
		head_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/Head/HeadSlot");
		helm = new MeshInstance3D();
		shoulder_right_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/ShoulderRight/ShoulderRightSlot");
		shoulder_right = new MeshInstance3D();
		shoulder_left_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/ShoulderLeft/ShoulderLeftSlot");
		shoulder_left = new MeshInstance3D();
		chest_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/Chest/ChestSlot");
		chest = new MeshInstance3D();

		mark_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/Mark/MarkSlot");
		mark = new MeshInstance3D();
		belt_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/Belt/BeltSlot");
		belt = new MeshInstance3D();
		glove_right_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/GloveRight/GloveRightSlot");
		glove_right = new MeshInstance3D();
		glove_left_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/GloveLeft/GloveLeftSlot");
		glove_left = new MeshInstance3D();
		main_hand_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/MainHand/MainHandSlot");
		main_hand = new MeshInstance3D();
		off_hand_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/OffHand/OffHandSlot");
		off_hand = new MeshInstance3D();
		leg_right_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/LegRight/LegRightSlot");
		leg_right = new MeshInstance3D();
		leg_left_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/LegLeft/LegLeftSlot");
		leg_left = new MeshInstance3D();
		foot_right_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/FootRight/FootRightSlot");
		foot_right = new MeshInstance3D();
		foot_left_slot = GetNode<Node3D>("Character_GameRig/Skeleton3D/FootLeft/FootLeftSlot");

		near_wall = GetNode<RayCast3D>("Controllers/WallCheck/NearWall");
		on_wall = GetNode<RayCast3D>("Controllers/WallCheck/OnWall");

		hurtbox = GetNode<Hurtbox>("Character_GameRig/Skeleton3D/Chest/ChestSlot/Hurtbox");
		hurtbox.AreaEntered += OnHurtboxBodyEntered;
		
		foot_left = new MeshInstance3D();


		tree = GetNode<AnimationTree>("Animation/AnimationTree");
		animation_player = GetNode<AnimationPlayer>("Animation/AnimationPlayer");
	
		_customSignals.RemoveEquipped += HandleRemoveEquipped;
		
		maximum_health = health;
		resource = maximum_resource;

		// GD.Print("max health player ", maximum_health);
		// GD.Print("max resource ", maximum_resource);
		// GD.Print("physical resistance", physical_resistance);
		// GD.Print("spell resistance ", spell_resistance);
		exclude.Add(vision.GetRid());
		exclude.Add(soft_target_small.GetRid());
		exclude.Add(soft_target_large.GetRid());
		exclude.Add(interact_area.GetRid());
		exclude.Add(hurtbox.GetRid());
		exclude.Add(main_hand_hitbox.GetRid());
		exclude.Add(GetRid());
		// exclude.Add(hitbox.GetRid());
		// GD.Print("exclude: " + exclude);
		movement_controller.GetPlayerInfo(this);
		equipment_controller.GetPlayerInfo(this);
		stat_controller.GetEntityInfo(this);
		ability_controller.GetPlayerInfo(this);
		ability_controller.LoadAbilities();

		ui.GetPlayerInfo(this);
		ui.hud.health.MaxValue = maximum_health;
		ui.hud.health.Value = health;
		ui.hud.resource.MaxValue = maximum_resource;
		ui.hud.resource.Value = resource;
		ui.hud.posture.MaxValue = maximum_posture;
		ui.hud.posture.Value = 0;
		ui.hud.xp.MaxValue = xp_to_level;
		ui.hud.xp.Value = xp;

		camera_rig.GetPlayerInfo(this);

		// GD.Print("This should be physical: " + main_hand_hitbox.damage_type);
		// GD.Print("Hurtbox type " + hurtbox.GetType());
	}

    private void OnHurtboxBodyEntered(Node3D body)
    {
		// GD.Print("Hitbox entered " + this.Name);
		if(body is MeleeHitbox box)
		{
			if(body.IsInGroup("ActiveHitbox") && body is MeleeHitbox)
			{
				damage_system.TakeDamage(box.damage_type, box.damage, box.is_critical);
				resource_system.Posture(box.posture_damage);
			}
		}
    }

	

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		// GD.Print("Hitbox disabled " + hitbox_collision.Disabled);
		CameraFollowsPlayer();
		Updater(); // Emits signals to other parts of the game
		CanUseAbilities();
		ability_controller.AssignAbilities();
		position = GlobalPosition;
		CheckInteract(); // Check if the player can interact with anything
		// if(ability_in_use != null)
		// {
		// 	// GD.Print("Ability in use " + ability_in_use.Name);
			
		// 	foreach(Ability ability in abilities_in_use)
		// 	{
		// 		GD.Print("ability in use " + ability.Name);
		// 	}
			
		// }
		

		// if(abilities_in_use.Count > 1)
		// {
		// 	GD.Print("Ability being used " + ability_in_use.Name + " ability type " + ability_in_use.resource.type);
		// }
		// else
		// {
		// 	ability_in_use = null;
		// 	GD.Print("No ability being used");
		// }
		
    }

    
    private void OnVisionEntered(Node3D body) // handler for area entered signal
	{
		if(body is Enemy enemy)
		{
			// GD.Print("Entity entered vision");
			targeting_system.EnemyEnteredVision(enemy);
		}
	}

	private void OnVisionExited(Node3D body) // handler for area exited signal
	{
		
		if (body is Enemy enemy)
		{
			targeting_system.EnemyExitedVision(enemy);
			
		}
		
	}

	private void OnSoftTargetSmallEntered(Node3D body)
    {
        if(body is Enemy enemy)
		{
			targeting_system.EnemyEnteredSoftSmall(enemy);
		}
    }
	private void OnSoftTargetSmallExited(Node3D body)
    {
       if(body is Enemy enemy)
		{
			targeting_system.EnemyExitedSoftSmall(enemy);
		}
    }

	private void OnSoftTargetLargeEntered(Node3D body)
    {
       if(body is Enemy enemy)
		{
			targeting_system.EnemyEnteredSoftLarge(enemy);
		}
    }

	private void OnSoftTargetLargeExited(Node3D body)
    {
       if(body is Enemy enemy)
		{
			targeting_system.EnemyExitedSoftLarge(enemy);
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
		// else if(is_climbing) // Use the rotation that is calculated in MovementController when the player is climbing
		// {
		// 	prev_y_rotation = GlobalRotation.Y;
		// 	if(prev_y_rotation != current_y_rotation)
		// 	{
		// 	GlobalRotation = GlobalRotation with {Y = current_y_rotation};
		// 	}
		// }
	}

	public void Updater() // Emit signals
	{
		if(stats_changed)
		{
			// _customSignals.EmitSignal(nameof(CustomSignals.UIHealth), health);
			stats_changed = false;
			stat_controller.UpdateStats();
			// _customSignals.EmitSignal(nameof(CustomSignals.PlayerInfo), this_player);
			stats_updated = false;
		}
		// _customSignals.EmitSignal(nameof(CustomSignals.PlayerPosition), GlobalPosition); // Sends player position to enemy
		// _customSignals.EmitSignal(nameof(CustomSignals.Targeting), targeting, mob_to_LookAt_pos);
	}

	public void CanUseAbilities()
	{
		if (ui.inventory_open)
		{
			can_use_abilities = false;
		}
		else
		{
			can_use_abilities = true;
		}
	}

	private void HandleRemoveEquipped() // Removes equiped items
    {
		GD.Print("remove equipped");
        head_slot.RemoveChild(main_node);
    }

	public void PrintStats()
	{
		GD.Print("Strength: " + strength);
		GD.Print("Dexterity: " + dexterity);
		GD.Print("intellect: " + intellect);
		GD.Print("Physical Resistance: " + physical_resistance);
	}

	public void CameraFollowsPlayer()
	{
		// var camera_transform = new Transform3D();
		// var pos = GlobalTransform.Origin;
		// camera_transform.Origin = pos;
		// camera_rig.GlobalTransform = camera_transform;

		var camera_transform = new Transform3D();
		var pos = player_mesh.GlobalTransform.Origin;
		camera_transform.Origin = pos;
		camera_rig.GlobalTransform = camera_transform;
		
	}

	
}
