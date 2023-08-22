using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance { get; private set; }
    private Configuration _config;
    
    [SerializeField]
    private InventorySlot[] hotbarSlots;

    [SerializeField]
    private GameObject selector;

    [HideInInspector]
    public InventorySlot activeSlot;

    private void Awake()
    {
        selector.SetActive(false);
        if (Instance != null)
        {
            Debug.LogError("Multiple Hotbar Managers In Scene");
            Debug.Break();
        }
        Instance = this;
        _config = Configuration.FetchConfig();
    }

    private void Start()
    {
        Select(1);
    }

    void Update()
    {
        // Changing inventory slots
        if (Input.GetKeyDown(_config.hotbar1))
            Select(1);
        else if (Input.GetKeyDown(_config.hotbar2))
            Select(2);
        else if (Input.GetKeyDown(_config.hotbar3))
            Select(3);
        else if (Input.GetKeyDown(_config.hotbar4))
            Select(4);
        
        // Using item
        if (
            Input.GetKeyDown(_config.interact) && 
            (activeSlot.GetComponentInChildren<InventoryItem>() != null))
            activeSlot.GetComponentInChildren<InventoryItem>().item.Use();
    }

    private void Select(int index)
    {
        selector.SetActive(true);

        // Select the slot
        int selected = index - 1;
        activeSlot = hotbarSlots[selected];

        // Change selector position
        selector.transform.position = activeSlot.transform.position;
    }
}
