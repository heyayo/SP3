using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mortality : MonoBehaviour
{
    [Header("Active Energy")]
    [SerializeField] private float activeEnergy;
    [SerializeField] private float activeRegen;
    [SerializeField] private float activeMax;

    [Header("Stored Energy")]
    [SerializeField] private float storedEnergy;
    [SerializeField] private float storedRegen;
    [SerializeField] private float storedMax;

    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float regen;
    [SerializeField] private float healthMax;

    [Header("Resistances")]
    [SerializeField] private float armour;
    [SerializeField] private float resist;
    [SerializeField] private AnimationCurve scaling;
    
    public float ActiveEnergy
    {
        get => activeEnergy;
        set
        {
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
            health = value;
            health = Math.Clamp(health, 0, healthMax);
            onHealthAdjust.Invoke();
        }
    }

    [Header("Events")]
    public UnityEvent onHealthAdjust;
    public UnityEvent onActiveEnergyAdjust;
    public UnityEvent onStoredEnergyAdjust;
    
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
    }
    
    private void Start()
    {
        activeEnergy = activeMax;
        storedEnergy = storedMax;
        health = healthMax;
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
                _afflictions.RemoveAt(i);
        }
    }

    private float CalculateReduction(float resistance, float pierce, PierceType pierceType)
    {
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
    
    /*
     * Apply Damage with Consideration for Armour/Shields and other possible effects
     * Damage | Total Damage Energy Damage dealt to the Player
     * ePierce | How much to ignore energy resistance (Armour Piercing)
     * PierceType | The type of piercing to use, either Flat or Percentage Based
     */
    public void ApplyEnergyDamage(float damage, float pierce = 0f, PierceType pierceType = PierceType.Flat)
    {
        float totalReduction = CalculateReduction(resist, pierce, pierceType);
        float finalDamage = damage - (damage * totalReduction);
        ActiveEnergy -= finalDamage;
    }

    public void ApplyHealthDamage(float damage, float pierce = 0f, PierceType pierceType = PierceType.Flat)
    {
        float totalReduction = CalculateReduction(armour, pierce, pierceType);
        float finalDamage = damage - (damage * totalReduction);
        Health -= finalDamage;
    }

    // Apply StoredEnergyDrain Affliction (Deals True StoredEnergy Damage)
    public void DrainStoredEnergy(float cost, float rate)
    {
        _afflictions.Add(new StoredEnergyDrain(cost,rate,this));
    }
    // Apply ActiveEnergy Affliction (Deals True ActiveEnergy Damage)
    public void DrainActiveEnergy(float cost, float rate)
    {
        _afflictions.Add(new ActiveEnergyDrain(cost,rate,this));
    }
    // Apply HealthDrain Affliction (Deals True Health Damage)
    public void DrainHealth(float cost, float rate)
    {
        _afflictions.Add(new HealthDrain(cost,rate,this));
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
        DEBUG_AdjustedArmour = CalculateReduction(armour, DEBUG_HealthPierce, DEBUG_HealthPierceType);
        DEBUG_AdjustedResist = CalculateReduction(resist, DEBUG_EnergyDamage, DEBUG_EnergyPierceType);
    }
}
