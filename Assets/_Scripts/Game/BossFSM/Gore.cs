using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gore : MonoBehaviour
{
    [SerializeField]
    private GameObject gorePrefab;

    [SerializeField]
    private Sprite[] gore;

    private void Start()
    {
        GetComponent<Mortality>().onHealthZero.AddListener(SpawnGore);
    }

    private void SpawnGore()
    {
        Vector2 dir;

        foreach (Sprite sprite in gore)
        {
            dir = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
            GameObject go = Instantiate(gorePrefab, gameObject.transform.position, Quaternion.identity);
            go.GetComponent<Rigidbody2D>().AddForce(dir * Random.Range(30, 50), ForceMode2D.Impulse);
            go.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
