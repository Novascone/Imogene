using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;


// Player class, handles movement, abilities, sends signals to the UI

public partial class Player : PlayerEntity
{

	// Player reference
	public Player this_player; // Player
	public RayCast3D raycast;
	public RayCast3D near_wall;
	public RayCast3D on_wall;
	public CameraRig camera_rig;
	public Area3D vision; // Area where the player can target enemies

	// Abilities
	// private Target target_ability; // Target enemies
	public List<AbilityResource> ability_resources = new List<AbilityResource>(); // List of Ability Resources to load abilities from. Each AbilityResource Contains int id, string name, string ability_path, Texture2D icon, string type as well as 5 PackedScenes containing modifiers for the ability
	public List<Ability> abilities = new List<Ability>(); // List of abilities the player has access to. The Abilities are loaded from a PackedScene which is a Node3D with a script attached to it
	public bool abilities_loaded = false; // Bool to check if abilities are loaded and to load them/ send out the proper signals
	public Ability ability_in_use; // The ability that the player is currently using
	public LinkedList<Ability> abilities_in_use = new LinkedList<Ability>();
	public bool test_abilities_assigned = false;
	
	// UI
	public bool l_cross_primary_selected; // Bool that tracks which left cross the player is using 
	public bool r_cross_primary_selected; // Bool that tracks which right cross the player is using 
	

	// Controllers
	public MovementController movementController;
	public AbilityController abilityController;
	public StatController statController;
	public EquipmentController equipmentController;

	//Player consumables
	public int consumable = 1;
	public ConsumableResource[] consumables = new ConsumableResource[4];


	// Player animation
	public AnimationTree tree; // Animation control


	// Ability Resources
	public AbilityResource roll = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Roll/roll.tres");
	public AbilityResource slash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Slash/Slash.tres");
	public AbilityResource thrust = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Thrust/Thrust.tres");
	public AbilityResource bash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Bash/Bash.tres");
	public AbilityResource jump = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Jump/Jump.tres");

	public float move_forward_clamber = 0;
	public float vertical_input;

	public bool targeting = false; // Is the entity targeting?= 1 - (50 * level / (50 * level + poison_resistance));
	

	

	public override void _Ready()
	{
		
		base._Ready();
		this_player = this;

		ability_resources.Add(roll);
		ability_resources.Add(slash);
		ability_resources.Add(thrust);
		ability_resources.Add(bash);
		ability_resources.Add(jump);

		l_cross_primary_selected = true;
		r_cross_primary_selected = true;		

		
		

		camera_rig = GetNode<CameraRig>("CameraRig");
		camera_rig.TopLevel = true;

		movementController = GetNode<MovementController>("Controllers/MovementController");
		abilityController = GetNode<AbilityController>("Controllers/AbilityController");
		statController = GetNode<StatController>("Controllers/StatController");
		equipmentController = GetNode<EquipmentController>("Controllers/EquipmentController");


		vision  = (Area3D)GetNode("Areas/Vision");
		vision.BodyEntered += OnVisionEntered;
		vision.BodyExited += OnVisionExited;

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
	
		_customSignals.RemoveEquipped += HandleRemoveEquipped;
		
		maximum_health = health;
		resource = maximum_resource / 2;

		GD.Print("max health player ", maximum_health);
		GD.Print("max resource ", maximum_resource);
		GD.Print("physical resistance", physical_resistance);
		GD.Print("spell resistance ", spell_resistance);
		exclude.Add(vision.GetRid());
		// exclude.Add(hitbox.GetRid());
		GD.Print("exclude: " + exclude);
		movementController.GetPlayerInfo(this);
		equipmentController.GetPlayerInfo(this);
		statController.GetEntityInfo(this);
		abilityController.GetPlayerInfo(this);
		abilityController.LoadAbilities();

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

		GD.Print("This should be physical: " + main_hand_hitbox.damage_type);
		GD.Print("Hurtbox type " + hurtbox.GetType());
	}

	private void OnHurtboxBodyEntered(Area3D body)
    {
		GD.Print("Hitbox entered " + this.Name);
		if(body is Hitbox box)
		{
			if(body.IsInGroup("ActiveHitbox") && body is Hitbox)
			{
				damage_system.TakeDamage(box.damage_type, box.damage, box.is_critical);
				resource_system.Posture(box.posture_damage);
			}
		}
    }

	

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		
		CameraFollowsPlayer();
		Updater(); // Emits signals to other parts of the game
		abilityController.AssignAbilities();
		position = GlobalPosition;


		CheckInteract(); // Check if the player can interact with anything
		targeting_system.EnemyCheck(); // Check for enemy
    }

	private void OnVisionEntered(Node3D body) // handler for area entered signal
	{
		if(body is Enemy enemy)
		{
			// GD.Print("Entity entered vision");
			targeting_system.EnemyEntered(enemy);
		}
	}

	private void OnVisionExited(Node3D body) // handler for area exited signal
	{
		
		if (body is Enemy enemy)
		{
			targeting_system.EnemyExited(enemy);
			
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

	public void Updater() // Emit signals
	{
		if(stats_changed)
		{
			// _customSignals.EmitSignal(nameof(CustomSignals.UIHealth), health);
			stats_changed = false;
			statController.UpdateStats();
			// _customSignals.EmitSignal(nameof(CustomSignals.PlayerInfo), this_player);
			stats_updated = false;
		}
		// _customSignals.EmitSignal(nameof(CustomSignals.PlayerPosition), GlobalPosition); // Sends player position to enemy
		// _customSignals.EmitSignal(nameof(CustomSignals.Targeting), targeting, mob_to_LookAt_pos);
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
		var camera_transform = new Transform3D();
		var pos = GlobalTransform.Origin;
		camera_transform.Origin = pos;
		camera_rig.GlobalTransform = camera_transform;
	}

	
}
