using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class MinoIdleState : EnemyState
{
    private MinotaurTwo _minotaur;

    public MinoIdleState(MinotaurTwo mino, EnemyStateMachine stateMachine) : base(stateMachine)
    {
        _minotaur = mino;
    }

    public override void EnterState()
    {
        Debug.Log("Enter Minotaur Idle State");
    }

    public override void ExitState()
    {
        Debug.Log("Exit Minotaur Idle State");
    }

    public override void FrameUpdate()
    {
        Vector2 d2p = _minotaur.target.position - _minotaur.transform.position;
        float d2pf = d2p.magnitude;
        if (d2pf <= 10)
        {
            _stateMachine.ChangeState(_minotaur.ChaseState);
        }

        if (_minotaur.Mortality.Health <= 50)
        {
            _stateMachine.ChangeState(_minotaur.HealState);
        }
    }

    public override void PhysicsUpdate()
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }
}

public class MinoChaseState : EnemyState
{
    private MinotaurTwo _minotaur;
    
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
        Vector2 d2p = _minotaur.target.position - _minotaur.transform.position;
        float d2pf = d2p.magnitude;
        if (d2pf <= 3)
        {
            _stateMachine.ChangeState(_minotaur.AttackState);
        }
        if (d2pf >= 10)
            _stateMachine.ChangeState(_minotaur.IdleState);
    }

    public override void PhysicsUpdate()
    {
        Vector2 d2p = _minotaur.target.position - _minotaur.transform.position;
        d2p.Normalize();
        
        _minotaur.rb.AddForce(d2p * _minotaur.moveSpeed);
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
            _stateMachine.ChangeState(_minotaur.ChaseState);
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
        await Task.Delay(2000);
        Debug.Log("ATTACK OVER");
        canLeave = true;
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
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        _minotaur.Mortality.Health = _minotaur.Mortality.__HealthMax;
        _stateMachine.ChangeState(_minotaur.IdleState);
    }

    public override void PhysicsUpdate()
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }
}
