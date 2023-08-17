using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance { get; private set; }
    
    private Configuration _config;
    
    [SerializeField]
    private InventorySlot[] hotbarSlots;

    [SerializeField]
    private GameObject selector;

    // Private variables
    private InventorySlot activeSlot;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Hotbar Managers In Scene");
            Debug.Break();
        }
        Instance = this;
        _config = Configuration.FetchConfig();
    }
    
    private void Start()
    {
        Select(1);
    }

    void Update()
    {
        // Changing inventory slots
        if (Input.GetKeyDown(_config.hotbar1))
            Select(1);
        else if (Input.GetKeyDown(_config.hotbar2))
            Select(2);
        else if (Input.GetKeyDown(_config.hotbar3))
            Select(3);
        else if (Input.GetKeyDown(_config.hotbar4))
            Select(4);

        // Using item
        if (Input.GetKeyDown(KeyCode.E) && (activeSlot
            .GetComponentInChildren<InventoryItem>() != null))
            activeSlot.GetComponentInChildren<InventoryItem>().item.Use();
    }

    private void Select(int index)
    {
        // Select the slot
        int selected = index - 1;
        activeSlot = hotbarSlots[selected];

        // Default all color of all slots
        foreach (InventorySlot slot in hotbarSlots)
        {
            slot.img.color = Color.white;
        }

        // Except the selected one
        activeSlot.img.color = new Color(100, 100, 100);

        // Change selector position
        selector.transform.SetParent(activeSlot.transform);
    }

    public Item GetActiveItem()
    {
        if (activeSlot.HeldItem != null)
            return activeSlot.HeldItem.item;
        return null;
    }
}
