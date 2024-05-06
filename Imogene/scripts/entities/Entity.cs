using Godot;
using System;

public partial class Entity : CharacterBody3D
{
    // Stats

    public int level;
    public float speed = 5.0f;
	public float health = 20; // Prelim health number
    public int resource = 20; // prelim resource number
   

    // Offense


    public int weapon_damage;
    public float attack_speed;


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
