using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGore : MonoBehaviour
{
    [SerializeField]
    private GameObject gorePrefab;

    [SerializeField]
    private Sprite[] gore;

    private bool spawnedGore;

    private void Start()
    {
        spawnedGore = false;
        GetComponent<Mortality>().onHealthZero.AddListener(SpawnGore);
    }

    private void SpawnGore()
    {
        // To prevent event for happening for multiple frames
        if (spawnedGore)
            return;

        Vector2 dir;

        foreach (Sprite sprite in gore)
        {
            dir = new Vector2(Random.Range(-360, 360), Random.Range(-360, 360)).normalized;
            GameObject go = Instantiate(gorePrefab, gameObject.transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().AddForce(dir * Random.Range(3, 6), ForceMode2D.Impulse);
            go.GetComponent<SpriteRenderer>().sprite = sprite;
            go.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-200, 200));
        }

        spawnedGore = true;
    }
}
