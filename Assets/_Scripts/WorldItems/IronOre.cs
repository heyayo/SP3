using UnityEngine;
using Task = System.Threading.Tasks.Task;

[RequireComponent(typeof(Mortality))]
public class IronOre : Interactable
{
    [Header("Ore' stats")]
    [SerializeField] private int chopProgress;
    [SerializeField] private int treeStrength;

    [Header("Necessary Stuff")]
    [SerializeField] private Item yieldItem;
    [SerializeField] private float respawnTimer;

    // Inventory Manager
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;
        _mortality.onHealthZero.AddListener(StartToRespawn);
    }

    private async void StartToRespawn()
    {
        gameObject.SetActive(false);
        inventoryManager.Drop(yieldItem,transform.position);
        _mortality.ResetToMax();
        await Task.Delay((int)(respawnTimer * 1000f));
        gameObject.SetActive(true);
    }

    public override void OnInteract()
    {
    }

    public override void OnMouseEnter()
    {
    }

    public override void OnMouseExit()
    {
    }
}
