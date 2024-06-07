using Godot;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;

public partial class CustomSignals : Node
{

	// Player to Ability / UI
	[Signal]
	public delegate void PlayerInfoEventHandler(Player s); // Send the player stats from the player to the UI
	[Signal]
	public delegate void AnimationFinishedEventHandler(string animation); // Send the player stats from the player to the UI
	[Signal]
	public delegate void AbilityAssignedEventHandler(string ability, string button_name, Texture2D icon); // Tells the player which ability has been assigned to which action button. Tells the HUD the same thing and send the icon of the ability as well.
	[Signal]
	public delegate void AbilityRemovedEventHandler(string ability, string button_removed); // Tells the player which ability has been assigned to which action button. Tells the HUD the same thing and send the icon of the ability as well.



	// Player to UI
	
	[Signal]
	public delegate void AvailableAbilitiesEventHandler(AbilityResource ability); // Tells the UI what abilities the player has available
	[Signal]
	public delegate void AddToAbilitySelectionEventHandler(string name, string type, Texture2D icon); // Adds abilities to their category in the UI
	[Signal]
	public delegate void LCrossPrimaryOrSecondaryEventHandler(bool l_cross_primary_selected_signal); // Tells the UI if the player has selected the primary or secondary LCross
	[Signal]
	public delegate void RCrossPrimaryOrSecondaryEventHandler(bool r_cross_primary_selected_signal); // Tells the UI if the player has selected the primary or secondary RCross
	[Signal]
	public delegate void UIHealthEventHandler(int amount); // Gives the UI the players starting health 																				*** NEEDS TO BE UPDATED ***
	[Signal]
	public delegate void UIResourceEventHandler(int amount); // Gives the UI the players starting resource																			*** NEEDS TO BE UPDATED ***
	[Signal]
	public delegate void UIHealthUpdateEventHandler(int amount); // Updates the UI with the players new health 																		*** NEEDS TO BE UPDATED ***
	[Signal]
	public delegate void UIResourceUpdateEventHandler(int amount); // Updates the UI with the players new resource 																	*** NEEDS TO BE UPDATED ***
	[Signal]
	public delegate void InteractEventHandler(Area3D area, bool in_interact_area, bool interacting); // Tells the UI if the player has entered an interact area
	[Signal]
	public delegate void WhichConsumableEventHandler(int consumable); // Tells the UI if the player has entered an interact area
	[Signal]
	public delegate void SendStatsEventHandler(
												int level, int strength, int dexterity, int intellect, int vitality, int stamina, int wisdom, int charisma,

												float total_dps, float physical_melee_dps, float spell_melee_dps, float physical_ranged_dps, float spell_ranged_dps,

												float physical_melee_power, float spell_melee_power, float physical_ranged_power, float spell_ranged_power,

												float wisdom_scaler, float physical_melee_power_mod, float physical_ranged_power_mod, float spell_ranged_power_mod, float power_mod_avg, 

												int damage_bonus, float combined_damage, float base_aps, float aps_modifiers, float aps, float aps_mod, float base_dps, float skill_mod,

												float crit_mod, float slash_damage, float thrust_damage, float blunt_damage, float bleed_damage, float poison_damage, float fire_damage,

												float cold_damage, float lightning_damage, float holy_damage, float critical_hit_chance, float critical_hit_damage, float attack_speed_increase,

												float cool_down_reduction, float posture_damage, int armor, int poise, int block_amount, int retaliation, int physical_resistance, int thrust_resistance,

												int slash_resistance, int blunt_resistance, int bleed_resistance, int poison_resistance, int curse_resistance, int spell_resistance, int fire_resistance,

												int cold_resistance, int lightning_resistance, int holy_resistance, float maximum_health, float health_bonus, float health_regen, float health_on_retaliate,

												float maximum_resource, float resource_regen, float resource_cost_reduction, float movement_speed, int maixmum_gold
												);
	



	// Player to object
	[Signal]
	public delegate void TargetingEventHandler(bool targeting, Vector3 position); // Tells the targeting Icon to show or not show
	[Signal]
	public delegate void PlayerPositionEventHandler(Vector3 position); // Tells an object where the player is



	//Inventory to Player
	[Signal]
	public delegate void UIPreventingMovementEventHandler(bool ui_preventing_movement); // Tells the player if the UI is preventing movement
	[Signal]
	public delegate void EquipableInfoEventHandler(ArmsResource item); // Gives the player information about an equipable item in their inventory
	[Signal]
	public delegate void ConsumableInfoEventHandler(ConsumableResource item); // Gives the player information about a consumable item in their inventory
	[Signal]
	public delegate void ItemInfoEventHandler(Item item); // Gives the player information about a generic item in their inventory



	// UI to Player
	[Signal]
	public delegate void RemoveEquippedEventHandler(); // Tells the player to remove what is equipped (only head slot right now)
	[Signal]
	public delegate void EquipConsumableEventHandler(ConsumableResource item, int consumable_slot); // Tells the player which consumable has been equipped



	

	// UI to UI
	[Signal]
	public delegate void AbilityUISecondaryOpenEventHandler(bool secondary_open); // Tells the UI if part of the ability UI is open
	[Signal]
	public delegate void AbilitySelectedEventHandler(AbilityButton selected_ability, Button cross_button); // Tells the UI which ability has been selected selected_ability is the ability selected via the AbilityButton Class, and cross button is the action button it is assigned to
	[Signal]
	public delegate void ButtonNameEventHandler(Button cross_button, string cross_button_name); // Tells Ability category the reference of the action button that is wanting to be assigned and its name
	[Signal]
	public delegate void ButtonToBeAssignedEventHandler(Button cross_button, Button representative_button); // Tells the Ability button which ability it should be. Also allows the setting of the preview button the user sees when assigning an ability
	[Signal]
	public delegate void OverSlotEventHandler(string slot); // Tells the UI which slot the player is currently over
	[Signal]
	public delegate void AbilityAcceptEventHandler(); // Tells the UI that the ability assignment has been accepted
	[Signal]
	public delegate void AbilityCancelEventHandler(); // Tells the UI that the ability assignment has been accepted
	[Signal]
	public delegate void HideCursorEventHandler(); // Tells the UI to close cursor
	[Signal]
	public delegate void CurrentAbilityBoundOnCrossButtonEventHandler(Texture2D icon); // Tells the Ability category which button is currently bound
	[Signal]
	public delegate void SendConsumableIconEventHandler(Texture2D icon); // Tells the UI which icon to assign the consumable
	


	// Object to player
	[Signal]
	public delegate void EnemyPositionEventHandler(Vector3 position);  // Tells the player where the enemy is

	// UI to object
	[Signal]
	public delegate void ZoomCameraEventHandler(bool zoom);  // Tells the camera to zoom



	// Object to object
	[Signal]
	public delegate void CameraPositionEventHandler(Vector3 position); // Tells a object where the camera is



	// Player to misc 																																								*** NOT SURE WHAT THIS IS FOR ***
	[Signal]
	public delegate void PlayerDamageEventHandler(float DamageAmount); 



	// Object to misc																																								*** NOT SURE WHAT THIS IS FOR ***
	[Signal]
	public delegate void EnemyDamageEventHandler(float DamageAmount);


}
