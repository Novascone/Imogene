using Godot;
using System;

public partial class Collectable : Resource
{
	public Collectable(CollectableType type, float amount)
	{
		Type = type;
		Amount = amount;
	}
	public enum CollectableType {None, XP, Gold}
	public CollectableType Type { get; set; } = CollectableType.None;
	public float Amount { get; set; } = 0.0f;
}
