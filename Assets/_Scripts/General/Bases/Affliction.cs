using System;
using UnityEngine;

[Serializable]
public abstract class Affliction
{
    public Mortality target;
    public float lifetime;
    public Sprite icon;

    public Affliction(float pLifetime, Mortality pTarget)
    {
        lifetime = pLifetime;
        target = pTarget;
        icon = Resources.Load<Sprite>("Sprites/Items/null");
        Debug.Log(icon);
    }
    public abstract void Begin();
    public abstract void Update();
    public abstract void End();
}
