using Godot;
using System;

public partial class Collectable : Resource
{
	public Collectable(CollectableType type_, float amount_)
	{
		type = type_;
		amount = amount_;
	}
	public enum CollectableType {None, XP, Gold}
	public CollectableType type { get; set; } = CollectableType.None;
	public float amount { get; set; } = 0.0f;
}
