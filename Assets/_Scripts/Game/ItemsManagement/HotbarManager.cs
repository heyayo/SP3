using UnityEngine;
using UnityEngine.EventSystems;

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
        // "Update function" for the items
        InventoryItem activeItem = activeSlot.GetComponentInChildren<InventoryItem>();
        if (activeItem != null)
            activeItem.item.WhileHolding();

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
        if (Input.GetKeyDown(_config.attack) && 
            (activeSlot.GetComponentInChildren<InventoryItem>() != null) &&
            !IsPointerOverUIElement())
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

    private bool IsPointerOverUIElement()
    {
        // Check if there is an EventSystem in the scene
        if (EventSystem.current == null)
            return false;

        // Create a pointer event data with the current mouse position
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        // Create a list to receive raycast results
        var results = new System.Collections.Generic.List<RaycastResult>();

        // Raycast against the UI using the pointer event data
        EventSystem.current.RaycastAll(eventData, results);

        // Check if any UI elements were hit
        return results.Count > 0;
    }
}
