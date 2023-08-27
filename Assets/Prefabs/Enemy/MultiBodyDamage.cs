using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBodyDamage : MonoBehaviour
{
    [SerializeField]
    private GameObject[] body;

    [SerializeField]
    private string bossName;

    private List<Mortality> mortalities = new List<Mortality>();

    public GameObject endCreditScene;

    void Start()
    {
        foreach (GameObject go in body)
        {
            mortalities.Add(go.GetComponent<Mortality>());

            go.GetComponent<Damagable>().onHit.AddListener(SyncHealth);
            var mortality = go.GetComponent<Mortality>();
            
            mortality.onHealthZero.AddListener(Death);
        }
    }

    private void SyncHealth()
    {
        float leastHealth = LeastHealth();

        foreach (Mortality mortality in mortalities)
        {
            mortality.Health = leastHealth;
        }
    }

    private void Death()
    {
        BossManager.Instance.KillBoss(bossName);
        Destroy(gameObject);
    }

    private float LeastHealth()
    {
        float lowestHealth = mortalities[0].Health;
        
        for (int i = 0; i < mortalities.Count; i++)
        {
            if (lowestHealth > mortalities[i].Health)
            {
                lowestHealth = mortalities[i].Health;
            }
        }

        return lowestHealth;
    }
}
