using UnityEngine;
public class UnicornEnemy : Enemy
{
    [HideInInspector] public Vector2 spawnLocation;
    [HideInInspector] public DamageSource _damageSource;
    [HideInInspector] public SpriteRenderer _sr;

    [field: Header("Unicorn States")]
    public UnicornIdle IdleState;
    public UnicornChase ChaseState;
    public UnicornAttack AttackState;

    [field:Header("State Settings")]
    public float wanderRange = 8f;

    [field:Header("Player Settinsgs")]
    public Transform target;
    public float moveSpeed = 8f;

    private Vector3 _originalScale;
    private void Awake()
    {
        base.Awake();
        IdleState = new UnicornIdle(this, stateMachine);
        ChaseState = new UnicornChase(this, stateMachine);
        AttackState = new UnicornAttack(this, stateMachine);
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
    }
}
