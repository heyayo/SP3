using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item Data")]
    [SerializeField]
    public string itemName;
    
    [SerializeField]
    public Sprite itemSprite;

    [SerializeField]
    public EQUIPTYPE EquipType;

    [SerializeField]
    public int itemCount;

    [TextArea]
    [SerializeField]
    public string itemDescription = "null";

    public enum EQUIPTYPE
    { 
        NONE,
        HEAD,
        CHEST,
        LEGS,
        FEET,
        PET
    }

    void OnEnable()
    {
        itemCount = 0;
    }

    public void SetData(Sprite itemSprite, int itemCount)
    {
        this.itemSprite = itemSprite;
        this.itemCount = itemCount;
    }

    // Virtual function
    virtual public void Use()
    {
    }
}
