using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float tickDamageRate = 2.0f; // Damage per second
    public float tickInterval = 0.5f; // Time interval between each tick
    private float lastTickTime;
    public float damageAmount = 10.0f;

    void Start()
    {
        lastTickTime = Time.time;
        
    }

    void Update()
    {
        // Apply tick damage
        if (Time.time - lastTickTime >= tickInterval)
        {
            ApplyTickDamage();
            lastTickTime = Time.time;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (enemyLayer == (enemyLayer | (1 << other.gameObject.layer)))
        {
            Mortality enemy = other.GetComponent<Mortality>();
            TargetDummyEnemy enemyui = other.GetComponent<TargetDummyEnemy>();
            if (enemy != null)
            {
                enemyui.TakeDamage(damageAmount);
                enemy.Health -= damageAmount;

                ApplyTickDamage(); // Apply tick damage on every stay frame
            }
        }
    }

    void ApplyTickDamage()

        
    {



        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.0f, 1.0f), 0f, enemyLayer);

        foreach (Collider2D collider in colliders)
        {
            Mortality enemy = collider.GetComponent<Mortality>();
            TargetDummyEnemy enemyui = collider.GetComponent<TargetDummyEnemy>();
            if (enemy != null)
            {
                enemyui.TakeDamage(damageAmount * tickDamageRate * tickInterval);
                enemy.Health -= damageAmount * tickDamageRate * tickInterval;
            }
        }
    }
}