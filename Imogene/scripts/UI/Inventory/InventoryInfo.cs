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
	private string physical_melee_power_info_text =  "";

	private Button spell_melee_power_label;
	private Label spell_melee_power_value;
	private RichTextLabel spell_melee_power_info;
	private string spell_melee_power_info_text =  "";

	private Button physical_ranged_power_label;
	private Label physical_ranged_power_value;
	private RichTextLabel physical_ranged_power_info;
	private string physical_ranged_power_info_text =  "";

	private Button spell_ranged_power_label;
	private Label spell_ranged_power_value;
	private RichTextLabel spell_ranged_power_info;
	private string spell_ranged_power_info_text =  "";
	
	private Button thrust_label;
	private Label thrust_value;
	private RichTextLabel thrust_info;
	private string thrust_info_text =  "";

	private Button slash_label;
	private Label slash_value;
	private RichTextLabel slash_info;
	private string slash_info_text =  "";

	private Button blunt_label;
	private Label blunt_value;
	private RichTextLabel blunt_info;
	private string blunt_info_text =  "";

	private Button bleed_label;
	private Label bleed_value;
	private RichTextLabel bleed_info;
	private string bleed_info_text =  "";

	private Button poison_label;
	private Label poison_value;
	private RichTextLabel poison_info;
	private string poison_info_text =  "";

	private Button fire_label;
	private Label fire_value;
	private RichTextLabel fire_info;
	private string fire_info_text =  "";

	private Button cold_label;
	private Label cold_value;
	private RichTextLabel cold_info;
	private string cold_info_text =  "";

	private Button lightning_label;
	private Label lightning_value;
	private RichTextLabel lightning_info;
	private string lightning_info_text =  "";

	private Button holy_label;
	private Label holy_value;
	private RichTextLabel holy_info;
	private string holy_info_text =  "";

	private Button critical_hit_chance_label;
	private Label critical_hit_chance_value;
	private RichTextLabel critical_hit_chance_info;
	private string critical_hit_chance_text =  "";

	private Button critical_hit_damage_label;
	private Label critical_hit_damage_value;
	private RichTextLabel critical_hit_damage_info;
	private string critical_hit_damage_text =  "";

	private Button attack_speed_label;
	private Label attack_speed_value;
	private RichTextLabel attack_speed_info;
	private string attack_speed_text =  "";

	private Button attack_speed_increase_label;
	private Label attack_speed_increase_value;
	private RichTextLabel attack_speed_increase_info;
	private string attack_speed_increase_text =  "";

	private Button cooldown_reduction_label;
	private Label cooldown_reduction_value;
	private RichTextLabel cooldown_reduction_info;
	private string cooldown_reduction_text =  "";

	private Button posture_damage_label;
	private Label posture_damage_value;
	private RichTextLabel posture_damage_info;
	private string posture_damage_text =  "";

	// Defense
	private Button armor_label;
	private Label armor_value;
	private RichTextLabel armor_info;
	private string armor_info_text =  "";

	private Button poise_label;
	private Label poise_value;
	private RichTextLabel poise_info;
	private string poise_info_text =  "";

	private Button block_amount_label;
	private Label block_amount_value;
	private RichTextLabel block_amount_info;
	private string block_amount_info_text =  "";

	private Button retaliation_label;
	private Label retaliation_value;
	private RichTextLabel retaliation_info;
	private string retaliation_info_text =  "";

	private Button physical_resistance_label;
	private Label physical_resistance_value;
	private RichTextLabel physical_resistance_info;
	private string physical_resistance_info_text =  "";

	private Button thrust_resistance_label;
	private Label thrust_resistance_value;
	private RichTextLabel thrust_resistance_info;
	private string thrust_resistance_info_text =  "";

	private Button slash_resistance_label;
	private Label slash_resistance_value;
	private RichTextLabel slash_resistance_info;
	private string slash_resistance_info_text =  "";

	private Button blunt_resistance_label;
	private Label blunt_resistance_value;
	private RichTextLabel blunt_resistance_info;
	private string blunt_resistance_info_text =  "";

	private Button bleed_resistance_label;
	private Label bleed_resistance_value;
	private RichTextLabel bleed_resistance_info;
	private string bleed_resistance_info_text =  "";

	private Button poison_resistance_label;
	private Label poison_resistance_value;
	private RichTextLabel poison_resistance_info;
	private string poison_resistance_info_text =  "";

	private Button curse_resistance_label;
	private Label curse_resistance_value;
	private RichTextLabel curse_resistance_info;
	private string curse_resistance_info_text =  "";

	private Button spell_resistance_label;
	private Label spell_resistance_value;
	private RichTextLabel spell_resistance_info;
	private string spell_resistance_info_text =  "";

	private Button fire_resistance_label;
	private Label fire_resistance_value;
	private RichTextLabel fire_resistance_info;
	private string fire_resistance_info_text =  "";

	private Button cold_resistance_label;
	private Label cold_resistance_value;
	private RichTextLabel cold_resistance_info;
	private string cold_resistance_info_text =  "";

	private Button lightning_resistance_label;
	private Label lightning_resistance_value;
	private RichTextLabel lightning_resistance_info;
	private string lighting_resistance_info_text =  "";

	private Button holy_resistance_label;
	private Label holy_resistance_value;
	private RichTextLabel holy_resistance_info;
	private string holy_resistance_info_text =  "";

	// Health
	private Button maximum_health_label;
	private Label maximum_health_value;
	private RichTextLabel maximum_health_info;
	private string maximum_health_info_text =  "";

	private Button health_bonus_label;
	private Label health_bonus_value;
	private RichTextLabel health_bonus_info;
	private string health_bonus_info_text =  "";

	private Button health_regen_label;
	private Label health_regen_value;
	private RichTextLabel health_regen_info;
	private string health_regen_info_text =  "";

	private Button health_retaliation_label;
	private Label health_retaliation_value;
	private RichTextLabel health_retaliation_info;
	private string health_retaliation_info_text =  "";

	// Resource
	private Button maximum_resource_label;
	private Label maximum_resource_value;
	private RichTextLabel maximum_resource_info;
	private string maximum_resource_info_text =  "";

	private Button resource_regen_label;
	private Label resource_regen_value;
	private RichTextLabel resource_regen_info;
	private string resource_regen_info_text =  "";

	private Button resource_cost_reduction_label;
	private Label resource_cost_reduction_value;
	private RichTextLabel resource_cost_reduction_info;
	private string resource_cost_reduction_info_text =  "";

	// Misc

	private Button movement_speed_label;
	private Label movement_speed_value;
	private RichTextLabel movement_speed_info;
	private string movement_speed_info_text =  "";


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
 		
		thrust_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Thrust/ThrustLabel");
		thrust_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Thrust/Value");
		thrust_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Thrust/ThrustLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		blunt_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Blunt/BluntLabel");
		blunt_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Blunt/Value");
		blunt_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Blunt/BluntLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		bleed_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Bleed/BleedLabel");
		bleed_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Bleed/Value");
		bleed_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Bleed/BleedLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		poison_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poison/PoisonLabel");
		poison_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poison/Value");
		poison_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poison/PoisonLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		fire_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Fire/FireLabel");
		fire_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Fire/Value");
		fire_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Fire/FireLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		cold_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Cold/ColdLabel");
		cold_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Cold/Value");
		cold_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Cold/ColdLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		lightning_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Lightning/LightningLabel");
		lightning_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Lightning/Value");
		lightning_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Lightning/LightningLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

		holy_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Holy/HolyLabel");
		holy_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Holy/Value");
		holy_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Holy/HolyLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

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

		health_regen_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/HealthRegenerationLabel");
		health_regen_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/Value");
		health_regen_info = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration/HealthRegenerationLabel/Info/MarginContainer/PanelContainer/RichTextLabel");

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
		// Possessions
		string gold = this_player.gold.ToString();
		
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

		damage_value.Text = damage;
		damage_info.Text = string.Format(damage_info_text, damage); // 1 variable(s)

		resistance_value.Text = "";
		resistance_info.Text = string.Format(resistance_info_text, 0); // 1 variable(s)

		recovery_value.Text = "";
		recovery_info.Text = string.Format(recovery_info_text, 0); // 1 variable(s)

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

	public void _on_physical_melee_power_label_focus_entered()
	{
		// Control info = (Control)recovery_label.GetChild(0);
		// info.Show();
	}

	public void _on_physical_melee_power_label_focus_exited()
	{
		// Control info = (Control)recovery_label.GetChild(0);
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
