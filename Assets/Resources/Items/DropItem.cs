using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private GameObject pickupItemPrefab;

    [Header("Drop Items")]
    [SerializeField]
    private Item[] items;

    [Header("Drop Chances (1 - 100)")]
    [SerializeField]
    private int[] chances;

    // Private variables
    private Dictionary<Item, int> itemList = new Dictionary<Item, int>();

    private void Start()
    {
        // Init the itemList
        int i = 0;
        foreach (Item item in items)
        {
            itemList.Add(item, chances[i]);
            i++;
        }
    }

    public void OnDeath()
    {
        int rng = Random.Range(1, 101);

        foreach (Item item in items)
        { 
            if (itemList[item] <= rng)
            {
                Vector2 dir = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                GameObject dropped = Instantiate(pickupItemPrefab, transform.position, Quaternion.identity);
                dropped.GetComponent<Rigidbody2D>().AddForce(dir * 20);
            }
        }
    }
}
