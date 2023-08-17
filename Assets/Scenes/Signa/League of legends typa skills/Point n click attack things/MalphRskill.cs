using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalphRskill : MonoBehaviour
{
    public float damageAmount = 50.0f; // Adjust the damage amount as needed
    public float destroyDelay = 3.0f; // Time in seconds before destroying the skill effect
    public LayerMask enemyLayer; // Layer mask for enemy detection

    private bool hasHitEnemy = false;

    void Start()
    {
        CheckForCollision(); // Check for collision with enemies upon creation
        Destroy(gameObject, destroyDelay); // Destroy the skill effect after destroyDelay seconds
    }

    void CheckForCollision()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f, enemyLayer);

        foreach (Collider2D collider in hitColliders)
        {
            Mortality enemy = collider.GetComponent<Mortality>();
            TargetDummyEnemy enemyui = collider.GetComponent<TargetDummyEnemy>();

            if (enemy != null)
            {
                if (!hasHitEnemy)
                {
                    enemyui.TakeDamage(damageAmount);
                    hasHitEnemy = true;
                    // You can add more logic or effects here
                }
            }
        }
    }
}