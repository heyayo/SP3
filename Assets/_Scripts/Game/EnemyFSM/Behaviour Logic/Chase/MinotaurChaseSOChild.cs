using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurChaseSOChild : EnemyChaseSOBase
{
    protected Minotaur minotaur;
    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();
        if (!enemy.isAggroed)
        {
            minotaur.stateMachine.ChangeState(minotaur.healingState);
            //enemy.stateMachine.ChangeState(enemy.idleState);
            enemy.enemyAnimator.SetBool("isDashing", false);
        }
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
    }

    public override void ResetValue()
    {
        base.ResetValue();
    }
}
