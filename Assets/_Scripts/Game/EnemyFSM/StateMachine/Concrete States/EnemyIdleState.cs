using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Enemy _enemy;
    
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine) : base(stateMachine)
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
        if (_enemy.isAggroed)
        {
            Debug.Log("Entered Chase State");
            _enemy.stateMachine.ChangeState(_enemy.chaseState);
        }
    }

    public override void PhysicsUpdate()
    {
    }
}
