using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ArmorSlot : InventorySlot, IBeginDragHandler
{
    // Event
    public UnityEvent slotEdited;
    public UnityEvent slotRemove;
    
    // Changed to awake to prevent usage in Start() functions before initialization
    private void Awake()
    {
        if (slotEdited == null)
            slotEdited = new UnityEvent();

        if (slotRemove == null)
            slotRemove = new UnityEvent();
    }

    override public void OnDropItem(InventoryItem draggedItem)
    {
        // There's an item being dragged
        if (draggedItem != null)
        {
            InventoryItem existingItem = GetComponentInChildren<InventoryItem>();

            if (draggedItem.item.EquipType != Item.EQUIPTYPE.EQUIPPABLE)
                return;

            // Slot is empty
            if (existingItem == null)
            {
                draggedItem.transform.SetParent(transform);
                draggedItem.parentAfterDrag = transform;
            }

            // Check if the dragged item and existing item are both armor pieces
            else if (draggedItem.item.EquipType == Item.EQUIPTYPE.EQUIPPABLE)
            {
                Transform draggedItemParent = draggedItem.transform.parent;
                Transform existingItemParent = existingItem.transform.parent;

                existingItem.parentAfterDrag = draggedItemParent;
                draggedItem.parentAfterDrag = existingItemParent;

                existingItem.transform.SetParent(draggedItemParent);
                draggedItem.transform.SetParent(existingItemParent);

                existingItem.transform.position = draggedItemParent.position;
                draggedItem.transform.position = existingItemParent.position;
            }

            slotEdited.Invoke();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        slotRemove.Invoke();
        slotEdited.Invoke();
    }
}
