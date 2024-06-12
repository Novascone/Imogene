using Godot;
using System;

public partial class ItemResource : Resource
{
    [Export]
    public int id { get; set; }
    [Export]
    public string name { get; set; }
    [Export]
    public string resource_path{ get; set; }
    [Export]
    public string second_resource_path{ get; set; }
    [Export]
    public Texture2D icon { get; set; }
    [Export]
    public int quantity { get; set; }
    [Export]
    public int stack_size { get; set; }
    [Export]
    public bool is_stackable { get; set; }
    [Export]
    public string type;
    

    public ItemResource Copy() => Duplicate() as ItemResource;

   
    public virtual void UseItem()
    {
        GD.Print("Used Item");
    }
    public virtual void EquipItem()
    {
        GD.Print("Not Equipable");
    }
    public virtual void PrintItemStats()
    {
        GD.Print("Not Equipable");
    }
    public virtual void AddItem(Arm item)
    {
        GD.Print("Not Equipable");
    }



}
