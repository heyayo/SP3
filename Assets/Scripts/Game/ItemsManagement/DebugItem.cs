using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/DebugItems")]
public class DebugItem : Item
{
    override public void Use()
    {
        Debug.Log("DEBUG ITEM WORKING");
    }
}
