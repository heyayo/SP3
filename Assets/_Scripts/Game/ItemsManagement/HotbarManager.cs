using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    [SerializeField]
    private InventorySlot[] hotbarSlots;

    [SerializeField]
    private GameObject selector;

    // Private variables
    private InventorySlot activeSlot;

    private void Start()
    {
        Select(1);
    }

    void Update()
    {
        // Changing inventory slots
        if (Input.GetKeyDown(KeyCode.Z))
            Select(1);
        else if (Input.GetKeyDown(KeyCode.X))
            Select(2);
        else if (Input.GetKeyDown(KeyCode.C))
            Select(3);
        else if (Input.GetKeyDown(KeyCode.V))
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
            slot.GetComponent<Image>().color = new Color(255, 255, 255);
        }

        // Except the selected one
        activeSlot.GetComponent<Image>().color = new Color(100, 100, 100);

        // Change selector position
        selector.transform.SetParent(activeSlot.transform);
    }
}
