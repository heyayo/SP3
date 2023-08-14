
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProjectileScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public bool isactive = false;
    public bool timeisstopped = false;

    private Dictionary<GameObject, Vector2> shotProjectiles = new Dictionary<GameObject, Vector2>();

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0 && isactive == true)
        {
            if (timeisstopped == true)
            {
                SpawnProjectile();
            }

            else
            {
                FireProjectile();
            }
        }
    }

    void SpawnProjectile()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate the direction towards the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Instantiate the projectile prefab
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        shotProjectiles.Add(projectile, direction);  // Append to the list
    }
    void FireProjectile()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate the direction towards the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Instantiate the projectile prefab
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Get the Rigidbody2D of the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Apply force to the projectile in the calculated direction
        rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
    }

    public void UntimeStop()
    {
        foreach (KeyValuePair<GameObject, Vector2> projectile in shotProjectiles)
        {
            if (projectile.Key != null)
            {
                Rigidbody2D rb = projectile.Key.GetComponent<Rigidbody2D>();

                // Remove the Rigidbody's existing velocity before applying new force
                rb.velocity = Vector2.zero;

                rb.AddForce(projectile.Value * projectileSpeed, ForceMode2D.Impulse);
            }
        }
    }

}