using Godot;
using System;

public partial class InventoryInfo : UI
{

	private bool stats_need_updated;

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
	private string strength_info_text = " Strength {0} \n * Primary stat for melee damage \n * Increases damage by {1} \n * Increases health by {2} ";

	private Button dexterity_label;
	private Label dexterity_value;
	private RichTextLabel dexterity_info;
	private string dexterity_info_text = " Dexterity {0} \n * Primary stat for melee \n * Calculated by physical ranged  and critical damage \n * Increases damage by {1} \n * Increases critical chance by {2} * Increases critical damage by {3}";

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
	private string stamina_info_text = " Stamina {0} \n * Primary stat for resource and regeneration \n * Increases total resource by {1} \n * Increases health and resource regeneration by {2} \n * Increases health by {3}";

	private Button wisdom_label;
	private Label wisdom_value;
	private RichTextLabel wisdom_info;
	private string wisdom_info_text = " Wisdom {0} \n * Primary stat for hit and interaction \n * Increases hit chance by {1}";

	private Button charisma_label;
	private Label charisma_value;
	private RichTextLabel charisma_info;
	private string charisma_info_text = "  Charisma {0} \n * Primary stat for character interaction \n * Increases special interactions";

	// Character details
	private Button damage_label;
	private Label damage_value;
	private RichTextLabel damage_info;
	private string damage_info_text = "  Damage {0} \n * Total damage per second done by character \n * Combination of physical power (melee and ranged), spell power(melee and ranged), \n    weapon damage, attack speed, critical hit chance, and critical hit damage";

	private Button resistance_label;
	private Label resistance_value;
	private RichTextLabel resistance_info;
	private string resistance_info_text = " Resistance {0} \n * Total damage the character can resist \n * Calculated by health, armor, resistances" ;

	private Button recovery_label;
	private Label recovery_value;
	private RichTextLabel recovery_info;
	private string recovery_info_text =  " Recovery {0} \n * How fast the character regenerates health, resource and posture \n * Calculated by stamina ";

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
	private string spell_melee_power_info_text =  " Spell melee power {0} \n Increases magic melee DPS and HPS by 1 every 15 points \n * +3 for every point of intellect +1 for every point of dexterity + 1 for every point of strength \n * Bonuses  obtainable on gear ";

	private Button physical_ranged_power_label;
	private Label physical_ranged_power_value;
	private RichTextLabel physical_ranged_power_info;
	private string physical_ranged_power_info_text =  " Physical ranged power {0} \n * Increases physical ranged dps by 1 every 15 points \n * +3 for every point of dexterity +1 for every point of strength \n * Bonuses obtainable on gear ";

	private Button spell_ranged_power_label;
	private Label spell_ranged_power_value;
	private RichTextLabel spell_ranged_power_info;
	private string spell_ranged_power_info_text =  " Spell ranged power {0} \n Increases magic ranged DPS and HPS by 1 every 15 points \n * +3 for every point of intellect +2 for every point of dexterity \n * Bonuses obtainable on gear ";
	
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

	private Button skills_label;
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

		strength_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/StrengthLabel");
		strength_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/Value");
		strength_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/StrengthLabel/Info/MarginContainer/PanelContainer/Label");

		dexterity_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/DexterityLabel");
		dexterity_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/Value");
		dexterity_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/DexterityLabel/Info/MarginContainer/PanelContainer/Label");

		intellect_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/IntellectLabel");
		intellect_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/Value");
		intellect_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/IntellectLabel/Info/MarginContainer/PanelContainer/Label");

		vitality_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/VitalityLabel");
		vitality_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/Value");
		vitality_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/VitalityLabel/Info/MarginContainer/PanelContainer/Label");

		stamina_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/StaminaLabel");
		stamina_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/Value");
		stamina_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/StaminaLabel/Info/MarginContainer/PanelContainer/Label");

		wisdom_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/WisdomLabel");
		wisdom_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/Value");
		wisdom_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/WisdomLabel/Info/MarginContainer/PanelContainer/Label");

		charisma_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/CharismaLabel");
		charisma_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/Value");
		charisma_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/CharismaLabel/Info/MarginContainer/PanelContainer/Label");

		// Stats details
		damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/DamageLabel");
		damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/Value");
		damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/DamageLabel/Info/MarginContainer/PanelContainer/Label");

		resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/ResistanceLabel");
		resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/Value");
		resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/ResistanceLabel/Info/MarginContainer/PanelContainer/Label");

		recovery_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/RecoveryLabel");
		recovery_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/Value");
		recovery_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/RecoveryLabel/Info/MarginContainer/PanelContainer/Label");

		level_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/LevelLabel");
		level_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/Value");
		level_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/LevelLabel/Info/MarginContainer/PanelContainer/Label");

		level_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/LevelLabel");
		level_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/Value");
		level_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/LevelLabel/Info/MarginContainer/PanelContainer/Label");

