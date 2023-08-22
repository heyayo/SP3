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
    private Transform playerTransform;
    private LineRenderer lineRenderer;

    [SerializeField] 
    private Animator animator;

    // Private variables
    private Vector2 dir;
    private Rigidbody2D rb;
    private Vector2 hookPosition;  // Set in EnterState

    // Timers
    private int idleTimer;
    private int moveTimer;

    private void Start()
    {
        EnterState(STATES.IDLE);

        playerTransform = PlayerManager.Instance.transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        DetermineState();

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
        if (currentState == STATES.IDLE && idleTimer <= 0 && (transform.position - planteraTransform.position).magnitude > 12f &&
             (transform.position - playerTransform.position).magnitude > 12f)
        {
            EnterState(STATES.HOOK);
        }
        else if (currentState == STATES.HOOK && moveTimer <= 0)
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
                animator.SetBool("Close", true);
                break;

            case STATES.HOOK:
                moveTimer = Random.Range(75, 150);
                hookPosition = HookPosition();
                animator.SetBool("Close", false);
                break;

            default:
                break;
        }
    }

    private void FaceFromPlantera()
    {
        dir = (planteraTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private Vector2 HookPosition()
    {
        float angle = Random.Range(0f, 360f);
        float x = 12f * Mathf.Cos(angle);
        float y = 12f * Mathf.Sin(angle);
        Vector2 pos = new Vector2(x, y) + new Vector2(playerTransform.position.x, playerTransform.position.y);
        Vector2 offset = (playerTransform.position - planteraTransform.position).normalized * 2f;

        return pos + offset;
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
        moveTimer--;
        FaceFromPlantera();

        transform.position = Vector3.MoveTowards(transform.position, hookPosition, 0.2f);
    }

    private void HandleVines()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, planteraTransform.position);
    }
}
