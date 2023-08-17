using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBat : Enemy
{
    private void Awake()
    {
        InitAwake();
    }
    // Start is called before the first frame update
    void Start()
    {   
        InitStart();
    }
}