		// Offense
		physical_melee_power_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalMeleePower/PhysicalMeleePowerLabel");
		physical_melee_power_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalMeleePower/Value");
		physical_melee_power_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalMeleePower/PhysicalMeleePowerLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		spell_melee_power_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellMeleePower/SpellMeleePowerLabel");
		spell_melee_power_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellMeleePower/Value");
		spell_melee_power_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellMeleePower/SpellMeleePowerLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		physical_ranged_power_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalRangedPower/PhysicalRangedPowerLabel");
		physical_ranged_power_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalRangedPower/Value");
		physical_ranged_power_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalRangedPower/PhysicalRangedPowerLabel/Info/MarginContainer/PanelContainer/RichTextLabel");
       
        spell_ranged_power_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellRangedPower/SpellRangedPowerLabel");
		spell_ranged_power_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellRangedPower/Value");
		spell_ranged_power_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellRangedPower/SpellRangedPowerLabel/Info/MarginContainer/PanelContainer/RichTextLabel");
 		
		thrust_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustDamage/ThrustDamageLabel");
		thrust_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustDamage/Value");
		thrust_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustDamage/ThrustDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		slash_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashDamage/SlashDamageLabel");
		slash_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashDamage/Value");
		slash_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashDamage/SlashDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		blunt_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntDamage/BluntDamageLabel");
		blunt_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntDamage/Value");
		blunt_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntDamage/BluntDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		bleed_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedDamage/BleedDamageLabel");
		bleed_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedDamage/Value");
		bleed_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedDamage/BleedDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		poison_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonDamage/PoisonDamageLabel");
		poison_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonDamage/Value");
		poison_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonDamage/PoisonDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		fire_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireDamage/FireDamageLabel");
		fire_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireDamage/Value");
		fire_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireDamage/FireDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		cold_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdDamage/ColdDamageLabel");
		cold_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdDamage/Value");
		cold_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdDamage/ColdDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		lightning_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningDamage/LightningDamageLabel");
		lightning_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningDamage/Value");
		lightning_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningDamage/LightningDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		holy_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyDamage/HolyDamageLabel");
		holy_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyDamage/Value");
		holy_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyDamage/HolyDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		critical_hit_chance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitChance/CriticalHitChanceLabel");
		critical_hit_chance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitChance/Value");
		critical_hit_chance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitChance/CriticalHitChanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		critical_hit_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitDamage/CriticalHitDamageLabel");
		critical_hit_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitDamage/Value");
		critical_hit_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitDamage/CriticalHitDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		attack_speed_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeed/AttackSpeedLabel");
		attack_speed_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeed/Value");
		attack_speed_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeed/AttackSpeedLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		attack_speed_increase_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeedIncrease/AttackSpeedIncreaseLabel");
		attack_speed_increase_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeedIncrease/Value");
		attack_speed_increase_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeedIncrease/AttackSpeedIncreaseLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		cooldown_reduction_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CooldownReduction/CooldownReductionLabel");
		cooldown_reduction_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CooldownReduction/Value");
		cooldown_reduction_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CooldownReduction/CooldownReductionLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		posture_damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureDamage/PostureDamageLabel");
		posture_damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureDamage/Value");
		posture_damage_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureDamage/PostureDamageLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		//Defense
		armor_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Armor/ArmorLabel");
		armor_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Armor/Value");
		armor_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Armor/ArmorLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		poise_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poise/PoiseLabel");
		poise_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poise/Value");
		poise_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poise/PoiseLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		block_amount_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BlockAmount/BlockAmountLabel");
		block_amount_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BlockAmount/Value");
		block_amount_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BlockAmount/BlockAmountLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		retaliation_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Retaliation/RetaliationLabel");
		retaliation_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Retaliation/Value");
		retaliation_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Retaliation/RetaliationLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		physical_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalResistance/PhysicalResistanceLabel");
		physical_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalResistance/Value");
		physical_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalResistance/PhysicalResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		thrust_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustResistance/ThrustResistanceLabel");
		thrust_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustResistance/Value");
		thrust_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustResistance/ThrustResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		slash_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashResistance/SlashResistanceLabel");
		slash_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashResistance/Value");
		slash_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashResistance/SlashResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		blunt_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntResistance/BluntResistanceLabel");
		blunt_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntResistance/Value");
		blunt_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntResistance/BluntResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		bleed_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedResistance/BleedResistanceLabel");
		bleed_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedResistance/Value");
		bleed_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedResistance/BleedResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		poison_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonResistance/PoisonResistanceLabel");
		poison_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonResistance/Value");
		poison_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonResistance/PoisonResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");
		
