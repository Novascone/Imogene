using Godot;
using System;

public partial class InventoryInfo : UI
{

	private bool stats_need_updated;

	string level_UI;
	string strength_UI;
	string dexterity_UI;
	string intellect_UI;
	string vitality_UI;
	string stamina_UI;
	string wisdom_UI;
	string charisma_UI;
	

	// Stat details
	string damage_UI;
	string resistance_UI;
	string recovery_UI;

	// Sheet

	// Offense

	string physical_melee_dps_UI;
	string spell_melee_dps_UI;
	string physical_ranged_dps_UI;
	string spell_ranged_dps_UI;

	string physical_melee_power_UI;
	string spell_melee_power_UI;
	string physical_ranged_power_UI;
	string spell_ranged_power_UI;
	string wisdom_scaler_UI;
	string slash_damage_UI;
	string thrust_damage_UI;
	string blunt_damage_UI;
	string bleed_damage_UI;
	string poison_damage_UI;
	string fire_damage_UI;
	string cold_damage_UI;
	string lightning_damage_UI;
	string holy_damage_UI;
	string critical_hit_chance_UI;
	string critical_hit_damage_UI;
	string attack_speed_UI;
	string attack_speed_increase_UI;
	string cool_down_reduction_UI;
	string posture_damage_UI;
	

	// Defense

	string armor_UI;
	string poise_UI;
	string block_amount_UI;
	string retaliation_UI;
	string physical_resistance_UI;
	string thrust_resistance_UI;
	string slash_resistance_UI;
	string blunt_resistance_UI;
	string bleed_resistance_UI;
	string poison_resistance_UI;
	string curse_resistance_UI;
	string spell_resistance_UI;
	string fire_resistance_UI;
	string cold_resistance_UI;
	string lightning_resistance_UI;
	string holy_resistance_UI;

	// Health

	string maximum_health_UI;
	string health_bonus_UI;
	string health_regen_UI;
	string posture_regen_UI;
	string health_on_retaliate_UI;

	// Resources

	string maximum_resource_UI;
	string resource_regen_UI;
	string resource_cost_reduction_UI;

	// Misc

	string movement_speed_UI;

	// Possessions
	string gold_UI;

	private Button head_slot;
	private Button shoulder_slot;
	private Button neck_slot;
	private Button chest_slot;
	private Button gloves_slot;
	private Button bracers_slot;
	private Button belt_slot;
	private Button ring1_slot;
	private Button ring2_slot;
	private Button mainhand_slot;
	private Button offhand_slot;
	private Button pants_slot;
	private Button boots_slot;


	private Button strength_label;
	private Label strength_value;
	private RichTextLabel strength_info;
	private string strength_info_text = " Strength {0} \n * Primary stat for melee damage \n * Increases damage by {1} \n * Increases health by {2} \n * Increases critical hit damage by {3}";

	private Button dexterity_label;
	private Label dexterity_value;
	private RichTextLabel dexterity_info;
	private string dexterity_info_text = " Dexterity {0}  \n * Primary stat for ranged damage\n * Increases damage by {1} \n * Increases critical chance by {2}";

	private Button intellect_label;
	private Label intellect_value;
	private RichTextLabel intellect_info;
	private string intellect_info_text = " Intellect {0} \n * main stat for spell damage \n * Increases spell damage by {1} \n * Increases spell hit chance by {2}";

	private Button vitality_label;
	private Label vitality_value;
	private RichTextLabel vitality_info;
	private string vitality_info_text = " Vitality {0} \n * Primary stat for health \n * Increases health points by {1}";

	private Button stamina_label;
	private Label stamina_value;
	private RichTextLabel stamina_info;
	private string stamina_info_text = " Stamina {0} \n * Primary stat for resource and regeneration \n * Increases health and resource regeneration by {2} \n * Increases health by {3}";

	private Button wisdom_label;
	private Label wisdom_value;
	private RichTextLabel wisdom_info;
	private string wisdom_info_text = " Wisdom {0} \n * Primary stat for interaction \n * Increases hit chance by {1}";

	private Button charisma_label;
	private Label charisma_value;
	private RichTextLabel charisma_info;
	private string charisma_info_text = "  Charisma {0} \n * Primary stat for character interaction \n * Increases special interactions";

	// Character details
	private Button damage_label;
	private Label damage_value;
	private RichTextLabel damage_info;
	private string damage_info_text = "  Damage {0} \n * Total damage per second done by character by the average of power modifiers \n * Physical melee dps {1} \n * spell melee dps {2} \n * Physical ranged dps {3} \n * spell ranged dps {4} \n These 4 catagories are your dps when using a skill of the respective type. \n * Combination of power modifiers melee and ranged, in both their physical and spell forms, \n     damage, attack speed, critical hit chance, and critical hit damage";

	private Button resistance_label;
	private Label resistance_value;
	private RichTextLabel resistance_info;
	private string resistance_info_text = " Resistance {0} \n * Total damage the character can resist \n * Calculated by health, armor, resistances" ;

	private Button recovery_label;
	private Label recovery_value;
	private RichTextLabel recovery_info;
	private string recovery_info_text =  " Recovery {0} \n * The average of health regen, resource regen, and posture regen \n * Calculated by stamina ";

