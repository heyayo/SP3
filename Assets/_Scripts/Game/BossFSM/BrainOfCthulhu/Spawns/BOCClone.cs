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
    private Transform playerTransform;
    private Mortality playerMortality;

    // Private variables
    Vector2 dir;
    Rigidbody2D rb;
    private float rotatedAmount;
    private SpriteRenderer sr;

    // Disappear and chase variables
    private int chaseTimer;

    private void Start()
    {
        EnterState(STATES.CHASE);

        gameObject.GetComponent<Mortality>();
        playerTransform = PlayerManager.Instance.transform;
        rb = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        sr.color *= new Color(1, 1, 1, opacity);  // Set opacity

        GetComponent<Damagable>().onHit.AddListener(HitSound);
        GetComponent<Mortality>().onHealthZero.AddListener(Death);
    }

    private void HitSound()
    {
        SoundManager.Instance.PlaySound(2);
    }

    private void Death()
    {
        SoundManager.Instance.PlaySound(3);
        Destroy(gameObject);
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
                Debug.Log("BOCCloneFSM Problem");
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