		curse_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CurseResistance/CurseResistanceLabel");
		curse_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CurseResistance/Value");
		curse_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CurseResistance/CurseResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		spell_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellResistance/SpellResistanceLabel");
		spell_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellResistance/Value");
		spell_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellResistance/SpellResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		fire_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireResistance/FireResistanceLabel");
		fire_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireResistance/Value");
		fire_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireResistance/FireResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		cold_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdResistance/ColdResistanceLabel");
		cold_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdResistance/Value");
		cold_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdResistance/ColdResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		lightning_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningResistance/LightningResistanceLabel");
		lightning_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningResistance/Value");
		lightning_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningResistance/LightningResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		holy_resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyResistance/HolyResistanceLabel");
		holy_resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyResistance/Value");
		holy_resistance_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyResistance/HolyResistanceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		// Health
		maximum_health_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumHealth/MaximumHealthLabel");
		maximum_health_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumHealth/Value");
		maximum_health_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumHealth/MaximumHealthLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		health_bonus_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthBonus/HealthBonusLabel");
		health_bonus_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthBonus/Value");
		health_bonus_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthBonus/HealthBonusLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		health_regen_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/HealthRegenerationLabel");
		health_regen_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/Value");
		health_regen_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/HealthRegenerationLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		health_retaliation_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRetaliation/HealthRetaliationLabel");
		health_retaliation_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRetaliation/Value");
		health_retaliation_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRetaliation/HealthRetaliationLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		//Resource 
		maximum_resource_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumResource/MaximumResourceLabel");
		maximum_resource_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumResource/Value");
		maximum_resource_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumResource/MaximumResourceLabel/Info/MarginContainer/PanelContainer/RichTextLabel");
		
		resource_regen_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceRegeneration/ResourceRegenerationLabel");
		resource_regen_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceRegeneration/Value");
		resource_regen_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceRegeneration/ResourceRegenerationLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		resource_cost_reduction_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceCostReduction/ResourceCostReductionLabel");
		resource_cost_reduction_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceCostReduction/Value");
		resource_cost_reduction_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceCostReduction/ResourceCostReductionLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		//Misc
		movement_speed_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MovementSpeed/MovementSpeedLabel");
		movement_speed_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MovementSpeed/Value");
		movement_speed_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MovementSpeed/MovementSpeedLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		
		

		rep_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/RepLabel");
		sheet_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/SheetLabel");

		// Character possessions
		
		gold_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/MatsGold/GoldLabel");

		mats_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/MatsGold/MatsLabel");

		// Misc
		trash_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BagSlots/TrashLabel");

