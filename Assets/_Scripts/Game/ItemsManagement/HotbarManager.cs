using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    private Configuration _config;
    
    [SerializeField]
    private InventorySlot[] hotbarSlots;

    [SerializeField]
    private GameObject selector;

    // Private variables
    private InventorySlot activeSlot;

    private void Awake()
    {
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
}
