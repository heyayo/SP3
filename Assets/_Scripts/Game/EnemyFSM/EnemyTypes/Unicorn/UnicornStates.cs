using UnityEngine;
[SerializeField]
public class UnicornIdle : EnemyState
{
    public UnicornEnemy _unicorn;

    private Vector3 _targetPos;
    private Vector2 _direction;

    public UnicornIdle(UnicornEnemy unicorn, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _unicorn = unicorn;
    }
        
    public override void EnterState()
    {
        _targetPos = GetRandomPointInCircle();
        _direction = (_targetPos - _unicorn.transform.position).normalized; // Calculate the unit vector from player to enemy
        _unicorn.enemyAnimator.SetBool("isWalking", true);
    }

    public override void ExitState()
    {
        Debug.Log("Exit Bat Idle State");
    }

    public override void FrameUpdate()
    {
        if (_unicorn.isAggroed)
        {
            _unicorn.stateMachine.ChangeState(_unicorn.ChaseState);
        }
        if ((_unicorn.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetRandomPointInCircle();
            _direction = (_targetPos - _unicorn.transform.position).normalized; // Calculate the unit vector from player to enemy
        }
        _unicorn.CheckLeftOrRightFacing(_direction);
    }

    public override void PhysicsUpdate()
    {
        _direction = (_targetPos - _unicorn.transform.position).normalized; // Calculate the unit vector from player to enemy
        _unicorn.MoveEnemy(_direction * _unicorn.moveSpeed);
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    private Vector3 GetRandomPointInCircle()
    {
        return _unicorn.spawnLocation + (Vector2)UnityEngine.Random.insideUnitCircle * _unicorn.wanderRange;
    }
}

public class UnicornChase : EnemyState
{
    public UnicornEnemy _unicorn;
    private Vector2 _direction;
    public UnicornChase(UnicornEnemy unicorn, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _unicorn = unicorn;
    }
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
      
    }

    public override void FrameUpdate()
    {
        if (!_unicorn.isAggroed)
        {
            _unicorn.stateMachine.ChangeState(_unicorn.IdleState);
        }
        else if (_unicorn.isInStrikingDistance)
        {
            _unicorn.stateMachine.ChangeState(_unicorn.AttackState);
        }
        _unicorn.CheckLeftOrRightFacing(_direction);
    }

    public override void PhysicsUpdate()
    {
        _direction = (_unicorn.target.position - _unicorn.transform.position).normalized;

        _unicorn.MoveEnemy(_direction * _unicorn.moveSpeed);
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {

    }
}

public class UnicornAttack : EnemyState
{
    public UnicornEnemy _unicorn;


    public UnicornAttack(UnicornEnemy unicorn, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _unicorn = unicorn;
    }

    public override void EnterState()
    {
        _unicorn.enemyAnimator.SetBool("isAttacking", true);
    }

    public override void ExitState()
    {
        _unicorn.enemyAnimator.SetBool("isAttacking", false);
    }

    public override void FrameUpdate()
    {
        _unicorn.enemyAnimator.SetBool("isWalking", false);
        if (!_unicorn.isInStrikingDistance)
        {
            _unicorn.stateMachine.ChangeState(_unicorn.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }
}
