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

    private void Awake()
    {
        // Create the events
        onHealthAdjust = new UnityEvent();
        onActiveEnergyAdjust = new UnityEvent();
        onStoredEnergyAdjust = new UnityEvent();
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        activeEnergy = activeMax;
        storedEnergy = storedMax;
        health = healthMax;
    }

    // Update is called once per frame
    private void Update()
    {
        // Mortality Regeneration
        activeEnergy += activeRegen * Time.deltaTime;
        storedEnergy += storedRegen * Time.deltaTime;
        health += regen * Time.deltaTime;

        for (int i = 0; i < _afflictions.Count; ++i)
        {
            _afflictions[i].Update();
            _afflictions[i].lifetime -= Time.deltaTime;
            if (_afflictions[i].lifetime <= 0)
                _afflictions.RemoveAt(i);
        }
    }

    // Apply StoredEnergyDrain Affliction
    public void DrainStoredEnergy(float cost, float rate)
    {
        _afflictions.Add(new StoredEnergyDrain(cost,rate,this));
    }
    // Apply ActiveEnergy Affliction
    public void DrainActiveEnergy(float cost, float rate)
    {
        _afflictions.Add(new ActiveEnergyDrain(cost,rate,this));
    }
    // Apply HealthDrain Affliction
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
    [ContextMenu("DEBUG_FullHealth")]
    private void DEBUG_FullHealth()
    {
        health = healthMax;
    }
    [ContextMenu("DEBUG_DrainHealth")]
    private void DEBUG_DrainHealth()
    {
        DrainHealth(50,5);
    }
}
