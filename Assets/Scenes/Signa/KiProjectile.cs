using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiProjectile : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float damageAmount = 10.0f;
    public float damn;
    private Rigidbody2D rb;
    private bool isPaused = false;
    private Vector2 previousVelocity; // Store the previous velocity
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForCollision();
    }

    void CheckForCollision()
    {
        Vector2 currentPosition = transform.position;
        Vector2 direction = Vector2.right; // Adjust direction if needed

        RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, 1.0f, enemyLayer);

        if (hit.collider != null)
        {
            // Check if the hit collider belongs to an enemy
            // Check if the hit collider belongs to an enemy
            Mortality enemyHp = hit.collider.GetComponent<Mortality>();
            TargetDummyEnemy enemy = hit.collider.GetComponent<TargetDummyEnemy>();
            if (enemy != null)
            {
                enemyHp.Health -= damageAmount;
                enemy.TakeDamage(damageAmount);
                Destroy(gameObject); // Destroy the projectile
            }
        }
    }
}