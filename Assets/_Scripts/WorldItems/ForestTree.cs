using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Mortality))]
public class ForestTree : Interactable
{
    [Header("Trees' stats")]
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
        await Task.Delay((int)(respawnTimer * 1000f));
        gameObject.SetActive(true);
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
