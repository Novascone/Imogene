using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;


// Ability class, abilities must have a script, a resource, and a scene
// The scene will be a Node3D named the same as the ability, capitalized
// The resource will have an ID, Name, Description, Ability Path, Icon, Type, and up to 5 modifiers
// The Ability Path is not utilized right now. The Type decides which category it will be in
// The script controls how the ability will function

public partial class Ability : Node3D
{
  
    public enum ClassType {General, Brigian, Mage, Monk, Rogue, Shaman}
    [Export] public ClassType class_type;
    public enum GeneralAbilityType {Melee, Ranged, Defensive, Movement, Unique, Toy}
    [Export] public GeneralAbilityType general_ability_type;
    public enum ClassAbilityType {None, Basic, Kernel, Defensive, Mastery, Movement, Specialized, Unique, Toy}
    [Export] public ClassAbilityType class_ability_type;
    public enum DamageType {None, Slash, Peirce, Blunt, Bleed, Poison, Fire, Cold, Lightning, Holy}
    [Export] public DamageType damage_type;
    [Export] public string description { get; set; }
    [Export] public Texture2D icon { get; set; }

    public MeleeHitbox melee_hitbox;
    public RangedHitbox ranged_hitbox;
    
    [Signal] public delegate void AbilityPressedEventHandler(Ability ability);
    [Signal] public delegate void AbilityQueueEventHandler(Ability ability);
    [Signal] public delegate void AbilityCheckEventHandler(Ability ability);
    [Signal] public delegate void AbilityReleasedEventHandler(Ability ability);
    [Signal] public delegate void AbilityFinishedEventHandler(Ability ability);
    
    // enum ability_t {active, passive}


    
    public string cross{ get; set; }
    public string level { get; set; }
    public string assigned_button { get; set; }
    public string action_button { get; set; }
    public bool cross_selected;
    public bool button_pressed;
    public bool button_released;
    public bool button_held;
    public int frames_held;
    public int frames_held_threshold = 20;

    public bool useable = true;
    public bool in_use = false;
    public int pressed = 0;
    public bool animation_finished = false;
    public bool ready_to_use;

    public int resource_change;
    public int charges;
    public int charges_used;
    public float cast_time;
    public Timer cooldown_timer;
    public bool rotate_on_soft;
    public bool rotate_on_soft_far;
    public bool rotate_on_soft_close;
    public bool rotate_on_held;

    public bool stop_movement_input;

    private CustomSignals _customSignals; // Custom signal instance

    public States state;

    public enum States
    {
        not_queued,
        queued
    }

   

   
    public override void _UnhandledInput(InputEvent @event) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        if(assigned_button != null && assigned_button != "")
        {
            if(@event.IsActionPressed(assigned_button))
            {

                GD.Print("Assigned button " + assigned_button);
                // GD.Print(this.Name + "Action strength " + );
                if(!button_pressed)
                {
                    EmitSignal(nameof(AbilityPressed), this);
                }
                
                button_pressed = true;
                frames_held = 1;
                button_released = false;
                // GD.Print(this.Name + " has been pressed");
            }
            if(@event.IsActionReleased(assigned_button))
            {
                if(!button_released)
                {
                    EmitSignal(nameof(AbilityReleased), this);
                }
                button_pressed = false;
                button_released = true;
                // GD.Print(this.Name + " has been released");              
                // GD.Print("button released");
            }
        }
		
	}


    public bool UIButton()
    {
        if(assigned_button == "B")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckHeld()
    {
        // GD.Print(Name + " calling checkheld");
    
        if(frames_held < frames_held_threshold)
        {
            button_held = false;
        }
        else
        {
            button_held = true;
        }
        if(frames_held > 0 && !button_released)
        {
            frames_held += 1;
        }
        else
        {
            frames_held = 0;
        }
            
        return button_held;
    }

    public virtual void DealDamage(Player player)
    {
        if(melee_hitbox != null)
        {
            if(player.entity_systems.damage_system.Crit(player))
            {
                GD.Print("Critical!");
                melee_hitbox.damage = MathF.Round(player.combined_damage * (1 + player.combined_damage), 2);
                melee_hitbox.posture_damage = player.posture_damage.current_value;
                melee_hitbox.is_critical = true;
                // GD.Print("Main Hand damage: " + player.main_hand_hitbox.damage);
            }
            else
            {
                melee_hitbox.damage = player.combined_damage;
                melee_hitbox.posture_damage = player.posture_damage.current_value;
                melee_hitbox.is_critical = false;
                // GD.Print("Main Hand damage: " + player.main_hand_hitbox.damage);
            }
        }
        if(ranged_hitbox != null)
        {
            if(player.entity_systems.damage_system.Crit(player)) // check if the play will crit
		{
			ranged_hitbox.damage = MathF.Round(player.combined_damage * (1 + player.critical_hit_damage.current_value), 2); // Set projectile damage
			ranged_hitbox.posture_damage = player.posture_damage.current_value / 3; // Set projectile posture damage 
			ranged_hitbox.is_critical = true;
		}
		else
		{
			
			ranged_hitbox.damage = player.combined_damage; // Set projectile damage
			ranged_hitbox.posture_damage = player.posture_damage.current_value / 3; // Set projectile posture damage 
			ranged_hitbox.is_critical = false;
		}
        }
        

        
    }

    public virtual void Execute(Player player) // Default execute
    {
        // GD.Print("access ability child");
    }

    public virtual void FrameCheck(Player player)
    {
        GD.Print("this is the base frame check");
    }



    public virtual void OnAnimationFinished(StringName animName)
    {
        // throw new NotImplementedException();
    }
}