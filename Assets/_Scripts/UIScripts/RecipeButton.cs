using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeButton : MonoBehaviour
{
	public CraftingRecipe recipe;

	public Sprite[] recipeSprites;

	public void Initialize()
	{
		recipeSprites = recipe.ItemRecipe();
	}
}
