using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/DragonBall")]
public class DragonBallItem : Item
{
    public override void Setup(GameObject pickupItem)
    {
        pickupItem.AddComponent<DragonBall>();
        pickupItem.tag = "DragonBall";
    }
}
