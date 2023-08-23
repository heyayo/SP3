using UnityEngine;
[SerializeField]
public class GolemIdle : EnemyState
{
    public GolemEnemy _golem;

    private Vector3 _targetPos;
    private Vector2 _direction;

    public GolemIdle(GolemEnemy golem, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _golem = golem;
    }
        
    public override void EnterState()
    {
        _targetPos = GetRandomPointInCircle();
        _direction = (_targetPos - _golem.transform.position).normalized; // Calculate the unit vector from player to enemy
    }

    public override void ExitState()
    {
        Debug.Log("Exit Bat Idle State");
    }

    public override void FrameUpdate()
    {
        if (_golem.isAggroed)
        {
            _golem.stateMachine.ChangeState(_golem.ChaseState);
        }
        if ((_golem.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetRandomPointInCircle();
            _direction = (_targetPos - _golem.transform.position).normalized; // Calculate the unit vector from player to enemy
        }
    }

    public override void PhysicsUpdate()
    {
        _direction = (_targetPos - _golem.transform.position).normalized; // Calculate the unit vector from player to enemy
        _golem.MoveEnemy(_direction * _golem.moveSpeed);
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    private Vector3 GetRandomPointInCircle()
    {
        return _golem.spawnLocation + (Vector2)UnityEngine.Random.insideUnitCircle * _golem.wanderRange;
    }
}

public class GolemChase : EnemyState
{
    public GolemEnemy _golem;
    private Vector2 _direction;
    public GolemChase(GolemEnemy golem, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _golem = golem;
    }
    public override void EnterState()
    {
        //Debug.Log("Entered Bat Chase State");
        _golem.enemyAnimator.SetBool("isDashing", true);
    }

    public override void ExitState()
    {
        _golem.enemyAnimator.SetBool("isDashing", false);
        //Debug.Log("Exit Bat Chase State");
    }

    public override void FrameUpdate()
    {
        if (!_golem.isAggroed)
        {
            _golem.stateMachine.ChangeState(_golem.IdleState);
        }
        else if (_golem.isInStrikingDistance)
        {
           _golem.stateMachine.ChangeState(_golem.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        _direction = (_golem.target.position - _golem.transform.position).normalized;

        _golem.MoveEnemy(_direction * _golem.moveSpeed);
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {

    }
}

public class GolemAttack : EnemyState
{
    public GolemEnemy _golem;

    private float _shootTimer = 0f;

    public GolemAttack(GolemEnemy golem, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _golem = golem;
    }

    public override void EnterState()
    {
        _golem.bulletPrefab.gameObject.SetActive(false);
        _shootTimer = 0f;
    }

    public override void ExitState()
    {
        _golem.bulletPrefab.gameObject.SetActive(false);
    }

    public override void FrameUpdate()
    {
        if (Vector2.Distance(_golem.target.position, _golem.transform.position) > _golem.distanceToCountExit)
        {
            _golem.exitTimer += Time.deltaTime;
            if (_golem.exitTimer > _golem.timeTillExit)
            {
                _golem.stateMachine.ChangeState(_golem.ChaseState);
            }
        }
        else
        {
            _golem.exitTimer = 0f;
        }
    }

    public override void PhysicsUpdate()
    {
        _golem.MoveEnemy(Vector2.zero);
        if (_shootTimer >= _golem.timeBetweenShots)
        {
            _golem.bulletPrefab.gameObject.SetActive(true);
            Rigidbody2D bullet = GameObject.Instantiate(_golem.bulletPrefab, _golem.transform.position, Quaternion.identity);
            Vector2 dir = (_golem.target.position - _golem.transform.position).normalized;
            bullet.velocity = dir * _golem.bulletSpeed;
            _shootTimer = 0f;
        }
        else
        {
            _golem.bulletPrefab.gameObject.SetActive(false);
        }
        _shootTimer += Time.deltaTime;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }
}
