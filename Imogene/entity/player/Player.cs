using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;


// Player class, handles movement, abilities, sends signals to the UI

public partial class Player : Entity
{

	// Player reference
	public MeshInstance3D player_mesh;
	public RayCast3D raycast;
	[Export] public CameraRig camera_rig;
	
	
	[Export] public Marker3D cast_point;
	[Export] public Node3D surrounding_hitbox;
	[Export] public NewUI ui;
	[Export] public Areas areas;
	[Export] public Controllers controllers;
	[Export] public Systems systems;
	[Export] public AnimationTree tree; // Animation control
	[Export] public AnimationPlayer animation_player;
	[Export] public Node abilities;
	

	// Abilities
	// private Target target_ability; // Target enemies
	public bool abilities_loaded = false; // Bool to check if abilities are loaded and to load them/ send out the proper signals
	public Ability ability_in_use; // The ability that the player is currently using
	public LinkedList<Ability> abilities_in_use = new LinkedList<Ability>();
	public List<Ability> ability_list = new List<Ability>();
	public bool test_abilities_assigned = false;
	
	
	public bool l_cross_primary_selected; // Bool that tracks which left cross the player is using 
	public bool r_cross_primary_selected; // Bool that tracks which right cross the player is using 

	
	

	//Player consumables
	public int consumable = 1;
	public ConsumableResource[] consumables = new ConsumableResource[4];


	// Player animation
	

	public float move_forward_clamber = 0;
	public float vertical_input;

	public bool ability_preventing_movement;

	// public CollisionShape3D hitbox_collision;
	
	public float jump_height = 30;
	public float jump_time_to_peak = 2f;
	public float jump_time_to_decent = 1.9f;

	public float jump_velocity ;
	public float jump_gravity;
	public float fall_gravity;

	public Vector2 blend_direction = Vector2.Zero; // Blend Direction of the player for changing animation
	
	
	// Objects to exclude from ray casting
	public Godot.Collections.Array<Rid> exclude = new Godot.Collections.Array<Rid>();

	[Export] public MeshInstance3D land_point;
	public Vector3 land_point_position;


	public bool is_climbing;
	public bool is_clambering;
	

	// Ability Variables
	public bool recovery_1 = false;
	public bool recovery_2 = false;
	public bool action_1_set;
	public bool action_2_set;
	public bool using_ability; // Is the entity using an ability?
	public bool can_use_abilities = true;



	

	public override void _Ready()
	{
		
		base._Ready();
		dr_lvl_scale = 50 * (float)level;
		rec_lvl_scale = 100 * (float)level;
	
		
		resource = maximum_resource/2;
		Ability jump = (Ability)controllers.ability_assigner.LoadAbility(this, "jump", "general", "active");
		// Ability slash = (Ability)ability_assigner.LoadAbility(this, "Slash", "General", "Active");
		Ability effect_test = (Ability)controllers.ability_assigner.LoadAbility(this, "effect_test", "general", "active");
		Ability kick = (Ability)controllers.ability_assigner.LoadAbility(this, "kick", "general", "active");
		Ability projectile = (Ability)controllers.ability_assigner.LoadAbility(this, "projectile", "general", "active");
		Ability whirlwind = (Ability)controllers.ability_assigner.LoadAbility(this, "whirlwind", "brigian", "active");
		Ability hitscan = (Ability)controllers.ability_assigner.LoadAbility(this, "hitscan", "general", "active");
		Ability dash = (Ability)controllers.ability_assigner.LoadAbility(this, "dash", "general", "active");
		
		controllers.ability_assigner.AssignAbility(this, jump, "A", "Right", "Primary");
		// ability_assigner.AssignAbility(this, slash, "RB", "Left", "Primary");
		controllers.ability_assigner.AssignAbility(this, effect_test, "RT", "Left", "Primary");
		controllers.ability_assigner.AssignAbility(this, kick, "LB", "Left", "Primary");
		controllers.ability_assigner.AssignAbility(this, projectile, "LT", "Left", "Primary");
		controllers.ability_assigner.AssignAbility(this, whirlwind, "X", "Right", "Primary");
		controllers.ability_assigner.AssignAbility(this, hitscan, "Y", "Right", "Primary");
		controllers.ability_assigner.AssignAbility(this, dash, "B", "Right", "Primary");


		jump_velocity = (float)(2.0 * jump_height / jump_time_to_peak);
		jump_gravity = (float)(-2.0 * jump_height / jump_time_to_peak * jump_time_to_peak);
		fall_gravity = (float)(-2.0 * jump_height / jump_time_to_decent * jump_time_to_decent);
		

		l_cross_primary_selected = true;
		r_cross_primary_selected = true;		


		camera_rig.TopLevel = true;

		controllers.movement_controller = GetNode<MovementController>("Controllers/MovementController");
		controllers.equipment_controller = GetNode<EquipmentController>("Controllers/EquipmentController");


		systems.vision_system.SubscribeToAreaSignals(this);
		systems.interact_system.SubscribeToInteractSignals(this);
		ui.hud.SubscribeToTargetingSignals(this);
		ui.hud.SubscribeToInteractSignals(this);

	
		// hurtbox.AreaEntered += OnHurtboxAreaEntered;
		// hurtbox.BodyEntered += OnHurtboxBodyEntered;
		entity_systems.damage_system.SubscribeToHurtboxSignals(this);

		controllers.input_controller.CrossChanged += HandleCrossChanged;

		systems.interact_system.NearInteractable += controllers.ability_controller.OnNearInteractable;
		systems.interact_system.ItemPickedUp += ui.inventory.main.OnItemPickedUp;
		systems.interact_system.InputPickUp += HandleInputPickUp;
		ui.inventory.main.DroppingItem += HandleDroppingItem;
		ui.CapturingInput += systems.interact_system.HandleCapturingInput;

		entity_controllers.stats_controller.StatsUpdate += ui.HandleUpdatedStats;
		

		controllers.ability_controller.ResourceEffect += entity_systems.resource_system.HandleResourceEffect;

		ui.InventoryToggle += HandleInventoryToggle;
		ui.abilities.categories.ClearAbilityBind += HandleClearAbilityBind;
		ui.abilities.categories.AbilityReassigned += HandleAbilityReassigned;


		
		entity_controllers.stats_controller.UpdateStats(this);
		

		maximum_health = health;
		resource = maximum_resource;

		exclude.Add(areas.vision.GetRid());
		exclude.Add(areas.near.GetRid());
		exclude.Add(areas.far.GetRid());
		exclude.Add(areas.interact.GetRid());
		exclude.Add(hurtbox.GetRid());
		exclude.Add(main_hand_hitbox.GetRid());
		exclude.Add(GetRid());

		controllers.ability_assigner.GetAbilities(this);
		controllers.ability_assigner.AssignAbilities(this);
	}

