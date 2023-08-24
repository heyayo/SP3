using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float damageAmount = 10.0f;
    public float homingForce = 5.0f; // Adjust the force for homing
    public Animator animator;

    private Rigidbody2D rb;
    private Transform targetEnemy;
    private bool hasHitEnemy = false;

    private enum MissileState
    {
        Homing,
        Hit
    }

    private MissileState currentState = MissileState.Homing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Destroy(gameObject, 5.0f);
        FindClosestEnemy();
    }

    void Update()
    {
        switch (currentState)
        {
            case MissileState.Homing:
                if (targetEnemy != null)
                {
                    Vector2 direction = (targetEnemy.position - transform.position).normalized;
                    rb.AddForce(direction * homingForce);

                    //// Calculate rotation to face the enemy
                    //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    // Calculate rotation to face the movement direction
                    float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                break;

            case MissileState.Hit:
                // Play "Boom" animation and then destroy the projectile
                // Disable the Rigidbody to stop movement
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                animator.SetTrigger("Boom");
                Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
                break;
        }

        CheckForCollision();
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Damagable");
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            targetEnemy = closestEnemy.transform;
        }
    }

    void CheckForCollision()
    {
        if (!hasHitEnemy)
        {
            Vector2 currentPosition = transform.position;
            Vector2 direction = Vector2.right; // Adjust direction if needed

            RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, 1.0f);

            if (hit.collider != null && hit.collider.CompareTag("Damagable"))
            {
                // Check if the hit collider belongs to an enemy
                Mortality enemy = hit.collider.GetComponent<Mortality>();

                if (enemy != null)
                {
                    hasHitEnemy = true;
                    currentState = MissileState.Hit;
                    enemy.Health -= damageAmount;
                }
            }
        }
    }
}