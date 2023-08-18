using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanteraFSM : MonoBehaviour
{
    [Header("Projectiles")]
    [SerializeField]
    private GameObject bulletSeed;

    [SerializeField]
    private GameObject hookPrefab;

    [SerializeField]
    private LayerMask playerLayer;

    public enum STATES
    { 
        CHASE,
        ATTACK,
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

    // Chase variables
    private int chaseTimer;
    private int shootTimer;

    // Attack variables
    private int attackTimer;
    private int attackCount;

    // Hooks' variables
    private GameObject[] hooks = new GameObject[8];

    private void Start()
    {
        enraged = false;
        EnterState(STATES.CHASE);

        mortality = gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Init hooks
        for (int i = 0; i < 1; i++)
        {
            // Spawn Randomly around Plantera
            float angle = Random.Range(0f, 360f);
            float x = 7f * Mathf.Cos(angle);
            float y = 7f * Mathf.Sin(angle);
            Vector2 spawnPos = new Vector2(x, y);

            // Spawn around the brain
            GameObject floater = Instantiate(hookPrefab, new Vector2(transform.position.x, transform.position.y)
                + spawnPos, Quaternion.identity);
            floater.GetComponent<PlanteraHookFSM>().planteraTransform = gameObject.transform;

            // Append to list
            hooks[i] = floater;
        }
    }

    private void FixedUpdate()
    {
        DetermineState();

        HitboxDamage();

        HandleHooks();

        switch (currentState)
        {
            case STATES.CHASE:
                ChaseState();
                break;

            case STATES.ATTACK:
                AttackState();
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
            // Transform at 50 % hp
            if (mortality.Health <= mortality.HealthMax / 2)
            {
                enraged = true;
                animator.SetBool("Enraged", true);
                EnterState(STATES.E_CHASE);
            }
            // Switch to attack
            if (currentState == STATES.CHASE && chaseTimer <= 0)
            {
                EnterState(STATES.ATTACK);
            }
            // Switch to chase
            else if (currentState == STATES.ATTACK && attackCount <= 0)
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
        }
    }

    private void EnterState(STATES toState)
    {
        currentState = toState;

        switch (toState)
        {
            case STATES.CHASE:
                chaseTimer = 600;
                shootTimer = 70;
                break;

            case STATES.ATTACK:
                attackTimer = 120;
                attackCount = 3;
                break;

            case STATES.E_CHASE:
                chaseTimer = 450;
                break;

            case STATES.E_ATTACK:
                attackTimer = 140;
                attackCount = 4;
                break;

            default:
                break;
        }
    }

    private void HitboxDamage()
    {
        // Hitbox stats
        Vector2 hitboxPos = transform.position + new Vector3(0, 0.5f, 0);
        float hitboxRadius = 1.8f;
        Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
        if (col != null)
            playerMortality.ApplyHealthDamage(10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0.5f, 0), 1.8f);
    }

    private void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    // ***********************************************************
    // ***********************************************************
    // States' Behaviours
    // ***********************************************************
    // ***********************************************************

    private void ChaseState()
    {
        chaseTimer--;
        shootTimer--;

        // Move toward player
        FacePlayer();
        rb.AddForce(dir * 2f);

        if (shootTimer <= 0)
        {
            GameObject bullet = Instantiate(bulletSeed, transform.position + new Vector3(dir.x, dir.y, 0) * 3, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(dir * 300f);
            shootTimer = 70;
        }
    }

    private void AttackState()
    {
        attackTimer--;
        FacePlayer();

        if (attackTimer <= 0)
        {
            // Dash
            rb.velocity = Vector2.zero;
            rb.AddForce(dir * 700);
            attackTimer = 120;
            attackCount--;
        }
    }

    private void E_ChaseState()
    {
        chaseTimer--;

        // Move toward player
        FacePlayer();
        rb.AddForce(dir * 12f);
    }

    private void E_AttackState()
    {
        attackTimer--;

        // Move toward player
        FacePlayer();
        rb.AddForce(dir * 12f);

        if (attackTimer <= 0)
        {
            // Dash
            rb.AddForce(dir * 600);

            // Reset
            attackTimer = 140;
            attackCount--;
        }
    }

    private void HandleHooks()
    {

    }
}
