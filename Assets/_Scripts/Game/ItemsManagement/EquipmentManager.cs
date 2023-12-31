using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] private ArmorSlot[] armorSlots;
    [SerializeField] private Mortality mortality;

    // Private Variables
    float defaultArmor = 0f;
    float defaultResist = 0f;

    private void Start()
    {
        defaultArmor = mortality.Armour;
        defaultResist = mortality.Resist;

        for (int i = 0; i < armorSlots.Length; i++)
        {
            armorSlots[i].slotEdited.AddListener(UpdateStats);
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
                mortality.Armour += current.item.armor;
                mortality.Resist += current.item.resist;
            }
        }
    }

    private void ClearStats()
    {
        mortality.Armour = mortality.__NativeArmour;
        mortality.Resist = mortality.__NativeResist;
    }
}
