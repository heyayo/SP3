using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBeeAi : MonoBehaviour
{
    [SerializeField] private Transform targetPoint;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float moveForce = 5f;
    [SerializeField] private float maxVelocityMagnitude = 5f;
    [SerializeField] private float detectionRadius = 1f; // Adjust as needed

    public bool activate = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (targetPoint != null && activate == true)
        {
            // Calculate the direction to the target point
            Vector2 direction = (targetPoint.position - transform.position).normalized;

            // Check for overlap with the targetLayer within detectionRadius
            Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, targetLayer);

            // Only flip the sprite if not colliding with targetLayer
            if (hitCollider == null)
            {
                // Calculate the new velocity based on the direction and speed
                Vector2 newVelocity = direction * moveForce;

                // Limit the velocity magnitude
                if (newVelocity.magnitude > maxVelocityMagnitude)
                {
                    newVelocity = newVelocity.normalized * maxVelocityMagnitude;
                }

                // Apply the new velocity to the Rigidbody
                rb.velocity = newVelocity;

                // Flip the sprite based on movement direction
                if (rb.velocity.magnitude > 0.1f)
                {
                    if (direction.x > 0)
                    {
                        spriteRenderer.flipX = false;
                    }
                    else if (direction.x < 0)
                    {
                        spriteRenderer.flipX = true;
                    }
                }
            }
        }
    }

    // Draw the detection radius in the Unity editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}