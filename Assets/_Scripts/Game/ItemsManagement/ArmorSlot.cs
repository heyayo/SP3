using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorSlot : InventorySlot, IDropHandler
{
    [Header("Equip Type")]
    [SerializeField]
    private Item.EQUIPTYPE equipType;

    override public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        // There's an item being dragged and it fits the armor slot type
        if (draggedItem != null)
        {
            InventoryItem existingItem = GetComponentInChildren<InventoryItem>();

            if (draggedItem.item.EquipType != equipType)
                return;

            // Slot is empty
            if (existingItem == null)
            {
                draggedItem.transform.SetParent(transform);
                draggedItem.parentAfterDrag = transform;
            }

            // Swap items between the slots
            else
            {
                Transform oldParent = draggedItem.parentAfterDrag;
                draggedItem.parentAfterDrag = existingItem.parentAfterDrag;
                existingItem.parentAfterDrag = oldParent;

                draggedItem.transform.SetParent(existingItem.parentAfterDrag);
                existingItem.transform.SetParent(oldParent);
            }
        }

        //// No item in slot
        //if (transform.childCount == 0)
        //{
        //    InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        //    // Check equip type before being able to put it there
        //    if (inventoryItem.GetComponent<InventoryItem>().item.EquipType == equipType)
        //        inventoryItem.parentAfterDrag = gameObject.transform;
        //}

        //// Item in slot (Swap)
        //else if (transform.childCount > 0)
        //{
        //    InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        //    InventoryItem swapItem = transform.GetComponentInChildren<InventoryItem>();

        //    // Check equip type before being able to put it there
        //    if (inventoryItem.GetComponent<InventoryItem>().item.EquipType == equipType)
        //    {
        //        swapItem.transform.SetParent(inventoryItem.transform.parent);
        //        inventoryItem.parentAfterDrag = gameObject.transform;
        //    }
        //}
    }
}
