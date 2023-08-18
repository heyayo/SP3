using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeOfCthulhu : Enemy, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    #region State Machine variables

    //[field: SerializeField] public EOCTransformState transformState { get; set; }
    [field: SerializeField] public EOCEnragedChaseState enragedChaseState { get; set; }
    #endregion

    #region Scriptable object Variables
   
    //[SerializeField] public EOCTransformSOBase TransformBase;
    [SerializeField] public EOCEnragedChaseSOBase EnragedChaseBase;

    //public EOCTransformSOBase transformBaseInstance { get; set; }
    public EOCEnragedChaseSOBase enragedChaseBaseInstance { get; set; }
    #endregion

    override protected void InitAwake()
    {
       
       // transformBaseInstance = Instantiate(TransformBase);
        enragedChaseBaseInstance = Instantiate(EnragedChaseBase);

        stateMachine = new EnemyStateMachine();
        
        //transformState = new EOCTransformState(this, stateMachine);
        enragedChaseState = new EOCEnragedChaseState(this, stateMachine);
    }
    override protected void InitStart()
    {
        //transformBaseInstance.Init(gameObject, this);

        currentHealth = maxHealth; 
        _rb = GetComponent<Rigidbody2D>();
       // stateMachine.Init(transformState); // Init state machine with idle animation
        enemyAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        stateMachine.
            currentEnemyState.
            PhysicsUpdate(); 
    }
    private void Update()
    {
        stateMachine.currentEnemyState.FrameUpdate();
    }

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
}
