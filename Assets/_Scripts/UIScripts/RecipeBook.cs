using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    [Header("Recipe Book Objects")]
    [SerializeField] private GameObject sliderContent;
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject slots;
    [SerializeField] private Image[] craftingRecipe;
    
    [Header("Resources")]
    [SerializeField] private GameObject recipe;
    [SerializeField] private CraftingRecipe[] recipes;

    private void Awake()
    {
        recipes = Resources.LoadAll<CraftingRecipe>("Crafting Recipes/");
        craftingRecipe = new Image[9];
        int i = 0;
        foreach (Transform slot in slots.transform)
        {
            craftingRecipe[i] = slot.GetComponent<Image>();
            ++i;
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        FillWithRecipes();
    }

    public void ShowRecipe(Sprite icon, Sprite[] crafting)
    {
        itemIcon.sprite = icon;
        for (int i = 0; i < 9; ++i)
        {
            craftingRecipe[i].sprite = crafting[i];
        }
    }

    public void ToggleRecipeBook()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    private void FillWithRecipes()
    {
        for (int i = 0; i < recipes.Length; ++i)
        {
            var slot = Instantiate(recipe, Vector3.zero, Quaternion.identity);
            var rectTransform = slot.GetComponent<RectTransform>();
            var text = slot.transform.GetChild(1).GetComponent<TMP_Text>();
            var icon = slot.transform.GetChild(0).GetComponent<Image>();
            var button = slot.GetComponent<Button>();

            // Apply Component
            var recipeButton = slot.AddComponent<RecipeButton>();
            
            // Parent to SliderContent
            slot.transform.SetParent(sliderContent.transform,false);
            
            // Set Position
            rectTransform.anchoredPosition = new Vector2(0, -12.5f - (25 * i));
            rectTransform.localScale = Vector3.one;

            // Customization
            text.text = recipes[i].YieldItem().itemName;
            icon.sprite = recipes[i].YieldItem().itemSprite;
            recipeButton.recipe = recipes[i];
            recipeButton.Initialize();
            button.onClick.AddListener(() =>
            {
                var item = slot.GetComponent<RecipeButton>();
                ShowRecipe(item.recipe.YieldItem().itemSprite,item.recipeSprites);
            });
            
            slot.SetActive(true);
        }
    }
}
