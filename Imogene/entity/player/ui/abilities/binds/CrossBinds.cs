using Godot;
using System;

public partial class CrossBinds : Control
{
	[Export] public CrossBindButton up { get; set; }
	[Export] public CrossBindButton left { get; set; }
	[Export] public CrossBindButton right { get; set; }
	[Export] public CrossBindButton down { get; set; }
}
