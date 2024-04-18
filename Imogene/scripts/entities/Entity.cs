using Godot;
using System;

public partial class Entity : CharacterBody3D
{
    public float speed = 5.0f;
	public int health = 20; // Prelim health number
    public int damage = 10; // Prelim damage number
    public int resource = 20; // prelim resource number
    public bool can_move = true; // Boolean to keep track of if the play is allowed to move
    public Vector3 enemy_position;

    public TNode LoadAbility<TNode>(PackedScene name) where TNode : Node
    {
        var scene = GD.Load<PackedScene>("res://scenes/abilities/" + name + "/" + name + ".tscn");
        var sceneNode = scene.Instantiate();
        return (TNode)sceneNode;
    }
}
