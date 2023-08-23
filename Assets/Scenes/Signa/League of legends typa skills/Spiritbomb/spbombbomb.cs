using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spbombbomb : MonoBehaviour
{
    public float overlapRadius = 1.0f;      // Radius for the overlap circle
    public LayerMask collisionLayer;        // Layer mask to filter which objects to consider

    private Rigidbody2D rb;
    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component attached to this GameObject
    }

    private void Update()
    {
        // Create an overlap circle at the GameObject's position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, overlapRadius, collisionLayer);

        // Check each collider in the overlap circle
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider's GameObject has either "Damagable" or "Interactable" tag
            if (collider.CompareTag("Damagable") || collisionLayer == (collisionLayer | (1 << collider.gameObject.layer)))
            {
                rb.velocity = Vector2.zero;
                Destroy(gameObject, 2f);
                // Trigger the "boom" animation in the Animator
                animator.SetTrigger("boom");
            }
        }
    }
}