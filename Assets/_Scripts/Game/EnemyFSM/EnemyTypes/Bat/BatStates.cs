using UnityEngine;
using System.Threading.Tasks;
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
    }

    public override void ExitState()
    {
        Debug.Log("Exit Bat Idle State");
    }

    public override void FrameUpdate()
    {
        if (_bat.isAggroed)
        {
            _bat.stateMachine.ChangeState(_bat.ChaseState);
        }
        if ((_bat.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetRandomPointInCircle();
            _direction = (_targetPos - _bat.transform.position).normalized; // Calculate the unit vector from player to enemy
        }
        if (_bat.Mortality.Health <= 50f)
        {
            _bat.stateMachine.ChangeState(_bat.TourettesState);
        }
        if (_bat.Mortality.Health <= 0f)
        {
            _bat.stateMachine.ChangeState(_bat.DeathState);
        }
    }

    public override void PhysicsUpdate()
    {
        _direction = (_targetPos - _bat.transform.position).normalized; // Calculate the unit vector from player to enemy
        _bat.MoveEnemy(_direction * _bat.moveSpeed);
        _bat.CheckLeftOrRightFacing(_direction);
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
    private Vector2 _direction;
    public BatChase(BatEnemy bat, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _bat = bat;
    }
    public override void EnterState()
    {
        //Debug.Log("Entered Bat Chase State");
        _bat.enemyAnimator.SetBool("isDashing", true);
    }

    public override void ExitState()
    {
        _bat.enemyAnimator.SetBool("isDashing", false);
        //Debug.Log("Exit Bat Chase State");
    }

    public override void FrameUpdate()
    {
        if (!_bat.isAggroed)
        {
            _bat.stateMachine.ChangeState(_bat.IdleState);
        }
        else if (_bat.isInStrikingDistance)
        {
           _bat.stateMachine.ChangeState(_bat.AttackState);
        }
        if (_bat.Mortality.Health <= 50)
        {
            _bat.stateMachine.ChangeState(_bat.TourettesState);
        }
        if (_bat.Mortality.Health <= 0f)
        {
            _bat.stateMachine.ChangeState(_bat.DeathState);
        }
    }

    public override void PhysicsUpdate()
    {
        _direction = (_bat.target.position - _bat.transform.position).normalized;
        _bat.MoveEnemy(_direction * _bat.moveSpeed);
        _bat.CheckLeftOrRightFacing(_direction);
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
        if (_bat.Mortality.Health <= 40)
        {
            _bat.stateMachine.ChangeState(_bat.TourettesState);
        }
        if (_bat.Mortality.Health <= 0f)
        {
            _bat.stateMachine.ChangeState(_bat.DeathState);
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

public class BatTourettes : EnemyState
{
    private BatEnemy _bat;
    private float _rotationSpeed = 500f;
    private Vector3 _rotateAmount = new Vector3( 0f, 0f, 45f );
    public BatTourettes(BatEnemy bat, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _bat = bat;
    }

    public override void EnterState()
    {
        Debug.Log("ENTERED THE TOURETTES STATE");
        WaitForTourettesState();
    }

    public override void ExitState()
    {
        Debug.Log("EXITED THE TOURETTES STATE");
    }

    public override void FrameUpdate()
    {
        _bat.transform.Rotate(_rotateAmount * _rotationSpeed * Time.deltaTime);
        if (_rotateAmount.z >= 45) _rotateAmount.z = -Mathf.Abs(_rotateAmount.z);
        if (_bat.Mortality.Health <= 0f)
        {
            _bat.stateMachine.ChangeState(_bat.DeathState);
        }
    }

    public override void PhysicsUpdate()
    {
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {

    }
    private async Task WaitForTourettesState()
    {
        await Task.Delay(4000);
        _bat.stateMachine.ChangeState(_bat.BleedState);
    }
}

public class BatBleed : EnemyState
{
    public BatEnemy _bat;
    private Vector2 _direction;
    bool isLunging = false;
    public BatBleed(BatEnemy bat, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _bat = bat;
    }

    public override void EnterState()
    {
        Debug.Log("BAT DAMAGE SOURCE IS " + _bat._damageSource);
        _bat.bulletPrefab.gameObject.SetActive(false);
        _bat._damageSource.bleedOverPercentage = 40;
        _bat._sr.color = Color.red;
        _bat.transform.rotation = Quaternion.identity;
        _bat.bulletSpeed *= 2f;
    }

    public override void ExitState()
    {
        _bat.bulletPrefab.gameObject.SetActive(false);
    }

    public override void FrameUpdate()
    {
        _direction = (_bat.target.position - _bat.transform.position).normalized;
        if (_bat.Mortality.Health <= 0f)
        {
            _bat.stateMachine.ChangeState(_bat.DeathState);
        }
    }

    public override void PhysicsUpdate()
    {
        _bat.CheckLeftOrRightFacing(_direction);
        if (!isLunging)
        {
            _bat.rb.AddForce(_direction * 100, ForceMode2D.Impulse);
            WaitForLunge();
        }
    }
    private async void WaitForLunge()
    {
        isLunging = true;
        await Task.Delay(2000);
        isLunging = false;
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {

    }
}

public class BatDeath : EnemyState
{
    private BatEnemy _bat;
    public BatDeath(BatEnemy bat,EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _bat = bat;
    }

    public override void EnterState()
    {
        WaitForExplode();
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
    private async void WaitForExplode()
    {
        _bat.enemyAnimator.SetTrigger("isExplode");
        await Task.Delay(1000);
        GameObject.Destroy(_bat.gameObject);
        GameObject.Destroy(_bat.bulletPrefab);
    }
}