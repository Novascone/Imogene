using Godot;
using System;
// Equipment controller
// Handles the equipping of items, including armor and consumables
// Armor equipping will be reworked once the update PC models are complete
public partial class EquipmentController : Controller
{
	[Export] public PackedScene ArmPrefab { get; set; }
	[Export] public Skeleton3D SkeletonNode { get; set; }

	public string resource_path;
	public string secondary_resource_path;

	private MeshInstance3D currentHelm;
	private MeshInstance3D currentChest;
	private MeshInstance3D currentShoulders;
	private MeshInstance3D currentGloves;
	private MeshInstance3D currentBelt;
	private MeshInstance3D currentLegs;
	private MeshInstance3D currentBoots;
	// private Player player;
	private CustomSignals _customSignals; // Custom signal instance

	
	public bool d_pad_right_pressed;
	public bool d_pad_right_released;
	public bool d_pad_left_pressed;
	public bool d_pad_left_released;
	public bool d_pad_up_pressed;
	public bool d_pad_up_released;
	public bool d_pad_down_pressed;
	public bool d_pad_down_released;
	public int d_pad_frames_held;
	public int d_pad_frames_held_threshold = 10;
	public bool d_pad_held;


	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.EquipableInfo += HandleEquipableInfo;
		_customSignals.EquipConsumable += HandleEquipConsumable;
	}

	public override void _PhysicsProcess(double delta)
	{
		// GD.Print(d_pad_frames_held);
		// if(d_pad_frames_held > d_pad_frames_held_threshold)
		// {
		// 	d_pad_held = true;
		// }
		// else
		// {
		// 	d_pad_held = false;
		// }
		// if(player.can_use_abilities)
		// {
		// 	if(d_pad_left_pressed || d_pad_right_pressed)
		// 	{
		// 		d_pad_frames_held += 1;
		// 	}
			
		// 	if(d_pad_frames_held < d_pad_frames_held_threshold && d_pad_left_released) // If the left D-Pad has been released, and the frames amount of frames it was less than the threshold change crosses
		// 	{
		// 		// player.l_cross_primary_selected = !player.l_cross_primary_selected;
		// 		// player.ui.hud.LCrossPrimaryOrSecondary(player.l_cross_primary_selected);
		// 		d_pad_left_released = false;
		// 		d_pad_frames_held = 0;
		// 		d_pad_held = false;
		// 		// _customSignals.EmitSignal(nameof(CustomSignals.LCrossPrimaryOrSecondary), l_cross_primary_selected);
		// 	}
		// 	else if(d_pad_frames_held >= d_pad_frames_held_threshold && d_pad_left_released) // If the frames held was greater than or equal to the threshold switch off-hand
		// 	{
		// 		GD.Print("Switch off-hand");
		// 		d_pad_left_released = false;
		// 		d_pad_frames_held = 0;
		// 	}
				
		// 	if(d_pad_frames_held < d_pad_frames_held_threshold && d_pad_right_released) // If the right D-Pad has been released, and the frames amount of frames it was less than the threshold change crosses
		// 	{
				
		// 		// player.r_cross_primary_selected = !player.r_cross_primary_selected;
		// 		// player.ui.hud.RCrossPrimaryOrSecondary(player.r_cross_primary_selected);
		// 		d_pad_right_released = false;
		// 		d_pad_frames_held = 0;
		// 		d_pad_held = false;
		// 		// _customSignals.EmitSignal(nameof(CustomSignals.	RCrossPrimaryOrSecondary), r_cross_primary_selected);
		// 	}
		// 	else if(d_pad_frames_held >= d_pad_frames_held_threshold && d_pad_right_released) // If the frames held was greater than or equal to the threshold switch main-hand
		// 	{
		// 		GD.Print("Switch main-hand");
		// 		d_pad_right_released = false;
		// 		d_pad_frames_held = 0;
		// 	}
				
		// 	if(d_pad_up_pressed)
		// 	{
		// 		// switch consumable
		// 		if(player.consumable < 4)
		// 		{
		// 			player.consumable += 1;
		// 		}
		// 		else if(player.consumable == 4)
		// 		{
		// 			player.consumable = 1;
		// 		}
		// 		d_pad_up_pressed = false;

		// 		_customSignals.EmitSignal(nameof(CustomSignals.WhichConsumable), player.consumable);
		// 		// player.ui.hud.WhichConsumable(player.consumable);
		// 	}
		// 	if(d_pad_down_pressed)
		// 	{
		// 		// use consumable
		// 		player.consumables[player.consumable]?.UseItem();
		// 		d_pad_down_pressed = false;
		// 	}

			
		// }
	}

    public override void _Input(InputEvent @event)
    {
        // if (Input.IsActionJustPressed("one"))
		// {
		// 	GD.Print("EquipHelmet Pressed");
		// 	if (!IsInstanceValid(currentHelm))
		// 	{
		// 		currentHelm = ArmPrefab.Instantiate() as MeshInstance3D;
		// 		SkeletonNode.AddChild(currentHelm);
		// 		currentHelm.Skeleton = SkeletonNode.GetPath();
		// 	}
		// 	else
		// 	{
		// 		currentHelm.QueueFree();
		// 	}
		// }
    }

	public override void _UnhandledInput(InputEvent @event) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        
        
		if(@event.IsActionPressed("D-PadLeft"))
		{
			d_pad_left_pressed = true;
			d_pad_left_released = false;
		}
		if(@event.IsActionReleased("D-PadLeft"))
		{
			d_pad_left_pressed = false;
			d_pad_left_released = true;
			
		}
		if(@event.IsActionPressed("D-PadRight"))
		{
			d_pad_right_pressed = true;
			d_pad_right_released = false;
			
		}
		if(@event.IsActionReleased("D-PadRight"))
		{
			d_pad_right_pressed = false;
			d_pad_right_released = true;
			
		}
		if(@event.IsActionPressed("D-PadUp"))
		{
			d_pad_up_pressed = true;
			d_pad_up_released = false;
		}
		if(@event.IsActionReleased("D-PadUp"))
		{
			d_pad_up_pressed = false;
			d_pad_up_released = true;
		}
		if(@event.IsActionPressed("D-PadDown"))
		{
			d_pad_down_pressed = true;
			d_pad_down_released = false;
		}
		if(@event.IsActionReleased("D-PadDown"))
		{
			d_pad_down_pressed = false;
			d_pad_down_released = true;
		}
		// if(@event.IsActionPressed("one"))
		// {
		// 	player.damage_system.TakeDamage("Physical", 10, false);
		// 	GD.Print("Health test");
		// 	GD.Print("Health : " + player.health);
		// }
		// if(@event.IsActionPressed("two"))
		// {
		// 	player.resource_system.Resource(10);
		// }
        // if(@event.IsActionPressed("three"))
		// {
		// 	player.resource_system.Posture(5);
		// }
		// if(@event.IsActionPressed("four"))
		// {
		// 	player.xp_system.GainXP(11);
		// }
		
	}

	public void GetEquipableInfo(ArmsResource arm) // Gets info from equipable items
    {
		// GD.Print("Item name: "  + arm.name);
        
		// var resource_2 = GD.Load<PackedScene>(resource_path_2);
		
		// GD.Print("received arm equip signal ");
		
		// if(arm.slot == "head")
		// {
		// 	resource_path = arm.resource_path;
		// 	PackedScene resource = GD.Load<PackedScene>(resource_path);
		// 	GD.Print("Helmet equipped");
			
		// 	player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
		// 	GD.Print(player.main_node);
		// 	player.head_slot.AddChild(player.main_node);
		// 	// if(equpipable.equipable_type is "Arm")
		// 	// {
		// 		GD.Print("Adding stats");
		// 		// Arm item_to_add = equpipable.arm_item;
		// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
		// 		AddEquipableStats(arm);
		// 		// player.stat_controller.UpdateStats();
		// 		// player.PrintStats();
		// 	// }
		// }
		// if(arm.slot == "shoulders")
		// {
		// 	resource_path = arm.resource_path;
		// 	secondary_resource_path = arm.second_resource_path;
		// 	PackedScene resource = GD.Load<PackedScene>(resource_path);
		// 	PackedScene secondary_resource = GD.Load<PackedScene>(secondary_resource_path);
			
		// 	GD.Print(resource);
		// 	player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
		// 	player.right_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
		// 	player.left_node = (Node3D)secondary_resource.GetState().GetNodeInstance(0).Instantiate();
		// 	player.shoulder_right_slot.AddChild(player.right_node);
		// 	player.shoulder_left_slot.AddChild(player.left_node);
		// 	// shoulder_right_slot.Hide();
		// 	// GD.Print("Shoulder equipped");
		// 	// GD.Print("Adding stats");
		// 		AddEquipableStats(arm);
		// 		// player.stat_controller.UpdateStats();
		// 		// player.PrintStats();
		// 	// }
		// }
		// if(arm.slot == "chest")
		// {
		// 	// GD.Print("chest equipped");
		// 	resource_path = arm.resource_path;
		// 	PackedScene resource = GD.Load<PackedScene>(resource_path);
		// 	player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
		// 	GD.Print(player.main_node);
		// 	player.chest_slot.AddChild(player.main_node);
		// 	// if(equpipable.equipable_type is "Arm")
		// 	// {
		// 		// GD.Print("Adding stats");
		// 		// Arm item_to_add = equpipable.arm_item;
		// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
		// 		AddEquipableStats(arm);
		// 		// player.stat_controller.UpdateStats();
		// 		// player.PrintStats();
		// 	// }
		// }
		// if(arm.slot == "gloves")
		// {
		// 	GD.Print("Gloves equipped");
			
		// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
		// 	GD.Print(main_node);
		// 	glove_left_slot.AddChild(main_node.GetChild(0));
		// 	glove_right_slot.AddChild(main_node.GetChild(1));
		// 	// if(equpipable.equipable_type is "Arm")
		// 	// {
		// 		GD.Print("Adding stats");
		// 		// Arm item_to_add = equpipable.arm_item;
		// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
		// 		AddEquipableStats(arm);
		// 		UpdateStats();
		// 		PrintStats();
		// 	// }
		// }
		// if(arm.slot == "belt")
		// {
		// 	GD.Print("Belt equipped");
			
		// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
		// 	GD.Print(main_node);
		// 	belt_slot.AddChild(main_node);
		// 	// if(equpipable.equipable_type is "Arm")
		// 	// {
		// 		GD.Print("Adding stats");
		// 		// Arm item_to_add = equpipable.arm_item;
		// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
		// 		AddEquipableStats(arm);
		// 		UpdateStats();
		// 		PrintStats();
		// 	// }
		// }
		// if(arm.slot == "legs")
		// {
		// 	GD.Print("Legs equipped");
			
		// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
		// 	GD.Print(main_node);
		// 	leg_left_slot.AddChild(main_node.GetChild(0));
		// 	leg_right_slot.AddChild(main_node.GetChild(1));
		// 	// if(equpipable.equipable_type is "Arm")
		// 	// {
		// 		GD.Print("Adding stats");
		// 		// Arm item_to_add = equpipable.arm_item;
		// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
		// 		AddEquipableStats(arm);
		// 		UpdateStats();
		// 		PrintStats();
		// 	// }
		// }
		// if(arm.slot == "main hand")
		// {
		// 	GD.Print("Main Hand equipped");
			
		// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
		// 	GD.Print(main_node);
		// 	main_hand_slot.AddChild(main_node.GetChild(0));
		// 	// if(equpipable.equipable_type is "Arm")
		// 	// {
		// 		GD.Print("Adding stats");
		// 		// Arm item_to_add = equpipable.arm_item;
		// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
		// 		AddEquipableStats(arm);
		// 		UpdateStats();
		// 		PrintStats();
		// 	// }
		// }
		// if(arm.slot == "off hand")
		// {
		// 	GD.Print("Off-Hand equipped");
			
		// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
		// 	GD.Print(main_node);
		// 	off_hand_slot.AddChild(main_node.GetChild(0));
		// 	// if(equpipable.equipable_type is "Arm")
		// 	// {
		// 		GD.Print("Adding stats");
		// 		// Arm item_to_add = equpipable.arm_item;
		// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
		// 		AddEquipableStats(arm);
		// 		UpdateStats();
		// 		PrintStats();
		// 	// }
		// }
		// if(arm.slot == "feet")
		// {
		// 	GD.Print("Feet equipped");
			
		// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
		// 	GD.Print(main_node);
		// 	foot_left_slot.AddChild(main_node.GetChild(0));
		// 	foot_right_slot.AddChild(main_node.GetChild(1));
		// 	// if(equpipable.equipable_type is "Arm")
		// 	// {
		// 		GD.Print("Adding stats");
		// 		// Arm item_to_add = equpipable.arm_item;
		// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
		// 		AddEquipableStats(arm);
		// 		UpdateStats();
		// 		PrintStats();
		// 	// }
		// }
		
    }

	// private void HandleEquipableInfo(ArmsResource arm) // Gets info from equipable items
    // {
	// 	// GD.Print("Item name: "  + arm.name);
        
	// 	// var resource_2 = GD.Load<PackedScene>(resource_path_2);
		
	// 	GD.Print("received arm equip signal ");
		
	// 	if(arm.slot == "head")
	// 	{
	// 		resource_path = arm.resource_path;
	// 		PackedScene resource = GD.Load<PackedScene>(resource_path);
	// 		GD.Print("Helmet equipped");
			
	// 		player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
	// 		GD.Print(player.main_node);
	// 		player.head_slot.AddChild(player.main_node);
	// 		// if(equpipable.equipable_type is "Arm")
	// 		// {
	// 			GD.Print("Adding stats");
	// 			// Arm item_to_add = equpipable.arm_item;
	// 			// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
	// 			AddEquipableStats(arm);
	// 			player.statController.UpdateStats();
	// 			player.PrintStats();
	// 		// }
	// 	}
	// 	if(arm.slot == "shoulders")
	// 	{
	// 		resource_path = arm.resource_path;
	// 		secondary_resource_path = arm.second_resource_path;
	// 		PackedScene resource = GD.Load<PackedScene>(resource_path);
	// 		PackedScene secondary_resource = GD.Load<PackedScene>(secondary_resource_path);
			
	// 		GD.Print(resource);
	// 		player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
	// 		player.right_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
	// 		player.left_node = (Node3D)secondary_resource.GetState().GetNodeInstance(0).Instantiate();
	// 		player.shoulder_right_slot.AddChild(player.right_node);
	// 		player.shoulder_left_slot.AddChild(player.left_node);
	// 		// shoulder_right_slot.Hide();
	// 		GD.Print("Shoulder equipped");
	// 		GD.Print("Adding stats");
	// 			AddEquipableStats(arm);
	// 			player.statController.UpdateStats();
	// 			player.PrintStats();
	// 		// }
	// 	}
	// 	if(arm.slot == "chest")
	// 	{
	// 		GD.Print("chest equipped");
	// 		resource_path = arm.resource_path;
	// 		PackedScene resource = GD.Load<PackedScene>(resource_path);
	// 		player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
	// 		GD.Print(player.main_node);
	// 		player.chest_slot.AddChild(player.main_node);
	// 		// if(equpipable.equipable_type is "Arm")
	// 		// {
	// 			GD.Print("Adding stats");
	// 			// Arm item_to_add = equpipable.arm_item;
	// 			// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
	// 			AddEquipableStats(arm);
	// 			player.statController.UpdateStats();
	// 			player.PrintStats();
	// 		// }
	// 	}
	// 	// if(arm.slot == "gloves")
	// 	// {
	// 	// 	GD.Print("Gloves equipped");
			
	// 	// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
	// 	// 	GD.Print(main_node);
	// 	// 	glove_left_slot.AddChild(main_node.GetChild(0));
	// 	// 	glove_right_slot.AddChild(main_node.GetChild(1));
	// 	// 	// if(equpipable.equipable_type is "Arm")
	// 	// 	// {
	// 	// 		GD.Print("Adding stats");
	// 	// 		// Arm item_to_add = equpipable.arm_item;
	// 	// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
	// 	// 		AddEquipableStats(arm);
	// 	// 		UpdateStats();
	// 	// 		PrintStats();
	// 	// 	// }
	// 	// }
	// 	// if(arm.slot == "belt")
	// 	// {
	// 	// 	GD.Print("Belt equipped");
			
	// 	// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
	// 	// 	GD.Print(main_node);
	// 	// 	belt_slot.AddChild(main_node);
	// 	// 	// if(equpipable.equipable_type is "Arm")
	// 	// 	// {
	// 	// 		GD.Print("Adding stats");
	// 	// 		// Arm item_to_add = equpipable.arm_item;
	// 	// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
	// 	// 		AddEquipableStats(arm);
	// 	// 		UpdateStats();
	// 	// 		PrintStats();
	// 	// 	// }
	// 	// }
	// 	// if(arm.slot == "legs")
	// 	// {
	// 	// 	GD.Print("Legs equipped");
			
	// 	// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
	// 	// 	GD.Print(main_node);
	// 	// 	leg_left_slot.AddChild(main_node.GetChild(0));
	// 	// 	leg_right_slot.AddChild(main_node.GetChild(1));
	// 	// 	// if(equpipable.equipable_type is "Arm")
	// 	// 	// {
	// 	// 		GD.Print("Adding stats");
	// 	// 		// Arm item_to_add = equpipable.arm_item;
	// 	// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
	// 	// 		AddEquipableStats(arm);
	// 	// 		UpdateStats();
	// 	// 		PrintStats();
	// 	// 	// }
	// 	// }
	// 	// if(arm.slot == "main hand")
	// 	// {
	// 	// 	GD.Print("Main Hand equipped");
			
	// 	// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
	// 	// 	GD.Print(main_node);
	// 	// 	main_hand_slot.AddChild(main_node.GetChild(0));
	// 	// 	// if(equpipable.equipable_type is "Arm")
	// 	// 	// {
	// 	// 		GD.Print("Adding stats");
	// 	// 		// Arm item_to_add = equpipable.arm_item;
	// 	// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
	// 	// 		AddEquipableStats(arm);
	// 	// 		UpdateStats();
	// 	// 		PrintStats();
	// 	// 	// }
	// 	// }
	// 	// if(arm.slot == "off hand")
	// 	// {
	// 	// 	GD.Print("Off-Hand equipped");
			
	// 	// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
	// 	// 	GD.Print(main_node);
	// 	// 	off_hand_slot.AddChild(main_node.GetChild(0));
	// 	// 	// if(equpipable.equipable_type is "Arm")
	// 	// 	// {
	// 	// 		GD.Print("Adding stats");
	// 	// 		// Arm item_to_add = equpipable.arm_item;
	// 	// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
	// 	// 		AddEquipableStats(arm);
	// 	// 		UpdateStats();
	// 	// 		PrintStats();
	// 	// 	// }
	// 	// }
	// 	// if(arm.slot == "feet")
	// 	// {
	// 	// 	GD.Print("Feet equipped");
			
	// 	// 	main_node = (Node3D)scene_to_load.GetState().GetNodeInstance(0).Instantiate();
	// 	// 	GD.Print(main_node);
	// 	// 	foot_left_slot.AddChild(main_node.GetChild(0));
	// 	// 	foot_right_slot.AddChild(main_node.GetChild(1));
	// 	// 	// if(equpipable.equipable_type is "Arm")
	// 	// 	// {
	// 	// 		GD.Print("Adding stats");
	// 	// 		// Arm item_to_add = equpipable.arm_item;
	// 	// 		// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
	// 	// 		AddEquipableStats(arm);
	// 	// 		UpdateStats();
	// 	// 		PrintStats();
	// 	// 	// }
	// 	// }
		
    // }
	

	public void AddEquipableStats(ArmsResource item)
	{

		// GD.Print("Adding equipable stats from the gear controller");
		player.strength += item.strength;
		player.dexterity += item.dexterity;
		player.intellect += item.intellect;
		player.vitality += item.vitality;
		player.stamina += item.stamina;
		player.wisdom += item.wisdom;
		player.charisma += item.charisma;
		player.critical_hit_chance += item.critical_hit_chance;
		player.critical_hit_damage += item.critical_hit_damage;
		player.armor += item.armor;
		player.poise += item.poise;	
		player.block_amount += item.block;
		player.retaliation += item.retaliation;
		player.physical_resistance += item.physical_resistance;
		player.thrust_resistance += item.thrust_resistance;
		player.slash_resistance += item.slash_resistance;
		player.blunt_resistance += item.blunt_resistance;
		player.bleed_resistance += item.bleed_resistance;
		player.poison_resistance += item.poison_resistance;
		player.curse_resistance += item.curse_resistance;
		player.spell_resistance += item.spell_resistance;
		player.fire_resistance += item.fire_resistance;
		player.cold_resistance += item.cold_resistance;
		player.lightning_resistance += item.lightning_resistance;
		player.holy_resistance += item.holy_resistance;
		player.health_bonus += item.health_bonus;
		player.health_regen += item.health_regen;
		player.health_on_retaliate += item.health_retaliate;
		player.resource_regen += item.resource_regen;
		player.resource_cost_reduction += item.resource_cost_reduction;
		// player.ui.inventory_info.stats_updated = false;
	}

	private void HandleEquipConsumable(ConsumableResource item, int consumable_slot)
    {
        player.consumables[consumable_slot] = item;
		// GD.Print(player.consumables[consumable_slot].name);
    }
	public void GetEquipConsumable(ConsumableResource item, int consumable_slot)
    {
        player.consumables[consumable_slot] = item;
		// GD.Print(player.consumables[consumable_slot].name);
    }

    
}