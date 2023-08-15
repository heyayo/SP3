using System;

[Serializable]
public abstract class Affliction
{
    public Mortality target;
    public float lifetime;

    public Affliction(float pLifetime, Mortality pTarget)
    {
        lifetime = pLifetime;
        target = pTarget;
    }
    public abstract void Update();
}
