using Godot;
using System;

public partial class InventoryButton : Button
{
	public Item inventory_item;
	public TextureRect icon;
	public Label quantity_label;
	private int index;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		icon = GetNode<TextureRect>("TextureRect");
		quantity_label = GetNode<Label>("Label");
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void UpdateItem(Item item, int index)
	{
		this.index = index;
		inventory_item = item;
		if(item == null)
		{
			icon.Texture = null;
			quantity_label.Text = string.Empty;
		}
		else
		{
			icon.Texture = item.icon;
			quantity_label.Text = item.quantity.ToString();
		}
		
	}


	 public void InventoryButtonPressed()
    {
		
		if(inventory_item != null)
		{
			inventory_item.UseItem();
		}
			
    }

}
