using UnityEngine;

[RequireComponent(typeof(Mortality))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IEnemyMoveable, ITriggerCheckable
{
    [field: Header("External Scripts")]
    [field: SerializeField] public Rigidbody2D rb { get; set; }
    [field: SerializeField] public Mortality Mortality { get; private set; }

    // ***************
    // Interfaces -  IDamageable, IEnemyMoveable, ITriggerCheckable contains optional methods for cleaner code
    // Base Enemy Script that handles creates and initiates all states
    // ***************
    [field: SerializeField] public float maxHealth { get; set; } = 100f;
    [field: SerializeField] public Animator enemyAnimator; // Every Enemy will have this
    public float currentHealth { get; set; }
    public bool isFacingRight { get; set; } = true;
        
    #region State Machine variables
    [field: SerializeField] public EnemyStateMachine stateMachine { get; set; }
    [field: SerializeField] public EnemyIdleState idleState { get; set; }
    [field: SerializeField] public EnemyChaseState chaseState { get; set; }
    [field: SerializeField] public EnemyAttackState attackState { get; set; }

    public bool isAggroed { get; set; }
    public bool isInStrikingDistance { get; set; }

    #endregion
        
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Mortality = GetComponent<Mortality>();
        enemyAnimator = GetComponent<Animator>();

        stateMachine = new EnemyStateMachine();
    }

    protected virtual void Start()
    { }

    private void FixedUpdate()  // Handle physics calculations in fixed update
    { stateMachine.currentEnemyState.PhysicsUpdate(); }
    private void Update() // Handles main logic in frame update
    { stateMachine.currentEnemyState.FrameUpdate(); ChildUpdate();} 

    protected virtual void ChildUpdate()
    { }

    #region Health / Die functions
    public void Damage(float dmgtotake)
    {
        currentHealth -= dmgtotake;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    #endregion 

    #region Movement functions
    public void MoveEnemy(Vector2 velocity)
    {
        rb.velocity = velocity;
        CheckLeftOrRightFacing(velocity);
        enemyAnimator.SetTrigger("isWalking");
    }

    public void CheckLeftOrRightFacing(Vector2 velocity)
    {
        if (isFacingRight && velocity.x < 0f) // Player is facing left
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else if (!isFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }
    #endregion 

    private void _AnimationTriggerEvent(AnimationTriggerType triggerType) // Animation trigger enum to handle enemy animations callback
    {
        stateMachine.currentEnemyState.AnimationTriggerEvent(triggerType);
    }

    #region
    public void SetAggroStatus(bool isAggroed)
    {
        this.isAggroed = isAggroed; 
    }

    public void SetStrikingDistance(bool isInStrikingDistance)
    {
        this.isInStrikingDistance = isInStrikingDistance;
    }
    #endregion
    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }
}
