using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text description;

    [SerializeField]
    private Image descriptionImg;

    public void OnDrop(PointerEventData eventData)
    {
        // No item in slot
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = gameObject.transform;
        }

        // Item in slot (Swap)
        else if (transform.childCount > 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            InventoryItem swapItem = transform.GetComponentInChildren<InventoryItem>();

            swapItem.transform.SetParent(inventoryItem.transform.parent);
            inventoryItem.parentAfterDrag = gameObject.transform;
        }
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
