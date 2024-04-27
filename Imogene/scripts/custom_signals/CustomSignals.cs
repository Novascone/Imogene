using Godot;
using System;

public partial class CustomSignals : Node
{
	[Signal]
	public delegate void PlayerDamageEventHandler(int DamageAmount);
	[Signal]
	public delegate void EnemyDamageEventHandler(int DamageAmount);
	[Signal]
	public delegate void EnemyPositionEventHandler(Vector3 position);
	[Signal]
	public delegate void PlayerPositionEventHandler(Vector3 position);
	[Signal]
	public delegate void CameraPositionEventHandler(Vector3 position);
	[Signal]
	public delegate void TargetingEventHandler(bool targeting, Vector3 position);
	[Signal]
	public delegate void UIHealthEventHandler(int amount);
	[Signal]
	public delegate void UIResourceEventHandler(int amount);
	[Signal]
	public delegate void UIHealthUpdateEventHandler(int amount);
	[Signal]
	public delegate void UIResourceUpdateEventHandler(int amount);
	[Signal]
	public delegate void InteractEventHandler(Area3D area, bool in_interact_area);
	[Signal]
	public delegate void ItemInfoEventHandler(Item item);
	[Signal]
	public delegate void ConsumableInfoEventHandler(Consumable item);
	[Signal]
	public delegate void EquipableInfoEventHandler(Equipable item);
	[Signal]
	public delegate void PlayerInfoEventHandler(player player);
	[Signal]
	public delegate void RemoveEquippedEventHandler();
	[Signal]
	public delegate void InteractPressedEventHandler(bool in_area);


}
