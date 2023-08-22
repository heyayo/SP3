using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInit : ScriptableObject
{
    // Private variables
    protected Transform transform;

    public void Init(Transform transform)
    {
        this.transform = transform;
    }

    virtual public void BossInitialize()
    { 
    }
}
