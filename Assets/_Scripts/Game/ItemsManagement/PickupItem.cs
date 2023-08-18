using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PickupItem : MonoBehaviour
{
    [SerializeField] public Item item;

    // Private Variables
    private InventoryManager inventoryManager;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        inventoryManager = InventoryManager.Instance;

        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        _rb.drag = 2;
    }

    public void SetSprite(Sprite sprite)
    { _spriteRenderer.sprite = sprite; }

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
