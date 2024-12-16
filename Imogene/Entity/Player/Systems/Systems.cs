using Godot;
using System;

public partial class Systems : Node
{	
	[Export] public VisionSystem vision_system;
	[Export] public InteractSystem interact_system;
	[Export] public TargetingSystem targeting_system;
	[Export] public XPSystem xp_system;
	
}
