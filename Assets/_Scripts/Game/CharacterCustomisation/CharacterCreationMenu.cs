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

    // Apply Customization and Play Game
    public void ApplyCharacter()
    {
        PlayerManager.hoodSprite = outfitChangers[0].bodyPart.sprite;
        PlayerManager.torsoSprite = outfitChangers[1].bodyPart.sprite;
        PlayerManager.leftShoulderSprite = outfitChangers[2].bodyPart.sprite;
        PlayerManager.leftHandSprite = outfitChangers[3].bodyPart.sprite;
        PlayerManager.rightShoulderSprite = outfitChangers[4].bodyPart.sprite;
        PlayerManager.rightHandSprite = outfitChangers[5].bodyPart.sprite;
        PlayerManager.faceSprite = outfitChangers[6].bodyPart.sprite;
        PlayerManager.leftBootSprite = outfitChangers[7].bodyPart.sprite;
        PlayerManager.rightBootSprite = outfitChangers[8].bodyPart.sprite;
        PlayerManager.pelvisSprite = outfitChangers[9].bodyPart.sprite;
        
        LoadingScreen.LoadGameWorld();
    }
}
