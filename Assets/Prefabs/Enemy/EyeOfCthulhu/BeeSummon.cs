using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSummon : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    public enum STATES
    {
        CHASE
    }

    private STATES currentState;
    private Mortality mortality;
    private Transform playerTransform;
    private Mortality playerMortality;

    // Private variables
    SpriteRenderer sr;

    Vector2 dir;
    Rigidbody2D rb;
    private Animator animator;
    private float rotatedAmount;

    // Hitbox stats
    Vector2 hitboxPos;

    private void Start()
    {
        EnterState(STATES.CHASE);

        sr = GetComponent<SpriteRenderer>();
        gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        DetermineState();

        switch (currentState)
        {
            case STATES.CHASE:
                ChaseState();
                break;

            default:
                Debug.Log("ServantOfCthulhuFSM Problem");
                break;
        }
    }

    private void DetermineState()
    {

    }

    private void EnterState(STATES toState)
    {
        currentState = toState;
    }

    private void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
        if (dir.x < 0)
        {
            sr.flipX = true;
        }

        if (dir.x > 0)
        {
            sr.flipX = false;
        }
    }

    // ***********************************************************
    // ***********************************************************
    // States' Behaviours
    // ***********************************************************
    // ***********************************************************

    private void ChaseState()
    {
        FacePlayer();
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * 300f);
    }
}
