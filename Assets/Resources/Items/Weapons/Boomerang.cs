using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Items/Boomerang")]
public class Boomerang :Item
{
    public GameObject boomerangPrefab;
    private Transform throwOrigin;
    public float throwForce = 10.0f;
    public float returnSpeed = 5.0f;

    private GameObject activeBoomerang;
    private bool isReturning = false;

    public override void WhileHolding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowBoomerang();
        }

        if (Input.GetMouseButtonUp(0))
        {
            DestroyActiveBoomerang();
        }
    }

   

  

    void ThrowBoomerang()
    {
        throwOrigin = PlayerManager.Instance.transform;
        
        activeBoomerang = Instantiate(boomerangPrefab, throwOrigin.position, Quaternion.identity);
    }

    void DestroyActiveBoomerang()
    {
        if (activeBoomerang != null)
        {
            Destroy(activeBoomerang);
            activeBoomerang = null;
        }
    }

  

}
