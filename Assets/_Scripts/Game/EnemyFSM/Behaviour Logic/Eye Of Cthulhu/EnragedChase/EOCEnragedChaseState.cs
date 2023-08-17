using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EOCEnragedChaseState : EnemyState
{
    public EOCEnragedChaseState(EyeOfCthulhu enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        baseInstance().DoAnimationTriggerEventLogic(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Entered EOC Enraged Chase State");
        baseInstance().DoEnterLogic(); 
    }

    public override void ExitState()
    {
        base.ExitState();
        baseInstance().DoExitLogic();
    }

    public override void FrameUpdate()  
    {
        base.FrameUpdate();
        baseInstance().DoFrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        baseInstance().DoPhysicsLogic();
    }

    private EOCEnragedChaseSOBase baseInstance()
    {
        return enemy.GetComponent<EyeOfCthulhu>().enragedChaseBaseInstance;
    }
}
