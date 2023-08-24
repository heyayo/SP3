using System.Threading.Tasks;
using UnityEngine;
[SerializeField]
public class UnicornIdle : EnemyState
{
    private UnicornEnemy _unicorn;

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
        if (_unicorn.Mortality.Health <= 50)
        {
            _unicorn.stateMachine.ChangeState(_unicorn.AttackState2);
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
    private UnicornEnemy _unicorn;
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
        if (_unicorn.Mortality.Health <= 50)
        {
            _unicorn.stateMachine.ChangeState(_unicorn.AttackState2);
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
    private UnicornEnemy _unicorn;

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
        //_unicorn.enemyAnimator.SetBool("isWalking", false);
        if (!_unicorn.isInStrikingDistance)
        {
            _unicorn.stateMachine.ChangeState(_unicorn.ChaseState);
        }
        if (_unicorn.Mortality.Health <= 50)
        {
            _unicorn.stateMachine.ChangeState(_unicorn.AttackState2);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }
}

public class UnicornAttack2 : EnemyState
{
    private UnicornEnemy _unicorn;
    private GameObject batInstance1;
    private BatCollisionManager bat;

    private bool batsSpawned = false; // Flag to track if bats have been spawned
    private Vector2 _direction;
    private float _timer;
    private float _batSpeed = 10f;
    public UnicornAttack2(UnicornEnemy unicorn, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _unicorn = unicorn;
    }

    public override void EnterState()
    {
        Debug.Log("ENTERED ATTACKING STATE 2");
        WaitAttack2State();
        // Only spawn bats if they haven't been spawned before
        if (!batsSpawned)
        {
            SpawnBats();
            batsSpawned = true; // Set the flag to true
        }
        _unicorn.enemyAnimator.SetTrigger("isAttacking2");
    }

    public override void ExitState()
    {
        Debug.Log("EXITED ATTACKING STATE 2");
    }

    public override void FrameUpdate()
    {
        _unicorn.barrier.transform.position = _unicorn.transform.position;
        if (bat.isStuck == true)
        {
            if (_timer < 5)
            {
                _unicorn.stuckText.SetActive(true);
                _unicorn.target.position = bat.prev;
            }else
            {
                _timer = 0;
                _batSpeed = 5f; 
                bat.isStuck = false;
                _unicorn.stuckText.SetActive(false);
            }
            _timer += Time.deltaTime;
            _unicorn.enemyAnimator.SetBool("isWalking", false);
        }
    }

    public override void PhysicsUpdate()
    {
        _unicorn.MoveEnemy(Vector2.zero);
        _direction = (_unicorn.target.position - batInstance1.transform.position).normalized; // Find dir between bat and target
        batInstance1.GetComponent<Rigidbody2D>().velocity = _direction * _batSpeed;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {

    }

    private async void WaitAttack2State()
    {
        await Task.Delay(2000);
    }

    private void SpawnBats()
    {
        batInstance1 = GameObject.Instantiate(_unicorn.bat);
        bat = batInstance1.GetComponent<BatCollisionManager>();
        bat.unicorn = _unicorn;
        Vector3 offset = new Vector3(2.0f, -1f, 0.0f); // Adjust the offset as needed
        batInstance1.transform.position = _unicorn.transform.position + offset;
        _unicorn.bubblee = GameObject.Instantiate(_unicorn.barrier,_unicorn.transform.position,Quaternion.identity);
    }
}

public class UnicornDeath : EnemyState
{
    private UnicornEnemy _unicorn;
    public UnicornDeath(UnicornEnemy unicorn, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _unicorn = unicorn;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    public override void EnterState()
    {
        Debug.Log("AAAA");
        WaitForDead();
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
    private async void WaitForDead()
    {
        await Task.Delay(2000);
        GameObject.Destroy(_unicorn.bubblee);
        GameObject.Destroy(_unicorn.gameObject);
    }
}
    