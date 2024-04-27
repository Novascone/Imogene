using Godot;
using System;

public partial class Item : Resource
{
    [Export]
    public int id { get; set; }
    [Export]
    public string name { get; set; }
    [Export]
    public string resource_path { get; set; }
    [Export]
    public Texture2D icon { get; set; }
    [Export]
    public int quantity { get; set; }
    [Export]
    public int stack_size { get; set; }
    [Export]
    public bool is_stackable { get; set; }
    [Export]
    public string type_of_item;

    public Item Copy() => Duplicate() as Item;

   
    public virtual void UseItem()
    {
        
        GD.Print("Used Item");
    }


}
