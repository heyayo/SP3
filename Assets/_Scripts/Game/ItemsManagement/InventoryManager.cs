using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private Configuration _config;
    private PlayerManager _player;
    
    [Header("Slot Access")]
    public InventorySlot[] inventorySlots;
    public ArmorSlot[] armourSlots;

    [SerializeField] public InventoryItem inventoryItemPrefab;
    [SerializeField] private GameObject inventoryParent;
    [SerializeField] private PickupItem pickupItemPrefab;

    // Private Variables
    private bool toggle = false;

    private void Start()
    {
        _config = Configuration.FetchConfig();
        inventoryParent.SetActive(false);
        _player = PlayerManager.Instance;
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.slotEdited.AddListener(UpdatePlayerStats);
        }

        foreach (ArmorSlot slot in armourSlots)
        {
            slot.slotEdited.AddListener(UpdatePlayerStats);
        }
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
            if (slot.GetHeldItem() == null)
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
            InventorySlot activeSlot = HotbarManager.Instance.activeSlot;
            InventoryItem drop = activeSlot.GetHeldItem();
            if (drop != null && drop.item.droppable)
            {
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
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, slot.transform);
        inventoryItem.InitializeItem(item.Clone());
    }

    private void DropItem(Item item)
    {
        // Drop item on the floor
        Transform player = _player.transform;

        // Direction to drop
        Vector2 direction = new Vector2(0, 0);
        //direction = player.gameObject.GetComponent<Movement>().facing;
        direction.x = _player.transform.localScale.x;

        PickupItem pickupItem = Instantiate(pickupItemPrefab,
            (Vector2)player.position + direction * 5,
            Quaternion.identity);
        pickupItem.item = item; // Assign Item
        pickupItem.SetSprite(item.itemSprite); // Assign Sprite
        pickupItem.GetComponent<Rigidbody2D>().AddForce(direction * 25,ForceMode2D.Impulse); // Apply Throw Force
        pickupItem.item.Setup(pickupItem.gameObject);
    }

    private void UpdatePlayerStats()
    {
        Mortality playerMortality = _player.GetComponent<Mortality>();
        
        // Default values
        float healthMax = playerMortality.__NativeHealthMax;
        float armor = playerMortality.__NativeArmour;
        float resist = playerMortality.__NativeResist;
        float attack = 0;

        foreach (ArmorSlot slot in armourSlots)
        {
            InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();
            if (inventoryItem != null)
            {
                Item item = inventoryItem.item;
                if (item != null)
                {
                    healthMax += item.health;
                    armor += item.armor;
                    resist += item.resist;
                    attack += item.attack;
                }
            }
        }

        playerMortality.__HealthMax = healthMax;
        playerMortality.Armour = armor;
        playerMortality.Resist = resist;
        // Attack = attack;

        // Reset health if it is above the max health
        if (playerMortality.Health > playerMortality.__HealthMax)
            playerMortality.Health = playerMortality.__HealthMax;
    }
}
