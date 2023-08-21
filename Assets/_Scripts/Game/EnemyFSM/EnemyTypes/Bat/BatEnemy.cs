using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatEnemy : Enemy
{
    [HideInInspector] public float timeBetweenShots = 0.3f;
    [HideInInspector] public float timeTillExit = 3f;
    [HideInInspector] public float distanceToCountExit = 4f;
    [HideInInspector] public float exitTimer;
    [HideInInspector] public float timer;
    [HideInInspector] public Vector2 spawnLocation;

    public BatIdle IdleState;
    public BatChase ChaseState;
    public BatAttack AttackState;

    [field:Header("State Settings")]
    public float wanderRange = 8f;

    [field: Header("Projectile Settings")]
    public Rigidbody2D bulletPrefab;
    public float bulletSpeed = 50f;

    [field:Header("Player Settinsgs")]
    public Transform target;
    public float moveSpeed = 8f;

    [field: Header("Bat Animator")]
    public Animator anim;

    private Vector3 _originalScale;
    private void Awake()
    {
        base.Awake();
        IdleState = new BatIdle(this, stateMachine);
        ChaseState = new BatChase(this, stateMachine);
        AttackState = new BatAttack(this, stateMachine);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.Instance.transform;
        spawnLocation = transform.position;
        _originalScale = transform.localScale;
        anim = GetComponent<Animator>();
        stateMachine.Init(IdleState);
    }
}
