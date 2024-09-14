using Godot;
using System;

public partial class Collectable : Resource
{
	public Collectable(string collectable_name, float collectable_amount)
	{
		name = collectable_name;
		amount = collectable_amount;
	}
	public string name;
	public float amount;
}
