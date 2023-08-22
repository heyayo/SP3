using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodOfDestroyerBoss : Enemy
{
    public bool isBoss { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        InitAwake();
    }

    // Update is called once per frame
    void Update()
    {
        InitStart();
    }
}
