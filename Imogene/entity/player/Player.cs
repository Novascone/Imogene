using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;

public partial class Player : Entity
{
	[Export] public CameraRig camera_rig { get; set; }
	[Export] public Marker3D cast_point { get; set; }
	[Export] public Node3D surrounding_hitbox { get; set; }
	[Export] public NewUI ui { get; set; }
	[Export] public Areas areas { get; set; }
	[Export] public Controllers controllers { get; set; }
	[Export] public Systems systems { get; set; }
	[Export] public Node abilities  { get; set; }
	public Ability ability_in_use  { get; set; } = null; // The ability that the player is currently using
	public LinkedList<Ability> abilities_in_use_list = new();
	public bool l_cross_primary_selected { get; set; } = true; // Bool that tracks which left cross the player is using 
	public bool r_cross_primary_selected { get; set; } = true;  // Bool that tracks which right cross the player is using 
	
	public Godot.Collections.Array<Rid> excluded_rids { get; set; } = new Godot.Collections.Array<Rid>();

	
    public override void _Input(InputEvent @event_)
    {
		
		
        if(@event_.IsActionPressed("one"))
		{
			controllers.movement_controller.movement_input_prevented = !controllers.movement_controller.movement_input_prevented;
			controllers.input_controller.directional_input_prevented = !controllers.input_controller.directional_input_prevented;
		}
		if(@event_.IsActionPressed("two"))
		{
			Chill chill = new();
			entity_controllers.status_effect_controller.AddStatusEffect(this, chill);
		}
		if(@event_.IsActionPressed("three"))
		{
			Daze daze = new();
			entity_controllers.status_effect_controller.AddStatusEffect(this, daze);
		}
		if(@event_.IsActionPressed("four"))
		{
			entity_controllers.status_effect_controller.RemoveMovementDebuffs(this);
		}
		if(@event_.IsActionPressed("five"))
		{
			Tether tether = new();
			entity_controllers.status_effect_controller.AddStatusEffect(this, tether);
		}
		
    }



    public override void _Ready()
	{
		
		base._Ready();
		entity_controllers.status_effect_controller.fear_duration = 3;
		
		power.current_value = 50;
		resource.max_value = resource.max_value / 2;

		Ability jump = (Ability)controllers.ability_assigner.LoadAbility(this, "jump", "general", "active");
		Ability slash = (Ability)controllers.ability_assigner.LoadAbility(this, "slash", "general", "active");
		Ability effect_test = (Ability)controllers.ability_assigner.LoadAbility(this, "effect_test", "general", "active");
		Ability kick = (Ability)controllers.ability_assigner.LoadAbility(this, "kick", "general", "active");
		Ability projectile = (Ability)controllers.ability_assigner.LoadAbility(this, "projectile", "general", "active");
		Ability whirlwind = (Ability)controllers.ability_assigner.LoadAbility(this, "whirlwind", "brigian", "active");
		Ability hitscan = (Ability)controllers.ability_assigner.LoadAbility(this, "hitscan", "general", "active");
		Ability dash = (Ability)controllers.ability_assigner.LoadAbility(this, "dash", "general", "active");
		
		controllers.ability_assigner.AssignAbility(this, jump, "A", "Right", "Primary");
		controllers.ability_assigner.AssignAbility(this, slash, "RB", "Left", "Primary");
		controllers.ability_assigner.AssignAbility(this, effect_test, "RT", "Left", "Primary");
		controllers.ability_assigner.AssignAbility(this, kick, "LB", "Left", "Primary");
		controllers.ability_assigner.AssignAbility(this, projectile, "LT", "Left", "Primary");
		controllers.ability_assigner.AssignAbility(this, whirlwind, "X", "Right", "Primary");
		controllers.ability_assigner.AssignAbility(this, hitscan, "Y", "Right", "Primary");
		controllers.ability_assigner.AssignAbility(this, dash, "B", "Right", "Primary");

		camera_rig.TopLevel = true;

		// Entity system signals
		entity_systems.resource_system.ResourceChange += ui.hud.main.HandleResourceChange;

		// Entity controller signals
		entity_controllers.stats_controller.UpdateStats += ui.inventory.depth_sheet.HandleUpdateStats;
		entity_controllers.stats_controller.UpdateStats += ui.inventory.main.character_outline.HandleUpdateStats;
		entity_controllers.status_effect_controller.AbilitiesPrevented += controllers.ability_controller.HandleAbilitiesPrevented;

		// System signals
		systems.vision_system.SubscribeToAreaSignals(this);
		systems.interact_system.SubscribeToInteractSignals(this);
		systems.interact_system.NearInteractable += controllers.ability_controller.OnNearInteractable;
		systems.interact_system.ItemPickedUp += ui.inventory.main.OnItemPickedUp;
		systems.interact_system.InputPickUp += HandleInputPickUp;
		systems.targeting_system.RotationForAbilityFinished += controllers.ability_controller.HandleRotationFinished;
	
		// Controller signals
		controllers.ability_controller.RotatePlayer += systems.targeting_system.HandleRotatePlayer;
		controllers.input_controller.CrossChanged += HandleCrossChanged;

		// UI signals
		ui.hud.SubscribeToTargetingSignals(this);
		ui.hud.SubscribeToInteractSignals(this);
		ui.inventory.main.DroppingItem += HandleDroppingItem;
		ui.CapturingInput += systems.interact_system.HandleUICapturingInput;
		ui.InventoryToggle += HandleInventoryToggle;
		ui.abilities.categories.ClearAbilityBind += HandleClearAbilityBind;
		ui.abilities.categories.AbilityReassigned += HandleAbilityReassigned;
		controllers.input_controller.Subscribe(this);
		controllers.movement_controller.Subscribe(this);

		level.base_value = 4;
		strength.base_value = 10;
		dexterity.base_value = 5;
		vitality.base_value = 20;
		intellect.base_value = 11;
		physical_resistance.base_value = 6;

		entity_controllers.stats_controller.SetStats(this);
		entity_controllers.stats_controller.Update(this);
		controllers.ability_assigner.GetAbilities(this);
		controllers.ability_assigner.AssignAbilities(this);

		// exclude.Add(areas.vision.GetRid());
		excluded_rids.Add(areas.near.GetRid());
		excluded_rids.Add(areas.far.GetRid());
		excluded_rids.Add(areas.interact.GetRid());
		excluded_rids.Add(hurtbox.GetRid());
		excluded_rids.Add(main_hand_hitbox.GetRid());
		excluded_rids.Add(GetRid());

		
	}

