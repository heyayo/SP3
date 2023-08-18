using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] protected TMP_Text description;
    [SerializeField] protected Image descriptionImg;

    public Image img
    {
        get => descriptionImg;
        set { descriptionImg = value; }
    }

    public InventoryItem GetHeldItem()
    {
        return GetComponentInChildren<InventoryItem>();
    }

    virtual public void OnDrop(PointerEventData eventData)
    {
        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        // There's an item being dragged
        if (draggedItem != null)
        {
            InventoryItem existingItem = GetComponentInChildren<InventoryItem>();

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

        //// There's an item being dragged
        //if (transform.childCount == 0)
        //{
        //    InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        //    inventoryItem.parentAfterDrag = gameObject.transform;
        //}

        //// Item in slot (Swap)
        //else if (transform.childCount > 0)
        //{
        //    InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        //    InventoryItem swapItem = transform.GetComponentInChildren<InventoryItem>();

        //    swapItem.transform.SetParent(inventoryItem.transform.parent);
        //    inventoryItem.parentAfterDrag = gameObject.transform;
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Update item description and image
        if (GetComponentInChildren<InventoryItem>() != null)
        {
            description.text = GetComponentInChildren<InventoryItem>().item.itemDescription;
            descriptionImg.sprite = GetComponentInChildren<InventoryItem>().item.itemSprite;
            return;
        }

        description.text = "";
        descriptionImg.sprite = null;
    }
}
