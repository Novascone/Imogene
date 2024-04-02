using Godot;
using System;

public partial class CustomSignals : Node
{
	[Signal]
	public delegate void PlayerDamageEventHandler(int DamageAmount);

	[Signal]
	public delegate void EnemyTargetedEventHandler();
	[Signal]
	public delegate void EnemyUnTargetedEventHandler();

}
