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
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
    
    void Start()
    {
        inventoryManager = InventoryManager.Instance;

        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        _rb.drag = 2;

        // Set sprite
        _spriteRenderer.sprite = item.itemSprite;
    }

    public void SetSprite(Sprite sprite)
    { _spriteRenderer.sprite = sprite; }

    private void FixedUpdate()
    {
        Vector2 dir = PlayerManager.Instance.transform.position - transform.position;
        if (dir.magnitude < 3f)
        {
            _rb.AddForce(dir / 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Inventory Manager:" + inventoryManager);
        inventoryManager.Add(item);
        Destroy(gameObject);
    }

    public void Setup(Item item)
    {
        this.item = item;
        SetSprite(item.itemSprite);
        this.item.Setup(gameObject);
    }
}