	private Button level_label;
	private Label level_value;
	private RichTextLabel level_info;
	private string level_info_text =  " Level {0} \n * Level of character";

	private Button rep_label;
	
	private Button sheet_label;

	// Offense
	private Button physical_melee_power_label;
	private Label physical_melee_power_value;
	private RichTextLabel physical_melee_power_info;
	private string physical_melee_power_info_text =  " Physical melee power {0} \n * Increases physical melee DPS by 1 every 15 points \n * +2 for every point of strength +1 for every point of dexterity \n * Bonuses obtainable on gear ";

	private Button spell_melee_power_label;
	private Label spell_melee_power_value;
	private RichTextLabel spell_melee_power_info;
	private string spell_melee_power_info_text =  " Spell melee power {0} \n Increases magic melee DPS by 1 every 15 points \n * +3 for every point of intellect +1 for every point of dexterity + 1 for every point of strength \n * Bonuses  obtainable on gear ";

	private Button physical_ranged_power_label;
	private Label physical_ranged_power_value;
	private RichTextLabel physical_ranged_power_info;
	private string physical_ranged_power_info_text =  " Physical ranged power {0} \n * Increases physical ranged DPS by 1 every 15 points \n * +3 for every point of dexterity +1 for every point of strength \n * Bonuses obtainable on gear ";

	private Button spell_ranged_power_label;
	private Label spell_ranged_power_value;
	private RichTextLabel spell_ranged_power_info;
	private string spell_ranged_power_info_text =  " Spell ranged power {0} \n Increases magic ranged DPS by 1 every 15 points \n * +3 for every point of intellect +2 for every point of dexterity \n * Bonuses obtainable on gear ";
	
	private Button wisdom_scaler_label;
	private Label wisdom_scaler_value;
	private RichTextLabel wisdom_scaler_info;
	private string wisdom_scaler_info_text =  " Wisdom Scaler {0} \n Increases by one for every 20 wisdom \n * Scales how powerful attacks that scale with wisdom are ";
	
	
	private Button thrust_damage_label;
	private Label thrust_damage_value;
	private RichTextLabel thrust_damage_info;
	private string thrust_damage_info_text =  " Thrust {0} \n * Percentage of melee damage inflicted as thrust \n * Increased by skills and gear ";

	private Button slash_damage_label;
	private Label slash_damage_value;
	private RichTextLabel slash_damage_info;
	private string slash_damage_info_text =  " Slash {0} \n * Percentage of melee damage inflicted as slash \n * Increased by skills and gear ";

	private Button blunt_damage_label;
	private Label blunt_damage_value;
	private RichTextLabel blunt_damage_info;
	private string blunt_damage_info_text =  " Blunt {0} \n * Percentage of melee damage inflicted as slash \n * Increased by skills and gear ";

	private Button bleed_damage_label;
	private Label bleed_damage_value;
	private RichTextLabel bleed_damage_info;
	private string bleed_damage_info_text =  " Bleed {0} \n * Amount of damage inflicted as bleed \n * Increased by skills and gear ";

	private Button poison_damage_label;
	private Label poison_damage_value;
	private RichTextLabel poison_damage_info;
	private string poison_damage_info_text =  " Poison {0} \n * Amount of damage inflicted as poison \n * Increased by skills and gear ";

	private Button fire_damage_label;
	private Label fire_damage_value;
	private RichTextLabel fire_damage_info;
	private string fire_damage_info_text =  " Fire {0} \n * Percentage of spell damage inflicted as fire \n * Increased by skills and gear ";

	private Button cold_damage_label;
	private Label cold_damage_value;
	private RichTextLabel cold_damage_info;
	private string cold_damage_info_text =  " Cold {0} \n * Percentage of spell damage inflicted as cold \n * Increased by skills and gear ";

	private Button lightning_damage_label;
	private Label lightning_damage_value;
	private RichTextLabel lightning_damage_info;
	private string lightning_damage_info_text =  " Lightning {0} \n * Percentage of spell damage inflicted as lightning \n * Increased by skills and gear ";

	private Button holy_damage_label;
	private Label holy_damage_value;
	private RichTextLabel holy_damage_info;
	private string holy_damage_info_text =  " Holy {0} \n * Percentage of spell damage inflicted as holy \n * Increased by skills and gear  ";

	private Button critical_hit_chance_label;
	private Label critical_hit_chance_value;
	private RichTextLabel critical_hit_chance_info;
	private string critical_hit_chance_text =  " Critical hit chance {0} \n * Percentage chance for a hit to be a critical hit \n * Increased by skills and gear ";

	private Button critical_hit_damage_label;
	private Label critical_hit_damage_value;
	private RichTextLabel critical_hit_damage_info;
	private string critical_hit_damage_text =  " Critical hit damage {0} \n * Multiplier applied to base damage if a hit is critical \n * Increased by skills and gear  ";

	private Button attack_speed_label;
	private Label attack_speed_value;
	private RichTextLabel attack_speed_info;
	private string attack_speed_info_text =  " Attack speed {0} \n * Based off of weapon speed \n * Increased by skills and gear ";

