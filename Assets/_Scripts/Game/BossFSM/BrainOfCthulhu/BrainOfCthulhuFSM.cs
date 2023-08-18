using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainOfCthulhuFSM : MonoBehaviour
{
    [SerializeField]
    private GameObject[] floaters;

    [SerializeField]
    private GameObject brainOfCthulhuClone;

    [SerializeField]
    private LayerMask playerLayer;

    public enum STATES
    { 
        CHASE,
        TELEPORT,
        E_CHASE,
        E_ATTACK
    }

    private STATES currentState;
    private Mortality mortality;
    private Transform playerTransform;
    private Mortality playerMortality;
    private bool enraged = true;

    // Private variables
    Vector2 dir;
    Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private float opacity = 1f;

    // Chase variables
    private int chaseTimer;

    // Attack variables
    private int attackTimer;
    private int attackCount;
    private bool attacked;

    // Teleport variables
    private int timeInvisible;
    private bool teleportDone;
    private int teleportCount = 2;
    private bool teleported;
    private int previousRng;

    private void Start()
    {
        enraged = false;
        EnterState(STATES.CHASE);

        mortality = gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
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

            case STATES.TELEPORT:
                TeleportState();
                break;

            case STATES.E_CHASE:
                E_ChaseState();
                break;

            case STATES.E_ATTACK:
                E_AttackState();
                break;

            default:
                Debug.Log("BrainOfCthulhuFSM Problem");
                break;
        }
    }

    private void DetermineState()
    {
        // First form
        if (!enraged)
        {
            // Transform at 50% hp
            if (mortality.Health <= mortality.__HealthMax / 2)
            {
                enraged = true;
                animator.SetBool("Enraged", true);
                EnterState(STATES.E_CHASE);
                opacity = 0.7f;
                sr.color = new Color(255, 255, 255, opacity);  // Set to opaque
            }
            // Switch to teleport
            if (currentState == STATES.CHASE && chaseTimer <= 0)
            {
                EnterState(STATES.TELEPORT);
            }
            else if (currentState == STATES.TELEPORT && teleportDone)
            {
                EnterState(STATES.CHASE);
            }
        }

        // Second form
        else
        {
            // Switch to attack
            if (currentState == STATES.E_CHASE && teleportCount <= 0 &&
                chaseTimer <= 0)
            {
                EnterState(STATES.E_ATTACK);
            }
            // Switch to teleport
            else if (currentState == STATES.E_CHASE && chaseTimer <= 0)
            {
                EnterState(STATES.TELEPORT);
            }
            // Switch to chase
            else if (currentState == STATES.TELEPORT && teleportDone)
            {
                EnterState(STATES.E_CHASE);
            }
            // Switch back to chase
            else if (currentState == STATES.E_ATTACK && attacked)
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
                chaseTimer = 150;
                break;

            case STATES.TELEPORT:
                teleportDone = false;
                timeInvisible = 60;
                teleported = false;
                break;

            case STATES.E_CHASE:
                chaseTimer = 120;
                break;

            case STATES.E_ATTACK:
                attackTimer = 50;
                attacked = false;
                attackCount = 0;
                teleportCount = 2;
                break;

            default:
                break;
        }
    }

    private void HitboxDamage()
    {
        // Hitbox stats
        Vector2 hitboxPos = transform.position - new Vector3(0, 0, 0);
        float hitboxRadius = 3f;
        Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
        if (col != null)
            playerMortality.ApplyHealthDamage(10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 0, 0), 3f);
    }

    private void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;

        // This enemy does not rotate
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private Vector2 TeleportPosition(Vector2 direction)
    {
        Vector2 tpDirection = direction.normalized;

        return playerTransform.position + new Vector3(tpDirection.x, tpDirection.y, 0) * 15f;
    }

    private Vector2 RandomTeleportDirection()
    {
        int rng;
        rng = Random.Range(1, 5);

        // Don't tp to the same position as before
        while (rng == previousRng)
            rng = Random.Range(1, 5);

        previousRng = rng;

        if (rng == 1)
            return new Vector2(-1, 1);  // Top left
        else if (rng == 2)
            return new Vector2(1, 1);  // Top right
        else if (rng == 3)
            return new Vector2(-1, -1);  // Bottom left
        else
            return new Vector2(1, -1);  // Bottom right
    }

    // ***********************************************************
    // ***********************************************************
    // States' Behaviours
    // ***********************************************************
    // ***********************************************************

    private void ChaseState()
    {
        chaseTimer--;

        FacePlayer();
        rb.AddForce(dir * 5f);
    }

    private void TeleportState()
    {
        // Constantly mvoing to player
        FacePlayer();
        rb.AddForce(dir * 5f);

        // Fading to invisible
        if (sr.color.a > 0 && !teleported)
        {
            sr.color -= new Color(0, 0, 0, 0.025f);
        }
        // Teleporting
        else if (sr.color.a <= 0 && !teleported)
        {
            gameObject.transform.position = TeleportPosition(RandomTeleportDirection());
            rb.velocity = Vector2.zero;
            teleported = true;
        }
        // Stay invisible for awhile
        else if (timeInvisible > 0)
        {
            timeInvisible--;
        }
        // Becoming visible again
        else if (sr.color.a < opacity)
        {
            timeInvisible--;
            sr.color += new Color(0, 0, 0, 0.025f);
        }
        // Done with teleport
        else
        {
            teleportCount--;
            teleportDone = true;
        }
    }

    private void E_ChaseState()
    {
        chaseTimer--;

        FacePlayer();
        rb.AddForce(dir * 10f);
    }

    private void E_AttackState()
    {
        if (attackCount >= 3)
            attacked = true;

        if (attackTimer > 0)
        {
            attackTimer--;

            FacePlayer();
            rb.AddForce(dir * 10f);
            return;
        }

        // Teleport
        gameObject.transform.position = TeleportPosition(RandomTeleportDirection());
        rb.velocity = Vector2.zero;

        // Top left clone
        if (previousRng != 1)
        {
            GameObject clone = Instantiate(brainOfCthulhuClone, new Vector3(TeleportPosition(new Vector2(-1, 1)).x,
                TeleportPosition(new Vector2(-1, 1)).y, 0), Quaternion.identity);
            // Changing it's visibility based on remaining health
            clone.GetComponent<BOCCloneFSM>().opacity = opacity - 1 / (mortality.__HealthMax / mortality.Health);
        }

        // Top Right clone
        if (previousRng != 2)
        {
            GameObject clone = Instantiate(brainOfCthulhuClone, new Vector3(TeleportPosition(new Vector2(1, 1)).x,
                TeleportPosition(new Vector2(1, 1)).y, 0), Quaternion.identity);
            // Changing it's visibility based on remaining health
            clone.GetComponent<BOCCloneFSM>().opacity = opacity - 1 / (mortality.__HealthMax / mortality.Health);
        }

        // Bottom left clone
        if (previousRng != 3)
        {
            GameObject clone = Instantiate(brainOfCthulhuClone, new Vector3(TeleportPosition(new Vector2(-1, -1)).x,
                TeleportPosition(new Vector2(-1, -1)).y, 0), Quaternion.identity);
            // Changing it's visibility based on remaining health
            clone.GetComponent<BOCCloneFSM>().opacity = opacity - 1 / (mortality.__HealthMax / mortality.Health);
        }
            
        // Bottom right clone
        if (previousRng != 4)
        {
            GameObject clone = Instantiate(brainOfCthulhuClone, new Vector3(TeleportPosition(new Vector2(1, -1)).x,
                TeleportPosition(new Vector2(1, -1)).y, 0), Quaternion.identity);
            // Changing it's visibility based on remaining health
            clone.GetComponent<BOCCloneFSM>().opacity = opacity - 1 / (mortality.__HealthMax / mortality.Health);
        }

        attackTimer = 200;
        attackCount++;
    }
}
