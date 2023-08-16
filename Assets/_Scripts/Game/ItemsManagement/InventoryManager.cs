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

    // Private Variables
    private bool toggle = false;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
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
}
