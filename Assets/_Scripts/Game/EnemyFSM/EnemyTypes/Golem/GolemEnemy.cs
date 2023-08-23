using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GolemEnemy : Enemy
{
    [HideInInspector] public float timeBetweenShots = 0.1f;
    [HideInInspector] public float timeTillExit = 1f;
    [HideInInspector] public float distanceToCountExit = 4f;
    [HideInInspector] public float exitTimer;
    [HideInInspector] public float timer;
    [HideInInspector] public Vector2 spawnLocation;
    [HideInInspector] public DamageSource _damageSource;
    [HideInInspector] public SpriteRenderer _sr;

    [field: Header("Golem States")]
    public GolemIdle IdleState;
    public GolemChase ChaseState;
    public GolemAttack AttackState;

    [field:Header("State Settings")]
    public float wanderRange = 8f;

    [field:Header("Player Settinsgs")]
    public Transform target;
    public float moveSpeed = 8f;

    [field: Header("Spike Gameobject")]
    public GameObject spike;
    [field: Header("Spike Animator")]
    public Animator spikeAnim;
    [field: Header("Projectile")]
    public Rigidbody2D projectilePrefab;
    public float bulletSpeed;

    [field: Header("UI Elements")]
    public GameObject warningText;

    private Vector3 _originalScale;
    private void Awake()
    {
        base.Awake();
        IdleState = new GolemIdle(this, stateMachine);
        ChaseState = new GolemChase(this, stateMachine);
        AttackState = new GolemAttack(this, stateMachine);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.Instance.transform;
        spawnLocation = transform.position;
        _damageSource = GetComponent<DamageSource>();
        _originalScale = transform.localScale;
        _sr = GetComponent<SpriteRenderer>();
        spikeAnim = GameObject.Find("GolemSpikes").GetComponent<Animator>();
        stateMachine.Init(IdleState);
        spike.SetActive(false);
    }
}
