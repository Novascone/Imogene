using Godot;
using System;

public partial class Collectable : Resource
{
	public Collectable(CollectableType type_, float amount_)
	{
		type = type_;
		amount = amount_;
	}
	public enum CollectableType {NONE, XP, GOLD}
	public CollectableType type { get; set; } = CollectableType.NONE;
	public float amount { get; set; } = 0.0f;
}
