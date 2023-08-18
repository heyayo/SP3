using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Enemy _enemy;
    
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _enemy = enemy;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void FrameUpdate()
    {
        if (!_enemy.isAggroed)
        {
            Debug.Log("Entered Chase State");
            _enemy.stateMachine.ChangeState(_enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
    }
}
