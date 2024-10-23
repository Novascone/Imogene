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
	[Export] public UI ui { get; set; }
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

		Ability jump = (Ability)AbilityAssigner.LoadAbility(this, "jump", "general", "active");
		Ability slash = (Ability)AbilityAssigner.LoadAbility(this, "slash", "general", "active");
		Ability effect_test = (Ability)AbilityAssigner.LoadAbility(this, "effect_test", "general", "active");
		Ability kick = (Ability)AbilityAssigner.LoadAbility(this, "kick", "general", "active");
		Ability projectile = (Ability)AbilityAssigner.LoadAbility(this, "projectile", "general", "active");
		Ability whirlwind = (Ability)AbilityAssigner.LoadAbility(this, "whirlwind", "brigian", "active");
		Ability hitscan = (Ability)AbilityAssigner.LoadAbility(this, "hitscan", "general", "active");
		Ability dash = (Ability)AbilityAssigner.LoadAbility(this, "dash", "general", "active");

        AbilityAssigner.AssignAbility(this, jump, "A", Ability.Cross.Right, Ability.Tier.Primary);
        AbilityAssigner.AssignAbility(this, slash, "RB", Ability.Cross.Left, Ability.Tier.Primary);
        AbilityAssigner.AssignAbility(this, effect_test, "RT", Ability.Cross.Left, Ability.Tier.Primary);
        AbilityAssigner.AssignAbility(this, kick, "LB", Ability.Cross.Left, Ability.Tier.Primary);
        AbilityAssigner.AssignAbility(this, projectile, "LT", Ability.Cross.Left, Ability.Tier.Primary);
        AbilityAssigner.AssignAbility(this, whirlwind, "X", Ability.Cross.Right, Ability.Tier.Primary);
        AbilityAssigner.AssignAbility(this, hitscan, "Y", Ability.Cross.Right, Ability.Tier.Primary);
        AbilityAssigner.AssignAbility(this, dash, "B", Ability.Cross.Right, Ability.Tier.Primary);

		camera_rig.TopLevel = true;

		// Entity system signals
		entity_systems.resource_system.ResourceChange += ui.hud.main.HandleResourceChange;

		// Entity controller signals
		entity_controllers.stats_controller.UpdateStats += ui.inventory.depth_sheet.HandleUpdateStats;
		entity_controllers.stats_controller.UpdateStats += ui.inventory.main.character_outline.HandleUpdateStats;


        // System signals
        VisionSystem.Subscribe(this);
		systems.interact_system.Subscribe(this);
		systems.interact_system.ItemPickedUp += ui.inventory.main.OnItemPickedUp;
		systems.interact_system.InputPickup += HandleInputPickUp;
	
		// Controller signals
		controllers.ability_controller.Subscribe(this);
		controllers.ability_controller.RotatePlayer += systems.targeting_system.HandleRotatePlayer;
		controllers.input_controller.CrossChanged += HandleCrossChanged;

		// UI signals
		ui.hud.Subscribe(this);
		ui.inventory.main.DroppingItem += HandleDroppingItem;
		
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
        AbilityController.AbilityFrameCheck(this);
		MoveAndSlide();
		
    }

    private void HandleInputPickUp(InteractableItem item_)
    {
        systems.interact_system.PickupItem(item_, this);
    }

    private void HandleDroppingItem()
    {
        ui.inventory.main.GetDropPosition(this);
    }

    private void HandleClearAbilityBind(string ability_name_)
    {
        AbilityAssigner.ClearAbility(this, ability_name_);
    }

    private void HandleAbilityReassigned(Ability.Cross cross_, Ability.Tier tier_, string bind_, string ability_name_, Texture2D icon_)
    {
        AbilityAssigner.ChangeAbilityAssignment(this, cross_, tier_, bind_, ability_name_);
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
        AbilityController.RemoveFromAbilityList(this, ability_);
	
    }

	public void CameraFollowsPlayer()
	{
		Vector3  camera_position = GlobalPosition;
		camera_rig.GlobalPosition = camera_position;	
	}

	public override void _ExitTree()
	{
		controllers.input_controller.Unsubscribe(this);
		controllers.ability_controller.Unsubscribe(this);
		controllers.movement_controller.Unsubscribe(this);

		systems.interact_system.Unsubscribe(this);
        VisionSystem.unsubscribe(this);
	}

    
}
