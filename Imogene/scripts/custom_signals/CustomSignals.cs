using Godot;
using System;

public partial class CustomSignals : Node
{
	[Signal]
	public delegate void PlayerDamageEventHandler(float DamageAmount);
	[Signal]
	public delegate void EnemyDamageEventHandler(float DamageAmount);
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
	public delegate void InteractEventHandler(Area3D area, bool in_interact_area, bool interacting);
	[Signal]
	public delegate void ItemInfoEventHandler(Item item);
	[Signal]
	public delegate void ConsumableInfoEventHandler(Consumable item);
	[Signal]
	public delegate void EquipableInfoEventHandler(Equipable item);
	[Signal]
	public delegate void PlayerInfoEventHandler(player player);
	[Signal]
	public delegate void OverSlotEventHandler(Button slot);
	[Signal]
	public delegate void RemoveEquippedEventHandler();
	


}
