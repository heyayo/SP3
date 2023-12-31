using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingTable : MonoBehaviour
{

    [Header("Everything else")]
    [SerializeField]
    private InventoryManager inventoryManager;

    [SerializeField]
    private InventorySlot[] inventorySlots;

    // Private Variables
    private string[][] craftingSlots;
    private CraftingRecipe[] recipes;

    void Start()
    {
        // Init the double array
        craftingSlots = new string[3][];

        for (int i = 0; i < 3; i++)
        {
            craftingSlots[i] = new string[3];
            for (int n = 0; n < 3; n++)
            {
                int index = i * 3 + n;
                craftingSlots[i][n] = "null";
            }
        }

        // Put everything into here
        recipes = Resources.LoadAll<CraftingRecipe>("Crafting Recipes/");
    }

    public void Craft()
    {
        // Update the array
        for (int i = 0; i < 3; i++)
        {
            for (int n = 0; n < 3; n++)
            {
                int index = i * 3 + n;

                // Only add to the array if there is an inventoryItem there
                if (inventorySlots[index].GetComponentInChildren<InventoryItem>() == null)
                    craftingSlots[i][n] = "null";
                else
                    craftingSlots[i][n] = inventorySlots[index].GetComponentInChildren<InventoryItem>().item.itemName;
            }
        }

        // Checking through which recipe matches
        foreach (CraftingRecipe recipe in recipes)
        {
            if (CompareRecipe(recipe.Recipe()))
            {
                inventoryManager.Add(recipe.YieldItem());
                ClearTable();
            }
        }
    }

    private bool CompareRecipe(string[][] arr)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int n = 0; n < 3; n++)
            {
                if (craftingSlots[i][n] != arr[i][n])
                    return false;
            }
        }

        return true;
    }

    private void ClearTable()
    {
        for (int i = 0; i < 9; i++)
        {
            if (inventorySlots[i].GetComponentInChildren<InventoryItem>() != null)
                Destroy(inventorySlots[i].GetComponentInChildren<InventoryItem>().gameObject);
        }
    }
}
