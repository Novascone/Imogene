using Godot;
using System;

public partial class CustomSignals : Node
{
	[Signal]
	public delegate void PlayerDamageEventHandler(int DamageAmount);
	// [Signal]
	// public delegate void EnemyTargetedEventHandler(Area3D mob);
	// [Signal]
	// public delegate void EnemyUnTargetedEventHandler();
	[Signal]
	public delegate void EnemyPositionEventHandler(Vector3 position);
	[Signal]
	public delegate void PlayerPositionEventHandler(Vector3 position);
	[Signal]
	public delegate void CameraPositionEventHandler(Vector3 position);
	[Signal]
	public delegate void TargetingEventHandler(bool targeting, Vector3 position);

}