	private Button attack_speed_increase_label;
	private Label attack_speed_increase_value;
	private RichTextLabel attack_speed_increase_info;
	private string attack_speed_increase_text =  " Attack speed increase {0} \n * Percentage of attack speed \n * Increased by skills and gear ";

	private Button cooldown_reduction_label;
	private Label cooldown_reduction_value;
	private RichTextLabel cooldown_reduction_info;
	private string cooldown_reduction_info_text =  " Cooldown reduction {0} \n * Cooldown reduction of all skills ";

	private Button posture_damage_label;
	private Label posture_damage_value;
	private RichTextLabel posture_damage_info;
	private string posture_damage_text =  " Posture damage {0} \n * Amount of enemy posture reduced every hit \n * Increased by skills and gear ";

	// Defense
	private Button armor_label;
	private Label armor_value;
	private RichTextLabel armor_info;
	private string armor_info_text =  " Total Armor {0} \n * Reduces incoming damage by {1} \n * Increased by gear ";

	private Button poise_label;
	private Label poise_value;
	private RichTextLabel poise_info;
	private string poise_info_text =  " Poise {0} \n * Reduces incoming posture damage and increases crowd control resistance ";

	private Button block_amount_label;
	private Label block_amount_value;
	private RichTextLabel block_amount_info;
	private string block_amount_info_text =  " Block amount {0} \n * Amount of damage that can be blocked by weapon/shield ";

	private Button retaliation_label;
	private Label retaliation_value;
	private RichTextLabel retaliation_info;
	private string retaliation_info_text =  " Retaliation {0} \n * Increases the amount of time you have to retaliate after being hit ";

	private Button physical_resistance_label;
	private Label physical_resistance_value;
	private RichTextLabel physical_resistance_info;
	private string physical_resistance_info_text =  " Physical Resistance {0} \n * Reduces incoming physical damage \n * Increased by skills and gear ";

	private Button thrust_resistance_label;
	private Label thrust_resistance_value;
	private RichTextLabel thrust_resistance_info;
	private string thrust_resistance_info_text =  " Thrust Resistance {0} \n * Reduces thrust effectiveness on character \n * Increased by skills and gear ";

	private Button slash_resistance_label;
	private Label slash_resistance_value;
	private RichTextLabel slash_resistance_info;
	private string slash_resistance_info_text =  " Slash Resistance {0} \n * Reduces Slash effectiveness on character \n * Increased by skills and gear ";

	private Button blunt_resistance_label;
	private Label blunt_resistance_value;
	private RichTextLabel blunt_resistance_info;
	private string blunt_resistance_info_text =  " Blunt Resistance {0} \n * Reduces Blunt effectiveness on character \n * Increased by skills and gear ";

	private Button bleed_resistance_label;
	private Label bleed_resistance_value;
	private RichTextLabel bleed_resistance_info;
	private string bleed_resistance_info_text =  " Bleed Resistance {0} \n * Reduces bleeding time \n * Increased by skills and gear ";

	private Button poison_resistance_label;
	private Label poison_resistance_value;
	private RichTextLabel poison_resistance_info;
	private string poison_resistance_info_text =  " Poison Resistance {0} \n * Reduces Poisoned time and initial poison damage \n * Increased by skills and gear ";

	private Button curse_resistance_label;
	private Label curse_resistance_value;
	private RichTextLabel curse_resistance_info;
	private string curse_resistance_info_text =  " Curse Resistance {0} \n * Reduces curse effectiveness \n * Increased by skills and gear ";

	private Button spell_resistance_label;
	private Label spell_resistance_value;
	private RichTextLabel spell_resistance_info;
	private string spell_resistance_info_text =  " Spell Resistance {0} \n * Reduces incoming magic damage \n * Increased by skills and gear ";

	private Button fire_resistance_label;
	private Label fire_resistance_value;
	private RichTextLabel fire_resistance_info;
	private string fire_resistance_info_text =  " Fire Resistance {0} \n * Reduces incoming fire damage \n * Increased by skills and gear ";

	private Button cold_resistance_label;
	private Label cold_resistance_value;
	private RichTextLabel cold_resistance_info;
	private string cold_resistance_info_text =  " Cold Resistance {0} \n * Reduces incoming cold damage \n * Increased by skills and gear ";

	private Button lightning_resistance_label;
	private Label lightning_resistance_value;
	private RichTextLabel lightning_resistance_info;
	private string lighting_resistance_info_text =  " Lightning Resistance {0} \n * Reduces incoming lightning damage \n * Increased by skills and gear ";

	private Button holy_resistance_label;
	private Label holy_resistance_value;
	private RichTextLabel holy_resistance_info;
	private string holy_resistance_info_text =  " Holy Resistance {0} \n * Reduces incoming holy damage \n * Increased by skills and gear ";

	// Health
	private Button maximum_health_label;
	private Label maximum_health_value;
	private RichTextLabel maximum_health_info;
	private string maximum_health_info_text =  " Maximum Health {0} \n * The total amount of health if health is reduced to zero you die ";

