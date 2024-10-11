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
    [Export] public ClassType class_type { get; set; }
    [Export] public GeneralAbilityType general_ability_type { get; set; }
    [Export] public ClassAbilityType class_ability_type { get; set; }
    [Export] public string description { get; set; }
    [Export] public Texture2D icon { get; set; }
    public States state { get; set; } = States.not_queued;
    public MeleeHitbox melee_hitbox { get; set; } = null;
    public RangedHitbox ranged_hitbox { get; set; } = null;
    public float ability_damage_modifier  { get; set; } = 0;
    public Cross cross { get; set; } = Cross.None;
    public Tier tier { get; set; } = Tier.None;
    public string assigned_button { get; set; } = "";
    public bool cross_selected { get; set; } = false;
    public bool button_pressed { get; set; } = false;
    public bool button_released { get; set; } = false;
    public bool button_held { get; set; } = false;
    public bool ability_finished { get; set; } = false;
    public int frames_held { get; set; } = 0;
    public int frames_held_threshold { get; set; } = 20;
    public bool useable { get; set; } = true;
    public bool in_use { get; set; } = false;
    public int resource_change  { get; set; } = 0;
    public int charges { get; set; } = 0;
    public int charges_used { get; set; } = 0;
    public float cast_time { get; set; } = 0;
    public SceneTreeTimer cooldown_timer { get; set; } = null;
    [Export] public Timer use_timer  { get; set; } = null;
    public bool rotate_on_soft { get; set; } = false;
    public bool rotate_on_soft_far { get; set; } = false;
    public bool rotate_on_soft_close { get; set; } = false;
    public bool rotate_on_held { get; set; } = false;

    [Signal] public delegate void AbilityPressedEventHandler(Ability ability);
    [Signal] public delegate void AbilityQueueEventHandler(Ability ability);
    [Signal] public delegate void AbilityCheckEventHandler(Ability ability);
    [Signal] public delegate void AbilityReleasedEventHandler(Ability ability);
    [Signal] public delegate void AbilityExecutingEventHandler(Ability ability);
    [Signal] public delegate void MovementAbilityExecutedEventHandler(bool executing);
    [Signal] public delegate void AbilityFinishedEventHandler(Ability ability);
    [Signal] public delegate void AbilityReleaseInputControlEventHandler(Ability ability);

    public enum ClassType {General, Brigian, Mage, Monk, Rogue, Shaman}
    public enum GeneralAbilityType {Melee, Ranged, Defensive, Movement, Unique, Toy}
    public enum ClassAbilityType {None, Basic, Kernel, Defensive, Mastery, Movement, Specialized, Unique, Toy}
    public enum Cross {None, Left, Right}
    public enum Tier {None, Primary, Secondary}
    public enum States{ not_queued, queued }
   
    public override void _UnhandledInput(InputEvent @event_) // Makes ability input unhandled so that the  UI can capture the input before it reaches the ability, this disables abilities from being used when interacting with the UI
	{
        if(assigned_button != null && assigned_button != "")
        {
            if(@event_.IsActionPressed(assigned_button))
            {

                if(!button_pressed)
                {
                    EmitSignal(nameof(AbilityPressed), this);
                }
                
                button_pressed = true;
                frames_held = 1;
                button_released = false;
            }
            if(@event_.IsActionReleased(assigned_button))
            {
                if(!button_released)
                {
                    EmitSignal(nameof(AbilityReleased), this);
                }
                button_pressed = false;
                button_released = true;
    
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

    public virtual void DealDamage(Player player_, float ability_damage_modifier_)
    {
        if(melee_hitbox != null)
        {
            if(DamageSystem.Critical(player_))
            {
                melee_hitbox.damage = MathF.Round(player_.combined_damage * (1 + player_.combined_damage), 2) * (1 + ability_damage_modifier);
                melee_hitbox.SetDamage(player_);
                melee_hitbox.posture_damage = player_.posture_damage.current_value;
                melee_hitbox.is_critical = true;
            }
            else
            {
                melee_hitbox.damage = player_.combined_damage * (1 + ability_damage_modifier);
                melee_hitbox.SetDamage(player_);
                melee_hitbox.posture_damage = player_.posture_damage.current_value;
                melee_hitbox.is_critical = false;
            }
        }
        if(ranged_hitbox != null)
        {
            if(DamageSystem.Critical(player_)) // check if the play will crit
		    {
                ranged_hitbox.damage = MathF.Round(player_.combined_damage * (1 + player_.critical_hit_damage.current_value), 2) * (1 + ability_damage_modifier); // Set projectile damage
                ranged_hitbox.SetDamage(player_);
                ranged_hitbox.posture_damage = player_.posture_damage.current_value / 3; // Set projectile posture damage 
                ranged_hitbox.is_critical = true;
		    }
            else
            {
                
                ranged_hitbox.damage = player_.combined_damage * (1 + ability_damage_modifier); // Set projectile damage
                ranged_hitbox.SetDamage(player_);
                ranged_hitbox.posture_damage = player_.posture_damage.current_value / 3; // Set projectile posture damage 
                ranged_hitbox.is_critical = false;
            }
        }        
    }

    public virtual void Execute(Player player_) // Default execute
    {
        ability_finished = false;
        EmitSignal(nameof(AbilityExecuting), this);
    }

    public virtual void FrameCheck(Player player_)
    {
        if(Input.IsActionJustReleased(assigned_button)) // Allow the player to move fully if the button is released
		{
			if(use_timer.TimeLeft == 0)
			{
				EmitSignal(nameof(AbilityFinished),this);
				ability_finished = true;
			}
			
		}
		if(Input.IsActionJustPressed(assigned_button) && state == States.not_queued) // if the button assigned to this ability is pressed, and the ability is not queued, queue the ability
		{
			EmitSignal(nameof(AbilityQueue), this);
		}
		else if (CheckHeld()) // If the button is held check cast timer, queue ability, and check if it can be used
		{
			if(use_timer.TimeLeft == 0)
			{
                if(state == States.not_queued)
                {
                    EmitSignal(nameof(AbilityQueue), this);
                }
				EmitSignal(nameof(AbilityCheck), this);
			}		
		}
		if(use_timer.TimeLeft == 0) // If not held check if ability can be used
		{
			EmitSignal(nameof(AbilityCheck),this);
		}	
    }

}