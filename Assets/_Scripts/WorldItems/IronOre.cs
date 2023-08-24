using UnityEngine;

public class IronOre : Interactable
{
    [Header("Ore' stats")]
    [SerializeField] private int chopProgress;
    [SerializeField] private int treeStrength;

    [Header("Necessary Stuff")]
    [SerializeField] private Item yieldItem;

    // Inventory Manager
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
    }

    public override void OnInteract()
    {
        ++chopProgress;
        if (chopProgress >= treeStrength)
        {
            inventoryManager.Add(yieldItem);
            Destroy(gameObject);
        }
    }

    public override void OnMouseEnter()
    {
    }

    public override void OnMouseExit()
    {
    }
}
