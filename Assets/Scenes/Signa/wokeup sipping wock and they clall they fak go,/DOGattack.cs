using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOGattack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float lineDuration = 0.5f;
    public float projectileSpeed = 10f;
    public float minAttackDelay = 4f;
    public float maxAttackDelay = 6f;

    [SerializeField] private Transform player;
    [SerializeField] private Material lineMaterial; // Add a serialized field for the material


    void Start()
    {
        
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minAttackDelay, maxAttackDelay);
            yield return new WaitForSeconds(delay);

            PerformAttack();
        }
    }

    void PerformAttack()
    {
        if (player != null)
        {
            // Create a line renderer
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.material = lineMaterial; // Assign the serialized material


            // Set the width of the line renderer
            lineRenderer.startWidth = 0.2f; // Adjust the value as needed
            lineRenderer.endWidth = 0.2f;   // Adjust the value as needed
            // Set the second point of the line renderer far away from the player
            Vector3 distantPoint = player.position + (player.position - transform.position).normalized * 10f;
            lineRenderer.SetPosition(1, distantPoint);

          

            // Set the sorting order to a high value
            lineRenderer.sortingOrder = 1; // Adjust the sorting order as needed

            // Destroy the line renderer after a delay
            StartCoroutine(DestroyLineRenderer(lineRenderer));

            // Shoot a projectile after a delay
            StartCoroutine(ShootProjectile(player.position));
        }
    }

    IEnumerator DestroyLineRenderer(LineRenderer lineRenderer)
    {
        yield return new WaitForSeconds(lineDuration);
        Destroy(lineRenderer);
    }

    IEnumerator ShootProjectile(Vector2 targetPosition)
    {
        yield return new WaitForSeconds(lineDuration + 0.5f); // Adjust the delay if needed

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Apply force to the projectile in the calculated direction
            rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        }
    }
}