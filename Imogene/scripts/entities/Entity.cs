using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

// The base class for all entities in game
// All stats are used for players
// Not all enemies utilize all these stats

public partial class Entity : CharacterBody3D
{

	[Export] public string identifier;

	// Hit and hurt boxes
	public Hitbox main_hand_hitbox;
	public Hitbox off_hand_hitbox;
	public Hurtbox hurtbox;

	// Systems
	public DamageSystem damage_system;
	public ResourceSystem resource_system;

	// Booleans
	public bool can_move = true; // Boolean to keep track of if the entity is allowed to move
    public bool jumping = false;
    public bool using_movement_ability;
    public bool on_floor;
    public bool attacking; // Boolean to keep track of if the entity is attacking
    public bool animation_triggered;
	public bool targeted;
	public bool dead = false;
	

	// Dot
	public string dot_damage_type;
	public float dot_in_seconds;
	public int dot_duration;
	public bool taking_dot;

	// Posture
	public bool posture_broken;

	// Slow
	public int slow_duration;
	public bool slowed;

	// Stun
	public int stun_duration;
	public bool stunned;

	// Cast
	public Timer cast_timer;

    // Stats
    public int level = 1; // Level of the entity
    public float speed = 7.0f; // Speed of the entity
    public float fall_speed = 40.0f; // How fast the player falls 
    public float jump_speed = 30.0f; // How fast the player jumps
	public float health = 200; // Prelim health number
    public float resource = 100; // prelim resource number
	public float posture;

	public int strength = 0; // Strength: A primary stat for melee damage. Contributes to Physical Melee Power, Physical Ranged Power, and Spell Melee Power
	public int dexterity = 0; // Dexterity: A primary stat for melee damage. Contributes to all Power stats
	public int intellect = 0; // Intellect: Primary stat spell damage for. Contributes to Spell Melee Power and Spell Ranged Power
	public int vitality = 0; // Vitality: Primary stat for health
	public int stamina = 0; // Primary stat for resource and regeneration
	public int wisdom = 0; // Increases the damage of abilities that use wisdom, also used for interactions
	public int charisma = 0; // Primary Stat for character interaction

	// Offense
	public float total_dps; // Total estimated dps, combination of all 4 types of attacks
	public float physical_melee_dps;
	public float spell_melee_dps;
	public float physical_ranged_dps;
	public float spell_ranged_dps;
    public int main_hand_damage;
	public float off_hand_damage;

	public float physical_melee_power; // Increases physical melee DPS by a magnitude every 100 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_melee_power; // Increases melee magic DPS a magnitude every 100 points + 3 for every point of intellect + 1 for every point of dexterity + 1 for every point strength
	public float physical_ranged_power; // Increases physical ranged DPS a magnitude every 100 points + 2 for every point of strength + 1 for every point of dexterity
	public float spell_ranged_power;  // Increases physical ranged DPS by a magnitude every 100 points + 3 for every point of dexterity + 1 for every point of strength
	public float wisdom_scaler; // Increases by one every 20 wisdom. Increases how powerful attacks that scale with wisdom are

	
	public float physical_melee_power_mod; // 1 + (power/100)
	public float physical_ranged_power_mod; 
	public float spell_melee_power_mod;
	public float spell_ranged_power_mod;
	public float power_mod_avg;

    
    public float attack_speed;
	public int damage_bonus = 0; // Damage bonus from gear
	public float combined_damage = 0; // Main hand, off-hand, and all damage from equipment
	public float base_aps = 0; // Base attacks per second of the entity, determined by weapons/ stance for players/ entities using weapons
	public float aps_modifiers = 0; // Modifiers to attacks per second, from skills or gear
	public float aps = 0; // Attacks per second
	public float base_dps = 0; // Base dps of the entity aps * combined damage
	public float skill_mod = 0; // Damage modification granted from skills or equipment
	public float crit_mod = 0; // 1 + (crit chance * crit damage)



	public float slash_damage = 0; // Percentage of damage given by slash
	public float thrust_damage = 0; // Percentage of damage given by thrust
	public float blunt_damage = 0; // Percentage of damage given by blunt
	public float bleed_damage = 0; // Percentage of damage given by bleed
	public float poison_damage = 0; // Percentage of damage given by poison
	public float fire_damage = 0; // Percentage of damage given by fire
	public float cold_damage = 0; // Percentage of damage given by cold
	public float lightning_damage = 0; // Percentage of damage given by lightning
	public float holy_damage = 0; // Percentage of damage given by holy
	public float critical_hit_chance = 0.5f; // Percentage change for hit to be a critical hit
	public float critical_hit_damage = 0.9f; // Multiplier applied to base damage if a hit is critical
	public float attack_speed_increase = 0; // aps modifiers as a percentage to be displayed
	public float cool_down_reduction = 0; // The percent reduction of cooldown
	public float posture_damage = 34; // How much damage each attack does to opponent posture **** this needs more defining as does the posture system ****
    public float damage; // How much damage the entity does

