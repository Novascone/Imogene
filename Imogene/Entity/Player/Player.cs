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
			PlayerControllers.MovementController.MovementInputPrevented = !PlayerControllers.MovementController.MovementInputPrevented;
			PlayerControllers.InputController.DirectionalInputPrevented = !PlayerControllers.InputController.DirectionalInputPrevented;
		}
		if(@event.IsActionPressed("two"))
		{
			Chill chill = new();
			EntityControllers.EntityStatusEffectsController.AddStatusEffect(this, chill);
		}
		if(@event.IsActionPressed("three"))
		{
			Daze daze = new();
			EntityControllers.EntityStatusEffectsController.AddStatusEffect(this, daze);
		}
		if(@event.IsActionPressed("four"))
		{
			EntityControllers.EntityStatusEffectsController.RemoveMovementDebuffs(this);
		}
		if(@event.IsActionPressed("five"))
		{
			Tether tether = new();
			EntityControllers.EntityStatusEffectsController.AddStatusEffect(this, tether);
		}
		
    }



    public override void _Ready()
	{
		
		base._Ready();
		EntityControllers.EntityStatusEffectsController.EntityFearDuration = 3;
		

		Resource.MaxValue = Resource.MaxValue / 2;

		Ability jump = (Ability)AbilityAssigner.LoadAbility(this, "Jump", "General", "Active");
		Ability slash = (Ability)AbilityAssigner.LoadAbility(this, "Slash", "General", "Active");
		Ability effect_test = (Ability)AbilityAssigner.LoadAbility(this, "EffectTest", "General", "Active");
		Ability kick = (Ability)AbilityAssigner.LoadAbility(this, "Kick", "General", "Active");
		Ability projectile = (Ability)AbilityAssigner.LoadAbility(this, "Projectile", "General", "Active");
		Ability whirlwind = (Ability)AbilityAssigner.LoadAbility(this, "Whirlwind", "Brigian", "Active");
		Ability hitscan = (Ability)AbilityAssigner.LoadAbility(this, "Hitscan", "General", "Active");
		Ability dash = (Ability)AbilityAssigner.LoadAbility(this, "Dash", "General", "Active");

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
		EntityControllers.EntityStatsController.UpdateStats += PlayerUI.inventory.depth_sheet.HandleUpdateStats;
		EntityControllers.EntityStatsController.UpdateStats += PlayerUI.inventory.main.character_outline.HandleUpdateStats;


        // System signals
        VisionSystem.Subscribe(this);
		PlayerSystems.InteractSystem.Subscribe(this);
		PlayerSystems.InteractSystem.ItemPickedUp += PlayerUI.inventory.main.OnItemPickedUp;
		PlayerSystems.InteractSystem.InputPickup += HandleInputPickUp;
	
		// Controller signals
		PlayerControllers.AbilityController.Subscribe(this);
		PlayerControllers.AbilityController.RotatePlayer += PlayerSystems.TargetingSystem.HandleRotatePlayer;
		PlayerControllers.InputController.CrossChanged += HandleCrossChanged;

		// UI signals
		PlayerUI.hud.Subscribe(this);
		PlayerUI.inventory.main.DroppingItem += HandleDroppingItem;
		
		PlayerUI.InventoryToggle += HandleInventoryToggle;
		PlayerUI.abilities.categories.ClearAbilityBind += HandleClearAbilityBind;
		PlayerUI.abilities.categories.AbilityReassigned += HandleAbilityReassigned;
		PlayerControllers.InputController.Subscribe(this);
		PlayerControllers.MovementController.Subscribe(this);

		Level.BaseValue = 4;
		Strength.BaseValue = 10;
		Dexterity.BaseValue = 5;
		Vitality.BaseValue = 20;
		Intellect.BaseValue = 11;

		EntityControllers.EntityStatsController.SetStats(this);
		EntityControllers.EntityStatsController.Update(this);
		
		ExcludedRIDs.Add(PlayerAreas.Near.GetRid());
		ExcludedRIDs.Add(PlayerAreas.Far.GetRid());
		ExcludedRIDs.Add(PlayerAreas.Interact.GetRid());
		ExcludedRIDs.Add(Hurtbox.GetRid());
		ExcludedRIDs.Add(MainHandHitbox.GetRid());
		ExcludedRIDs.Add(GetRid());

		
	}

    public override void _PhysicsProcess(double delta)
    {
	
		CameraFollowsPlayer();
		PlayerSystems.TargetingSystem.RayCast.FollowPlayer(this);
		PlayerControllers.InputController.SetInput(this);
		PlayerControllers.MovementController.MovePlayer(this, PlayerControllers.InputController.LeftInputStrength, delta);
		PlayerSystems.TargetingSystem.Target(this);
        AbilityController.AbilityFrameCheck(this);
		MoveAndSlide();
		
    }

    private void HandleInputPickUp(InteractableItem item)
    {
        PlayerSystems.InteractSystem.PickupItem(item, this);
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
		PlayerControllers.AbilityController.QueueAbility(this, ability);
		PlayerControllers.AbilityController.CheckCanUseAbility(this, ability);
    }

	internal void OnAbilityQueue(Ability ability)
    {
		
        PlayerControllers.AbilityController.QueueAbility(this, ability);
    }

    internal void OnAbilityCheck(Ability ability)
    {
		
        PlayerControllers.AbilityController.CheckCanUseAbility(this, ability);
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
		PlayerControllers.InputController.Unsubscribe(this);
		PlayerControllers.AbilityController.Unsubscribe(this);
		PlayerControllers.MovementController.Unsubscribe(this);

		PlayerSystems.InteractSystem.Unsubscribe(this);
        VisionSystem.unsubscribe(this);
	}

    
}
