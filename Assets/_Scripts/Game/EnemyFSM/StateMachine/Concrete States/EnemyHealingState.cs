using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealingState : EnemyState
{
    public EnemyHealingState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        minotaur.enemyHealingStateInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        minotaur = enemy.GetComponent<Minotaur>();
        minotaur.enemyHealingStateInstance.DoEnterLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        minotaur.enemyHealingStateInstance.DoExitLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        minotaur.enemyHealingStateInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        minotaur.enemyHealingStateInstance.DoFrameUpdateLogic();
    }
}