    private void HandleInputPickUp(InteractableItem item)
    {
        systems.interact_system.PickUpItem(item, this);
    }

    private void HandleDroppingItem()
    {
		GD.Print("Received signal for dropping item");
        ui.inventory.main.GetDropPosition(this);
    }

    private void HandleClearAbilityBind(string ability_name)
    {
        controllers.ability_assigner.ClearAbility(this, ability_name);
    }

    private void HandleAbilityReassigned(string cross, string level, string bind, string ability_name, Texture2D icon)
    {
		GD.Print("Player received ability reassigned signal");
        controllers.ability_assigner.ChangeAbilityAssignment(this, cross, level, bind, ability_name);
    }

    // private void OnHurtboxAreaEntered(Area3D area)
    // {
    //     if(area is MeleeHitbox melee_box)
	// 	{
	// 		if(area.IsInGroup("ActiveHitbox") && area is MeleeHitbox)
	// 		{
	// 			foreach(StatusEffect status_effect in melee_box.effects)
	// 			{
	// 				GD.Print("Applying " + status_effect.Name + " to " + Name);
	// 				status_effect.Apply(this);
	// 				// if(status_effect.effect_type == "movement")
	// 				// {
	// 				// 	previous_movement_effects_count = movement_effects.Count;
	// 				// }
	// 			}
	// 			damage_system.TakeDamage(melee_box.damage_type, melee_box.damage, melee_box.is_critical);
	// 			resource_system.Posture(this, melee_box.posture_damage);
	// 		}
	// 	}
    // }

    // private void OnHurtboxBodyEntered(Node3D body)
    // {
	// 	// GD.Print("Hitbox entered " + this.Name);
	// 	if(body is RangedHitbox ranged_box)
	// 	{
	// 		if(body.IsInGroup("ActiveHitbox") && body is RangedHitbox)
	// 		{
	// 			foreach(StatusEffect status_effect in ranged_box.effects)
	// 			{
	// 				GD.Print("Applying " + status_effect.Name + " to " + Name);
	// 				status_effect.Apply(this);
	// 				// if(status_effect.effect_type == "movement")
	// 				// {
	// 				// 	previous_movement_effects_count = movement_effects.Count;
	// 				// }
	// 			}
	// 			damage_system.TakeDamage(ranged_box.damage_type, ranged_box.damage, ranged_box.is_critical);
	// 			resource_system.Posture(this, ranged_box.posture_damage);
	// 		}
	// 	}
    // }

	

	

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _PhysicsProcess(double delta)
    {
		
		CameraFollowsPlayer();
		controllers.input_controller.SetInput(this);
		controllers.movement_controller.MovePlayer(this, controllers.input_controller.input_strength, delta);
		systems.targeting_system.Target(this);
		controllers.ability_controller.AbilityFrameCheck(this);
		MoveAndSlide();

	
		
    }

	private void HandleCrossChanged(string cross)
    {
		if(!ui.preventing_movement)
		{
			ui.SwitchCrosses(cross);
			if(cross == "Left")
			{
				l_cross_primary_selected = !l_cross_primary_selected;
			}
			else if(cross == "Right")
			{
				r_cross_primary_selected = !r_cross_primary_selected;
			}
		}
		
    }

	private void HandleInventoryToggle()
    {
		camera_rig.Zoom();
    }

	 internal void OnAbilityPressed(Ability ability)
    {
        GD.Print(ability.Name + " has been pressed and the player has received the signal");
		controllers.ability_controller.QueueAbility(this, ability);
		controllers.ability_controller.CheckCanUseAbility(this, ability);
    }

	internal void OnAbilityQueue(Ability ability)
    {
		GD.Print("Queueing ability again");
        controllers.ability_controller.QueueAbility(this, ability);
    }

    internal void OnAbilityCheck(Ability ability)
    {
		GD.Print("Checking ability again");
        controllers.ability_controller.CheckCanUseAbility(this, ability);
    }

	internal void OnAbilityReleased(Ability ability)
    {
        GD.Print("Ability released");
    }

	internal void OnAbilityFinished(Ability ability)
    {
		// GD.Print("Removing ability from list");
        controllers.ability_controller.RemoveFromAbilityList(this, ability);
		if(ability.general_ability_type == Ability.GeneralAbilityType.Movement)
		{
			using_movement_ability = false;
		}
    }

	public void CameraFollowsPlayer()
	{
		var camera_transform = new Transform3D();
		var pos = GlobalTransform.Origin;
		camera_transform.Origin = pos;
		camera_rig.GlobalTransform = camera_transform;	
	}

    
}
