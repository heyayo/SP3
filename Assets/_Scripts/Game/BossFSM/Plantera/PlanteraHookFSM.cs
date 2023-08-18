using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanteraHookFSM : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    public enum STATES
    {
        IDLE,
        HOOK
    }

    // Plantera will set this
    public Transform planteraTransform;

    // Private variables
    private STATES currentState;
    private Mortality mortality;
    private Transform playerTransform;
    private Mortality playerMortality;
    private LineRenderer lineRenderer;

    // Private variables
    private Vector2 dir;
    private Rigidbody2D rb;
    private Vector2 hookPosition;  // Set in EnterState

    // Hitbox stats
    private Vector2 hitboxPos;

    // Timers
    private int idleTimer;

    private void Start()
    {
        EnterState(STATES.IDLE);

        gameObject.GetComponent<Mortality>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        DetermineState();

        HitboxDamage();

        HandleVines();

        switch (currentState)
        {
            case STATES.IDLE:
                IdleState();
                break;

            case STATES.HOOK:
                HookState();
                break;

            default:
                Debug.Log("PlanteraHookFSM Problem");
                break;
        }
    }

    private void DetermineState()
    {
        if (currentState == STATES.IDLE && idleTimer <= 0 && (transform.position - planteraTransform.position).magnitude > 15f)
        {
            EnterState(STATES.HOOK);
        }
        else if (currentState == STATES.HOOK && (new Vector2(transform.position.x, 
            transform.position.y) - hookPosition).magnitude < 0.2f)
        {
            EnterState(STATES.IDLE);
        }
    }

    private void EnterState(STATES toState)
    {
        currentState = toState;

        switch (toState)
        {
            case STATES.IDLE:
                idleTimer = Random.Range(150, 250);
                break;

            case STATES.HOOK:
                hookPosition = HookPosition();
                break;

            default:
                break;
        }
    }

    private void HitboxDamage()
    {
        // Hitbox stats
        Vector2 hitboxPos = transform.position - new Vector3(0, 0, 0);
        float hitboxRadius = 0.7f;
        Collider2D col = Physics2D.OverlapCircle(hitboxPos, hitboxRadius, playerLayer);
        if (col != null)
            playerMortality.ApplyHealthDamage(10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 0, 0), 0.7f);
    }

    private void FaceFromPlantera()
    {
        dir = (planteraTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y + Random.Range(0f, 0.2f), dir.x + Random.Range(0f, 0.2f)) * Mathf.Rad2Deg;  // Offset a little bit randomly
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private Vector2 HookPosition()
    {
        float angle = Random.Range(0f, 360f);
        float x = 10f * Mathf.Cos(angle);
        float y = 10f * Mathf.Sin(angle);
        Vector2 pos = new Vector2(x, y) + new Vector2(playerTransform.position.x, playerTransform.position.y);

        return pos;
    }

    // ***********************************************************
    // ***********************************************************
    // States' Behaviours
    // ***********************************************************
    // ***********************************************************


    private void IdleState()
    {
        idleTimer--;
        FaceFromPlantera();
    }

    private void HookState()
    {
        FaceFromPlantera();

        transform.position = Vector3.MoveTowards(transform.position, hookPosition, 0.1f);
    }

    private void HandleVines()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, planteraTransform.position);
    }
}