	private Button health_bonus_label;
	private Label health_bonus_value;
	private RichTextLabel health_bonus_info;
	private string health_bonus_info_text =  " Health bonus {0} \n * Bonus applied to total health \n * Increased by skills and gear ";

	private Button health_regen_label;
	private Label health_regen_value;
	private RichTextLabel health_regen_info;
	private string health_regen_info_text =  " Health regen {0} \n * Amount of health regenerated per second \n * Increased by skills and gear ";

	private Button posture_regen_label;
	private Label posture_regen_value;
	private RichTextLabel posture_regen_info;
	private string posture_regen_info_text =  " Posture regen {0} \n * Amount of posture regenerated per second \n * Increased by skills and gear ";

	private Button health_retaliation_label;
	private Label health_retaliation_value;
	private RichTextLabel health_retaliation_info;
	private string health_retaliation_info_text =  " Health on Retaliation {0} \n * Amount of health you can regain during retaliation period \n * Increased by skills and gear ";

	// Resource
	private Button maximum_resource_label;
	private Label maximum_resource_value;
	private RichTextLabel maximum_resource_info;
	private string maximum_resource_info_text =  " Maximum resource {0} \n * Total amount of resource \n * Increased by skills and gear ";

	private Button resource_regen_label;
	private Label resource_regen_value;
	private RichTextLabel resource_regen_info;
	private string resource_regen_info_text =  " Resource regen {0} \n * Amount of resource regenerated per second \n * Increased by skills and gear ";

	private Button resource_cost_reduction_label;
	private Label resource_cost_reduction_value;
	private RichTextLabel resource_cost_reduction_info;
	private string resource_cost_reduction_info_text =  " Resource cost reduction {0} \n * Amount resource cost of skills are reduced by \n * Increased by skills and gear ";

	// Misc

	private Button movement_speed_label;
	private Label movement_speed_value;
	private RichTextLabel movement_speed_info;
	private string movement_speed_info_text =  " Movement speed {0} \n * Movement speed of character \n * Increased by skills and gear ";


	private Button mats_label;
	private Button gold_label;

	private Button trash_label;

	private Button abilities_label;
	private Button journal_quests_label;
	private Button achievements_label;
	private Button social_label;
	private Button options_label;



	private CustomSignals _customSignals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		stats_need_updated = true;

		head_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Head");
		shoulder_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Shoulder");
		neck_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Neck");
		chest_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Chest");
		gloves_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Gloves");
		bracers_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Bracers");
		belt_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Belt");
		ring1_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Ring1");
		ring2_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Ring2");
		mainhand_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/MainHand");
		offhand_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/OffHand");
		pants_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Pants");
		boots_slot = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Boots");

		strength_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/Label");
		strength_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/Value");
		strength_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/Label/Info/MarginContainer/PanelContainer/Label");

		dexterity_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/Label");
		dexterity_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/Value");
		dexterity_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/Label/Info/MarginContainer/PanelContainer/Label");

		intellect_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/Label");
		intellect_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/Value");
		intellect_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/Label/Info/MarginContainer/PanelContainer/Label");

		vitality_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/Label");
		vitality_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/Value");
		vitality_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/Label/Info/MarginContainer/PanelContainer/Label");

		stamina_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/Label");
		stamina_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/Value");
		stamina_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/Label/Info/MarginContainer/PanelContainer/Label");

		wisdom_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/Label");
		wisdom_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/Value");
		wisdom_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/Label/Info/MarginContainer/PanelContainer/Label");

		charisma_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/Label");
		charisma_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/Value");
		charisma_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/Label/Info/MarginContainer/PanelContainer/Label");

		// Stats details
		damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/Label");
		damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/Value");
		damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/Label/Info/MarginContainer/PanelContainer/Label");

		resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/Label");
		resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/Value");
		resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/Label/Info/MarginContainer/PanelContainer/Label");

		recovery_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/Label");
		recovery_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/Value");
		recovery_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/Label/Info/MarginContainer/PanelContainer/Label");

		level_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/Label");
		level_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/Value");
		level_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/Label/Info/MarginContainer/PanelContainer/Label");


		// Offense
		physical_melee_power_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalMeleePower/Label");
		physical_melee_power_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalMeleePower/Value");
		physical_melee_power_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalMeleePower/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		spell_melee_power_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellMeleePower/Label");
		spell_melee_power_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellMeleePower/Value");
		spell_melee_power_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellMeleePower/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		physical_ranged_power_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalRangedPower/Label");
		physical_ranged_power_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalRangedPower/Value");
		physical_ranged_power_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalRangedPower/Label/Info/MarginContainer/PanelContainer/RichTextLabel");
	   
		spell_ranged_power_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellRangedPower/Label");
		spell_ranged_power_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellRangedPower/Value");
		spell_ranged_power_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellRangedPower/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		wisdom_scaler_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/WisdomScaler/Label");
		wisdom_scaler_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/WisdomScaler/Value");
		wisdom_scaler_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/WisdomScaler/Label/Info/MarginContainer/PanelContainer/RichTextLabel");
 		
