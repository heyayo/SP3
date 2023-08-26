using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Items/Placeable")]
public class Placeable : Item
{
    [SerializeField]
    private GameObject placeItem;

    public override void Use()
    {
        if (placeItem == null)
            return;

        Instantiate(placeItem, PlayerManager.Instance.transform.position, Quaternion.identity);
        consumed.Invoke();
    }
}