		skills_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/SkillsLabel");
		journal_quests_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/JournalQuestsLabel");
		achievements_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/AchievementsLabel");
		social_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/SocialLabel");
		options_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/BottomButtons/OptionsLabel");
		

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		_customSignals.PlayerInfo += HandlePlayerInfo;
	}

    private void HandlePlayerInfo(player player)
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
		string level = this_player.level.ToString();
		string strength = this_player.strength.ToString();
		string dexterity = this_player.dexterity.ToString();
		string intellect = this_player.intellect.ToString();
		string vitality = this_player.vitality.ToString();
		string stamina = this_player.stamina.ToString();
		string wisdom = this_player.wisdom.ToString();
		string charisma = this_player.charisma.ToString();

		// Stat details
		string damage = this_player.damage.ToString();

		// Sheet

		// Offense
		string physical_melee_power = this_player.physical_melee_power.ToString();
		string spell_melee_power = this_player.spell_melee_power.ToString();
		string physical_ranged_power = this_player.physical_ranged_power.ToString();
		string spell_ranged_power = this_player.spell_ranged_power.ToString();
		// Thrust
		// Slash
		// Blunt
		// Bleed
		// Poison
		// Fire
		// Cold
		// Lightning
		// Holy
		string critical_hit_chance = this_player.critical_hit_chance.ToString();
		string critical_hit_damage = this_player.critical_hit_damage.ToString();
		string attack_speed = this_player.attack_speed.ToString();
		// Attack Speed Increase
		// Cooldown reduction
		// Posture Damage

		// Defense

		// Armor
		// Poise 
		// Block Amount
		// Retaliation
		// Physical Resistance
		// Thrust Resistance
		// Slash Resistance
		// Blunt Resistance
		// Bleed Resistance
		// Poison Resistance
		// Curse Resistance
		// Spell Resistance
		// Fire Resistance
		// Cold Resistance
		// Lightning Resistance
		// Holy Resistance

		// Health

		string maximum_health = this_player.health.ToString();
		// Health Bonus
		// Health Regen
		// Health on Retaliate

		// Resources

		string maximum_resource = this_player.resource.ToString();
		// Resource Regen
		// Resource Cost Reduction

		// Misc

		// Movement Speed

		// Possessions
		string gold = this_player.gold.ToString();
		
		// Base Stats
		strength_value.Text = strength;
		strength_info.Text = string.Format(strength_info_text, strength, 0, 0); // 3 variable(s)

		dexterity_value.Text = dexterity;
		dexterity_info.Text = string.Format(dexterity_info_text, dexterity, 0, 0, 0); // 4 variable(s)

		intellect_value.Text = intellect;
		intellect_info.Text = string.Format(intellect_info_text, intellect, 0, 0); // 3 variable(s)

		vitality_value.Text = vitality;
		vitality_info.Text = string.Format(vitality_info_text, vitality, 0, 0); // 2 variable(s)

		stamina_value.Text = stamina;
		stamina_info.Text = string.Format(stamina_info_text, stamina, 0, 0, 0); // 4 variable(s)

		wisdom_value.Text = wisdom;
		wisdom_info.Text = string.Format(wisdom_info_text, wisdom, 0); // 2 variable(s)

		charisma_value.Text = charisma;
		charisma_info.Text = string.Format(charisma_info_text, charisma); // 1 variable(s)

		// Stats Summary
		damage_value.Text = damage;
		damage_info.Text = string.Format(damage_info_text, damage); // 1 variable(s)

		resistance_value.Text = "";
		resistance_info.Text = string.Format(resistance_info_text, 0); // 1 variable(s)

		recovery_value.Text = "";
		recovery_info.Text = string.Format(recovery_info_text, 0); // 1 variable(s)

		// Sheet


		// Offense
		physical_melee_power_value.Text = physical_melee_power;
		physical_melee_power_info.Text = string.Format(physical_melee_power_info_text, physical_melee_power);

		spell_melee_power_value.Text = spell_melee_power;
		spell_melee_power_info.Text = string.Format(spell_melee_power_info_text, spell_melee_power);

		physical_ranged_power_value.Text = physical_ranged_power;
		physical_ranged_power_info.Text = string.Format(physical_ranged_power_info_text, physical_ranged_power);

		spell_ranged_power_value.Text = spell_ranged_power;
		spell_ranged_power_info.Text = string.Format(spell_ranged_power_info_text, spell_ranged_power);

		// thrust_damage_value.Text =;
		thrust_damage_info.Text = string.Format(thrust_damage_info_text, 0);

		// slash_damage_value.Text =;
		slash_damage_info.Text = string.Format(slash_damage_info_text, 0);

		// blunt_damage_value.Text =;
		blunt_damage_info.Text = string.Format(blunt_damage_info_text, 0);

		// bleed_damage_value.Text =;
		bleed_damage_info.Text = string.Format(bleed_damage_info_text, 0);

		// poison_damage_value.Text =;
		poison_damage_info.Text = string.Format(poison_damage_info_text, 0);

		// fire_damage_value.Text =;
		fire_damage_info.Text = string.Format(fire_damage_info_text, 0);

		// cold_damage_value.Text =;
		cold_damage_info.Text = string.Format(cold_damage_info_text, 0);

		// lightning_damage_value.Text =;
		lightning_damage_info.Text = string.Format(lightning_damage_info_text, 0);

		// holy_damage_value.Text =;
		holy_damage_info.Text = string.Format(holy_damage_info_text, 0);

		critical_hit_chance_value.Text = critical_hit_chance;
		critical_hit_chance_info.Text = string.Format(critical_hit_chance_text, critical_hit_chance);

		critical_hit_damage_value.Text = critical_hit_damage;
		critical_hit_damage_info.Text = string.Format(critical_hit_damage_text, critical_hit_damage);

		attack_speed_value.Text = attack_speed;
		attack_speed_info.Text = string.Format(attack_speed_info_text, attack_speed);

		// attack_speed_increase_value.Text =;
		attack_speed_increase_info.Text = string.Format(attack_speed_increase_text, 0);

		// cooldown_reduction_value.Text =;
		cooldown_reduction_info.Text = string.Format(cooldown_reduction_info_text, 0);

		// posture_damage_value.Text =;
		posture_damage_info.Text = string.Format(posture_damage_text, 0);

		// Defense

		// armor_value.Text =;
		armor_info.Text = string.Format(armor_info_text, 0, 0);

		// poise_value.Text =;
		poise_info.Text = string.Format(poise_info_text, 0);

		// block_amount_value.Text =;
		block_amount_info.Text = string.Format(block_amount_info_text, 0);

		// retaliation_value.Text =;
		retaliation_info.Text = string.Format(retaliation_info_text, 0);

		// physical_resistance_value.Text =;
		physical_resistance_info.Text = string.Format(physical_resistance_info_text, 0);

		// thrust_resistance_value.Text =;
		thrust_resistance_info.Text = string.Format(thrust_resistance_info_text, 0);

		// slash_resistance_value.Text =;
		slash_resistance_info.Text = string.Format(slash_resistance_info_text, 0);

		// blunt_resistance_value.Text =;
		blunt_resistance_info.Text = string.Format(blunt_resistance_info_text, 0);

		// bleed_resistance_value.Text =;
		bleed_resistance_info.Text = string.Format(bleed_resistance_info_text, 0);

		// poison_resistance_value.Text =;
		poison_resistance_info.Text = string.Format(poison_resistance_info_text, 0);

		// curse_resistance_value.Text =;
		curse_resistance_info.Text = string.Format(curse_resistance_info_text, 0);

		// spell_resistance_value.Text =;
		spell_resistance_info.Text = string.Format(spell_resistance_info_text, 0);

		// fire_resistance_value.Text =;
		fire_resistance_info.Text = string.Format(fire_resistance_info_text, 0);

		// cold_resistance_value.Text =;
		cold_resistance_info.Text = string.Format(cold_resistance_info_text, 0);

		// lightning_resistance_value.Text =;
		lightning_resistance_info.Text = string.Format(lighting_resistance_info_text, 0);

		// holy_resistance_value.Text =;
		holy_resistance_info.Text = string.Format(holy_resistance_info_text, 0);

		// Health

		maximum_health_value.Text = maximum_health;
		maximum_health_info.Text = string.Format(maximum_health_info_text, maximum_health);

		// health_bonus_value.Text =;
		health_bonus_info.Text = string.Format(health_bonus_info_text, 0);

		// health_regen_value.Text =;
		health_regen_info.Text = string.Format(health_regen_info_text, 0);

		// health_retaliation_value.Text =;
		health_retaliation_info.Text = string.Format(health_retaliation_info_text, 0);

		// Resource

		maximum_resource_value.Text = maximum_resource;
		maximum_resource_info.Text = string.Format(maximum_resource_info_text, 0);

		// resource_regen_value.Text =;
		resource_regen_info.Text = string.Format(resource_regen_info_text, 0);

		// resource_cost_reduction_value.Text =;
		resource_cost_reduction_info.Text = string.Format(resource_cost_reduction_info_text, 0);

		// Misc

		// movement_speed_value.Text =;
		movement_speed_info.Text = string.Format(movement_speed_info_text, 0);

		

		level_value.Text = level;
		level_info.Text = string.Format(level_info_text, level); // 1 variable(s)

		gold_label.Text = gold;
		
		GD.Print("updated");
		
	}

	public void _on_head_focus_entered()
	{
		over_head = true;
		Control info = (Control)head_slot.GetChild(1);
		info.Show();
	}

	public void _on_head_focus_exited()
	{
		over_head = false;
		Control info = (Control)head_slot.GetChild(1);
		info.Hide();

	}

	public void _on_shoulder_focus_entered()
	{
		over_shoulders = true;
		Control info = (Control)shoulder_slot.GetChild(1);
		info.Show();
	}

	public void _on_shoulder_focus_exited()
	{
		over_shoulders = false;
		
		Control info = (Control)shoulder_slot.GetChild(1);
		info.Hide();
	}

	public void _on_neck_focus_entered()
	{
		over_neck = true;
		Control info = (Control)neck_slot.GetChild(1);
		info.Show();
	}

	public void _on_neck_focus_exited()
	{
		over_neck = false;
		Control info = (Control)neck_slot.GetChild(1);
		info.Hide();
	}

	public void _on_chest_focus_entered()
	{
		over_chest = true;
		Control info = (Control)chest_slot.GetChild(1);
		info.Show();
	}

	public void _on_chest_focus_exited()
	{
		over_chest = false;
		Control info = (Control)chest_slot.GetChild(1);
		info.Hide();
	}

	public void _on_gloves_focus_entered()
	{
		over_gloves = true;
		Control info = (Control)gloves_slot.GetChild(1);
		info.Show();
	}

	public void _on_gloves_focus_exited()
	{
		over_gloves = false;
		Control info = (Control)gloves_slot.GetChild(1);
		info.Hide();
	}

	public void _on_bracers_focus_entered()
	{
		over_bracers = true;
		Control info = (Control)bracers_slot.GetChild(1);
		info.Show();
	}

	public void _on_bracers_focus_exited()
	{
		over_bracers = false;
		Control info = (Control)bracers_slot.GetChild(1);
		info.Hide();
	}

	public void _on_belt_focus_entered()
	{
		over_belt = true;
		Control info = (Control)belt_slot.GetChild(1);
		info.Show();
	}

	public void _on_belt_focus_exited()
	{
		over_belt = false;
		Control info = (Control)belt_slot.GetChild(1);
		info.Hide();
	}

	public void _on_ring_1_focus_entered()
	{
		over_ring1 = true;
		Control info = (Control)ring1_slot.GetChild(1);
		info.Show();
	}

	public void _on_ring_1_focus_exited()
	{
		over_ring1 = false;
		Control info = (Control)ring1_slot.GetChild(1);
		info.Hide();
	}

	public void _on_ring_2_focus_entered()
	{
		over_ring2 = true;
		Control info = (Control)ring2_slot.GetChild(1);
		info.Show();
	}

	public void _on_ring_2_focus_exited()
	{
		over_ring2 = false;
		Control info = (Control)ring2_slot.GetChild(1);
		info.Hide();
	}

	public void _on_main_hand_focus_entered()
	{
		over_main = true;
		Control info = (Control)mainhand_slot.GetChild(1);
		info.Show();
	}

	public void _on_main_hand_focus_exited()
	{
		over_main = false;
		Control info = (Control)mainhand_slot.GetChild(1);
		info.Hide();
		
	}

	public void _on_off_hand_focus_entered()
	{
		over_off = true;
		Control info = (Control)offhand_slot.GetChild(1);
		info.Show();
	}

	public void _on_off_hand_focus_exited()
	{
		over_off = false;
		Control info = (Control)offhand_slot.GetChild(1);
		info.Hide();
	}

	public void _on_pants_focus_entered()
	{
		over_pants = true;
		Control info = (Control)pants_slot.GetChild(1);
		info.Show();
	}

	public void _on_pants_focus_exited()
	{
		over_pants = false;
		Control info = (Control)pants_slot.GetChild(1);
		info.Hide();
	}

	public void _on_boots_focus_entered()
	{
		over_boots = true;
		Control info = (Control)boots_slot.GetChild(1);
		info.Show();
	}

	public void _on_boots_focus_exited()
	{
		over_boots = false;
		Control info = (Control)boots_slot.GetChild(1);
		info.Hide();
	}

	public void _on_level_label_focus_entered()
	{
		Control info = (Control)level_label.GetChild(0);
		info.Show();
	}

	public void _on_level_label_focus_exited()
	{
		Control info = (Control)level_label.GetChild(0);
		info.Hide();
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

	public void _on_strength_label_focus_entered()
	{
		Control info = (Control)strength_label.GetChild(0);
		info.Show();
	}

	public void _on_strength_label_focus_exited()
	{
		Control info = (Control)strength_label.GetChild(0);
		info.Hide();
	}
	public void _on_dexterity_label_focus_entered()
	{
		Control info = (Control)dexterity_label.GetChild(0);
		info.Show();
	}

	public void _on_dexterity_label_focus_exited()
	{
		Control info = (Control)dexterity_label.GetChild(0);
		info.Hide();
	}

	public void _on_intellect_label_focus_entered()
	{
		Control info = (Control)intellect_label.GetChild(0);
		info.Show();
	}

	public void _on_intellect_label_focus_exited()
	{
		Control info = (Control)intellect_label.GetChild(0);
		info.Hide();
	}

	public void _on_vitality_label_focus_entered()
	{
		Control info = (Control)vitality_label.GetChild(0);
		info.Show();
	}

	public void _on_vitality_label_focus_exited()
	{
		Control info = (Control)vitality_label.GetChild(0);
		info.Hide();
	}

	public void _on_stamina_label_focus_entered()
	{
		Control info = (Control)stamina_label.GetChild(0);
		info.Show();
	}

	public void _on_stamina_label_focus_exited()
	{
		Control info = (Control)stamina_label.GetChild(0);
		info.Hide();
	}

	public void _on_wisdom_label_focus_entered()
	{
		Control info = (Control)wisdom_label.GetChild(0);
		info.Show();
	}

	public void _on_wisdom_label_focus_exited()
	{
		Control info = (Control)wisdom_label.GetChild(0);
		info.Hide();
	}

	public void _on_charisma_label_focus_entered()
	{
		Control info = (Control)charisma_label.GetChild(0);
		info.Show();
	}

	public void _on_charisma_label_focus_exited()
	{
		Control info = (Control)charisma_label.GetChild(0);
		info.Hide();
	}

	public void _on_damage_label_focus_entered()
	{
		Control info = (Control)damage_label.GetChild(0);
		info.Show();
	}

	public void _on_damage_label_focus_exited()
	{
		Control info = (Control)damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_resistance_label_focus_entered()
	{
		Control info = (Control)resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_resistance_label_focus_exited()
	{
		Control info = (Control)resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_recovery_label_focus_entered()
	{
		Control info = (Control)recovery_label.GetChild(0);
		info.Show();
	}

	public void _on_recovery_label_focus_exited()
	{
		Control info = (Control)recovery_label.GetChild(0);
		info.Hide();
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

	// Offense

	public void _on_physical_melee_power_label_focus_entered()
	{
		Control info = (Control)physical_melee_power_label.GetChild(0);
		info.Show();
	}

	public void _on_physical_melee_power_label_focus_exited()
	{
		Control info = (Control)physical_melee_power_label.GetChild(0);
		info.Hide();
	}

	public void _on_spell_melee_power_label_focus_entered()
	{
		Control info = (Control)spell_melee_power_label.GetChild(0);
		info.Show();
	}

	public void _on_spell_melee_power_label_focus_exited()
	{
		Control info = (Control)spell_melee_power_label.GetChild(0);
		info.Hide();
	}

	public void _on_physical_ranged_power_label_focus_entered()
	{
		Control info = (Control)physical_ranged_power_label.GetChild(0);
		info.Show();
	}

	public void _on_physical_ranged_power_label_focus_exited()
	{
		Control info = (Control)physical_ranged_power_label.GetChild(0);
		info.Hide();
	}

	public void _on_spell_ranged_power_label_focus_entered()
	{
		Control info = (Control)spell_ranged_power_label.GetChild(0);
		info.Show();
	}

	public void _on_spell_ranged_power_label_focus_exited()
	{
		Control info = (Control)spell_ranged_power_label.GetChild(0);
		info.Hide();
	}

	public void _on_thrust_damage_label_focus_entered()
	{
		Control info = (Control)thrust_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_thrust_damage_label_focus_exited()
	{
		Control info = (Control)thrust_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_slash_damage_label_focus_entered()
	{
		Control info = (Control)slash_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_slash_damage_label_focus_exited()
	{
		Control info = (Control)slash_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_blunt_damage_label_focus_entered()
	{
		Control info = (Control)blunt_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_blunt_damage_label_focus_exited()
	{
		Control info = (Control)blunt_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_bleed_damage_label_focus_entered()
	{
		Control info = (Control)bleed_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_bleed_damage_label_focus_exited()
	{
		Control info = (Control)bleed_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_poison_damage_label_focus_entered()
	{
		Control info = (Control)poison_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_poison_damage_label_focus_exited()
	{
		Control info = (Control)poison_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_fire_damage_label_focus_entered()
	{
		Control info = (Control)fire_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_fire_damage_label_focus_exited()
	{
		Control info = (Control)fire_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_cold_damage_label_focus_entered()
	{
		Control info = (Control)cold_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_cold_damage_label_focus_exited()
	{
		Control info = (Control)cold_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_lightning_damage_label_focus_entered()
	{
		Control info = (Control)lightning_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_lightning_damage_label_focus_exited()
	{
		Control info = (Control)lightning_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_holy_damage_label_focus_entered()
	{
		Control info = (Control)holy_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_holy_damage_label_focus_exited()
	{
		Control info = (Control)holy_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_critical_hit_chance_label_focus_entered()
	{
		Control info = (Control)critical_hit_chance_label.GetChild(0);
		info.Show();
	}

	public void _on_critical_hit_chance_label_focus_exited()
	{
		Control info = (Control)critical_hit_chance_label.GetChild(0);
		info.Hide();
	}

	public void _on_critical_hit_damage_label_focus_entered()
	{
		Control info = (Control)critical_hit_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_critical_hit_damage_label_focus_exited()
	{
		Control info = (Control)critical_hit_damage_label.GetChild(0);
		info.Hide();
	}

	public void _on_attack_speed_label_focus_entered()
	{
		Control info = (Control)attack_speed_label.GetChild(0);
		info.Show();
	}

	public void _on_attack_speed_label_focus_exited()
	{
		Control info = (Control)attack_speed_label.GetChild(0);
		info.Hide();
	}

	public void _on_attack_speed_increase_label_focus_entered()
	{
		Control info = (Control)attack_speed_increase_label.GetChild(0);
		info.Show();
	}

	public void _on_attack_speed_increase_label_focus_exited()
	{
		Control info = (Control)attack_speed_increase_label.GetChild(0);
		info.Hide();
	}

	public void _on_cooldown_reduction_label_focus_entered()
	{
		Control info = (Control)cooldown_reduction_label.GetChild(0);
		info.Show();
	}

	public void _on_cooldown_reduction_label_focus_exited()
	{
		Control info = (Control)cooldown_reduction_label.GetChild(0);
		info.Hide();
	}

	public void _on_posture_damage_label_focus_entered()
	{
		Control info = (Control)posture_damage_label.GetChild(0);
		info.Show();
	}

	public void _on_posture_damage_label_focus_exited()
	{
		Control info = (Control)posture_damage_label.GetChild(0);
		info.Hide();
	}
	

	// Defense

	public void _on_armor_label_focus_entered()
	{
		Control info = (Control)armor_label.GetChild(0);
		info.Show();
	}

	public void _on_armor_label_focus_exited()
	{
		Control info = (Control)armor_label.GetChild(0);
		info.Hide();
	}

	public void _on_poise_label_focus_entered()
	{
		Control info = (Control)poise_label.GetChild(0);
		info.Show();
	}

	public void _on_poise_label_focus_exited()
	{
		Control info = (Control)poise_label.GetChild(0);
		info.Hide();
	}

	public void _on_block_amount_label_focus_entered()
	{
		Control info = (Control)block_amount_label.GetChild(0);
		info.Show();
	}

	public void _on_block_amount_label_focus_exited()
	{
		Control info = (Control)block_amount_label.GetChild(0);
		info.Hide();
	}

	public void _on_retaliation_label_focus_entered()
	{
		Control info = (Control)retaliation_label.GetChild(0);
		info.Show();
	}

	public void _on_retaliation_label_focus_exited()
	{
		Control info = (Control)retaliation_label.GetChild(0);
		info.Hide();
	}

	public void _on_physical_resistance_label_focus_entered()
	{
		Control info = (Control)physical_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_physical_resistance_label_focus_exited()
	{
		Control info = (Control)physical_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_thrust_resistance_label_focus_entered()
	{
		Control info = (Control)thrust_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_thrust_resistance_label_focus_exited()
	{
		Control info = (Control)thrust_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_slash_resistance_label_focus_entered()
	{
		Control info = (Control)slash_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_slash_resistance_label_focus_exited()
	{
		Control info = (Control)slash_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_blunt_resistance_label_focus_entered()
	{
		Control info = (Control)blunt_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_blunt_resistance_label_focus_exited()
	{
		Control info = (Control)blunt_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_bleed_resistance_label_focus_entered()
	{
		Control info = (Control)bleed_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_bleed_resistance_label_focus_exited()
	{
		Control info = (Control)bleed_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_poison_resistance_label_focus_entered()
	{
		Control info = (Control)poison_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_poison_resistance_label_focus_exited()
	{
		Control info = (Control)poison_resistance_label.GetChild(0);
		info.Hide();
	}


	public void _on_curse_resistance_label_focus_entered()
	{
		Control info = (Control)curse_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_curse_resistance_label_focus_exited()
	{
		Control info = (Control)curse_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_spell_resistance_label_focus_entered()
	{
		Control info = (Control)spell_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_spell_resistance_label_focus_exited()
	{
		Control info = (Control)spell_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_fire_resistance_label_focus_entered()
	{
		Control info = (Control)fire_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_fire_resistance_label_focus_exited()
	{
		Control info = (Control)fire_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_cold_resistance_label_focus_entered()
	{
		Control info = (Control)cold_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_cold_resistance_label_focus_exited()
	{
		Control info = (Control)cold_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_lightning_resistance_label_focus_entered()
	{
		Control info = (Control)lightning_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_lightning_resistance_label_focus_exited()
	{
		Control info = (Control)lightning_resistance_label.GetChild(0);
		info.Hide();
	}

	public void _on_holy_resistance_label_focus_entered()
	{
		Control info = (Control)holy_resistance_label.GetChild(0);
		info.Show();
	}

	public void _on_holy_resistance_label_focus_exited()
	{
		Control info = (Control)holy_resistance_label.GetChild(0);
		info.Hide();
	}

	// Health

	public void _on_maximum_health_label_focus_entered()
	{
		Control info = (Control)maximum_health_label.GetChild(0);
		info.Show();
	}

	public void _on_maximum_health_label_focus_exited()
	{
		Control info = (Control)maximum_health_label.GetChild(0);
		info.Hide();
	}

	public void _on_health_bonus_label_focus_entered()
	{
		Control info = (Control)health_bonus_label.GetChild(0);
		info.Show();
	}

	public void _on_health_bonus_label_focus_exited()
	{
		Control info = (Control)health_bonus_label.GetChild(0);
		info.Hide();
	}

	public void _on_health_regeneration_label_focus_entered()
	{
		Control info = (Control)health_regen_label.GetChild(0);
		info.Show();
	}

	public void _on_health_regeneration_label_focus_exited()
	{
		Control info = (Control)health_regen_label.GetChild(0);
		info.Hide();
	}

	public void _on_health_retaliation_label_focus_entered()
	{
		Control info = (Control)health_retaliation_label.GetChild(0);
		info.Show();
	}

	public void _on_health_retaliation_label_focus_exited()
	{
		Control info = (Control)health_retaliation_label.GetChild(0);
		info.Hide();
	}

	// Resource

	public void _on_maximum_resource_label_focus_entered()
	{
		Control info = (Control)maximum_resource_label.GetChild(0);
		info.Show();
	}

	public void _on_maximum_resource_label_focus_exited()
	{
		Control info = (Control)maximum_resource_label.GetChild(0);
		info.Hide();
	}

	public void _on_resource_regeneration_label_focus_entered()
	{
		Control info = (Control)resource_regen_label.GetChild(0);
		info.Show();
	}

	public void _on_resource_regeneration_label_focus_exited()
	{
		Control info = (Control)resource_regen_label.GetChild(0);
		info.Hide();
	}

	public void _on_resource_cost_reduction_label_focus_entered()
	{
		Control info = (Control)resource_cost_reduction_label.GetChild(0);
		info.Show();
	}

	public void _on_resource_cost_reduction_label_focus_exited()
	{
		Control info = (Control)resource_cost_reduction_label.GetChild(0);
		info.Hide();
	}

	// Misc

	public void _on_movement_speed_label_focus_entered()
	{
		Control info = (Control)movement_speed_label.GetChild(0);
		info.Show();
	}

	public void _on_movement_speed_label_focus_exited()
	{
		Control info = (Control)movement_speed_label.GetChild(0);
		info.Hide();
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

	public void _on_skills_focus_entered()
	{	
		Control info = (Control)skills_label.GetChild(0);
		info.Show();
	}

	public void _on_skills_focus_exited()
	{
		Control info = (Control)skills_label.GetChild(0);
		info.Hide();
	}

	public void _on_journal_quests_focus_entered()
	{	
		Control info = (Control)journal_quests_label.GetChild(0);
		info.Show();
	}

	public void _on_journal_quests_focus_exited()
	{
		Control info = (Control)journal_quests_label.GetChild(0);
		info.Hide();
	}

	public void _on_achievements_focus_entered()
	{	
		Control info = (Control)achievements_label.GetChild(0);
		info.Show();
	}

	public void _on_achievements_focus_exited()
	{
		Control info = (Control)achievements_label.GetChild(0);
		info.Hide();
	}

	public void _on_social_focus_entered()
	{	
		Control info = (Control)social_label.GetChild(0);
		info.Show();
	}

	public void _on_social_focus_exited()
	{
		Control info = (Control)social_label.GetChild(0);
		info.Hide();
	}

	public void _on_options_focus_entered()
	{	
		Control info = (Control)options_label.GetChild(0);
		info.Show();
	}

	public void _on_options_focus_exited()
	{
		Control info = (Control)options_label.GetChild(0);
		info.Hide();
	}


}
