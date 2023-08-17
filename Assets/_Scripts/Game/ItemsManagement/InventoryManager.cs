using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    
    [Header("Slot Access")]
    public InventorySlot[] inventorySlots;
    public ArmorSlot[] armourSlots;

    [SerializeField]
    public GameObject inventoryItemPrefab;

    [SerializeField]
    private GameObject inventoryParent;

    [SerializeField]
    private GameObject PickupItemPrefab;

    // Private Variables
    private bool toggle = false;
    private Configuration _config;

    private void Start()
    {
        _config = Configuration.FetchConfig();
        inventoryParent.SetActive(false);
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Inventory Managers In Scene");
            Debug.Break();
        }
        Instance = this;
    }
    
    public void Add(Item item)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.GetComponentInChildren<InventoryItem>() == null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }

        DropItem(item);
    }

    private void Update()
    {
        // Dropping items
        if (Input.GetKeyDown(_config.dropItem))
        {
            InventorySlot activeSlot = gameObject.GetComponent<HotbarManager>().activeSlot;
            if (activeSlot.GetComponentInChildren<InventoryItem>() != null)
            {
                InventoryItem drop = activeSlot.GetComponentInChildren<InventoryItem>();
                DropItem(drop.item);
                Destroy(drop.gameObject);
            }
        }

        // opening and closing inventory
        if (Input.GetKeyDown(_config.openInventory))
        {
            toggle = !toggle;
            inventoryParent.SetActive(toggle);
        }
    }

    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    private void DropItem(Item item)
    {
        // Drop item on the floor
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        // Direction to drop
        Vector2 direction = new Vector2(0, 0);
        direction = player.gameObject.GetComponent<Movement>().facing;

        GameObject droppedItem = Instantiate(PickupItemPrefab, new Vector2(player.position.x, player.position.y)
            + direction * 1.2f, Quaternion.identity);

        // PickupItem
        PickupItem pickupItem = droppedItem.GetComponent<PickupItem>();
        pickupItem.item = item;

        // Sprite
        SpriteRenderer spriteRenderer = droppedItem.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = item.itemSprite;

        // Rigidbody2D
        Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 200);
    }
}
