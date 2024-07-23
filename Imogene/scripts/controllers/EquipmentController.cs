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

	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.EquipableInfo += HandleEquipableInfo;
		_customSignals.EquipConsumable += HandleEquipConsumable;
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

	public void GetEquipableInfo(ArmsResource arm) // Gets info from equipable items
    {
		// GD.Print("Item name: "  + arm.name);
        
		// var resource_2 = GD.Load<PackedScene>(resource_path_2);
		
		GD.Print("received arm equip signal ");
		
		if(arm.slot == "head")
		{
			resource_path = arm.resource_path;
			PackedScene resource = GD.Load<PackedScene>(resource_path);
			GD.Print("Helmet equipped");
			
			player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
			GD.Print(player.main_node);
			player.head_slot.AddChild(player.main_node);
			// if(equpipable.equipable_type is "Arm")
			// {
				GD.Print("Adding stats");
				// Arm item_to_add = equpipable.arm_item;
				// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
				AddEquipableStats(arm);
				player.statController.UpdateStats();
				player.PrintStats();
			// }
		}
		if(arm.slot == "shoulders")
		{
			resource_path = arm.resource_path;
			secondary_resource_path = arm.second_resource_path;
			PackedScene resource = GD.Load<PackedScene>(resource_path);
			PackedScene secondary_resource = GD.Load<PackedScene>(secondary_resource_path);
			
			GD.Print(resource);
			player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
			player.right_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
			player.left_node = (Node3D)secondary_resource.GetState().GetNodeInstance(0).Instantiate();
			player.shoulder_right_slot.AddChild(player.right_node);
			player.shoulder_left_slot.AddChild(player.left_node);
			// shoulder_right_slot.Hide();
			GD.Print("Shoulder equipped");
			GD.Print("Adding stats");
				AddEquipableStats(arm);
				player.statController.UpdateStats();
				player.PrintStats();
			// }
		}
		if(arm.slot == "chest")
		{
			GD.Print("chest equipped");
			resource_path = arm.resource_path;
			PackedScene resource = GD.Load<PackedScene>(resource_path);
			player.main_node = (Node3D)resource.GetState().GetNodeInstance(0).Instantiate();
			GD.Print(player.main_node);
			player.chest_slot.AddChild(player.main_node);
			// if(equpipable.equipable_type is "Arm")
			// {
				GD.Print("Adding stats");
				// Arm item_to_add = equpipable.arm_item;
				// GD.Print("arm_item from player: " + item_to_add.physical_resistance);
				AddEquipableStats(arm);
				player.statController.UpdateStats();
				player.PrintStats();
			// }
		}
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

		GD.Print("Adding equipable stats from the gear controller");
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
		player.ui.inventory_info.stats_updated = false;
	}

	private void HandleEquipConsumable(ConsumableResource item, int consumable_slot)
    {
        player.consumables[consumable_slot] = item;
		GD.Print(player.consumables[consumable_slot].name);
    }
	public void GetEquipConsumable(ConsumableResource item, int consumable_slot)
    {
        player.consumables[consumable_slot] = item;
		GD.Print(player.consumables[consumable_slot].name);
    }

    
}