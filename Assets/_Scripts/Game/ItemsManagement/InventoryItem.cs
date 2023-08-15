using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item;
    public Image image;

    [HideInInspector]
    public Transform parentAfterDrag;

    void Start()
    {
        InitializeItem(item);
    }

    public void Add(Item item, int count)
    {
        if (item == null)
        {
            InitializeItem(item);
            item.itemCount = count;
            return;
        }

        item.itemCount += count;
    }

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.itemSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(parentAfterDrag);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}