		thrust_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustDamage/Label");
		thrust_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustDamage/Value");
		thrust_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		slash_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashDamage/Label");
		slash_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashDamage/Value");
		slash_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		blunt_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntDamage/Label");
		blunt_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntDamage/Value");
		blunt_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		bleed_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedDamage/Label");
		bleed_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedDamage/Value");
		bleed_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		poison_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonDamage/Label");
		poison_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonDamage/Value");
		poison_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		fire_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireDamage/Label");
		fire_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireDamage/Value");
		fire_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		cold_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdDamage/Label");
		cold_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdDamage/Value");
		cold_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		lightning_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningDamage/Label");
		lightning_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningDamage/Value");
		lightning_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		holy_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyDamage/Label");
		holy_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyDamage/Value");
		holy_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		critical_hit_chance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitChance/Label");
		critical_hit_chance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitChance/Value");
		critical_hit_chance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitChance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		critical_hit_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitDamage/Label");
		critical_hit_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitDamage/Value");
		critical_hit_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		attack_speed_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeed/Label");
		attack_speed_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeed/Value");
		attack_speed_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeed/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		attack_speed_increase_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeedIncrease/Label");
		attack_speed_increase_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeedIncrease/Value");
		attack_speed_increase_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeedIncrease/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		cooldown_reduction_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CooldownReduction/Label");
		cooldown_reduction_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CooldownReduction/Value");
		cooldown_reduction_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CooldownReduction/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		posture_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureDamage/Label");
		posture_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureDamage/Value");
		posture_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureDamage/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		//Defense
		armor_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Armor/Label");
		armor_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Armor/Value");
		armor_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Armor/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		poise_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poise/Label");
		poise_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poise/Value");
		poise_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poise/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		block_amount_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BlockAmount/Label");
		block_amount_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BlockAmount/Value");
		block_amount_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BlockAmount/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		retaliation_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Retaliation/Label");
		retaliation_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Retaliation/Value");
		retaliation_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Retaliation/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		physical_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalResistance/Label");
		physical_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalResistance/Value");
		physical_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		thrust_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustResistance/Label");
		thrust_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustResistance/Value");
		thrust_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		slash_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashResistance/Label");
		slash_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashResistance/Value");
		slash_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		blunt_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntResistance/Label");
		blunt_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntResistance/Value");
		blunt_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		bleed_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedResistance/Label");
		bleed_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedResistance/Value");
		bleed_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		poison_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonResistance/Label");
		poison_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonResistance/Value");
		poison_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");
		
		curse_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CurseResistance/Label");
		curse_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CurseResistance/Value");
		curse_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CurseResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		spell_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellResistance/Label");
		spell_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellResistance/Value");
		spell_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		fire_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireResistance/Label");
		fire_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireResistance/Value");
		fire_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		cold_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdResistance/Label");
		cold_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdResistance/Value");
		cold_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		lightning_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningResistance/Label");
		lightning_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningResistance/Value");
		lightning_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		holy_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyResistance/Label");
		holy_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyResistance/Value");
		holy_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyResistance/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		// Health
		maximum_health_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumHealth/Label");
		maximum_health_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumHealth/Value");
		maximum_health_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumHealth/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		health_bonus_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthBonus/Label");
		health_bonus_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthBonus/Value");
		health_bonus_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthBonus/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		health_regen_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/Label");
		health_regen_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/Value");
		health_regen_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		posture_regen_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureRegeneration/Label");
		posture_regen_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureRegeneration/Value");
		posture_regen_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureRegeneration/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		health_retaliation_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRetaliation/Label");
		health_retaliation_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRetaliation/Value");
		health_retaliation_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRetaliation/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		//Resource 
		maximum_resource_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumResource/Label");
		maximum_resource_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumResource/Value");
		maximum_resource_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumResource/Label/Info/MarginContainer/PanelContainer/RichTextLabel");
		
		resource_regen_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceRegeneration/Label");
		resource_regen_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceRegeneration/Value");
		resource_regen_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceRegeneration/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		resource_cost_reduction_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceCostReduction/Label");
		resource_cost_reduction_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceCostReduction/Value");
		resource_cost_reduction_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceCostReduction/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		//Misc
		movement_speed_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MovementSpeed/Label");
		movement_speed_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MovementSpeed/Value");
		movement_speed_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MovementSpeed/Label/Info/MarginContainer/PanelContainer/RichTextLabel");

		
		

		rep_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/RepLabel");
		sheet_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/SheetLabel");

		// Character possessions
		
		gold_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/MatsGold/GoldLabel");

		mats_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/MatsGold/MatsLabel");

		// Misc
		trash_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BagSlots/TrashLabel");

