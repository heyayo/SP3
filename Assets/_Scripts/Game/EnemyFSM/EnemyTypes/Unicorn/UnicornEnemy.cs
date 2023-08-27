using UnityEngine;
using TMPro;
public class UnicornEnemy : Enemy
{
    [HideInInspector] public Vector2 spawnLocation;
    [HideInInspector] public DamageSource _damageSource;
    [HideInInspector] public SpriteRenderer _sr;

    [field: Header("Unicorn States")]
    public UnicornIdle IdleState;
    public UnicornChase ChaseState;
    public UnicornAttack AttackState;
    public UnicornAttack2 AttackState2;
    public UnicornDeath DeathState;

    [field:Header("State Settings")]
    public float wanderRange = 8f;

    [field:Header("Player Settinsgs")]
    public Transform target;
    public float moveSpeed = 8f;

    [field: Header("Second Phase/Bat Settings")]
    public GameObject bat;
    private Vector3 _originalScale;

    [field: Header("UI Elements")]
    public GameObject stuckText;


    [field: Header("Bubble Barrier")]
    public GameObject barrier;

    public GameObject bubblee;

    private void Awake()
    {
        base.Awake();
        IdleState = new UnicornIdle(this, stateMachine);
        ChaseState = new UnicornChase(this, stateMachine);
        AttackState = new UnicornAttack(this, stateMachine);
        AttackState2 = new UnicornAttack2(this, stateMachine);
        DeathState = new UnicornDeath(this, stateMachine);
    }

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.Instance.transform;
        spawnLocation = transform.position;
        _damageSource = GetComponent<DamageSource>();
        _originalScale = transform.localScale;
        _sr = GetComponent<SpriteRenderer>();
        stateMachine.Init(IdleState);
        Mortality.onHealthZero.AddListener(() =>
        {
            stateMachine.ChangeState(DeathState);
        });

        stuckText = InventoryManager.Instance.stuckTexta;
    }
}
