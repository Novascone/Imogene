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
	private RichTextLabel strength_info_label;
	private string strength_info_text = " Strength {0} \n * Primary stat for melee damage \n * Increases damage by {1} \n * Increases health by {2} ";

	private Button dexterity_label;
	private Label dexterity_value;
	private RichTextLabel dexterity_info_label;
	private string dexterity_info_text = " Dexterity {0} \n * Primary stat for melee \n * Calculated by physical ranged  and critical damage \n * Increases damage by {1} \n * Increases critical chance by {2} * Increases critical damage by {3}";

	private Button intellect_label;
	private Label intellect_value;
	private RichTextLabel intellect_info_label;
	private string intellect_info_text = " Intellect {0} \n * main stat for spell damage \n * Increases spell damage by {1} \n * Increases spell hit chance by {2}";

	private Button vitality_label;
	private Label vitality_value;
	private RichTextLabel vitality_info_label;
	private string vitality_info_text = " Vitality {0} \n * Primary stat for health \n * Increases health points by {1}";

	private Button stamina_label;
	private Label stamina_value;
	private RichTextLabel stamina_info_label;
	private string stamina_info_text = " Stamina {0} \n * Primary stat for resource and regeneration \n * Increases total resource by {1} \n * Increases health and resource regeneration by {2} \n * Increases health by {3}";

	private Button wisdom_label;
	private Label wisdom_value;
	private RichTextLabel wisdom_info_label;
	private string wisdom_info_text = " Wisdom {0} \n * Primary stat for hit and interaction \n * Increases hit chance by {1}";

	private Button charisma_label;
	private Label charisma_value;
	private RichTextLabel charisma_info_label;
	private string charisma_info_text = "  Charisma {0} \n * Primary stat for character interaction \n * Increases special interactions";

	// Character details
	private Button damage_label;
	private Label damage_value;
	private RichTextLabel damage_info_label;
	private string damage_info_text = "  Damage {0} \n * Total damage per second done by character \n * Combination of physical attack power (melee and ranged), spell attack power(melee and ranged), \n    weapon damage, attack speed, critical hit chance, and critical hit damage";

	private Button resistance_label;
	private Label resistance_value;
	private RichTextLabel resistance_info_label;
	private string resistance_info_text = " Resistance {0} \n * Total damage the character can resist \n * Calculated by health, armor, resistances" ;

	private Button recovery_label;
	private Label recovery_value;
	private RichTextLabel recovery_info_label;
	private string recovery_info_text =  " Recovery {0} \n * How fast the character regenerates health, resource and posture \n * Calculated by stamina ";

	private Button level_label;
	private Label level_value;
	private RichTextLabel level_info_label;
	private string level_info_text =  " Level {0} \n * Level of character";

	private Button rep_label;
	
	private Button sheet_label;
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
		strength_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Strength/StrengthLabel/Info/MarginContainer/PanelContainer/Label");

		dexterity_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/DexterityLabel");
		dexterity_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/Value");
		dexterity_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Dexterity/DexterityLabel/Info/MarginContainer/PanelContainer/Label");

		intellect_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/IntellectLabel");
		intellect_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/Value");
		intellect_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Intellect/IntellectLabel/Info/MarginContainer/PanelContainer/Label");

		vitality_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/VitalityLabel");
		vitality_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/Value");
		vitality_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Vitality/VitalityLabel/Info/MarginContainer/PanelContainer/Label");

		stamina_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/StaminaLabel");
		stamina_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/Value");
		stamina_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Stamina/StaminaLabel/Info/MarginContainer/PanelContainer/Label");

		wisdom_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/WisdomLabel");
		wisdom_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/Value");
		wisdom_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Wisdom/WisdomLabel/Info/MarginContainer/PanelContainer/Label");

		charisma_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/CharismaLabel");
		charisma_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/Value");
		charisma_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/BaseStats/Charisma/CharismaLabel/Info/MarginContainer/PanelContainer/Label");

		// Stats details
		damage_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/DamageLabel");
		damage_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/Value");
		damage_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Damage/DamageLabel/Info/MarginContainer/PanelContainer/Label");

		resistance_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/ResistanceLabel");
		resistance_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/Value");
		resistance_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Resistance/ResistanceLabel/Info/MarginContainer/PanelContainer/Label");

		recovery_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/RecoveryLabel");
		recovery_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/Value");
		recovery_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Details/Recovery/RecoveryLabel/Info/MarginContainer/PanelContainer/Label");

		level_label = GetNode<Button>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/LevelLabel");
		level_value = GetNode<Label>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/Value");
		level_info_label = GetNode<RichTextLabel>("CharacterInventoryContainer/FullInventory/CharacterInventory/CharacterSheet/CharacterOutline/StatsOutline/Level/LevelLabel/Info/MarginContainer/PanelContainer/Label");


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
		strength_info_label.Text = string.Format(strength_info_text, strength, 0, 0); // 3 variable(s)

		dexterity_value.Text = dexterity;
		dexterity_info_label.Text = string.Format(dexterity_info_text, dexterity, 0, 0, 0); // 4 variable(s)

		intellect_value.Text = intellect;
		intellect_info_label.Text = string.Format(intellect_info_text, intellect, 0, 0); // 3 variable(s)

		vitality_value.Text = vitality;
		vitality_info_label.Text = string.Format(vitality_info_text, vitality, 0, 0); // 2 variable(s)

		stamina_value.Text = stamina;
		stamina_info_label.Text = string.Format(stamina_info_text, stamina, 0, 0, 0); // 4 variable(s)

		wisdom_value.Text = wisdom;
		wisdom_info_label.Text = string.Format(wisdom_info_text, wisdom, 0); // 2 variable(s)

		charisma_value.Text = charisma;
		charisma_info_label.Text = string.Format(charisma_info_text, charisma); // 1 variable(s)

		damage_value.Text = damage;
		damage_info_label.Text = string.Format(damage_info_text, damage); // 1 variable(s)

		resistance_value.Text = "";
		resistance_info_label.Text = string.Format(resistance_info_text, 0); // 1 variable(s)

		recovery_value.Text = "";
		recovery_info_label.Text = string.Format(recovery_info_text, 0); // 1 variable(s)

		level_value.Text = level;
		level_info_label.Text = string.Format(level_info_text, level); // 1 variable(s)

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