		abilities_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/AbilitiesLabel");
		journal_quests_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/JournalQuestsLabel");
		achievements_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/AchievementsLabel");
		social_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/SocialLabel");
		options_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/OptionsLabel");
		

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;
		_customSignals.SendStats += HandleSendStats;
	}

    private void HandleSendStats(
								int level, int strength, int dexterity, int intellect, int vitality, int stamina, int wisdom, int charisma, float total_dps,

								float physical_melee_dps, float spell_melee_dps, float physical_ranged_dps, float spell_ranged_dps, float physical_melee_power,

								float spell_melee_power, float physical_ranged_power, float spell_ranged_power, float wisdom_scaler, float physical_melee_power_mod,

								float physical_ranged_power_mod, float spell_ranged_power_mod, float power_mod_avg, int damage_bonus, float combined_damage, float base_aps,

								float aps_modifiers, float aps, float aps_mod, float base_dps, float skill_mod, float crit_mod, float slash_damage, float thrust_damage,

								float blunt_damage, float bleed_damage, float poison_damage, float fire_damage, float cold_damage, float lightning_damage, float holy_damage,

								float critical_hit_chance, float critical_hit_damage, float attack_speed_increase, float cool_down_reduction, float posture_damage, int armor,

								int poise, int block_amount, int retaliation, int physical_resistance, int thrust_resistance, int slash_resistance, int blunt_resistance,

								int bleed_resistance, int poison_resistance, int curse_resistance, int spell_resistance, int fire_resistance, int cold_resistance,

								int lightning_resistance, int holy_resistance, float maximum_health, float health_bonus, float health_regen, float health_regen_bonus, float posture_regen,

								float posture_regen_bonus,  float health_on_retaliate, float resistance, float maximum_resource, float resource_regen, float resource_cost_reduction,
								
								float recovery, float movement_speed, int maximum_gold
								)
								{
									GD.Print("receiving stats");
									GD.Print("Strength received: " + strength);
									level_UI = level.ToString();
									strength_UI = strength.ToString();
									dexterity_UI = dexterity.ToString();
									intellect_UI = intellect.ToString();
									vitality_UI = vitality.ToString();
									stamina_UI = stamina.ToString();
									wisdom_UI = wisdom.ToString();
									charisma_UI = charisma.ToString();
									

									// Stat details
									damage_UI = total_dps.ToString();
									resistance_UI = resistance.ToString();
									recovery_UI = recovery.ToString();
									// Sheet

									// Offense

									physical_melee_dps_UI = physical_melee_dps.ToString();
									spell_melee_dps_UI = spell_melee_dps.ToString();
									physical_ranged_dps_UI = physical_ranged_dps.ToString();
									spell_ranged_dps_UI = spell_ranged_dps.ToString();

									physical_melee_power_UI = physical_melee_power.ToString();
									spell_melee_power_UI = spell_melee_power.ToString();
									physical_ranged_power_UI = physical_ranged_power.ToString();
									spell_ranged_power_UI = spell_ranged_power.ToString();
									wisdom_scaler_UI = wisdom_scaler.ToString();
									slash_damage_UI = slash_damage.ToString();
									thrust_damage_UI = thrust_damage.ToString();
									blunt_damage_UI = blunt_damage.ToString();
									bleed_damage_UI = bleed_damage.ToString();
									poison_damage_UI = poison_damage.ToString();
									fire_damage_UI = fire_damage.ToString();
									cold_damage_UI = cold_damage.ToString();
									lightning_damage_UI = lightning_damage.ToString();
									holy_damage_UI = holy_damage.ToString();
									critical_hit_chance_UI = critical_hit_chance.ToString();
									critical_hit_damage_UI = critical_hit_damage.ToString();
									attack_speed_UI = aps.ToString();
									attack_speed_increase_UI = attack_speed_increase.ToString();
									cool_down_reduction_UI = cool_down_reduction.ToString();
									posture_damage_UI = posture_damage.ToString();
									

									// Defense

									armor_UI = armor.ToString();
									poise_UI = poise.ToString();
									block_amount_UI = block_amount.ToString();
									retaliation_UI = retaliation.ToString();
									physical_resistance_UI = physical_resistance.ToString();
									thrust_resistance_UI = thrust_resistance.ToString();
									slash_resistance_UI = slash_resistance.ToString();
									blunt_resistance_UI = blunt_resistance.ToString();
									bleed_resistance_UI = bleed_resistance.ToString();
									poison_resistance_UI = poison_resistance.ToString();
									curse_resistance_UI = curse_resistance.ToString();
									spell_resistance_UI = spell_resistance.ToString();
									fire_resistance_UI = fire_resistance.ToString();
									cold_resistance_UI = cold_resistance.ToString();
									lightning_resistance_UI = lightning_resistance.ToString();
									holy_resistance_UI = holy_resistance.ToString();

									// Health

									maximum_health_UI = maximum_health.ToString();
									health_bonus_UI = health_bonus.ToString();
									health_regen_UI = health_regen.ToString();
									posture_regen_UI = posture_regen.ToString();
									health_on_retaliate_UI = health_on_retaliate.ToString();

									// Resources

									maximum_resource_UI = maximum_resource.ToString();
									resource_regen_UI = resource_regen.ToString();
									resource_cost_reduction_UI = resource_cost_reduction.ToString();

									// Misc

									movement_speed_UI = movement_speed.ToString();

									// Possessions
									gold_UI = maximum_gold.ToString();
									CharacterInfo();
		
								}

    private void HandlePlayerInfo(Player player)
	{
		this_player = player;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(stats_need_updated)
		{
			if(this_player != null)
			{
				CharacterInfo();
				stats_need_updated = false;
			}
		}	

	}

	private void CharacterInfo()
	{
		// Basic stats
		
		// Base Stats
		strength_value.Text = strength_UI;
		strength_info.Text = string.Format(strength_info_text, strength_UI, 0, strength_UI, strength_UI); // 3 variable(s)

		dexterity_value.Text = dexterity_UI;
		dexterity_info.Text = string.Format(dexterity_info_text, dexterity_UI, 0, dexterity_UI); // 4 variable(s)

		intellect_value.Text = intellect_UI;
		intellect_info.Text = string.Format(intellect_info_text, intellect_UI, 0, 0); // 3 variable(s)

		vitality_value.Text = vitality_UI;
		vitality_info.Text = string.Format(vitality_info_text, vitality_UI, 2 * vitality_UI.ToInt()); // 2 variable(s)

		stamina_value.Text = stamina_UI;
		stamina_info.Text = string.Format(stamina_info_text, stamina_UI, stamina_UI, stamina_UI, stamina_UI); // 4 variable(s)

		wisdom_value.Text = wisdom_UI;
		wisdom_info.Text = string.Format(wisdom_info_text, wisdom_UI, 0); // 2 variable(s)

		charisma_value.Text = charisma_UI;
		charisma_info.Text = string.Format(charisma_info_text, charisma_UI); // 1 variable(s)

		// Stats Summary
		damage_value.Text = damage_UI;
		damage_info.Text = string.Format(damage_info_text, damage_UI, physical_melee_dps_UI, spell_melee_dps_UI, physical_ranged_dps_UI, spell_ranged_dps_UI); // 1 variable(s)

		resistance_value.Text = resistance_UI;
		resistance_info.Text = string.Format(resistance_info_text, resistance_UI); // 1 variable(s)

		recovery_value.Text = recovery_UI;
		recovery_info.Text = string.Format(recovery_info_text, recovery_UI); // 1 variable(s)

		// Sheet


		// Offense
		physical_melee_power_value.Text = physical_melee_power_UI;
		physical_melee_power_info.Text = string.Format(physical_melee_power_info_text, physical_melee_power_UI);

		spell_melee_power_value.Text = spell_melee_power_UI;
		spell_melee_power_info.Text = string.Format(spell_melee_power_info_text, spell_melee_power_UI);

		physical_ranged_power_value.Text = physical_ranged_power_UI;
		physical_ranged_power_info.Text = string.Format(physical_ranged_power_info_text, physical_ranged_power_UI);

		spell_ranged_power_value.Text = spell_ranged_power_UI;
		spell_ranged_power_info.Text = string.Format(spell_ranged_power_info_text, spell_ranged_power_UI);

		wisdom_scaler_value.Text = wisdom_scaler_UI;
		wisdom_scaler_info.Text = string.Format(wisdom_scaler_info_text, wisdom_scaler_UI);

		slash_damage_value.Text = slash_damage_UI;
		slash_damage_info.Text = string.Format(slash_damage_info_text, 0);

		thrust_damage_value.Text = thrust_damage_UI;
		thrust_damage_info.Text = string.Format(thrust_damage_info_text, 0);

		blunt_damage_value.Text = blunt_damage_UI;
		blunt_damage_info.Text = string.Format(blunt_damage_info_text, 0);

		bleed_damage_value.Text = bleed_damage_UI;
		bleed_damage_info.Text = string.Format(bleed_damage_info_text, 0);

		poison_damage_value.Text = poison_damage_UI;
		poison_damage_info.Text = string.Format(poison_damage_info_text, 0);

		fire_damage_value.Text = fire_damage_UI;
		fire_damage_info.Text = string.Format(fire_damage_info_text, 0);

		cold_damage_value.Text = cold_damage_UI;
		cold_damage_info.Text = string.Format(cold_damage_info_text, 0);

		lightning_damage_value.Text = lightning_damage_UI;
		lightning_damage_info.Text = string.Format(lightning_damage_info_text, 0);

		holy_damage_value.Text = holy_damage_UI;
		holy_damage_info.Text = string.Format(holy_damage_info_text, 0);

		critical_hit_chance_value.Text = critical_hit_chance_UI;
		critical_hit_chance_info.Text = string.Format(critical_hit_chance_text, critical_hit_chance_UI);

		critical_hit_damage_value.Text = critical_hit_damage_UI;
		critical_hit_damage_info.Text = string.Format(critical_hit_damage_text, critical_hit_damage_UI);

		attack_speed_value.Text = attack_speed_UI;
		attack_speed_info.Text = string.Format(attack_speed_info_text, attack_speed_UI);

		attack_speed_increase_value.Text = attack_speed_UI;
		attack_speed_increase_info.Text = string.Format(attack_speed_increase_text, 0);

		cooldown_reduction_value.Text = cool_down_reduction_UI;
		cooldown_reduction_info.Text = string.Format(cooldown_reduction_info_text, 0);

		posture_damage_value.Text = posture_damage_UI;
		posture_damage_info.Text = string.Format(posture_damage_text, 0);

		// Defense

		armor_value.Text = armor_UI;
		armor_info.Text = string.Format(armor_info_text, 0, 0);

		poise_value.Text = poise_UI;
		poise_info.Text = string.Format(poise_info_text, 0);

		block_amount_value.Text = block_amount_UI;
		block_amount_info.Text = string.Format(block_amount_info_text, 0);

		retaliation_value.Text = retaliation_UI;
		retaliation_info.Text = string.Format(retaliation_info_text, 0);

		physical_resistance_value.Text = physical_resistance_UI;
		physical_resistance_info.Text = string.Format(physical_resistance_info_text, 0);

		thrust_resistance_value.Text = thrust_resistance_UI;
		thrust_resistance_info.Text = string.Format(thrust_resistance_info_text, 0);

		slash_resistance_value.Text = slash_resistance_UI;
		slash_resistance_info.Text = string.Format(slash_resistance_info_text, 0);

		blunt_resistance_value.Text = blunt_resistance_UI;
		blunt_resistance_info.Text = string.Format(blunt_resistance_info_text, 0);

		bleed_resistance_value.Text = bleed_resistance_UI;
		bleed_resistance_info.Text = string.Format(bleed_resistance_info_text, 0);

		poison_resistance_value.Text = poison_resistance_UI;
		poison_resistance_info.Text = string.Format(poison_resistance_info_text, 0);

		curse_resistance_value.Text = curse_resistance_UI;
		curse_resistance_info.Text = string.Format(curse_resistance_info_text, 0);

		spell_resistance_value.Text = spell_resistance_UI;
		spell_resistance_info.Text = string.Format(spell_resistance_info_text, 0);

		fire_resistance_value.Text = fire_resistance_UI;
		fire_resistance_info.Text = string.Format(fire_resistance_info_text, 0);

		cold_resistance_value.Text = cold_resistance_UI;
		cold_resistance_info.Text = string.Format(cold_resistance_info_text, 0);

		lightning_resistance_value.Text = lightning_resistance_UI;
		lightning_resistance_info.Text = string.Format(lighting_resistance_info_text, 0);

		holy_resistance_value.Text = holy_resistance_UI;
		holy_resistance_info.Text = string.Format(holy_resistance_info_text, 0);

		// Health

		maximum_health_value.Text = maximum_health_UI;
		maximum_health_info.Text = string.Format(maximum_health_info_text, maximum_health_UI);

		health_bonus_value.Text = health_bonus_UI;
		health_bonus_info.Text = string.Format(health_bonus_info_text, 0);

		health_regen_value.Text = health_regen_UI;
		health_regen_info.Text = string.Format(health_regen_info_text, health_regen_UI);

		posture_regen_value.Text = posture_regen_UI;
		posture_regen_info.Text = string.Format(posture_regen_info_text, posture_regen_UI);

		health_retaliation_value.Text = health_on_retaliate_UI;
		health_retaliation_info.Text = string.Format(health_retaliation_info_text, 0);

		// Resource

		maximum_resource_value.Text = maximum_resource_UI;
		maximum_resource_info.Text = string.Format(maximum_resource_info_text, 0);

		resource_regen_value.Text = resource_regen_UI;
		resource_regen_info.Text = string.Format(resource_regen_info_text, 0);

		resource_cost_reduction_value.Text = resource_cost_reduction_UI;
		resource_cost_reduction_info.Text = string.Format(resource_cost_reduction_info_text, 0);

		// Misc

		movement_speed_value.Text = movement_speed_UI;
		movement_speed_info.Text = string.Format(movement_speed_info_text, 0);

		

		level_value.Text = level_UI;
		level_info.Text = string.Format(level_info_text, level_UI); // 1 variable(s)

		gold_label.Text = gold_UI;
		
		// GD.Print("updated");
		
	}


	public void _on_rep_focus_entered()
	{
		// Control info = (Control)rep_label.GetChild(0);
		// info.Show();
	}

	public void _on_rep_focus_exited()
	{
		// Control info = (Control)rep_label.GetChild(0);
		// info.Hide();
	}

	public void _on_sheet_focus_entered()
	{
		// Control info = (Control)sheet_label.GetChild(0);
		// info.Show();
	}

	public void _on_sheet_focus_exited()
	{
		// Control info = (Control)sheet_label.GetChild(0);
		// info.Hide();
	}


	public void _on_mats_label_focus_entered()
	{
		Control info = (Control)mats_label.GetChild(0);
		info.Show();
	}

	public void _on_mats_label_focus_exited()
	{
		Control info = (Control)mats_label.GetChild(0);
		info.Hide();
	}

	public void _on_gold_focus_entered()
	{
		Control info = (Control)gold_label.GetChild(0);
		info.Show();
	}

	public void _on_gold_focus_exited()
	{
		Control info = (Control)gold_label.GetChild(0);
		info.Hide();
	}

	public void _on_trash_focus_entered()
	{	
		Control info = (Control)trash_label.GetChild(0);
		info.Show();
	}

	public void _on_trash_focus_exited()
	{
		Control info = (Control)trash_label.GetChild(0);
		info.Hide();
	}

	public void _on_trash_area_2d_area_entered(Area2D area)
	{	
		over_trash = true;
	}

	public void _on_trash_area_2d_area_exited(Area2D area)
	{
		over_trash = false;
	}

}
