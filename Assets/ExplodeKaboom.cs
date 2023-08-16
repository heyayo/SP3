using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeKaboom : MonoBehaviour
{
    public float damageAmount = 50.0f; // Adjust the damage amount as needed
    public LayerMask enemyLayer; // Layer mask for enemy detection

    private bool hasHitEnemy = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
      
    }

    public void CheckForCollision()
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
                    enemy.Health -= damageAmount;
                    enemyui.TakeDamage(damageAmount);
                    hasHitEnemy = true;
                    // You can add more logic or effects here
                }
            }
        }
    }

    // Called by animation event to destroy the skill effect after the animation finishes
    public void DestroyEffect()
    {
        Destroy(gameObject);
    }
}