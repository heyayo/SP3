using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    [Header("Row 1")]
    [SerializeField]
    private Item[] recipeR1;

    [Header("Row 2")]
    [SerializeField]
    private Item[] recipeR2;

    [Header("Row 3")]
    [SerializeField]
    private Item[] recipeR3;

    [Header("Item Yield")]
    [SerializeField]
    private Item yieldItem;

    // Private Variables
    private string[][] recipe;

    void OnEnable()
    {
        // Init the recipe
        recipe = new string[3][];
        for (int i = 0; i < 3; i++)
            recipe[i] = new string[3];

        for (int i = 0; i < 3; i++)
        {
            // Row 1
            if (recipeR1[i] != null)
                recipe[0][i] = recipeR1[i].itemName;
            else
                recipe[0][i] = "null";

            // Row 2
            if (recipeR2[i] != null)
                recipe[1][i] = recipeR2[i].itemName;
            else
                recipe[1][i] = "null";

            // Row 3
            if (recipeR3[i] != null)
                recipe[2][i] = recipeR3[i].itemName;
            else
                recipe[2][i] = "null";
        }
    }

    public string[][] Recipe()
    {
        return recipe;
    }

    public Item YieldItem()
    {
        return yieldItem;
    }
}