    public override void _PhysicsProcess(double delta_)
    {
	
		CameraFollowsPlayer();
		systems.targeting_system.ray_cast.FollowPlayer(this);
		controllers.input_controller.SetInput(this);
		controllers.movement_controller.MovePlayer(this, controllers.input_controller.input_strength, delta_);
		systems.targeting_system.Target(this);
		controllers.ability_controller.AbilityFrameCheck(this);
		MoveAndSlide();
		
    }

    private void HandleInputPickUp(InteractableItem item_)
    {
        systems.interact_system.PickUpItem(item_, this);
    }

    private void HandleDroppingItem()
    {
        ui.inventory.main.GetDropPosition(this);
    }

    private void HandleClearAbilityBind(string ability_name_)
    {
        controllers.ability_assigner.ClearAbility(this, ability_name_);
    }

    private void HandleAbilityReassigned(string cross_, string level_, string bind_, string ability_name_, Texture2D icon_)
    {
        controllers.ability_assigner.ChangeAbilityAssignment(this, cross_, level_, bind_, ability_name_);
    }

	private void HandleCrossChanged(string cross_)
    {
		if(!ui.preventing_movement)
		{
			ui.SwitchCrosses(cross_);
			if(cross_ == "Left")
			{
				l_cross_primary_selected = !l_cross_primary_selected;
			}
			else if(cross_ == "Right")
			{
				r_cross_primary_selected = !r_cross_primary_selected;
			}
		}
		
    }

	private void HandleInventoryToggle()
    {
		camera_rig.Zoom();
    }

	 internal void OnAbilityPressed(Ability ability_)
    {
		controllers.ability_controller.QueueAbility(this, ability_);
		controllers.ability_controller.CheckCanUseAbility(this, ability_);
    }

	internal void OnAbilityQueue(Ability ability_)
    {
		
        controllers.ability_controller.QueueAbility(this, ability_);
    }

    internal void OnAbilityCheck(Ability ability_)
    {
		
        controllers.ability_controller.CheckCanUseAbility(this, ability_);
    }

	internal void OnAbilityReleased(Ability ability_)
    {
        
    }

	internal void OnAbilityFinished(Ability ability_)
    {
        controllers.ability_controller.RemoveFromAbilityList(this, ability_);
		// systems.targeting_system.rotating_to_soft_target = false;
		// controllers.movement_controller.movement_input_prevented = false;
    }

	public void CameraFollowsPlayer()
	{
		Vector3  camera_position = GlobalPosition;
		camera_rig.GlobalPosition = camera_position;	
	}

	public override void _ExitTree()
	{
		controllers.input_controller.Unsubscribe(this);
	}

    
}
