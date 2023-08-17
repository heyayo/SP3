using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    // ***************
    // Interfaces -  IDamageable, IEnemyMoveable, ITriggerCheckable contains optional methods for cleaner code
    // Base Enemy Script that handles creates and initiates all states
    // ***************
    [field: SerializeField] public float maxHealth { get; set; } = 100f;
    public Rigidbody2D _rb { get; set; }
    public float currentHealth { get; set; }
    public bool isFacingRight { get; set; } = true;

    #region State Machine variables
    [field: SerializeField] public EnemyStateMachine stateMachine { get; set; }
    [field: SerializeField] public EnemyIdleState idleState { get; set; }
    [field: SerializeField] public EnemyChaseState chaseState { get; set; }
    [field: SerializeField] public EnemyAttackState attackState { get; set; }

    public bool isAggroed { get; set; }
    public bool isInStrikingDistance { get; set; }

    [field: SerializeField]  public Animator enemyAnimator; // Every Enemy will have this
    #endregion

    #region Scriptable object Variables
    [SerializeField] private EnemyIdleSOBase EnemyIdleBase;
    [SerializeField] private EnemyChaseSOBase EnemyChaseBase;
    [SerializeField] private EnemyAttackSOBase EnemyAttackBase;

    public EnemyIdleSOBase enemyIdleBaseInstance{ get; set; }
    public EnemyChaseSOBase enemyChaseBaseInstance { get; set;}
    public EnemyAttackSOBase enemyAttackStateInstance { get; set; }
    #endregion

    protected void InitAwake() // InitAwake is called in the awake function in the scripts that inherit from this class
    {
        // Since our states are in a script thats not monobehaviour, we have to manually instantiate
        enemyIdleBaseInstance = Instantiate(EnemyIdleBase);
        enemyChaseBaseInstance = Instantiate(EnemyChaseBase);
        enemyAttackStateInstance = Instantiate(EnemyAttackBase);

        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }
    protected void InitStart() // InitStart is called in the start function in the scripts that inherit from this class
    {
        enemyIdleBaseInstance.Init(gameObject, this);
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackStateInstance.Init(gameObject, this);

        currentHealth = maxHealth; 
        _rb = GetComponent<Rigidbody2D>();
        Debug.Log(idleState);
        stateMachine.Init(idleState); // Init state machine with idle animation
        enemyAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()  // Handle physics calculations in fixed update
    {
        stateMachine.
            currentEnemyState.
            PhysicsUpdate(); 
    }
    private void Update() // Handles main logic in frame update
    {
        stateMachine.currentEnemyState.FrameUpdate();
    }

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
        _rb.velocity = velocity;
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
