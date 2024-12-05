using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;


public partial class Player : Entity
{
	[Export] public CameraRig PlayerCameraRig { get; set; }
	[Export] public Marker3D CastPoint { get; set; }
	[Export] public Node3D SurroundingHitbox { get; set; }
	[Export] public UI PlayerUI { get; set; }
	[Export] public Areas PlayerAreas { get; set; }
	[Export] public Controllers PlayerControllers { get; set; }
	[Export] public Systems PlayerSystems { get; set; }
	[Export] public Node Abilities  { get; set; }
	public Ability AbilityInUse  { get; set; } = null; // The ability that the player is currently using
	public LinkedList<Ability> AbilitiesInUseList = new();
	public bool LCrossPrimarySelected { get; set; } = true; // Bool that tracks which left cross the player is using 
	public bool RCossPrimarySelected { get; set; } = true;  // Bool that tracks which right cross the player is using 
	
	public Godot.Collections.Array<Rid> ExcludedRIDs { get; set; } = new Godot.Collections.Array<Rid>();

	
    public override void _Input(InputEvent @event)
    {
		
		
        if(@event.IsActionPressed("one"))
		{
			PlayerControllers.movement_controller.movement_input_prevented = !PlayerControllers.movement_controller.movement_input_prevented;
			PlayerControllers.input_controller.directional_input_prevented = !PlayerControllers.input_controller.directional_input_prevented;
		}
		if(@event.IsActionPressed("two"))
		{
			Chill chill = new();
			EntityControllers.status_effect_controller.AddStatusEffect(this, chill);
		}
		if(@event.IsActionPressed("three"))
		{
			Daze daze = new();
			EntityControllers.status_effect_controller.AddStatusEffect(this, daze);
		}
		if(@event.IsActionPressed("four"))
		{
			EntityControllers.status_effect_controller.RemoveMovementDebuffs(this);
		}
		if(@event.IsActionPressed("five"))
		{
			Tether tether = new();
			EntityControllers.status_effect_controller.AddStatusEffect(this, tether);
		}
		
    }



    public override void _Ready()
	{
		
		base._Ready();
		EntityControllers.status_effect_controller.fear_duration = 3;
		

		Resource.MaxValue = Resource.MaxValue / 2;

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

		PlayerCameraRig.TopLevel = true;

		// Entity system signals
		EntitySystems.resource_system.ResourceChange += PlayerUI.hud.main.HandleResourceChange;

		// Entity controller signals
		EntityControllers.stats_controller.UpdateStats += PlayerUI.inventory.depth_sheet.HandleUpdateStats;
		EntityControllers.stats_controller.UpdateStats += PlayerUI.inventory.main.character_outline.HandleUpdateStats;


        // System signals
        VisionSystem.Subscribe(this);
		PlayerSystems.interact_system.Subscribe(this);
		PlayerSystems.interact_system.ItemPickedUp += PlayerUI.inventory.main.OnItemPickedUp;
		PlayerSystems.interact_system.InputPickup += HandleInputPickUp;
	
		// Controller signals
		PlayerControllers.ability_controller.Subscribe(this);
		PlayerControllers.ability_controller.RotatePlayer += PlayerSystems.targeting_system.HandleRotatePlayer;
		PlayerControllers.input_controller.CrossChanged += HandleCrossChanged;

		// UI signals
		PlayerUI.hud.Subscribe(this);
		PlayerUI.inventory.main.DroppingItem += HandleDroppingItem;
		
		PlayerUI.InventoryToggle += HandleInventoryToggle;
		PlayerUI.abilities.categories.ClearAbilityBind += HandleClearAbilityBind;
		PlayerUI.abilities.categories.AbilityReassigned += HandleAbilityReassigned;
		PlayerControllers.input_controller.Subscribe(this);
		PlayerControllers.movement_controller.Subscribe(this);

		Level.BaseValue = 4;
		Strength.BaseValue = 10;
		Dexterity.BaseValue = 5;
		Vitality.BaseValue = 20;
		Intellect.BaseValue = 11;

		EntityControllers.stats_controller.SetStats(this);
		EntityControllers.stats_controller.Update(this);
		
		ExcludedRIDs.Add(PlayerAreas.near.GetRid());
		ExcludedRIDs.Add(PlayerAreas.far.GetRid());
		ExcludedRIDs.Add(PlayerAreas.interact.GetRid());
		ExcludedRIDs.Add(Hurtbox.GetRid());
		ExcludedRIDs.Add(MainHandHitbox.GetRid());
		ExcludedRIDs.Add(GetRid());

		
	}

    public override void _PhysicsProcess(double delta)
    {
	
		CameraFollowsPlayer();
		PlayerSystems.targeting_system.ray_cast.FollowPlayer(this);
		PlayerControllers.input_controller.SetInput(this);
		PlayerControllers.movement_controller.MovePlayer(this, PlayerControllers.input_controller.input_strength, delta);
		PlayerSystems.targeting_system.Target(this);
        AbilityController.AbilityFrameCheck(this);
		MoveAndSlide();
		
    }

    private void HandleInputPickUp(InteractableItem item)
    {
        PlayerSystems.interact_system.PickupItem(item, this);
    }

    private void HandleDroppingItem()
    {
        PlayerUI.inventory.main.GetDropPosition(this);
    }

    private void HandleClearAbilityBind(string abilityName)
    {
        AbilityAssigner.ClearAbility(this, abilityName);
    }

    private void HandleAbilityReassigned(Ability.Cross cross, Ability.Tier tier, string bind, string abilityName, Texture2D icon)
    {
        AbilityAssigner.ChangeAbilityAssignment(this, cross, tier, bind, abilityName);
    }

	private void HandleCrossChanged(string cross)
    {
		if(!PlayerUI.preventing_movement)
		{
			PlayerUI.SwitchCrosses(cross);
			if(cross == "Left")
			{
				LCrossPrimarySelected = !LCrossPrimarySelected;
			}
			else if(cross == "Right")
			{
				RCossPrimarySelected = !RCossPrimarySelected;
			}
		}
		
    }

	private void HandleInventoryToggle()
    {
		PlayerCameraRig.Zoom();
    }

	 internal void OnAbilityPressed(Ability ability)
    {
		PlayerControllers.ability_controller.QueueAbility(this, ability);
		PlayerControllers.ability_controller.CheckCanUseAbility(this, ability);
    }

	internal void OnAbilityQueue(Ability ability)
    {
		
        PlayerControllers.ability_controller.QueueAbility(this, ability);
    }

    internal void OnAbilityCheck(Ability ability)
    {
		
        PlayerControllers.ability_controller.CheckCanUseAbility(this, ability);
    }

	internal void OnAbilityReleased(Ability ability)
    {
        
    }

	internal void OnAbilityFinished(Ability ability)
    {
        AbilityController.RemoveFromAbilityList(this, ability);
	
    }

	public void CameraFollowsPlayer()
	{
		Vector3  camera_position = GlobalPosition;
		PlayerCameraRig.GlobalPosition = camera_position;	
	}

	public override void _ExitTree()
	{
		PlayerControllers.input_controller.Unsubscribe(this);
		PlayerControllers.ability_controller.Unsubscribe(this);
		PlayerControllers.movement_controller.Unsubscribe(this);

		PlayerSystems.interact_system.Unsubscribe(this);
        VisionSystem.unsubscribe(this);
	}

    
}
