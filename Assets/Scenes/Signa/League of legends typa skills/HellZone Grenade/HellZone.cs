using System.Collections;
using UnityEngine;

public class HellZone : MonoBehaviour
{
    private Rigidbody2D rb2D;        // Reference to the projectile's Rigidbody2D component
    private Vector2 originalForce;   // Store the original force applied to the projectile
    private Transform targetEnemy;   // Reference to the closest damageable enemy

    private void Start()
    {
        Destroy(gameObject, 8f);
        rb2D = GetComponent<Rigidbody2D>();   // Get the reference to the Rigidbody2D component

        // Store the initial force applied to the projectile
        originalForce = rb2D.velocity;

        StartCoroutine(MoveRoutine());   // Start the coroutine for the movement behavior
    }

    private IEnumerator MoveRoutine()
    {
        rb2D.velocity = new Vector2(originalForce.x, originalForce.y);

        yield return new WaitForSeconds(1.5f);  // Wait for 3 seconds

        rb2D.velocity = Vector2.zero;  // Stop the projectile

        float startTime = Time.time;  // Record the start time

        while (Time.time - startTime < 1f)  // Continue for 1 second
        {
            FindClosestEnemy();  // Find the closest damageable enemy
            yield return null;   // Yielding null here allows the Update loop to run
        }

        if (targetEnemy != null)
        {
            Vector2 directionToEnemy = (targetEnemy.position - transform.position).normalized;
            rb2D.velocity = directionToEnemy * 200f;  // Move towards the enemy at high velocity
        }
    }

    void FindClosestEnemy()
    {
        Collider2D closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Find all colliders with the "Damageable" tag within a radius of 20 units
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 60.0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Damagable"))  // Check if the collider has the "Damageable" tag
            {
                float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = collider;
                }
            }
        }
        if (closestEnemy != null)
        {
            targetEnemy = closestEnemy.transform;
        }
    }
}