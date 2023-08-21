using UnityEngine;
[SerializeField]
public class BatIdle : EnemyState
{
    public BatEnemy _bat;

    private Vector3 _targetPos;
    private Vector2 _direction;

    public BatIdle(BatEnemy bat, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _bat = bat;
    }

    public override void EnterState()
    {
        _targetPos = GetRandomPointInCircle();
        _direction = (_targetPos - _bat.transform.position).normalized; // Calculate the unit vector from player to enemy
        Debug.Log("Enter Bat Idle State");
    }

    public override void ExitState()
    {
        Debug.Log("Exit Bat Idle State");
    }

    public override void FrameUpdate()
    {
        Vector2 d2p = _bat.target.position - _bat.transform.position;
        float d2pf = d2p.magnitude;
        if (d2pf <= 10)
        {
            _stateMachine.ChangeState(_bat.ChaseState);
        }
        if ((_bat.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetRandomPointInCircle();
            _direction = (_targetPos - _bat.transform.position).normalized; // Calculate the unit vector from player to enemy
        }
    }

    public override void PhysicsUpdate()
    {
        _bat.MoveEnemy(_direction * _bat.moveSpeed);
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    private Vector3 GetRandomPointInCircle()
    {
        return _bat.spawnLocation + (Vector2)UnityEngine.Random.insideUnitCircle * _bat.wanderRange;
    }

}

public class BatChase : EnemyState
{
    public BatEnemy _bat;
    public BatChase(BatEnemy bat, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _bat = bat;
    }


    public override void EnterState()
    {
        Debug.Log("Entered Bat Chase State");
        _bat.anim.SetBool("isDashing", true);
    }

    public override void ExitState()
    {
        _bat.anim.SetBool("isDashing", false);
        Debug.Log("Exit Bat Chase State");
    }

    public override void FrameUpdate()
    {
        //Vector2 d2p = _bat.target.position - _bat.transform.position;
        //float d2pf = d2p.magnitude;
        //if (d2pf <= 3)
        //{
        //    _stateMachine.ChangeState(_bat.AttackState);
        //}
        //if (d2pf >= 10)
        //    _stateMachine.ChangeState(_bat.IdleState);

        if (_bat.isInStrikingDistance)
        {
            _stateMachine.ChangeState(_bat.AttackState);
        }
        else
        {
            _stateMachine.ChangeState(_bat.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        Vector2 d2p = (_bat.target.position - _bat.transform.position).normalized;

        _bat.MoveEnemy(d2p * _bat.moveSpeed);
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {

    }
}

public class BatAttack : EnemyState
{
    public BatEnemy _bat;

    private float _shootTimer = 0f;

    public BatAttack(BatEnemy bat, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _bat = bat;
    }

    public override void EnterState()
    {
        _bat.bulletPrefab.gameObject.SetActive(false);
        _shootTimer = 0f;
    }

    public override void ExitState()
    {
        _bat.bulletPrefab.gameObject.SetActive(false);
    }

    public override void FrameUpdate()
    {
        if (Vector2.Distance(_bat.target.position, _bat.transform.position) > _bat.distanceToCountExit)
        {
            _bat.exitTimer += Time.deltaTime;
            if (_bat.exitTimer > _bat.timeTillExit)
            {
                _bat.stateMachine.ChangeState(_bat.ChaseState);
            }
        }
        else
        {
            _bat.exitTimer = 0f;
        }
    }

    public override void PhysicsUpdate()
    {
        _bat.MoveEnemy(Vector2.zero);
        if (_shootTimer >= _bat.timeBetweenShots)
        {
            _bat.bulletPrefab.gameObject.SetActive(true);
            Rigidbody2D bullet = GameObject.Instantiate(_bat.bulletPrefab, _bat.transform.position, Quaternion.identity);
            Vector2 dir = (_bat.target.position - _bat.transform.position).normalized;
            bullet.velocity = dir * _bat.bulletSpeed;
            _shootTimer = 0f;
        }
        else
        {
            _bat.bulletPrefab.gameObject.SetActive(false);
        }
        _shootTimer += Time.deltaTime;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }
}

