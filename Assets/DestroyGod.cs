//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DestroyGod : MonoBehaviour
//{
//    [SerializeField]
//    private GameObject servantOfCthulhu;

//    [SerializeField]
//    private LayerMask playerLayer;

//    public enum STATES
//    {
//        CHASE,
//        ATTACK
//    }

//    private STATES currentState;
//    private Mortality mortality;
//    private Transform playerTransform;
//    private Mortality playerMortality;

//    // Private variables
//    Vector2 dir;
//    Rigidbody2D rb;
//    private Animator animator;

//    // Chase variables
//    private int chaseTimer;

//    // Attack variables
//    private int attackTimer;
//    private int attackCount;

//    private void Start()
//    {
//        EnterState(STATES.CHASE);

//        gameObject.GetComponent<Mortality>();
//        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
//        playerMortality = GameObject.FindGameObjectWithTag("Player").GetComponent<Mortality>();
//        rb = GetComponent<Rigidbody2D>();
//    }

//    private void FixedUpdate()
//    {
//        DetermineState();

//        Debug.Log(currentState);
//        Debug.Log(attackCount);

//        switch (currentState)
//        {
//            case STATES.CHASE:
//                ChaseState();
//                break;

//            case STATES.ATTACK:
//                AttackState();
//                break;

//            default:
//                Debug.Log("DestroyGod Problem");
//                break;
//        }
//    }

//    private void DetermineState()
//    {
//        // Transform at 50% hp
//        //if (mortality.Health <= mortality.HealthMax / 2)
//        //{
//        //    EnterState(STATES.TRANSFORM);
//        //}
//        // Switch to attack
//        if (currentState == STATES.CHASE && chaseTimer <= 0)
//        {
//            EnterState(STATES.ATTACK);
//        }
//        // Switch to summon
//        else if (currentState == STATES.ATTACK && attackCount <= 0)
//        {
//            EnterState(STATES.CHASE);
//        }
//    }

//    private void EnterState(STATES toState)
//    {
//        currentState = toState;

//        switch (toState)
//        {
//            case STATES.CHASE:
//                chaseTimer = 320;
//                break;

//            case STATES.ATTACK:
//                attackTimer = 0;
//                attackCount = 3;
//                break;
//        }
//    }

//    private void FacePlayer()
//    {
//        dir = (playerTransform.position - transform.position).normalized;
//        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
//        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
//    }

//    // ***********************************************************
//    // ***********************************************************
//    // States' Behaviours
//    // ***********************************************************
//    // ***********************************************************

//    private void ChaseState()
//    {
//        chaseTimer--;

//        // Move toward player
//        FacePlayer();
//        rb.velocity = Vector2.zero;
//        rb.AddForce(dir * 10f);
//    }

//    private void AttackState()
//    {
//        attackTimer--;

//        if (attackTimer <= 0)
//        {
//            // Dash
//            FacePlayer();
//            rb.AddForce(dir * 500);
//            attackTimer = 120;
//            attackCount--;
//        }
//    }
//}