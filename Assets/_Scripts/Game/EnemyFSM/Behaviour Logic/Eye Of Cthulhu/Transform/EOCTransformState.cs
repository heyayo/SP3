using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EOCTransformState : EnemyState
{
    public EOCTransformState(EyeOfCthulhu enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.GetComponent<EyeOfCthulhu>().transformBaseInstance.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered EOC Transform State");
        enemy.GetComponent<EyeOfCthulhu>().transformBaseInstance.DoEnterLogic(); 
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.GetComponent<EyeOfCthulhu>().transformBaseInstance.DoExitLogic();
    }

    public override void FrameUpdate()  
    {
        base.FrameUpdate();
        enemy.GetComponent<EyeOfCthulhu>().transformBaseInstance.DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.GetComponent<EyeOfCthulhu>().transformBaseInstance.DoPhysicsLogic();
    }
}
