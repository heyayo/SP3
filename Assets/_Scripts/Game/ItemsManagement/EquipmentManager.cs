using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField]
    private ArmorSlot[] armorSlots;

    [SerializeField]
    private Mortality mortality;

    // Private Variables
    float defaultArmor = 0f;
    float defaultResist = 0f;

    private void Start()
    {
        defaultArmor = mortality.armour;
        defaultResist = mortality.resist;

        for (int i = 0; i < armorSlots.Length; i++)
        {
            armorSlots[i].slotEdited.AddListener(UpdateStats);
            armorSlots[i].slotRemove.AddListener(ClearStats);
        }
    }

    private void UpdateStats()
    {
        ClearStats();

        // Adding the armor and resistance to the player
        for (int i = 0; i < armorSlots.Length; i++)
        {
            InventoryItem current = armorSlots[i].GetComponentInChildren<InventoryItem>();
            if (current != null)
            {
                mortality.armour += current.item.armor;
                mortality.resist += current.item.resist;
            }
        }
    }

    private void ClearStats()
    {
        mortality.armour = defaultArmor;
        mortality.resist = defaultResist;
    }
}
