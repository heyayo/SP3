using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Item Data")]
    [SerializeField] public string itemName;
    [SerializeField] public Sprite itemSprite;
    [TextArea] [SerializeField]
    public string itemDescription = "null";

    [Header("Equipment")]
    [SerializeField] public EQUIPTYPE EquipType;
    [SerializeField] public float armor = 0f;
    [SerializeField] public float resist = 0f;

    public UnityEvent consumed;

    public enum EQUIPTYPE
    { 
        NONE,
        HEAD,
        CHEST,
        LEGS,
        FEET,
        PET
    }

    private void OnEnable()
    {
        if (consumed == null)
            consumed = new UnityEvent();
    }

    // Virtual function
    public virtual void Use()
    {
        Debug.Log("This Item does not have a use");
    }

    public virtual void Setup(GameObject pickupItem)
    {
        
    }
}
