using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationMenu : MonoBehaviour
{
    public List<OutfitChanger> outfitChangers = new List<OutfitChanger>();
    public void RandomiseCharacter()
    {
        foreach (OutfitChanger changer in outfitChangers)
        {
            changer.Randomise();
        }
    }

    public void ApplyCharacter()
    {
        PlayerManager.hoodSprite = outfitChangers[0].bodyPart.sprite;
    }
}
