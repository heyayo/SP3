using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class MinoIdleState : EnemyState
{
    private MinotaurTwo _minotaur;

    private Vector3 _targetPos;
    private Vector2 _direction;

    public MinoIdleState(MinotaurTwo mino, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _minotaur = mino;
    }

    public override void EnterState()
    {
        //Debug.Log("Enter Minotaur Idle State");
        _targetPos = GetRandomPointInCircle();
        _direction = (_targetPos - _minotaur.transform.position).normalized; // Calculate the unit vector from player to enemy
    }

    public override void ExitState()
    {
        //Debug.Log("Exit Minotaur Idle State");
    }

    public override void FrameUpdate()
    {
        #region Old Code
        //Vector2 d2p = _minotaur.target.position - _minotaur.transform.position;
        //float d2pf = d2p.magnitude;
        //if (d2pf <= 10)
        //{
        //    _stateMachine.ChangeState(_minotaur.ChaseState);
        //}
        #endregion
        //Debug.Log("IS AGGROED IS: " + _minotaur.isAggroed);
        if (_minotaur.isAggroed)
        {
            _minotaur.stateMachine.ChangeState(_minotaur.ChaseState);
        }
        if (_minotaur.Mortality.Health <= 50)
        {
            _minotaur.stateMachine.ChangeState(_minotaur.FleeState);
        }

        if ((_minotaur.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetRandomPointInCircle();
            _direction = (_targetPos - _minotaur.transform.position).normalized; // Calculate the unit vector from player to enemy
        }
    }

    public override void PhysicsUpdate()
    {
        _minotaur.MoveEnemy(_direction * _minotaur.moveSpeed);
        _minotaur.CheckLeftOrRightFacing(_direction);
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    private Vector3 GetRandomPointInCircle()
    {
        return _minotaur.spawnLocation + (Vector2)UnityEngine.Random.insideUnitCircle * _minotaur.wanderRange;
    }
}

public class MinoChaseState : EnemyState
{
    private MinotaurTwo _minotaur;
    private Vector2 _direction;
    public MinoChaseState(MinotaurTwo mino, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _minotaur = mino;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Minotaur Chase State");
    }

    public override void ExitState()
    {
        Debug.Log("Exit Minotaur Chase State");
    }

    public override void FrameUpdate()
    {
        if (_minotaur.isInStrikingDistance)
        {
            _minotaur.stateMachine.ChangeState(_minotaur.AttackState);
        }
        if (!_minotaur.isAggroed)
        {
            _minotaur.stateMachine.ChangeState(_minotaur.IdleState);
        }
        if (_minotaur.Mortality.Health <= 50)
        {
            _minotaur.stateMachine.ChangeState(_minotaur.FleeState);
        }
    }

    public override void PhysicsUpdate()
    {
        _direction = (_minotaur.target.position - _minotaur.transform.position).normalized;
        _minotaur.MoveEnemy(_direction * _minotaur.moveSpeed);
        _minotaur.CheckLeftOrRightFacing(_direction);
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }
}

public class MinoAttackState : EnemyState
{
    private MinotaurTwo _minotaur;
    private bool canLeave = false;
    
    public MinoAttackState(MinotaurTwo enemy, EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _minotaur = enemy;
    }

    public override void EnterState()
    {
        canLeave = false;
        WaitABit();
        _minotaur.enemyAnimator.SetTrigger("Attack");
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        if (canLeave)
        {
            _minotaur.stateMachine.ChangeState(_minotaur.ChaseState);
        }
        if (_minotaur.Mortality.Health <= 50)
        {
            _minotaur.stateMachine.ChangeState(_minotaur.FleeState);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    private async Task WaitABit()
    {
        Debug.Log("ATTACK");
        await Task.Delay(1000);
        Debug.Log("ATTACK OVER");
        canLeave = true;
    }
}

public class MinoFleeState : EnemyState
{
    private MinotaurTwo _minotaur;
    private Vector2 _direction;
    public MinoFleeState(MinotaurTwo enemy,  EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _minotaur = enemy;
    }

    public override void EnterState()
    {
        Debug.Log("Entered Flee State");
        DelayState();
    }

    public override void ExitState()
    {
        Debug.Log("Exited Flee State");
    }

    public override void FrameUpdate()
    {
        _direction = -(_minotaur.target.position - _minotaur.transform.position).normalized;
        _minotaur.CheckLeftOrRightFacing(_direction);
    }

    public override void PhysicsUpdate()
    {
        _minotaur.MoveEnemy(_direction * _minotaur.moveSpeed);
        _minotaur.CheckLeftOrRightFacing(_direction);
    }
    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {

    }

    private async void DelayState()
    {
        await Task.Delay(3000);
        _stateMachine.ChangeState(_minotaur.HealState);
    }
}
public class MinoHealState : EnemyState
{
    private MinotaurTwo _minotaur;
    public MinoHealState(MinotaurTwo enemy, EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _minotaur = enemy;
    }

    public override void EnterState()
    {
        Debug.Log("ENTERED HEAL STATE");
        _minotaur.MoveEnemy(Vector2.zero); // Stop enemy when enter state
        _minotaur.Mortality.ApplyAffliction(new HealthHeal(50, 2, _minotaur.Mortality));
    }

    public override void ExitState()
    {
        Debug.Log("EXITED HEAL STATE");
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

}
