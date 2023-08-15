using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable
{
    bool isAggroed { get; set; }
    bool isInStrikingDistance { get; set; }
    void SetAggroStatus(bool isAggroed);
    void SetStrikingDistance(bool isInStrikingDistance);
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
