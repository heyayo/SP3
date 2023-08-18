using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterFSM : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    public enum STATES
    { 
        FLOATING,
        CHASE
    }

    // The brain will set this
    public Transform brainTransform;

    // Private variables
    private STATES currentState;
    private Mortality mortality;
    private Transform playerTransform;
    private Mortality playerMortality;

    // Private variables
    Vector2 dir;
    Rigidbody2D rb;

    // Hitbox stats
    Vector2 hitboxPos;

    private void Start()
    {
        EnterState(STATES.FLOATING);

        gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        DetermineState();

        HitboxDamage();

        switch (currentState)
        {
            case STATES.FLOATING:
                FloatingState();
                break;

            case STATES.CHASE:
                ChaseState();
                break;

            default:
                Debug.Log("FloaterFSM Problem");
                break;
        }
    }

    private void DetermineState()
    {
        if (currentState == STATES.FLOATING && (playerTransform.position - transform.position).magnitude < 5)
        {
            EnterState(STATES.CHASE);
        }
        else
        {
            EnterState(STATES.FLOATING);
        }
    }

    private void EnterState(STATES toState)
    {
        currentState = toState;
    }

    private void HitboxDamage()
    {
        // Hitbox stats
        Vector2 hitboxPos = transform.position - new Vector3(0, 0, 0);
        float hitboxRadius = 0.75f;
        Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
        if (col != null)
            playerMortality.ApplyHealthDamage(10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 0, 0), 0.75f);
    }

    private void FacePlayer()
    {
        dir = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private void FaceBrain()
    {
        dir = (brainTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y + Random.Range(0f, 0.2f), dir.x + Random.Range(0f, 0.2f)) * Mathf.Rad2Deg;  // Offset a little bit randomly
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    // ***********************************************************
    // ***********************************************************
    // States' Behaviours
    // ***********************************************************
    // ***********************************************************


    private void FloatingState()
    {
        FaceBrain();
        rb.AddForce(dir * 60f);
    }

    private void ChaseState()
    {
        FacePlayer();
        rb.AddForce(dir * 80f);
    }
}
