using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class Ability : Node3D
{
    [Export] public Timer UseTimer  { get; set; }
    [Export] public ClassType TypeOfAbility { get; set; }
    [Export] public GeneralAbilityType AbilityGeneralType { get; set; }
    [Export] public ClassAbilityType AbilityClassType { get; set; }
    [Export] public string Description { get; set; }
    [Export] public Texture2D Icon { get; set; }
    public States State { get; set; } = States.NotQueued;
    public MeleeHitbox MeleeHitbox { get; set; } = null;
    public RangedHitbox RangedHitbox { get; set; } = null;
    public float DamageModifier  { get; set; } = 0.0f;
    public Cross AbilityCross { get; set; } = Cross.None;
    public Tier AbilityTier { get; set; } = Tier.None;
    public string AssignedButton { get; set; } = "";
    public bool CrossSelected { get; set; } = false;
    public bool ButtonPressed { get; set; } = false;
    public bool ButtonReleased { get; set; } = false;
    public bool ButtonHeld { get; set; } = false;
    public bool AbilityExecuted { get; set; } = false;
    public int FramesHeld { get; set; } = 0;
    public int FramesHeldThreshold { get; set; } = 20;
    public bool InUse { get; set; } = false;
    public int ResourceChange  { get; set; } = 0;
    public int Charges { get; set; } = 0;
    public int ChargesUsed { get; set; } = 0;
    public float CastTime { get; set; } = 0;
    public SceneTreeTimer CooldownTimer { get; set; } = null;
    public bool RotateOnSoft { get; set; } = false;
    public bool RotateOnHeld { get; set; } = false;

    [Signal] public delegate void AbilityPressedEventHandler(Ability ability);
    [Signal] public delegate void AbilityQueueEventHandler(Ability ability);
    [Signal] public delegate void AbilityCheckEventHandler(Ability ability);
    [Signal] public delegate void AbilityReleasedEventHandler(Ability ability);
    [Signal] public delegate void AbilityExecutingEventHandler(Ability ability);
    [Signal] public delegate void MovementAbilityExecutedEventHandler(bool executing);
    [Signal] public delegate void AbilityFinishedEventHandler(Ability ability);
    [Signal] public delegate void AbilityReleaseInputControlEventHandler(Ability ability);

    public enum ClassType {None, General, Brigian, Mage, Monk, Rogue, Shaman}
    public enum GeneralAbilityType {None, Melee, Ranged, Defensive, Movement, Unique, Toy}
    public enum ClassAbilityType {None, Basic, Kernel, Defensive, Mastery, Movement, Specialized, Unique, Toy}
    public enum Cross {None, Left, Right}
    public enum Tier {None, Primary, Secondary}
    public enum States{ NotQueued, Queued }
   
    public override void _UnhandledInput(InputEvent @event) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        if(AssignedButton != null && AssignedButton != "")
        {
            if(@event.IsActionPressed(AssignedButton))
            {

                if(!ButtonPressed)
                {
                    EmitSignal(nameof(AbilityPressed), this);
                }
                
                ButtonPressed = true;
                FramesHeld = 1;
                ButtonReleased = false;
            }
            if(@event.IsActionReleased(AssignedButton))
            {
                if(!ButtonReleased)
                {
                    EmitSignal(nameof(AbilityReleased), this);
                }
                ButtonPressed = false;
                ButtonReleased = true;
    
            }
        }
		
	}

    public virtual void Execute(Player player) // Default execute
    {
        State = States.NotQueued;
        AbilityExecuted = false;
        EmitSignal(nameof(AbilityExecuting), this);
    }

    public bool CheckHeld()
    {
    
        if(FramesHeld < FramesHeldThreshold)
        {
            ButtonHeld = false;
        }
        else
        {
            ButtonHeld = true;
        }
        if(FramesHeld > 0 && !ButtonReleased)
        {
            FramesHeld += 1;
        }
        else
        {
            FramesHeld = 0;
        }
            
        return ButtonHeld;
    }

    public virtual void DealDamage(Player player, float abilityDamageModifier)
    {
        if(MeleeHitbox != null)
        {
           
            MeleeHitbox.damage = player.CombinedDamage * (1 + abilityDamageModifier);
            MeleeHitbox.SetDamage(player);
            MeleeHitbox.posture_damage = 0;
            MeleeHitbox.is_critical = false;
            
        }
        if(RangedHitbox != null)
        {
           
            RangedHitbox.damage = player.CombinedDamage * (1 + abilityDamageModifier); // Set projectile damage
            RangedHitbox.SetDamage(player);
            RangedHitbox.posture_damage = 0; // Set projectile posture damage 
            RangedHitbox.is_critical = false;
            
        }        
    }

 
    public virtual void FrameCheck(Player player)
    {
        if(Input.IsActionJustReleased(AssignedButton)) // Allow the player to move fully if the button is released
		{
			if(UseTimer.TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityFinished),this);
				AbilityExecuted = true;
			}
		}
		if(Input.IsActionJustPressed(AssignedButton) && State == States.NotQueued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		{
			EmitSignal(nameof(AbilityQueue), this);
		}
		else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(UseTimer.TimeLeft == 0)
			{
                if(State == States.NotQueued)
                {
                    EmitSignal(nameof(AbilityQueue), this);
                }
				EmitSignal(nameof(AbilityCheck), this);
			}		
		}
		if(UseTimer.TimeLeft == 0) // If not held check if ability can be used
		{
			EmitSignal(nameof(AbilityCheck),this);
		}	
    }
}
