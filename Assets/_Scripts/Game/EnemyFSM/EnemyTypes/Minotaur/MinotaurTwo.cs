using UnityEngine;

public class MinotaurTwo : Enemy
{
    [HideInInspector] public Vector2 spawnLocation;
    // Additional States
    public MinoIdleState IdleState;
    public MinoChaseState ChaseState;
    public MinoAttackState AttackState; // Reuse Generic Attack State cause no specialization
    public MinoFleeState FleeState;
    public MinoHealState HealState;

    public Transform target;
        
    public float moveSpeed = 8;
    public float wanderRange = 20;

    private Vector3 _originalScale;
    private int _moveSpeedHash;

    protected virtual void Awake()
    {
        base.Awake();
        IdleState = new MinoIdleState(this,stateMachine);
        ChaseState = new MinoChaseState(this,stateMachine);
        AttackState = new MinoAttackState(this,stateMachine);
        HealState = new MinoHealState(this,stateMachine);
        FleeState = new MinoFleeState(this, stateMachine);

        _moveSpeedHash = Animator.StringToHash("moveSpeed");
    }
    protected virtual void Start()
    {
        base.Start();
        stateMachine.Init(IdleState);

        target = PlayerManager.Instance.transform;
        _originalScale = transform.localScale;
        spawnLocation = transform.position;
    }

    protected override void ChildUpdate()
    {
        var velo = rb.velocity;
        enemyAnimator.SetFloat(_moveSpeedHash, velo.magnitude);
        var scale = transform.localScale;
        if (velo.x > 0)
            scale.x = _originalScale.x;
        else if (velo.x < 0)
            scale.x = -_originalScale.x;
        transform.localScale = scale;
    }
}
