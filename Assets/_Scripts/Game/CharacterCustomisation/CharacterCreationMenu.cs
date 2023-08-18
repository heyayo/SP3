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
        PlayerManager.HoodSprite = outfitChangers[0].bodyPart.sprite;
        PlayerManager.TorsoSprite = outfitChangers[1].bodyPart.sprite;
        PlayerManager.LeftShoulderSprite = outfitChangers[2].bodyPart.sprite;
        PlayerManager.LeftHandSprite = outfitChangers[3].bodyPart.sprite;
        PlayerManager.RightShoulderSprite = outfitChangers[4].bodyPart.sprite;
        PlayerManager.RightHandSprite = outfitChangers[5].bodyPart.sprite;
        PlayerManager.FaceSprite = outfitChangers[6].bodyPart.sprite;
        PlayerManager.LeftBootSprite = outfitChangers[7].bodyPart.sprite;
        PlayerManager.RightBootSprite = outfitChangers[8].bodyPart.sprite;
        PlayerManager.PelvisSprite = outfitChangers[9].bodyPart.sprite;
        
        LoadingScreen.LoadGameWorld();
    }
}
