using Godot;
using System;

public partial class Systems : Node
{	
	[Export] public VisionSystem VisionSystem;
	[Export] public InteractSystem InteractSystem;
	[Export] public TargetingSystem TargetingSystem;
	[Export] public XPSystem XPSystem;
	
}
