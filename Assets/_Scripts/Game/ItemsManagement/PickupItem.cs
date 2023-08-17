using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField]
    public Item item;

    // Private Variables
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();

        if (gameObject.GetComponentInChildren<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();  // Every pickupItem should have a rigidbody
            rb.gravityScale = 0;
            rb.drag = 2;
        }
    }

    private void FixedUpdate()
    {
        Vector2 dir = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        if (dir.magnitude < 3f)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(dir / 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inventoryManager.Add(item);
            Destroy(gameObject);
        }
    }
}
