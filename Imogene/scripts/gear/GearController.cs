using Godot;
using System;

public partial class GearController : Node
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
	private Player player;
	private CustomSignals _customSignals; // Custom signal instance

	public override void _Ready()
	{
		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.EquipableInfo += HandleEquipableInfo;
	}

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("EquipHelmet"))
		{
			GD.Print("EquipHelmet Pressed");
			if (!IsInstanceValid(currentHelm))
			{
				currentHelm = ArmPrefab.Instantiate() as MeshInstance3D;
				SkeletonNode.AddChild(currentHelm);
				currentHelm.Skeleton = SkeletonNode.GetPath();
			}
			else
			{
				currentHelm.QueueFree();
			}
		}
    }

	 private void HandleEquipableInfo(ArmsResource arm) // Gets info from equipable items
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
				player.AddEquipableStats(arm);
				player.UpdateStats();
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
				player.AddEquipableStats(arm);
				player.UpdateStats();
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
				player.AddEquipableStats(arm);
				player.UpdateStats();
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

	public void GetPlayerInfo(Player s)
	{
		player = s;
	}
}