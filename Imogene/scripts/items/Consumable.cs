using Godot;
using System;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;

public partial class Consumable : Item
{
  
	[Export]
	public int heal_amount = 10;
    
    public override void UseItem()
    {
        GD.Print("Heal the player for " + heal_amount);
    }

    public override void EquipItem()
    {
        
    }
}
