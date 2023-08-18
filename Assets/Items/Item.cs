using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item Data")]
    [SerializeField] public string itemName;
    [SerializeField] public Sprite itemSprite;
    [TextArea] [SerializeField]
    public string itemDescription = "null";

    [Header("Equpiment")]
    [SerializeField] public EQUIPTYPE EquipType;
    [SerializeField] public float armor = 0f;
    [SerializeField] public float resist = 0f;

    public enum EQUIPTYPE
    { 
        NONE,
        HEAD,
        CHEST,
        LEGS,
        FEET,
        PET
    }

    // Virtual function
    public virtual void Use()
    {
        Debug.Log("This Item does not have a use");
    }
}
