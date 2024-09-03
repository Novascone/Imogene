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
	public bool abilities_loaded = false; // Bool to check if abilities are loaded and to load them/ send out the proper signals
	public Ability ability_in_use; // The ability that the player is currently using
	public LinkedList<Ability> abilities_in_use = new LinkedList<Ability>();
	public List<Ability> ability_list = new List<Ability>();
	public bool test_abilities_assigned = false;
	
	// UI
	[Export] public NewUI ui;
	public bool l_cross_primary_selected; // Bool that tracks which left cross the player is using 
	public bool r_cross_primary_selected; // Bool that tracks which right cross the player is using 
	

	// Controllers
	[Export] public InputController input_controller;
	[Export] public MovementController movement_controller;
	[Export] public AbilityAssigner ability_assigner;
	[Export] public AbilityController ability_controller;
	public StatController stat_controller;
	public EquipmentController equipment_controller;

	//Player consumables
	public int consumable = 1;
	public ConsumableResource[] consumables = new ConsumableResource[4];


	// Player animation
	public AnimationTree tree; // Animation control
	public AnimationPlayer animation_player;

	// Abilities
	[Export] public Node abilities;

	public float move_forward_clamber = 0;
	public float vertical_input;

	public bool targeting = false; // Is the entity targeting?= 1 - (50 * level / (50 * level + poison_resistance));

	public bool ability_preventing_movement;

	public CollisionShape3D hitbox_collision;
	
	public float jump_height = 30;
	public float jump_time_to_peak = 2f;
	public float jump_time_to_decent = 1.9f;

	public float jump_velocity ;
	public float jump_gravity;
	public float fall_gravity;


	

	public override void _Ready()
	{
		
		base._Ready();

		

		Ability jump = (Ability)ability_assigner.LoadAbility(this, "Jump", "General", "Active");
		// Ability slash = (Ability)ability_assigner.LoadAbility(this, "Slash", "General", "Active");
		Ability effect_test = (Ability)ability_assigner.LoadAbility(this, "EffectTest", "General", "Active");
		Ability kick = (Ability)ability_assigner.LoadAbility(this, "Kick", "General", "Active");
		Ability projectile = (Ability)ability_assigner.LoadAbility(this, "Projectile", "General", "Active");
		Ability whirlwind = (Ability)ability_assigner.LoadAbility(this, "Whirlwind", "Brigian", "Active");
		Ability hitscan = (Ability)ability_assigner.LoadAbility(this, "Hitscan", "General", "Active");
		Ability dash = (Ability)ability_assigner.LoadAbility(this, "Dash", "General", "Active");
		
		ability_assigner.AssignAbility(this, jump, "A", "Right", "Primary");
		// ability_assigner.AssignAbility(this, slash, "RB", "Left", "Primary");
		ability_assigner.AssignAbility(this, effect_test, "RT", "Left", "Primary");
		ability_assigner.AssignAbility(this, kick, "LB", "Left", "Primary");
		ability_assigner.AssignAbility(this, projectile, "LT", "Left", "Primary");
		ability_assigner.AssignAbility(this, whirlwind, "X", "Right", "Primary");
		ability_assigner.AssignAbility(this, hitscan, "Y", "Right", "Primary");
		ability_assigner.AssignAbility(this, dash, "RB", "Left", "Primary");


		jump_velocity = (float)(2.0 * jump_height / jump_time_to_peak);
		jump_gravity = (float)(-2.0 * jump_height / jump_time_to_peak * jump_time_to_peak);
		fall_gravity = (float)(-2.0 * jump_height / jump_time_to_decent * jump_time_to_decent);
		

		l_cross_primary_selected = true;
		r_cross_primary_selected = true;		

		cast_point = GetNode<Marker3D>("Character_GameRig/Skeleton3D/MainHand/CastPoint");
		

		camera_rig = GetNode<CameraRig>("CameraRig");
		camera_rig.TopLevel = true;

		movement_controller = GetNode<MovementController>("Controllers/MovementController");
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

		


		near_wall = GetNode<RayCast3D>("Controllers/WallCheck/NearWall");
		on_wall = GetNode<RayCast3D>("Controllers/WallCheck/OnWall");

		hurtbox = GetNode<Hurtbox>("Character_GameRig/Skeleton3D/Chest/ChestSlot/Hurtbox");
		hurtbox.AreaEntered += OnHurtboxAreaEntered;
		hurtbox.BodyEntered += OnHurtboxBodyEntered;
		
		
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
		// movement_controller.GetPlayerInfo(this);
		equipment_controller.GetPlayerInfo(this);
		stat_controller.GetEntityInfo(this);
		ability_assigner.GetAbilities(this);
		ability_assigner.AssignAbilities(this);

		// ability_controller.SubscribeToUI(ui);

		
		
		// ui.GetPlayerInfo(this);
		// ui.hud.health.MaxValue = maximum_health;
		// ui.hud.health.Value = health;
		// ui.hud.resource.MaxValue = maximum_resource;
		// ui.hud.resource.Value = resource;
		// ui.hud.posture.MaxValue = maximum_posture;
		// ui.hud.posture.Value = 0;
		// ui.hud.xp.MaxValue = xp_to_level;
		// ui.hud.xp.Value = xp;

		

		

		camera_rig.GetPlayerInfo(this);

		

		

		// GD.Print("This should be physical: " + main_hand_hitbox.damage_type);
		// GD.Print("Hurtbox type " + hurtbox.GetType());
	}

    private void OnHurtboxAreaEntered(Area3D area)
    {
        if(area is MeleeHitbox melee_box)
		{
			if(area.IsInGroup("ActiveHitbox") && area is MeleeHitbox)
			{
				foreach(StatusEffect status_effect in melee_box.effects)
				{
					GD.Print("Applying " + status_effect.Name + " to " + Name);
					status_effect.Apply(this);
					// if(status_effect.effect_type == "movement")
					// {
					// 	previous_movement_effects_count = movement_effects.Count;
					// }
				}
				damage_system.TakeDamage(melee_box.damage_type, melee_box.damage, melee_box.is_critical);
				resource_system.Posture(melee_box.posture_damage);
			}
		}
    }

    private void OnHurtboxBodyEntered(Node3D body)
    {
		// GD.Print("Hitbox entered " + this.Name);
		if(body is RangedHitbox ranged_box)
		{
			if(body.IsInGroup("ActiveHitbox") && body is RangedHitbox)
			{
				foreach(StatusEffect status_effect in ranged_box.effects)
				{
					GD.Print("Applying " + status_effect.Name + " to " + Name);
					status_effect.Apply(this);
					// if(status_effect.effect_type == "movement")
					// {
					// 	previous_movement_effects_count = movement_effects.Count;
					// }
				}
				damage_system.TakeDamage(ranged_box.damage_type, ranged_box.damage, ranged_box.is_critical);
				resource_system.Posture(ranged_box.posture_damage);
			}
		}
    }

	

	

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		// GD.Print("Hitbox disabled " + hitbox_collision.Disabled);
		if(ability_in_use != null)
		{
			GD.Print("ability in use" + ability_in_use);
			ability_in_use.FrameCheck(this);
		}

		GD.Print("using movement ability " + using_movement_ability);
		GD.Print("player velocity " + Velocity);

		
		CameraFollowsPlayer();
		// CheckInteract(); // Check if the player can interact with anything
		input_controller.SetInput(this);
		movement_controller.MovePlayer(this, input_controller.input_strength, delta);
		MoveAndSlide();
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

	 internal void OnAbilityPressed(Ability ability)
    {
        GD.Print(ability.Name + " has been pressed and the player has received the signal");
		ability_controller.QueueAbility(this, ability);
		ability_controller.CheckCanUseAbility(this, ability);
    }

	internal void OnAbilityQueue(Ability ability)
    {
		GD.Print("Queueing ability again");
        ability_controller.QueueAbility(this, ability);
    }

    internal void OnAbilityCheck(Ability ability)
    {
		GD.Print("Checking ability again");
        ability_controller.CheckCanUseAbility(this, ability);
    }

	internal void OnAbilityReleased(Ability ability)
    {
        GD.Print("Ability released");
    }

	internal void OnAbilityFinished(Ability ability)
    {
		GD.Print("Removing ability from list");
        ability_controller.RemoveFromAbilityList(this, ability);
		if(ability.general_ability_type == Ability.GeneralAbilityType.Movement)
		{
			using_movement_ability = false;
		}
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

	

	public void SmoothRotation(Vector3 direction) // Rotates the player character smoothly with lerp
	{
		GD.Print("Rotating smoothly");
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
