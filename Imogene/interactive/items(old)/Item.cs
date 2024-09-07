using Godot;
using System;

public partial class Item : Node
{
	[Export] 
	public ItemResource item_resource {get; set;}
	public string name;
	// Called when the node enters the scene tree for the first time.
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	 public virtual void UseItem()
    {
        GD.Print("Used Item");
    }
    public virtual void EquipItem()
    {
        GD.Print("Not Equipable");
    }
	public void PrintResource()
	{
		GD.Print("printing resource");
		GD.Print("item resource icon " + item_resource.icon);
		GD.Print("item resource name " + item_resource.name);
		GD.Print("item resource type " + item_resource.type);
	}
}