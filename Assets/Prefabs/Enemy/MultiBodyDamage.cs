using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBodyDamage : MonoBehaviour
{
    private GameObject[] body;
    private List<Mortality> mortalities = new List<Mortality>();

    void Start()
    {
        foreach (Mortality mortality in GetComponentsInChildren<Mortality>())
        {
            mortalities.Add(mortality);
        }

        foreach (GameObject go in GetComponentsInChildren<GameObject>())
        {
            go.GetComponent<Damagable>().hit.AddListener(SyncHealth);
            go.GetComponent<Mortality>().onHealthZero.AddListener(Death);
        }
    }

    private void FixedUpdate()
    {
        foreach (Mortality mortality in mortalities)
            Debug.Log(mortality.Health);
    }

    private void SyncHealth()
    {
        foreach (Mortality mortality in mortalities)
        {
            mortality.Health = LeastHealth();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    private float LeastHealth()
    {
        float lowestHealth = mortalities[0].Health;
        
        foreach(Mortality mortality in mortalities)
        {
            if (mortality.Health < lowestHealth)
            {
                lowestHealth = mortality.Health;
            }
        }

        return lowestHealth;
    }
}
