using Godot;
using System;
using System.Text.RegularExpressions;

public partial class InventoryInfo : UI
{

	public UI this_ui;
	
	public bool stats_updated;

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

	// Base stats
	private StrengthStat strength_stat;
	private DexterityStat dexterity_stat;
	private IntellectStat intellect_stat;
	private VitalityStat vitality_stat;
	private StaminaStat stamina_stat;
	private WisdomStat wisdom_stat;
	private CharismaStat charisma_stat;

	// Character details
	private DamageStat damage_stat;
	private ResistanceStat resistance_stat;
	private RecoveryStat recovery_stat;
	private LevelStat level_stat;

	

	private Button rep_label;
	
	private Button sheet_label;

	// Offense
	private PhysicalMeleePowerStat physical_melee_power_stat;
	private SpellMeleePowerStat spell_melee_power_stat;
	private PhysicalRangedPowerStat physical_ranged_power_stat;
	private SpellRangedPowerStat spell_ranged_power_stat;
	private WisdomScalerStat wisdom_scaler_stat;
	private CriticalHitChanceStat critical_hit_chance_stat;
	private CriticalHitDamageStat critical_hit_damage_stat;
	private AttackSpeedStat attack_speed_stat;
	private AttackSpeedIncreaseStat attack_speed_increase_stat;
	private CooldownReductionStat cooldown_reduction_stat;
	
	// Defense
	private ArmorStat armor_stat;
	private PoiseStat poise_stat;
	private BlockAmountStat block_amount_stat;
	private RetaliationStat retaliation_stat;
	private PhysicalResistanceStat physical_resistance_stat;
	private PierceResistanceStat thrust_resistance_stat;
	private SlashResistanceStat slash_resistance_stat;
	private BluntResistanceStat blunt_resistance_stat;
	private BleedResistanceStat bleed_resistance_stat;
	private PoisonResistanceStat poison_resistance_stat;
	private CurseResistanceStat curse_resistance_stat;
	private SpellResistanceStat spell_resistance_stat;
	private FireResistanceStat fire_resistance_stat;
	private ColdResistanceStat cold_resistance_stat;
	private LightningResistanceStat lightning_resistance_stat;
	private HolyResistanceStat holy_resistance_stat;

	// Health
	private MaximumHealthStat maximum_health_stat;
	private HealthBonusStat health_bonus_stat;
	private HealthRegenerationStat health_regeneration_stat;
	private HealthRetaliationStat health_retaliation_stat;
	
	// Resource
	private MaximumResourceStat maximum_resource_stat;
	private ResourceRegenerationStat resource_regeneration_stat;
	private ResourceCostReductionStat resource_cost_reduction_stat;
	private PostureRegenerationStat posture_regeneration_stat;
	
	// Misc
	private MovementSpeedStat movement_speed_stat;
	


	private Button mats_label;
	private Button gold_label;

	private Button trash_label;

	private Button abilities_label;
	private Button journal_label;
	private Button achievements_label;
	private Button social_label;
	private Button options_label;



	// private CustomSignals _customSignals;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		stats_updated = false;
		head_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Head");
		shoulder_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Shoulder");
		neck_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Neck");
		chest_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Chest");
		gloves_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Gloves");
		bracers_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Bracers");
		belt_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Belt");
		ring1_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Ring1");
		ring2_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Ring2");
		mainhand_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/MainHand");
		offhand_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/OffHand");
		pants_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Pants");
		boots_slot = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/Armor/Boots");

		
		strength_stat = GetNode<StrengthStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength");
		dexterity_stat = GetNode<DexterityStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity");
		intellect_stat = GetNode<IntellectStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect");
		vitality_stat = GetNode<VitalityStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality");
		stamina_stat = GetNode<StaminaStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina");
		wisdom_stat = GetNode<WisdomStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom");
		charisma_stat = GetNode<CharismaStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma");

