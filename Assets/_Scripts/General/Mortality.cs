using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Events;

/*
 * Active Energy
 * The Energy Used by the Player and Enemies to launch Regular Attacks
 *
 * Stored Energy
 * The Energy Used by the Player and Enemies to undergo transformations or other boosts
 *
 * Health
 * health
 *
 * Regen Values
 * The amount regenerated every second
 *
 * Native Values
 * These values are the values before applying any external factors like armor/equipment/(de)buffs
 * Meant for reference when removing (de)buffs
 *
 * Immunity
 * "immune" boolean, controls the ability for the character to take damage, might expand into a better system which tells the attacker we are immune later
 */

public class Mortality : MonoBehaviour
{
    [Header("Active Energy")]
    [SerializeField] private float activeEnergy;
    [SerializeField] private float nativeActiveRegen;
    [SerializeField] private float activeRegen;
    [SerializeField] private float activeMax;

    [Header("Stored Energy")]
    [SerializeField] private float storedEnergy;
    [SerializeField] private float nativeStoredRegen;
    [SerializeField] private float storedRegen;
    [SerializeField] private float storedMax;

    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float nativeRegen;
    [SerializeField] private float regen;
    [SerializeField] private float healthMax;

    [Header("Resistances")] 
    [SerializeField] private float nativeArmour;
    [SerializeField] private float nativeResist;
    [SerializeField] private float effectiveArmour;
    [SerializeField] private float effectiveResist;
    [SerializeField] private AnimationCurve scaling;

    [Header("Immunity")]
    [SerializeField] private bool immune = false;
    
    public float ActiveEnergy
    {
        get => activeEnergy;
        set
        {
            if (immune) return;
            activeEnergy = value;
            activeEnergy = Math.Clamp(activeEnergy, 0, activeMax);
            onActiveEnergyAdjust.Invoke();
        }
    }
    public float StoredEnergy
    {
        get => storedEnergy;
        set
        {
            if (immune) return;
            storedEnergy = value;
            storedEnergy = Math.Clamp(storedEnergy, 0, storedMax);
            onStoredEnergyAdjust.Invoke();
        }
    }
    public float Health
    {
        get => health;
        set
        {
            if (immune) return;
            health = value;
            health = Math.Clamp(health, 0, healthMax);
            onHealthAdjust.Invoke();
        }
    }
    
    // Regen Properties
    public float HealthRegen
    {
        get => regen;
        set { regen = value; }
    }
    public float StoredRegen
    {
        get => storedRegen;
        set { storedRegen = value; }
    }
    public float ActiveRegen
    {
        get => activeRegen;
        set { activeRegen = value; }
    }

    // Native Regen Stat Properties
    // Double _ To Prevent Accidental Writes
    public float __NativeHealthRegen
    {
        get => nativeRegen;
        set { nativeRegen = value; }
    }
    public float __NativeStoredRegen
    {
        get => nativeStoredRegen;
        set { nativeStoredRegen = value; }
    }
    public float __NativeActiveRegen
    {
        get => nativeActiveRegen;
        set { nativeActiveRegen = value; }
    }
    
    // Max Stat Properties
    // Double _ To Prevent Accidental Writes
    public float __HealthMax
    {
        get => healthMax;
        set { healthMax = value; }
    }
    public float __StoredEnergyMax
    {
        get => storedMax;
        set { storedMax = value; }
    }
    public float __ActiveEnergyMax
    {
        get => activeMax;
        set { activeMax = value; }
    }

    // Resistances Properties
    public float Armour
    {
        get => effectiveArmour;
        set
        {
            onArmourAdjust.Invoke();
            effectiveArmour = value;
        }
    }
    public float Resist
    {
        get => effectiveResist;
        set
        {
            onResistAdjust.Invoke();
            effectiveResist = value;
        }
    }
    public float __NativeArmour
    {
        get => nativeArmour;
        set
        {
            float delta = value - nativeArmour;
            Armour += delta;
            nativeArmour = value > 1000 ? 1000 : value;
        }
    }
    public float __NativeResist
    {
        get => nativeResist;
        set
        {
            float delta = value - nativeResist;
            Resist += delta;
            nativeResist = value > 1000 ? 1000 : value;
        }
    }

    public bool Immunity
    {
        get => immune;
        set
        {
            immune = value;
            onImmunityChange.Invoke();
        }
    }

