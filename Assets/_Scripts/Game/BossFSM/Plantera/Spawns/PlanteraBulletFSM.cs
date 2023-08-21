using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanteraBulletFSM : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    public enum STATES
    { 
        CHASE
    }

    private STATES currentState;
    private Transform playerTransform;

    // Private variables
    Vector2 dir;
    Rigidbody2D rb;
    private int chaseTimer;

    private void Start()
    {
        EnterState(STATES.CHASE);

        gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
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
                Debug.Log("PlanteraSporeFSM Problem");
                break;
        }
    }

    private void DetermineState()
    {

    }

    private void EnterState(STATES toState)
    {
        currentState = toState;

        chaseTimer = 600;
    }

    private void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
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

        if (chaseTimer <= 0)
            Destroy(gameObject);
    }
}