		// Stats details
		damage_stat = GetNode<DamageStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage");
		resistance_stat = GetNode<ResistanceStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance");
		recovery_stat = GetNode<RecoveryStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery");
		level_stat = GetNode<LevelStat>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level");


		// Offense
		physical_melee_power_stat = GetNode<PhysicalMeleePowerStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalMeleePower");
		spell_melee_power_stat = GetNode<SpellMeleePowerStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellMeleePower");
		physical_ranged_power_stat = GetNode<PhysicalRangedPowerStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalRangedPower");
		spell_ranged_power_stat = GetNode<SpellRangedPowerStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellRangedPower");
		wisdom_scaler_stat = GetNode<WisdomScalerStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/WisdomScaler");
		critical_hit_chance_stat = GetNode<CriticalHitChanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitChance");
		critical_hit_damage_stat = GetNode<CriticalHitDamageStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CriticalHitDamage");
		attack_speed_stat = GetNode<AttackSpeedStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeed");
		attack_speed_increase_stat = GetNode<AttackSpeedIncreaseStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/AttackSpeedIncrease");
		cooldown_reduction_stat = GetNode<CooldownReductionStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CooldownReduction");

		//Defense
		armor_stat = GetNode<ArmorStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Armor");
		poise_stat = GetNode<PoiseStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Poise");
		block_amount_stat = GetNode<BlockAmountStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BlockAmount");
		retaliation_stat = GetNode<RetaliationStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/Retaliation");
		physical_resistance_stat = GetNode<PhysicalResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PhysicalResistance");
		thrust_resistance_stat = GetNode<PierceResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ThrustResistance");
		slash_resistance_stat = GetNode<SlashResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SlashResistance");
		blunt_resistance_stat = GetNode<BluntResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BluntResistance");
		bleed_resistance_stat = GetNode<BleedResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/BleedResistance");
		poison_resistance_stat = GetNode<PoisonResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PoisonResistance");
		curse_resistance_stat = GetNode<CurseResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/CurseResistance");
		spell_resistance_stat = GetNode<SpellResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/SpellResistance");
		fire_resistance_stat = GetNode<FireResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/FireResistance");
		cold_resistance_stat = GetNode<ColdResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ColdResistance");
		lightning_resistance_stat = GetNode<LightningResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/LightningResistance");
		holy_resistance_stat = GetNode<HolyResistanceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HolyResistance");
		
		// Health
		maximum_health_stat = GetNode<MaximumHealthStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumHealth");
		health_bonus_stat = GetNode<HealthBonusStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthBonus");
		health_regeneration_stat = GetNode<HealthRegenerationStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRegeneration");
		health_retaliation_stat = GetNode<HealthRetaliationStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/HealthRetaliation");
		
		//Resource 
		maximum_resource_stat = GetNode<MaximumResourceStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MaximumResource");
		resource_regeneration_stat = GetNode<ResourceRegenerationStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceRegeneration");
		resource_cost_reduction_stat = GetNode<ResourceCostReductionStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/ResourceCostReduction");
		posture_regeneration_stat = GetNode<PostureRegenerationStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/PostureRegeneration");
	
		//Misc
		movement_speed_stat = GetNode<MovementSpeedStat>("FullInventory/CharacterSheetDepth/ScrollContainer/StatsSheet/VBoxContainer/MovementSpeed");
		
		

		rep_label = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/RepLabel");
		sheet_label = GetNode<Button>("FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/SheetLabel");

		// Character possessions
		
		gold_label = GetNode<Button>("FullInventory/CharacterInventory/MatsGold/GoldLabel");

		mats_label = GetNode<Button>("FullInventory/CharacterInventory/MatsGold/MatsLabel");

		// Misc
		trash_label = GetNode<Button>("FullInventory/CharacterInventory/BagSlots/TrashLabel");

