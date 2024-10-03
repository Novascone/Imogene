using Godot;
using System;

public partial class Collectable : Resource
{
	public Collectable(CollectableType collectable_type, float collectable_amount)
	{
		type = collectable_type;
		amount = collectable_amount;
	}
	public enum CollectableType {XP, Gold}
	public CollectableType type { get; set; }
	public float amount;
}
