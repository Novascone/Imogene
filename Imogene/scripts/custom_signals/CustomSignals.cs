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


}