        [Header("Events")]
    public UnityEvent onHealthAdjust;
    public UnityEvent onActiveEnergyAdjust;
    public UnityEvent onStoredEnergyAdjust;
    public UnityEvent onArmourAdjust;
    public UnityEvent onResistAdjust;
    public UnityEvent onImmunityChange;
    public UnityEvent onAfflictionAdd;
    public UnityEvent onAfflictionExpire;
    
    private List<Affliction> _afflictions = new List<Affliction>();

    /*
     * Enumerator for handling Armour/Health Piercing Types
     * None | No Piercing, no matter the number
     * Flat | The Number provided will be deducted flat
     * Percentage | The Number should Range from 0-1 (0-100%) and will deduct as a percentage
     */
    public enum PierceType
    {
        None,
        Flat,
        Percentage
    }

    private void Awake()
    {
        // Create the events
        onHealthAdjust = new UnityEvent();
        onActiveEnergyAdjust = new UnityEvent();
        onStoredEnergyAdjust = new UnityEvent();
        onArmourAdjust = new UnityEvent();
        onResistAdjust = new UnityEvent();
        onImmunityChange = new UnityEvent();
        onAfflictionAdd = new UnityEvent();
        onAfflictionExpire = new UnityEvent();
    }
    
    private void Start()
    {
        activeEnergy = activeMax;
        storedEnergy = storedMax;
        health = healthMax;

        immune = false;
    }

    private void Update()
    {
        // Mortality Regeneration
        ActiveEnergy += activeRegen * Time.deltaTime;
        StoredEnergy += storedRegen * Time.deltaTime;
        Health += regen * Time.deltaTime;

        for (int i = 0; i < _afflictions.Count; ++i)
        {
            _afflictions[i].Update();
            _afflictions[i].lifetime -= Time.deltaTime;
            if (_afflictions[i].lifetime <= 0)
            {
                _afflictions[i].End();
                _afflictions.RemoveAt(i);
                onAfflictionExpire.Invoke();
            }
        }
    }

    // Helper Function to Apply Affliction
    public void ApplyAffliction(Affliction affliction)
    {
        _afflictions.Add(affliction);
        affliction.Begin();
        onAfflictionAdd.Invoke();
    }
    
    /*
     * Apply Damage with Consideration for Armour/Shields and other possible effects
     * Damage | Total Damage Energy Damage dealt to the Player
     * ePierce | How much to ignore energy resistance (Armour Piercing)
     * PierceType | The type of piercing to use, either Flat or Percentage Based
     * bleed | Percentage of energy damage bleeds over into Health
     */
    public void ApplyEnergyDamage(float damage, float pierce = 0f, PierceType pierceType = PierceType.Flat, float bleed = 0, float hpPierce = 0, PierceType hpPierceType = PierceType.Flat)
    {
        float totalReduction = CalculateReduction(effectiveResist, pierce, pierceType);
        float finalDamage = damage - (damage * totalReduction);
        ActiveEnergy -= finalDamage;
        bleed /= 100f;
        bleed = Math.Clamp(bleed, 0, 1);
        float bleedOver = finalDamage * bleed;
        ApplyHealthDamage(bleedOver,hpPierce,hpPierceType);
    }

    public void ApplyHealthDamage(float damage, float pierce = 0f, PierceType pierceType = PierceType.Flat)
    {
        float totalReduction = CalculateReduction(effectiveArmour, pierce, pierceType);
        float finalDamage = damage - (damage * totalReduction);
        Health -= finalDamage;
    }
    
    /*
     * Immunity Options
     * EnableImmunity, enables Immunity for the character until disabled
     * DisableImmunity, counteracts the first function
     * ToggleImmunity, Disables if Enabled, Enables if Disabled
     * AfflictImmunity, Applies an Affliction that gives Immunity
     */
    public void EnableImmunity()
    { Immunity = true; }
    public void DisableImmunity()
    { Immunity = false; }
    public void ToggleImmunity()
    { Immunity = !Immunity; }
    public void AfflictImmunity(float time)
    {
        ApplyAffliction(new Immunity(time,this));
    }
    
    // Apply StoredEnergyDrain Affliction (Deals True StoredEnergy Damage)
    public void DrainStoredEnergy(float cost, float rate)
    {
        ApplyAffliction(new StoredEnergyDrain(cost,rate,this));
    }
    // Apply ActiveEnergy Affliction (Deals True ActiveEnergy Damage)
    public void DrainActiveEnergy(float cost, float rate)
    {
        ApplyAffliction(new ActiveEnergyDrain(cost,rate,this));
    }
    // Apply HealthDrain Affliction (Deals True Health Damage)
    public void DrainHealth(float cost, float rate)
    {
        ApplyAffliction(new HealthDrain(cost,rate,this));
    }
    
