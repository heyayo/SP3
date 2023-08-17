using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeOfCthulhuFSM : MonoBehaviour
{
    [SerializeField]
    private GameObject servantOfCthulhu;

    [SerializeField]
    private LayerMask playerLayer;

    public enum STATES
    { 
        CHASE,
        ATTACK,
        SUMMON,
        TRANSFORM,
        E_CHASE,
        E_ATTACK
    }

    private STATES currentState;
    private Mortality mortality;
    private Transform playerTransform;
    private Mortality playerMortality;
    private bool enraged;

    // Private variables
    Vector2 dir;
    Rigidbody2D rb;
    private Animator animator;
    private float rotatedAmount;

    // Chase variables
    private int chaseTimer;
    private int dashTimer;

    // Attack variables
    private int attackTimer;
    private int attackCount;

    // Summon variables
    private int summonTimer;
    private int spawnTimer;

    private void Start()
    {
        enraged = false;
        EnterState(STATES.TRANSFORM);

        gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        DetermineState();

        HitboxDamage();

        switch (currentState)
        {
            case STATES.CHASE:
                ChaseState();
                break;

            case STATES.ATTACK:
                AttackState();
                break;

            case STATES.SUMMON:
                SummonState();
                break;

            case STATES.TRANSFORM:
                TransformState();
                break;

            case STATES.E_CHASE:
                E_ChaseState();
                break;

            case STATES.E_ATTACK:
                E_AttackState();
                break;

            default:
                Debug.Log("EyeOfCthulhuFSM Problem");
                break;
        }
    }

    private void DetermineState()
    {
        // First form
        if (!enraged)
        {
            // Transform at 50% hp
            //if (mortality.Health <= mortality.HealthMax / 2)
            //{
            //    EnterState(STATES.TRANSFORM);
            //}
            // Switch to attack
            if (currentState == STATES.CHASE && chaseTimer <= 0)
            {
                EnterState(STATES.ATTACK);
            }
            // Switch to summon
            else if (currentState == STATES.ATTACK && attackCount <= 0)
            {
                EnterState(STATES.SUMMON);
            }
            // Switch to chase
            else if (currentState == STATES.SUMMON && summonTimer <= 0)
            {
                EnterState(STATES.CHASE);
            }
        }

        // Second form
        else
        {

            // Switch to attack
            if (currentState == STATES.E_CHASE && chaseTimer <= 0)
            {
                EnterState(STATES.E_ATTACK);
            }
            // Switch to chase
            else if (currentState == STATES.E_ATTACK && attackCount <= 0)
            {
                EnterState(STATES.E_CHASE);
            }
            // Switch to E_CHASE when first entering rage mode
            else if (currentState == STATES.TRANSFORM)
            {
                EnterState(STATES.E_CHASE);
            }
        }
    }

    private void EnterState(STATES toState)
    {
        currentState = toState;

        switch (toState)
        {
            case STATES.TRANSFORM:
                rotatedAmount = 0f;
                break;

            case STATES.CHASE:
                chaseTimer = 320;
                dashTimer = 75;
                break;

            case STATES.ATTACK:
                attackTimer = 50;
                attackCount = 3;
                break;

            case STATES.SUMMON:
                summonTimer = 1200;
                spawnTimer = 400;
                break;

            case STATES.E_CHASE:
                chaseTimer = 350;
                dashTimer = 120;
                break;

            case STATES.E_ATTACK:
                attackTimer = 50;
                attackCount = 6;
                break;

            default:
                break;
        }
    }

    private void HitboxDamage()
    {
        // Hitbox stats
        Vector2 hitboxPos = transform.position - new Vector3(0, 0.5f, 0);
        float hitboxRadius = 1.2f;
        Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
        if (col != null)
            playerMortality.ApplyHealthDamage(10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 0.5f, 0), 1.2f);
    }

    private void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    // ***********************************************************
    // ***********************************************************
    // States' Behaviours
    // ***********************************************************
    // ***********************************************************

    private void ChaseState()
    {
        chaseTimer--;
        dashTimer--;

        // Move toward player
        if (dashTimer <= 0)
        {
            FacePlayer();
            rb.AddForce(dir * 200f);
            dashTimer = 70;
        }
    }

    private void AttackState()
    {
        attackTimer--;

        if (attackTimer <= 0)
        {
            // Dash
            FacePlayer();
            rb.AddForce(dir * 800);
            attackTimer = 120;
            attackCount--;
        }
    }

    private void SummonState()
    {
        summonTimer--;
        spawnTimer--;

        FacePlayer();
        Vector2 moveDir = ((playerTransform.position + new Vector3(0, 4, 0)) - transform.position).normalized;
        rb.AddForce(moveDir * 5);

        if (spawnTimer <= 0)
        {
            Instantiate(servantOfCthulhu, transform.position + new Vector3(dir.x, dir.y, 0) * 5f, Quaternion.identity);
            spawnTimer = 150;
        }
    }

    private void TransformState()
    {
        float rotationSpeed = 5f;
        float rotationAmount = 500f;

        // Slow > fast rotation
        if (rotatedAmount < rotationAmount)
            gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);

        rotatedAmount += rotationSpeed;

        if (rotatedAmount >= rotationAmount && gameObject.GetComponent<Rigidbody2D>().angularVelocity <= 20f)
        {
            animator.SetBool("Enraged", true);
            enraged = true;
        }
    }

    private void E_ChaseState()
    {
        chaseTimer--;
        dashTimer--;

        // Move toward player
        if (dashTimer <= 0)
        {
            rb.AddForce(dir * 250f);
            dashTimer = 60;
        }
        // Wait awhile before continuing to pursue player
        else if (dashTimer < 45)
        {
            FacePlayer();
            rb.AddForce(dir * 10f);
        }
    }

    private void E_AttackState()
    {
        attackTimer--;

        if (attackTimer <= 0)
        {
            // Dash
            FacePlayer();
            rb.velocity = Vector2.zero;
            rb.AddForce(dir * 1200);
            attackTimer = 40;
            attackCount--;
        }
    }
}
