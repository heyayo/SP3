using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAttackfire : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign your projectile prefab here
    private float projectileImpulse = 28f;

    private float fireCooldown = 1f; // Time in seconds between firing
    private float lastFireTime; // Records the time of the last firing
    private bool hasFired;
    private float timer;

    private void Start()
    {
        hasFired = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ( timer - lastFireTime >= fireCooldown && hasFired == false)
        {
            FireProjectile();
            hasFired = true;
        }
    }

    void FireProjectile()
    {
        Transform firePoint = transform.GetChild(0); // Get the transform of the first child
        GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 impulseDirection = firePoint.right;
            rb.AddForce(impulseDirection * projectileImpulse, ForceMode2D.Impulse);
        }
    }
}