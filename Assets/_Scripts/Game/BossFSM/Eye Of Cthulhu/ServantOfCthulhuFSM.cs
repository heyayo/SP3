using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServantOfCthulhuFSM : MonoBehaviour
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
    Vector2 dir;
    Rigidbody2D rb;
    private Animator animator;
    private float rotatedAmount;

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

    private void HitboxDamage()
    {
        // Hitbox stats
        Vector2 hitboxPos = transform.position - new Vector3(0, 0.1f, 0);
        float hitboxRadius = 0.25f;
        Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
        if (col != null)
            playerMortality.ApplyHealthDamage(10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 0.1f, 0), 0.25f);
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
        FacePlayer();
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * 50f);
    }
}
