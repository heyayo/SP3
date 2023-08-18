using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Enemy _enemy;
    
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
        _enemy = enemy;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entered chase state");
    }

    public override void ExitState()
    { 
    }

    public override void FrameUpdate()
    {
        _enemy.enemyAnimator.SetBool("isDashing", true);
        if (_enemy.isInStrikingDistance) // If _enemy collides with striking distance collider, then change to attack state
        {
            _stateMachine.ChangeState(_enemy.attackState);
        }
        if (!_enemy.isAggroed)
        {
            //_enemy.stateMachine.ChangeState(_enemy.idleState);
            _enemy.enemyAnimator.SetBool("isDashing", false);
        }
    }   

    public override void PhysicsUpdate()
    {
    }
}
