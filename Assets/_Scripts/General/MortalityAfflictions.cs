using System;
using UnityEditor.Tilemaps;
using UnityEngine;

[Serializable]
public abstract class MortalityDrain : Affliction
{
    public float totalDrain;
    protected float _drainTracker;
    protected float _drainRate;

    public MortalityDrain(float drain, float lifetime, Mortality target) : base(lifetime, target)
    {
        totalDrain = drain;
        _drainRate = drain / lifetime;
        _drainTracker = 0;
    }
}

[Serializable]
public class StoredEnergyDrain : MortalityDrain
{
    public StoredEnergyDrain(float total, float lifetime, Mortality target) : base(total, lifetime, target) {}
    public override void Begin() { }
    public override void Update()
    {
        float drain = _drainRate * Time.deltaTime;
        _drainTracker += drain;
        // Prevent Lag Spike Issues
        if (_drainTracker > totalDrain)
        {
            lifetime = 0;
            drain = _drainTracker - totalDrain;
        }

        target.StoredEnergy -= drain;
    }
    public override void End() { }
}

[Serializable]
public class ActiveEnergyDrain : MortalityDrain
{
    public ActiveEnergyDrain(float total, float lifetime, Mortality target) : base(total, lifetime, target) {}
    public override void Begin() { }
    public override void Update()
    {
        float drain = _drainRate * Time.deltaTime;
        _drainTracker += drain;
        // Prevent Lag Spike Issues
        if (_drainTracker > totalDrain)
        {
            lifetime = 0;
            drain = _drainTracker - totalDrain;
        }

        target.ActiveEnergy -= drain;
    }
    public override void End() { }
}

[Serializable]
public class HealthDrain : MortalityDrain
{
    public HealthDrain(float total, float lifetime, Mortality target) : base(total, lifetime, target) {}
    public override void Begin() { }
    public override void Update()
    {
        float drain = _drainRate * Time.deltaTime;
        _drainTracker += drain;
        // Prevent Lag Spike Issues
        if (_drainTracker > totalDrain)
        {
            lifetime = 0;
            drain = _drainTracker - totalDrain;
        }

        target.Health -= drain;
    }
    public override void End() { }
}

public class Immunity : Affliction
{
    public Immunity(float lifetime, Mortality target) : base(lifetime, target) {}
    public override void Begin() { }
    public override void Update()
    {
        target.EnableImmunity();
    }
    public override void End() { target.DisableImmunity(); target.ApplyAffliction(new ImmunityBurn(1,target)); }
}

public class ImmunityBurn : Affliction
{
    public ImmunityBurn(float pLifetime, Mortality pTarget) : base(pLifetime, pTarget) { }
    public override void Begin() { }
    public override void Update()
    {
        target.DisableImmunity();
    }
    public override void End() { }
}