		abilities_label = GetNode<Button>("FullInventory/CharacterInventory/BottomButtons/AbilitiesLabel");
		journal_label = GetNode<Button>("FullInventory/CharacterInventory/BottomButtons/JournalLabel");
		achievements_label = GetNode<Button>("FullInventory/CharacterInventory/BottomButtons/AchievementsLabel");
		social_label = GetNode<Button>("FullInventory/CharacterInventory/BottomButtons/SocialLabel");
		options_label = GetNode<Button>("FullInventory/CharacterInventory/BottomButtons/OptionsLabel");
		

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
		// _customSignals.PlayerInfo += HandlePlayerInfo;
		// _customSignals.SendStats += HandleSendStats;
	}

	public void GetUIInfo(UI i)
	{
		this_ui = i;
	}

	public void SetPlayerStats()
	{
		// GD.Print("receiving stats");
		// GD.Print("Strength received: " + this_ui.this_player.strength);
		// level_UI = this_ui.this_player.level.ToString();
		// strength_UI = this_ui.this_player.strength.ToString();
		// dexterity_UI = this_ui.this_player.dexterity.ToString();
		// intellect_UI = this_ui.this_player.intellect.ToString();
		// vitality_UI = this_ui.this_player.vitality.ToString();
		// stamina_UI = this_ui.this_player.stamina.ToString();
		// wisdom_UI = this_ui.this_player.wisdom.ToString();
		// charisma_UI = this_ui.this_player.charisma.ToString();
		

		// // Stat details
		// damage_UI = this_ui.this_player.total_dps.ToString();
		// resistance_UI = this_ui.this_player.resistance.ToString();
		// recovery_UI = this_ui.this_player.recovery.ToString();
		// // Sheet

		// // Offense

		// physical_melee_dps_UI = this_ui.this_player.physical_melee_dps.ToString();
		// spell_melee_dps_UI = this_ui.this_player.spell_melee_dps.ToString();
		// physical_ranged_dps_UI = this_ui.this_player.physical_ranged_dps.ToString();
		// spell_ranged_dps_UI = this_ui.this_player.spell_ranged_dps.ToString();

		// physical_melee_power_UI = this_ui.this_player.physical_melee_power.ToString();
		// spell_melee_power_UI = this_ui.this_player.spell_melee_power.ToString();
		// physical_ranged_power_UI = this_ui.this_player.physical_ranged_power.ToString();
		// spell_ranged_power_UI = this_ui.this_player.spell_ranged_power.ToString();
		// wisdom_scaler_UI = this_ui.this_player.wisdom_scaler.ToString();
		// slash_damage_UI = this_ui.this_player.slash_damage.ToString();
		// thrust_damage_UI = this_ui.this_player.pierce_damage.ToString();
		// blunt_damage_UI = this_ui.this_player.blunt_damage.ToString();
		// bleed_damage_UI = this_ui.this_player.bleed_damage.ToString();
		// poison_damage_UI = this_ui.this_player.poison_damage.ToString();
		// fire_damage_UI = this_ui.this_player.fire_damage.ToString();
		// cold_damage_UI = this_ui.this_player.cold_damage.ToString();
		// lightning_damage_UI = this_ui.this_player.lightning_damage.ToString();
		// holy_damage_UI = this_ui.this_player.holy_damage.ToString();
		// critical_hit_chance_UI = this_ui.this_player.critical_hit_chance.ToString();
		// critical_hit_damage_UI = this_ui.this_player.critical_hit_damage.ToString();
		// attack_speed_UI = this_ui.this_player.aps.ToString();
		// attack_speed_increase_UI = this_ui.this_player.attack_speed_increase.ToString();
		// cool_down_reduction_UI = this_ui.this_player.cooldown_reduction.ToString();
		// posture_damage_UI = this_ui.this_player.posture_damage.ToString();
		

		// // Defense

		// armor_UI = this_ui.this_player.armor.ToString();
		// poise_UI = this_ui.this_player.poise.ToString();
		// block_amount_UI = this_ui.this_player.block_amount.ToString();
		// retaliation_UI = this_ui.this_player.retaliation.ToString();
		// physical_resistance_UI = this_ui.this_player.physical_resistance.ToString();
		// thrust_resistance_UI = this_ui.this_player.pierce_damage.ToString();
		// slash_resistance_UI = this_ui.this_player.slash_resistance.ToString();
		// blunt_resistance_UI = this_ui.this_player.blunt_resistance.ToString();
		// bleed_resistance_UI = this_ui.this_player.bleed_resistance.ToString();
		// poison_resistance_UI = this_ui.this_player.poison_resistance.ToString();
		// curse_resistance_UI = this_ui.this_player.curse_resistance.ToString();
		// spell_resistance_UI = this_ui.this_player.spell_resistance.ToString();
		// fire_resistance_UI = this_ui.this_player.fire_resistance.ToString();
		// cold_resistance_UI = this_ui.this_player.cold_resistance.ToString();
		// lightning_resistance_UI = this_ui.this_player.lightning_resistance.ToString();
		// holy_resistance_UI = this_ui.this_player.holy_resistance.ToString();

		// // Health

		// maximum_health_UI = this_ui.this_player.maximum_health.ToString();
		// health_bonus_UI = this_ui.this_player.health_bonus.ToString();
		// health_regen_UI = this_ui.this_player.health_regen.ToString();
		// posture_regen_UI = this_ui.this_player.posture_regen.ToString();
		// health_on_retaliate_UI = this_ui.this_player.health_on_retaliate.ToString();

		// // Resources

		// maximum_resource_UI = this_ui.this_player.maximum_resource.ToString();
		// resource_regen_UI = this_ui.this_player.resource_regen.ToString();
		// resource_cost_reduction_UI = this_ui.this_player.resource_cost_reduction.ToString();

		// // Misc

		// movement_speed_UI = this_ui.this_player.movement_speed.ToString();

		// // Possessions
		// gold_UI = this_ui.this_player.maximum_gold.ToString();
		// CharacterInfo();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(this_ui != null && !stats_updated)
		{
			
			SetPlayerStats();
			stats_updated = true;
			
		}	

	}

	private void CharacterInfo()
	{
		
		// Base Stats
		// strength_stat.GetStatInfo(strength_UI);
		// dexterity_stat.GetStatInfo(dexterity_UI);
		// intellect_stat.GetStatInfo(intellect_UI);
		// vitality_stat.GetStatInfo(vitality_UI);
		// stamina_stat.GetStatInfo(stamina_UI);
		// wisdom_stat.GetStatInfo(wisdom_UI);
		// charisma_stat.GetStatInfo(charisma_UI);
	
		// // Stats Summary
		// damage_stat.GetStatInfo(damage_UI, physical_melee_dps_UI, spell_melee_dps_UI, physical_ranged_dps_UI, spell_ranged_dps_UI);
		// resistance_stat.GetStatInfo(resistance_UI);
		// recovery_stat.GetStatInfo(recovery_UI);

		// // Sheet
		// // Offense
		// physical_melee_power_stat.GetStatInfo(physical_melee_power_UI);
		// spell_melee_power_stat.GetStatInfo(spell_melee_power_UI);
		// physical_ranged_power_stat.GetStatInfo(physical_ranged_power_UI);
		// spell_ranged_power_stat.GetStatInfo(spell_melee_power_UI);
		// wisdom_scaler_stat.GetStatInfo(wisdom_scaler_UI);
		// critical_hit_chance_stat.GetStatInfo(critical_hit_chance_UI);
		// critical_hit_damage_stat.GetStatInfo(critical_hit_damage_UI);
		// attack_speed_stat.GetStatInfo(attack_speed_UI);
		// attack_speed_increase_stat.GetStatInfo(attack_speed_increase_UI);
		// cooldown_reduction_stat.GetStatInfo(cool_down_reduction_UI);

		// // Defense
		// armor_stat.GetStatInfo(armor_UI);
		// poise_stat.GetStatInfo(poise_UI);
		// block_amount_stat.GetStatInfo(block_amount_UI);
		// retaliation_stat.GetStatInfo(retaliation_UI);
		// physical_resistance_stat.GetStatInfo(physical_resistance_UI);
		// thrust_resistance_stat.GetStatInfo(thrust_resistance_UI);
		// slash_resistance_stat.GetStatInfo(slash_resistance_UI);
		// blunt_resistance_stat.GetStatInfo(blunt_resistance_UI);
		// bleed_resistance_stat.GetStatInfo(bleed_resistance_UI);
		// poison_resistance_stat.GetStatInfo(poison_resistance_UI);
		// curse_resistance_stat.GetStatInfo(curse_resistance_UI);
		// spell_resistance_stat.GetStatInfo(spell_resistance_UI);
		// fire_resistance_stat.GetStatInfo(spell_resistance_UI);
		// cold_resistance_stat.GetStatInfo(cold_resistance_UI);
		// lightning_resistance_stat.GetStatInfo(lightning_resistance_UI);
		// holy_resistance_stat.GetStatInfo(holy_resistance_UI);

		// // Health
		// maximum_health_stat.GetStatInfo(maximum_health_UI);
		// health_bonus_stat.GetStatInfo(health_bonus_UI);
		// health_regeneration_stat.GetStatInfo(health_regen_UI);
		// health_retaliation_stat.GetStatInfo(health_on_retaliate_UI);
		
		// // Resource
		// maximum_resource_stat.GetStatInfo(maximum_resource_UI);
		// resource_regeneration_stat.GetStatInfo(resource_regen_UI);
		// resource_cost_reduction_stat.GetStatInfo(resource_cost_reduction_UI);
		// posture_regeneration_stat.GetStatInfo(posture_regen_UI);

		// // Misc
		// movement_speed_stat.GetStatInfo(movement_speed_UI);

		
		// level_stat.GetStatInfo(level_UI);
		

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
		// GD.Print("over_trash");
		// this_ui.over_trash = true;
		// GD.Print(this_ui.over_trash);
	}

	public void _on_trash_area_2d_area_exited(Area2D area)
	{
		over_trash = false;
	}

	// private void HandlePlayerInfo(Player player)
	// {
	// 	this_player = player;
	// }

	// private void HandleSendStats(
	// 							int level, int strength, int dexterity, int intellect, int vitality, int stamina, int wisdom, int charisma, float total_dps,

	// 							float physical_melee_dps, float spell_melee_dps, float physical_ranged_dps, float spell_ranged_dps, float physical_melee_power,

	// 							float spell_melee_power, float physical_ranged_power, float spell_ranged_power, float wisdom_scaler, float physical_melee_power_mod,

	// 							float physical_ranged_power_mod, float spell_ranged_power_mod, float power_mod_avg, int damage_bonus, float combined_damage, float base_aps,

	// 							float aps_modifiers, float aps, float base_dps, float skill_mod, float crit_mod, float slash_damage, float thrust_damage,

	// 							float blunt_damage, float bleed_damage, float poison_damage, float fire_damage, float cold_damage, float lightning_damage, float holy_damage,

	// 							float critical_hit_chance, float critical_hit_damage, float attack_speed_increase, float cool_down_reduction, float posture_damage, int armor,

	// 							int poise, int block_amount, int retaliation, int physical_resistance, int thrust_resistance, int slash_resistance, int blunt_resistance,

	// 							int bleed_resistance, int poison_resistance, int curse_resistance, int spell_resistance, int fire_resistance, int cold_resistance,

	// 							int lightning_resistance, int holy_resistance, float maximum_health, float health_bonus, float health_regen, float health_regen_bonus, float maximum_posture, float posture_regen,

	// 							float posture_regen_bonus,  float health_on_retaliate, float resistance, float maximum_resource, float resource_regen, float resource_cost_reduction,
								
	// 							float recovery, float movement_speed, int maximum_gold
	// 							)
	// 							{
	// 								GD.Print("receiving stats");
	// 								GD.Print("Strength received: " + strength);
	// 								level_UI = level.ToString();
	// 								strength_UI = strength.ToString();
	// 								dexterity_UI = dexterity.ToString();
	// 								intellect_UI = intellect.ToString();
	// 								vitality_UI = vitality.ToString();
	// 								stamina_UI = stamina.ToString();
	// 								wisdom_UI = wisdom.ToString();
	// 								charisma_UI = charisma.ToString();
									

	// 								// Stat details
	// 								damage_UI = total_dps.ToString();
	// 								resistance_UI = resistance.ToString();
	// 								recovery_UI = recovery.ToString();
	// 								// Sheet

	// 								// Offense

	// 								physical_melee_dps_UI = physical_melee_dps.ToString();
	// 								spell_melee_dps_UI = spell_melee_dps.ToString();
	// 								physical_ranged_dps_UI = physical_ranged_dps.ToString();
	// 								spell_ranged_dps_UI = spell_ranged_dps.ToString();

	// 								physical_melee_power_UI = physical_melee_power.ToString();
	// 								spell_melee_power_UI = spell_melee_power.ToString();
	// 								physical_ranged_power_UI = physical_ranged_power.ToString();
	// 								spell_ranged_power_UI = spell_ranged_power.ToString();
	// 								wisdom_scaler_UI = wisdom_scaler.ToString();
	// 								slash_damage_UI = slash_damage.ToString();
	// 								thrust_damage_UI = thrust_damage.ToString();
	// 								blunt_damage_UI = blunt_damage.ToString();
	// 								bleed_damage_UI = bleed_damage.ToString();
	// 								poison_damage_UI = poison_damage.ToString();
	// 								fire_damage_UI = fire_damage.ToString();
	// 								cold_damage_UI = cold_damage.ToString();
	// 								lightning_damage_UI = lightning_damage.ToString();
	// 								holy_damage_UI = holy_damage.ToString();
	// 								critical_hit_chance_UI = critical_hit_chance.ToString();
	// 								critical_hit_damage_UI = critical_hit_damage.ToString();
	// 								attack_speed_UI = aps.ToString();
	// 								attack_speed_increase_UI = attack_speed_increase.ToString();
	// 								cool_down_reduction_UI = cool_down_reduction.ToString();
	// 								posture_damage_UI = posture_damage.ToString();
									

	// 								// Defense

	// 								armor_UI = armor.ToString();
	// 								poise_UI = poise.ToString();
	// 								block_amount_UI = block_amount.ToString();
	// 								retaliation_UI = retaliation.ToString();
	// 								physical_resistance_UI = physical_resistance.ToString();
	// 								thrust_resistance_UI = thrust_resistance.ToString();
	// 								slash_resistance_UI = slash_resistance.ToString();
	// 								blunt_resistance_UI = blunt_resistance.ToString();
	// 								bleed_resistance_UI = bleed_resistance.ToString();
	// 								poison_resistance_UI = poison_resistance.ToString();
	// 								curse_resistance_UI = curse_resistance.ToString();
	// 								spell_resistance_UI = spell_resistance.ToString();
	// 								fire_resistance_UI = fire_resistance.ToString();
	// 								cold_resistance_UI = cold_resistance.ToString();
	// 								lightning_resistance_UI = lightning_resistance.ToString();
	// 								holy_resistance_UI = holy_resistance.ToString();

	// 								// Health

	// 								maximum_health_UI = maximum_health.ToString();
	// 								health_bonus_UI = health_bonus.ToString();
	// 								health_regen_UI = health_regen.ToString();
	// 								posture_regen_UI = posture_regen.ToString();
	// 								health_on_retaliate_UI = health_on_retaliate.ToString();

	// 								// Resources

	// 								maximum_resource_UI = maximum_resource.ToString();
	// 								resource_regen_UI = resource_regen.ToString();
	// 								resource_cost_reduction_UI = resource_cost_reduction.ToString();

	// 								// Misc

	// 								movement_speed_UI = movement_speed.ToString();

	// 								// Possessions
	// 								gold_UI = maximum_gold.ToString();
	// 								CharacterInfo();
		
	// 							}

}
