using UnityEngine;
using System.Threading.Tasks;
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
        if (_golem.Mortality.Health <= 0f)
        {
            _golem.stateMachine.ChangeState(_golem.DeathState);
        }
        _golem.CheckLeftOrRightFacing(_direction);
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
        if (_golem.Mortality.Health <= 0f)
        {
            _golem.stateMachine.ChangeState(_golem.DeathState);
        }
        _golem.CheckLeftOrRightFacing(_direction);
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
     
    public GolemAttack(GolemEnemy golem, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _golem = golem;
    }

    public override void EnterState()
    {
        _golem.enemyAnimator.SetBool("isAttacking", true);
        _golem.projectilePrefab.gameObject.SetActive(false);
    }

    public override void ExitState()
    {
        _golem.projectilePrefab.gameObject.SetActive(false);
    }

    public override void FrameUpdate()
    {
        if (_golem.Mortality.Health <= 0f)
        {
            _golem.stateMachine.ChangeState(_golem.DeathState);
        }
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            if (Vector2.Distance(_golem.target.position, _golem.transform.position) > _golem.distanceToCountExit)
            {
                _golem.exitTimer += Time.deltaTime;
                _golem.projectilePrefab.gameObject.SetActive(true);
                Rigidbody2D bullet = GameObject.Instantiate(_golem.projectilePrefab, _golem.transform.position, Quaternion.identity);
                Vector2 dir = (_golem.target.position - _golem.transform.position).normalized;
                bullet.velocity = dir * _golem.bulletSpeed;
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
        else if (rand == 1)
        {
            if(!_golem.warningText.activeInHierarchy)
                WarningDelay();
        }
    }

    public override void PhysicsUpdate()
    {
        _golem.MoveEnemy(Vector2.zero);
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    private async void WarningDelay()
    {
        _golem.warningText.SetActive(true);
        await Task.Delay(1500);
        _golem.warningText.SetActive(false);
        _golem.spikeAnim.SetTrigger("isSpike");
        _golem.spike.transform.position = _golem.target.transform.position;
        await Task.Delay(500);
        _golem.spike.SetActive(true);
        //_golem.spikeAnim.SetTrigger("isSpike");
        _golem.enemyAnimator.SetBool("isAttacking", false);
    }
}

public class GolemDeath : EnemyState
{
    private GolemEnemy _golem;
    public GolemDeath(GolemEnemy golem, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _golem = golem;
    }

    public override void EnterState()
    {
        Debug.Log("ENTERED GOLEM DEATH STATE");
        ExplodeEnemy();
    }

    public override void ExitState()
    {
    
    }

    public override void FrameUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
    
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {

    }
    private async void ExplodeEnemy()
    {
        _golem.enemyAnimator.SetTrigger("isExplode");
        await Task.Delay(2000);
        GameObject.Destroy(_golem.gameObject);
        GameObject.Destroy(_golem.spike);
    }
}
