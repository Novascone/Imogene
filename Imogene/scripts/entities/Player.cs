using Godot;
using System.Collections.Generic;


// Player class, handles movement, abilities, sends signals to the UI

public partial class Player : PlayerEntity
{

	// Player reference
	public Player this_player; // Player
	public RayCast3D raycast;

	// Abilities
	private Target target_ability; // Target enemies
	public List<AbilityResource> ability_resources = new List<AbilityResource>(); // List of Ability Resources to load abilities from. Each AbilityResource Contains int id, string name, string ability_path, Texture2D icon, string type as well as 5 PackedScenes containing modifiers for the ability
	public List<Ability> abilities = new List<Ability>(); // List of abilities the player has access to. The Abilities are loaded from a PackedScene which is a Node3D with a script attached to it
	public bool abilities_loaded = false; // Bool to check if abilities are loaded and to load them/ send out the proper signals
	public Ability ability_in_use; // The ability that the player is currently using
	public LinkedList<Ability> abilities_in_use = new LinkedList<Ability>();
	public bool test_abilities_assigned = false;
	
	// Crosses
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

	private MeshInstance3D targeting_icon; // Targeting icon

	// Ability Resources
	public AbilityResource roll = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Roll/roll.tres");
	public AbilityResource slash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Slash/Slash.tres");
	public AbilityResource thrust = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Thrust/Thrust.tres");
	public AbilityResource bash = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Bash/Bash.tres");
	public AbilityResource jump = ResourceLoader.Load<AbilityResource>("res://scripts/abilities/Jump/Jump.tres");

	

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

		// hurtbox = GetNode<Area3D>("Hurtbox");
		hurtbox.AreaEntered += OnHurtboxEntered;
		hurtbox.AreaExited += OnHurtboxExited;

		movementController = GetNode<MovementController>("MovementController");
		abilityController = GetNode<AbilityController>("AbilityController");
		statController = GetNode<StatController>("StatController");
		equipmentController = GetNode<EquipmentController>("EquipmentController");

		head_slot = GetNode<Node3D>("Skeleton3D/Head/HeadSlot");
		helm = new MeshInstance3D();
		shoulder_right_slot = GetNode<Node3D>("Skeleton3D/ShoulderRight/ShoulderRightSlot");
		shoulder_right = new MeshInstance3D();
		shoulder_left_slot = GetNode<Node3D>("Skeleton3D/ShoulderLeft/ShoulderLeftSlot");
		shoulder_left = new MeshInstance3D();
		chest_slot = GetNode<Node3D>("Skeleton3D/Chest/ChestSlot");
		chest = new MeshInstance3D();
		mark_slot = GetNode<Node3D>("Skeleton3D/Mark/MarkSlot");
		mark = new MeshInstance3D();
		belt_slot = GetNode<Node3D>("Skeleton3D/Belt/BeltSlot");
		belt = new MeshInstance3D();
		glove_right_slot = GetNode<Node3D>("Skeleton3D/GloveRight/GloveRightSlot");
		glove_right = new MeshInstance3D();
		glove_left_slot = GetNode<Node3D>("Skeleton3D/GloveLeft/GloveLeftSlot");
		glove_left = new MeshInstance3D();
		main_hand_slot = GetNode<Node3D>("Skeleton3D/MainHand/MainHandSlot");
		main_hand = new MeshInstance3D();
		off_hand_slot = GetNode<Node3D>("Skeleton3D/OffHand/OffHandSlot");
		off_hand = new MeshInstance3D();
		leg_right_slot = GetNode<Node3D>("Skeleton3D/LegRight/LegRightSlot");
		leg_right = new MeshInstance3D();
		leg_left_slot = GetNode<Node3D>("Skeleton3D/LegLeft/LegLeftSlot");
		leg_left = new MeshInstance3D();
		foot_right_slot = GetNode<Node3D>("Skeleton3D/FootRight/FootRightSlot");
		foot_right = new MeshInstance3D();
		foot_left_slot = GetNode<Node3D>("Skeleton3D/FootLeft/FootLeftSlot");
		foot_left = new MeshInstance3D();


		tree = GetNode<AnimationTree>("AnimationTree");
		// tree.AnimationFinished += OnAnimationFinished;

		// raycast = GetNode<RayCast3D>("RayCast3D");
		

		// hitbox = (Area3D)GetNode("Skeleton3D/MainHand/axe/Hitbox");
		// hitbox.AreaEntered += OnHitboxEntered;

		_customSignals.EnemyPosition += HandleEnemyPosition;
		_customSignals.RemoveEquipped += HandleRemoveEquipped;
		
		GD.Print("max health ", maximum_health);
		GD.Print("max resource ", maximum_resource);
		GD.Print("physical resistance", physical_resistance);
		GD.Print("spell resistance ", spell_resistance);
		exclude.Add(vision.GetRid());
		// exclude.Add(hitbox.GetRid());
		GD.Print("exclude: " + exclude);
		movementController.GetPlayerInfo(this);
		equipmentController.GetPlayerInfo(this);
		statController.GetPlayerInfo(this);
		abilityController.GetPlayerInfo(this);
		abilityController.LoadAbilities();

		GD.Print("This should be physical: " + main_hand_hitbox.damage_type);
		GD.Print("Hurtbox type " + hurtbox.GetType());
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		SignalEmitter(); // Emits signals to other parts of the game
		abilityController.AssignAbilities();
		position = GlobalPosition;

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

		CheckInteract(); // Check if the player can interact with anything
		EnemyCheck(); // Check for enemy
    }

	private void OnHitboxEntered(Area3D hitbox) // handler for area entered signal
	{
		if(hitbox.IsInGroup("enemy"))
		{
			
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
			statController.UpdateStats();
			_customSignals.EmitSignal(nameof(CustomSignals.PlayerInfo), this_player);
			stats_updated = false;
		}
		
		_customSignals.EmitSignal(nameof(CustomSignals.PlayerPosition), GlobalPosition); // Sends player position to enemy
		_customSignals.EmitSignal(nameof(CustomSignals.Targeting), targeting, mob_to_LookAt_pos);
	}


	private void HandleDamage(float DamageAmount) // Sends damage amount to enemy
	{
			DamageAmount += damage;
	}

	private void HandleEnemyPosition(Vector3 position) // Gets enemy position from enemy
    {
        enemy_position = position;
    }

	// UI

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

	
}
