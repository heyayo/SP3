using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOCCloneFSM : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    public float opacity = 0.7f;

    public enum STATES
    {
        CHASE,
        DISAPPEAR
    }

    private STATES currentState;
    private Mortality mortality;
    private Transform playerTransform;
    private Mortality playerMortality;

    // Private variables
    Vector2 dir;
    Rigidbody2D rb;
    private Animator animator;
    private float rotatedAmount;
    private SpriteRenderer sr;

    // Disappear and chase variables
    private int chaseTimer;

    // Hitbox stats
    Vector2 hitboxPos;

    private void Start()
    {
        EnterState(STATES.CHASE);

        gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        sr.color *= new Color(1, 1, 1, opacity);  // Set opacity
    }

    private void FixedUpdate()
    {
        DetermineState();

        switch (currentState)
        {
            case STATES.CHASE:
                ChaseState();
                break;

            case STATES.DISAPPEAR:
                DisappearState();
                break;

            default:
                Debug.Log("ServantOfCthulhuFSM Problem");
                break;
        }
    }

    private void DetermineState()
    {
        if (currentState == STATES.CHASE && chaseTimer <= 0)
        {
            EnterState(STATES.DISAPPEAR);
        }
    }

    private void EnterState(STATES toState)
    {
        currentState = toState;

        switch (toState)
        {
            case STATES.CHASE:
                chaseTimer = 100;
                break;

            case STATES.DISAPPEAR:
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

    // ***********************************************************
    // ***********************************************************
    // States' Behaviours
    // ***********************************************************
    // ***********************************************************

    private void ChaseState()
    {
        chaseTimer--;

        FacePlayer();
        rb.AddForce(dir * 10f);
    }

    private void DisappearState()
    {
        // Constantly move towards player
        FacePlayer();
        rb.AddForce(dir * 10f);

        // Slowly fade away
        if (sr.color.a > 0)
            sr.color -= new Color(0, 0, 0, 0.025f);
        else
            Destroy(gameObject);
    }
}
