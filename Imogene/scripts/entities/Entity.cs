using Godot;
using System;

public partial class Entity : CharacterBody3D
{
    // Stats

    public int level;
    public float speed = 5.0f;
	public float health = 20; // Prelim health number
    public int resource = 20; // prelim resource number
    public int strength = 1;
    public int dexterity = 1;
    public int intellect = 1;
    public int vitality = 1;
    public int stamina = 1;
    public int wisdom = 1;
    public int charisma = 1;

    // Offense

    public int physical_melee_attack_abilities;
    public int physical_ranged_attack_abilities;
    public int spell_melee_attack_abilities;
    public int spell_ranged_attack_abilities;

    public float physical_melee_attack_ability_ratio;
    public float physical_ranged_attack_ability_ratio;
    public float spell_melee_attack_ability_ratio;
    public float spell_ranged_attack_ability_ratio;
    public int total_attack_abilities;

    public int weapon_damage;
    public float attack_speed;

    public float physical_melee_power;
    public float physical_ranged_power;
    public float spell_melee_power;
    public float spell_ranged_power;

    public float critical_hit_chance;
    public float critical_hit_damage;

    public float physical_melee_damage;
    public float physical_ranged_damage;
    public float spell_melee_damage;
    public float spell_ranged_damage;

    public float damage;

    // Defense

    

    // Possessions
    public int gold = 0;

    public bool can_move = true; // Boolean to keep track of if the play is allowed to move
    public bool attacking;
    public bool dead = false;
    public Vector3 enemy_position;

    
    
    public void TakeDamage(float amount)
    {
        if(health - amount > 0)
        {
            health -= amount;
        }
        else
        {
            dead = true;
            GD.Print("dead");
        }
    }
    public  Node LoadAbility(String name)
    {
        var scene = GD.Load<PackedScene>("res://scripts/abilities/" + name + "/" + name + ".tscn");
        var sceneNode = scene.Instantiate();
        return sceneNode;
    }
}
