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
    [SerializeField] public float health = 0f;
    [SerializeField] public float armor = 0f;
    [SerializeField] public float resist = 0f;
    [SerializeField] public float attack = 0f;

    [HideInInspector]
    public UnityEvent consumed;

    public enum EQUIPTYPE
    { 
        NONE,
        EQUIPPABLE
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
}
