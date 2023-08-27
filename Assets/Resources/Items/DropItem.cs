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
    private Dictionary<int, int> itemList = new Dictionary<int, int>();
    private bool itemsDropped;

    private void Start()
    {
        GetComponent<Mortality>().onHealthZero.AddListener(OnDeath);
        itemsDropped = false;

        // Init the itemList
        int i = 0;
        foreach (Item item in items)
        {
            itemList.Add(i, chances[i]);
            i++;
        }
    }

    public void OnDeath()
    {
        if (itemsDropped)
            return;

        for(int i = 0; i < items.Length; i++)
        {
            int rng = Random.Range(1, 101);

            if (itemList[i] >= rng)
            {
                Vector2 dir = new Vector2(Random.Range(-360, 360), Random.Range(-360, 360)).normalized;
                GameObject dropped = Instantiate(pickupItemPrefab, transform.position, Quaternion.identity);
                dropped.GetComponent<Rigidbody2D>().AddForce(dir * Random.Range(1f, 2f), ForceMode2D.Impulse);
                dropped.GetComponent<PickupItem>().item = items[i].Clone();
            }
        }

        itemsDropped = true;
    }
}
