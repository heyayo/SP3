using UnityEngine;

public class ForestTree : Interactable
{
    [Header("Trees' stats")]
    [SerializeField] private int chopProgress;
    [SerializeField] private int treeStrength;

    [Header("Necessary Stuff")]
    [SerializeField]
    private Item yieldItem;

    // Inventory Manager
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
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
}