	// Defense
	public int armor = 20; // Total armor the entity has increases armor damage reduction 
	public int poise = 0; // Decreases how fast posture depletes **** this needs more defining as does the posture system ****
	public int block_amount = 0; // How much damage the entity can block when blocking
 	public int retaliation = 0; // Increases retaliation period
	public int physical_resistance = 0; // Increases physical damage resistance
	public int thrust_resistance = 0; // Increases thrust damage resistance
	public int slash_resistance = 0; // Increases slash damage resistance
	public int blunt_resistance = 0; // Increases blunt damage resistance
	public int bleed_resistance = 0; // Increases bleed damage resistance
	public int poison_resistance = 0; // Increases poison damage resistance
	public int curse_resistance = 0; // Increases curse resistance
	public int spell_resistance = 0; // Increases spell damage resistance
	public int fire_resistance = 0; // Increases fire damage resistance
	public int cold_resistance = 0; // Increases cold damage resistance
	public int lightning_resistance = 0; // Increases lightning damage resistance
	public int holy_resistance = 0; // Increases holy damage resistance

	public float dr_armor; // 1 + (armor / (dr_level_scale)) repeat for all resistances
	public float dr_phys;
	public float dr_slash;
	public float dr_thrust;
	public float dr_blunt;
	public float dr_bleed;
	public float dr_poison;
	public float dr_curse;
	public float dr_spell;
	public float dr_fire;
	public float dr_cold;
	public float dr_lightning;
	public float dr_holy;

	public float dr_lvl_scale; // level * 50 <-- set in player entity / on enemy script
	public float avg_res_dr; // Average of all resistances

	public float resistance; // Estimated total resistance stat



	// Health

	public float maximum_health;
	public float health_bonus = 0; // Health bonus from skills and gear
	public float health_regen = 0; // Health regenerated every second 1 + (stamina/rec_lvl_scale * health_regen_bonus)
	public float health_regen_bonus = 0; // Bonus to health regeneration from skills and gear
	public float health_on_retaliate = 0; // How much health is gain from a successful attack during retaliation period

	// Resource

	public float maximum_resource;
	public float resource_regen = 0; // Resource regenerated every second 1 + (stamina/rec_lvl_scale *  resource_regen_bonus)
	public float resource_regen_bonus = 0; // Bonus to health regeneration from skills and gear
	public float maximum_posture = 100;
	public float posture_regen = 0; // Posture regenerated every second 1 + (stamina/rec_lvl_scale * (1 + poise/100)
	public float posture_regen_bonus = 0; // Bonus to posture regen from skills and gear
	public float resource_cost_reduction = 0; // Resource cost reduction from skills and gear
	public float rec_lvl_scale; // level * 100 <-- set in player entity

	public float recovery; // Estimated total recovery stat, average of the three types of recovery

	// Misc
	public float movement_speed = 0; // How fast the entity moves

	// Materials

	public int maximum_gold; // How much gold the entity can carry


    public Vector3 direction; // Direction 
	public Vector3 position; // Position 
	public Vector3 velocity; // Velocity 
	public float prev_y_rotation; // Rotation before current rotation
	public float current_y_rotation; // Current rotation


    // Possessions
    public int gold = 0;

    // public Vector3 enemy_position;


	private CustomSignals _customSignals;

    public override void _Ready()
    {

		damage_system = GetNode<DamageSystem>("Systems/DamageSystem");
		damage_system.GetEntityInfo(this);

		resource_system = GetNode<ResourceSystem>("Systems/ResourceSystem");
		resource_system.GetEntityInfo(this);

        
		hurtbox = GetNode<Hurtbox>("Areas/Hurtbox");
		hurtbox.AreaEntered += OnHurtboxBodyEntered;

		// Timers
		
		cast_timer = GetNode<Timer>("Systems/CastTimer");
		cast_timer.Timeout += OnCastTickTimeout;

		_customSignals = GetNode<CustomSignals>("/root/CustomSignals");
    }

	
    private void OnCastTickTimeout()
    {
        throw new NotImplementedException();
    }

   
    private void OnHurtboxBodyEntered(Area3D body)
    {
		GD.Print("Hitbox entered " + this.Name);
		if(body is Hitbox box)
		{
			if(body.IsInGroup("ActiveHitbox") && body is Hitbox)
			{
				damage_system.TakeDamage(box.damage_type, box.damage, box.is_critical);
				resource_system.Posture(box.posture_damage);
			}
		}
    }

	
    public  Node LoadAbility(String name) // Loads an ability from a string
    {
        var scene = GD.Load<PackedScene>("res://scripts/abilities/" + name + "/" + name + ".tscn");
        var sceneNode = scene.Instantiate();
        return sceneNode;
    }

    public bool Fall(double delta) // bring the player back to the ground
	{
		if(!IsOnFloor())
		{
			velocity.Y -= fall_speed * (float)delta;
            return true;
		}
		else
		{
            return false;
		}
		
	}
}
