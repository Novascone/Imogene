using Godot;
using System;

public partial class Entity : CharacterBody3D
{
    public float speed = 5.0f;
	public int health = 20; // Prelim health number
    public int damage = 1; // Prelim damage number
    public int resource = 20; // prelim resource number
    public bool can_move = true; // Boolean to keep track of if the play is allowed to move
    public bool attacking;
    public bool dead = false;
    public Vector3 enemy_position;
    
    public void TakeDamage(int amount)
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
        var scene = GD.Load<PackedScene>("res://scenes/abilities/" + name + "/" + name + ".tscn");
        var sceneNode = scene.Instantiate();
        return sceneNode;
    }
}
