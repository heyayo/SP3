using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zenith : MonoBehaviour
{
    public GameObject boomerangPrefab;
    public Transform throwOrigin;
    public float throwForce = 10.0f;
    public float returnSpeed = 5.0f;

    private GameObject activeBoomerang;
    private bool isReturning = false;

    void Update()
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
        activeBoomerang = Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
    }

    void DestroyActiveBoomerang()
    {
        if (activeBoomerang != null)
        {
            Destroy(activeBoomerang);
            activeBoomerang = null;
        }
    }

    void ReturnBoomerang()
    {
        if (activeBoomerang != null && !isReturning)
        {
            Vector2 directionToOrigin = (throwOrigin.position - activeBoomerang.transform.position).normalized;
            Rigidbody2D rb = activeBoomerang.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = directionToOrigin * returnSpeed;
                float angle = Mathf.Atan2(directionToOrigin.y, directionToOrigin.x) * Mathf.Rad2Deg;
                activeBoomerang.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if (Vector2.Distance(activeBoomerang.transform.position, throwOrigin.position) < 0.1f)
            {
                isReturning = false;
                Destroy(activeBoomerang);
            }
        }
    }
}