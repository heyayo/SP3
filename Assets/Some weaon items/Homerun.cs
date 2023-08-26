using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/HOMEEERRUUUNNN")]
public class Homerun : Item
{
 

    private Transform playerTransform; // Reference to the player's transform

    private void OnEnable()
    {
        // Find and assign the player's transform using the tag "Player"
        GameObject playerObject = PlayerManager.Instance.gameObject;
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    public override void Use()
    {
     
    }
}