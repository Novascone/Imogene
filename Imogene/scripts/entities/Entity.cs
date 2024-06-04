using Godot;
using System;

// The base class for all entities in game

public partial class Entity : CharacterBody3D
{
    // Stats

    public int level; // Level of the entity
    public float speed = 7.0f; // Speed of the entity
    public float fall_speed = 40.0f;
    public float jump_speed = 20.0f;
	public float health = 20; // Prelim health number
    public int resource = 20; // prelim resource number
   

    // Offense


    public int weapon_damage;
    public float attack_speed;


    public float damage; // How much damage the entity does

    // Defense


    // Possessions
    public int gold = 0;

    public bool can_move = true; // Boolean to keep track of if the entity is allowed to move
    public bool jumping = false;
    public bool using_movement_ability;
    public bool on_floor;
    public bool attacking; // Boolean to keep track of if the entity is attacking
    public bool animation_triggered;
    public bool dead = false;
    public Vector3 enemy_position;

    
    
    public void TakeDamage(float amount) // Applies damage to an entity
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
    public  Node LoadAbility(String name) // Loads an ability from a string
    {
        var scene = GD.Load<PackedScene>("res://scripts/abilities/" + name + "/" + name + ".tscn");
        var sceneNode = scene.Instantiate();
        return sceneNode;
    }

    
}