    // External Affliction Access
    public Affliction[] GetAfflictions()
    {
        return _afflictions.ToArray();
    }
    
    // Calculate Damage Reduction
    private float CalculateReduction(float resistance, float pierce, PierceType pierceType)
    {
        pierce /= 100f;
        float adjustedResistance = resistance;
        switch (pierceType)
        {
            case PierceType.Flat:
            {
                /*
                 * Reduce Armour by Pierce
                 * Prevent Armour from going negative
                 */
                adjustedResistance = resistance - pierce;
                adjustedResistance = adjustedResistance < 0 ? 0 : adjustedResistance;
                break;
            }
            case PierceType.Percentage:
            {
                /*
                 * Clamp ePierce between 0-1 to prevent over mitigation resulting in healing
                 * Adjust Armour value to account for piercing
                 */
                pierce = Math.Clamp(pierce, 0, 1);
                adjustedResistance = resistance - (resistance * pierce);
                break;
            }
        }

        float rating = adjustedResistance / 1000f;
        return scaling.Evaluate(rating);
    }
    
    // Unity Inspector Debug Functions
    
    [ContextMenu("DEBUG_FullStoredEnergy")]
    private void DEBUG_FullStoredEnergy()
    {
        storedEnergy = storedMax;
    }
    [ContextMenu("DEBUG_FullActiveEnergy")]
    private void DEBUG_FullActiveEnergy()
    {
        activeEnergy = activeMax;
    }
    [ContextMenu("DEBUG_FullHealth")]
    private void DEBUG_FullHealth()
    {
        health = healthMax;
    }
    [ContextMenu("DEBUG_DrainStoredEnergy")]
    private void DEBUG_DrainStoredEnergy()
    {
        DrainStoredEnergy(100,5);
    }
    [ContextMenu("DEBUG_DrainActiveEnergy")]
    private void DEBUG_DrainActiveEnergy()
    {
        DrainActiveEnergy(100,5);
    }
    [ContextMenu("DEBUG_DrainHealth")]
    private void DEBUG_DrainHealth()
    {
        DrainHealth(50,5);
    }

    [Header("DEBUG | Energy Damager")]
    [SerializeField] private float DEBUG_EnergyDamage;
    [SerializeField] private float DEBUG_EnergyPierce;
    [SerializeField] private PierceType DEBUG_EnergyPierceType;

    [ContextMenu("DEBUG_DealEnergyDamage")]
    private void DEBUG_DealEnergyDamage()
    {
        ApplyEnergyDamage(DEBUG_EnergyDamage,DEBUG_EnergyPierce,DEBUG_EnergyPierceType);
    }

    [Header("DEBUG | Health Damager")]
    [SerializeField] private float DEBUG_HealthDamage;
    [SerializeField] private float DEBUG_HealthPierce;
    [SerializeField] private PierceType DEBUG_HealthPierceType;

    [ContextMenu("DEBUG_DealHealthDamage")]
    private void DEBUG_DealHealthDamage()
    {
        ApplyHealthDamage(DEBUG_HealthDamage,DEBUG_HealthPierce,DEBUG_HealthPierceType);
    }

    [Header("DEBUG | Preview Reduction Numbers")]
    [SerializeField] private float DEBUG_AdjustedArmour;
    [SerializeField] private float DEBUG_AdjustedResist;
    
    [ContextMenu("DEBUG_PreviewReductionNumbers")]
    private void DEBUG_PreviewReductionNumbers()
    {
        DEBUG_AdjustedArmour = CalculateReduction(effectiveArmour, DEBUG_HealthPierce, DEBUG_HealthPierceType);
        DEBUG_AdjustedResist = CalculateReduction(effectiveResist, DEBUG_EnergyDamage, DEBUG_EnergyPierceType);
    }

    [Header("DEBUG | Resistance Adder")]
    [SerializeField] private float DEBUG_AddArmour;
    [SerializeField] private float DEBUG_AddResist;

    [ContextMenu("DEBUG_AddResistances")]
    private void DEBUG_AddResistances()
    {
        Armour += DEBUG_AddArmour;
        Resist += DEBUG_AddResist;
    }

    [ContextMenu("DEBUG_AddNativeResistances")]
    private void DEBUG_AddNativeResistances()
    {
        __NativeArmour += DEBUG_AddArmour;
        __NativeResist += DEBUG_AddResist;
    }
}
